using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000386 RID: 902
	public class DesertionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600358A RID: 13706 RVA: 0x000E7FF2 File Offset: 0x000E61F2
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000E800B File Offset: 0x000E620B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000E8010 File Offset: 0x000E6210
		public void DailyTickParty(MobileParty mobileParty)
		{
			if (!Campaign.Current.DesertionEnabled)
			{
				return;
			}
			if (mobileParty.IsActive && !mobileParty.IsDisbanding && mobileParty.Party.MapEvent == null && (mobileParty.IsLordParty || mobileParty.IsCaravan))
			{
				TroopRoster troopRoster = null;
				if (mobileParty.MemberRoster.TotalRegulars > 0)
				{
					this.PartiesCheckDesertionDueToMorale(mobileParty, ref troopRoster);
					this.PartiesCheckDesertionDueToPartySizeExceedsPaymentRatio(mobileParty, ref troopRoster);
				}
				if (troopRoster != null && troopRoster.Count > 0)
				{
					CampaignEventDispatcher.Instance.OnTroopsDeserted(mobileParty, troopRoster);
				}
				if (mobileParty.Party.NumberOfAllMembers <= 0)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000E80B0 File Offset: 0x000E62B0
		private void PartiesCheckForTroopDesertionEffectiveMorale(MobileParty party, int stackNo, int desertIfMoraleIsLessThanValue, out int numberOfDeserters, out int numberOfWoundedDeserters)
		{
			int num = 0;
			int num2 = 0;
			float morale = party.Morale;
			if (party.IsActive && party.MemberRoster.Count > 0)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(stackNo);
				float num3 = MathF.Pow((float)elementCopyAtIndex.Character.Level / 100f, 0.1f * (((float)desertIfMoraleIsLessThanValue - morale) / (float)desertIfMoraleIsLessThanValue));
				for (int i = 0; i < elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber; i++)
				{
					if (num3 < MBRandom.RandomFloat)
					{
						num++;
					}
				}
				for (int j = 0; j < elementCopyAtIndex.WoundedNumber; j++)
				{
					if (num3 < MBRandom.RandomFloat)
					{
						num2++;
					}
				}
			}
			numberOfDeserters = num;
			numberOfWoundedDeserters = num2;
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000E8170 File Offset: 0x000E6370
		private void PartiesCheckDesertionDueToPartySizeExceedsPaymentRatio(MobileParty mobileParty, ref TroopRoster desertedTroopList)
		{
			int partySizeLimit = mobileParty.Party.PartySizeLimit;
			if ((mobileParty.IsLordParty || mobileParty.IsCaravan) && mobileParty.Party.NumberOfAllMembers > partySizeLimit && mobileParty != MobileParty.MainParty && mobileParty.MapEvent == null)
			{
				int num = mobileParty.Party.NumberOfAllMembers - partySizeLimit;
				for (int i = 0; i < num; i++)
				{
					CharacterObject character = mobileParty.MapFaction.BasicTroop;
					int num2 = 99;
					bool flag = false;
					for (int j = 0; j < mobileParty.MemberRoster.Count; j++)
					{
						CharacterObject characterAtIndex = mobileParty.MemberRoster.GetCharacterAtIndex(j);
						if (!characterAtIndex.IsHero && characterAtIndex.Level < num2 && mobileParty.MemberRoster.GetElementNumber(j) > 0)
						{
							num2 = characterAtIndex.Level;
							character = characterAtIndex;
							flag = (mobileParty.MemberRoster.GetElementWoundedNumber(j) > 0);
						}
					}
					if (num2 < 99)
					{
						if (flag)
						{
							mobileParty.MemberRoster.AddToCounts(character, -1, false, -1, 0, true, -1);
						}
						else
						{
							mobileParty.MemberRoster.AddToCounts(character, -1, false, 0, 0, true, -1);
						}
					}
				}
			}
			bool flag2 = mobileParty.IsWageLimitExceeded();
			if (mobileParty.Party.NumberOfAllMembers > mobileParty.LimitedPartySize || flag2)
			{
				int numberOfDeserters = Campaign.Current.Models.PartyDesertionModel.GetNumberOfDeserters(mobileParty);
				int num3 = 0;
				while (num3 < numberOfDeserters && mobileParty.MemberRoster.TotalRegulars > 0)
				{
					int stackNo = -1;
					int num4 = 9;
					int num5 = 1;
					int num6 = 0;
					while (num6 < mobileParty.MemberRoster.Count && mobileParty.MemberRoster.TotalRegulars > 0)
					{
						CharacterObject characterAtIndex2 = mobileParty.MemberRoster.GetCharacterAtIndex(num6);
						int elementNumber = mobileParty.MemberRoster.GetElementNumber(num6);
						if (!characterAtIndex2.IsHero && elementNumber > 0 && characterAtIndex2.Tier < num4)
						{
							num4 = characterAtIndex2.Tier;
							stackNo = num6;
							num5 = Math.Min(elementNumber, numberOfDeserters - num3);
						}
						num6++;
					}
					MobilePartyHelper.DesertTroopsFromParty(mobileParty, stackNo, num5, 0, ref desertedTroopList);
					num3 += num5;
				}
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000E8388 File Offset: 0x000E6588
		private bool PartiesCheckDesertionDueToMorale(MobileParty party, ref TroopRoster desertedTroopList)
		{
			int moraleThresholdForTroopDesertion = Campaign.Current.Models.PartyDesertionModel.GetMoraleThresholdForTroopDesertion(party);
			bool result = false;
			if (party.Morale < (float)moraleThresholdForTroopDesertion && party.MemberRoster.TotalManCount > 0)
			{
				for (int i = party.MemberRoster.Count - 1; i >= 0; i--)
				{
					if (!party.MemberRoster.GetCharacterAtIndex(i).IsHero)
					{
						int num = 0;
						int num2 = 0;
						this.PartiesCheckForTroopDesertionEffectiveMorale(party, i, moraleThresholdForTroopDesertion, out num, out num2);
						if (num + num2 > 0)
						{
							if (party.IsLordParty && party.MapFaction.IsKingdomFaction)
							{
								this._numberOfDesertersFromLordParty++;
							}
							result = true;
							MobilePartyHelper.DesertTroopsFromParty(party, i, num, num2, ref desertedTroopList);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400113B RID: 4411
		private int _numberOfDesertersFromLordParty;
	}
}
