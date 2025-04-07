using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003CF RID: 975
	public class RecruitPrisonersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003C04 RID: 15364 RVA: 0x00120080 File Offset: 0x0011E280
		public override void RegisterEvents()
		{
			CampaignEvents.OnMainPartyPrisonerRecruitedEvent.AddNonSerializedListener(this, new Action<FlattenedTroopRoster>(this.OnMainPartyPrisonerRecruited));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickAIMobileParty));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTickMainParty));
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x001200D4 File Offset: 0x0011E2D4
		private void HourlyTickMainParty()
		{
			MobileParty mainParty = MobileParty.MainParty;
			TroopRoster memberRoster = mainParty.MemberRoster;
			TroopRoster prisonRoster = mainParty.PrisonRoster;
			if (memberRoster.Count != 0 && memberRoster.TotalManCount > 0 && prisonRoster.Count != 0 && prisonRoster.TotalRegulars > 0 && mainParty.MapEvent == null)
			{
				int num = MBRandom.RandomInt(0, prisonRoster.Count);
				bool flag = false;
				for (int i = num; i < prisonRoster.Count + num; i++)
				{
					int index = i % prisonRoster.Count;
					CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
					if (characterAtIndex.IsRegular)
					{
						CharacterObject characterObject = characterAtIndex;
						int elementNumber = mainParty.PrisonRoster.GetElementNumber(index);
						int num2 = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(mainParty.Party, characterObject);
						if (!flag && num2 < elementNumber)
						{
							flag = this.GenerateConformityForTroop(mainParty, characterObject, 1);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001201C0 File Offset: 0x0011E3C0
		private void DailyTickAIMobileParty(MobileParty mobileParty)
		{
			TroopRoster prisonRoster = mobileParty.PrisonRoster;
			if (!mobileParty.IsMainParty && mobileParty.IsLordParty && prisonRoster.Count != 0 && prisonRoster.TotalRegulars > 0 && mobileParty.MapEvent == null)
			{
				int num = MBRandom.RandomInt(0, prisonRoster.Count);
				bool flag = false;
				for (int i = num; i < prisonRoster.Count + num; i++)
				{
					int index = i % prisonRoster.Count;
					CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
					if (characterAtIndex.IsRegular)
					{
						CharacterObject characterObject = characterAtIndex;
						int elementNumber = mobileParty.PrisonRoster.GetElementNumber(index);
						int num2 = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(mobileParty.Party, characterObject);
						if (!flag && num2 < elementNumber)
						{
							flag = this.GenerateConformityForTroop(mobileParty, characterObject, 24);
						}
						if (Campaign.Current.Models.PrisonerRecruitmentCalculationModel.ShouldPartyRecruitPrisoners(mobileParty.Party))
						{
							int conformityCost;
							if (this.IsPrisonerRecruitable(mobileParty, characterObject, out conformityCost))
							{
								int num3 = mobileParty.LimitedPartySize - mobileParty.MemberRoster.TotalManCount;
								int num4 = MathF.Min((num3 > 0) ? ((num3 > num2) ? num2 : num3) : 0, prisonRoster.GetElementNumber(characterObject));
								if (num4 > 0)
								{
									this.RecruitPrisonersAi(mobileParty, characterObject, num4, conformityCost);
								}
							}
						}
						else if (flag)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x00120314 File Offset: 0x0011E514
		private bool GenerateConformityForTroop(MobileParty mobileParty, CharacterObject troop, int hours = 1)
		{
			int xpAmount = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityChangePerHour(mobileParty.Party, troop) * hours;
			mobileParty.PrisonRoster.AddXpToTroop(xpAmount, troop);
			return true;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x00120350 File Offset: 0x0011E550
		private void ApplyPrisonerRecruitmentEffects(MobileParty mobileParty, CharacterObject troop, int num)
		{
			int prisonerRecruitmentMoraleEffect = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(mobileParty.Party, troop, num);
			mobileParty.RecentEventsMorale += (float)prisonerRecruitmentMoraleEffect;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0012038C File Offset: 0x0011E58C
		private void RecruitPrisonersAi(MobileParty mobileParty, CharacterObject troop, int num, int conformityCost)
		{
			mobileParty.PrisonRoster.GetElementNumber(troop);
			mobileParty.PrisonRoster.GetElementXp(troop);
			mobileParty.PrisonRoster.AddToCounts(troop, -num, false, 0, -conformityCost * num, true, -1);
			mobileParty.MemberRoster.AddToCounts(troop, num, false, 0, 0, true, -1);
			CampaignEventDispatcher.Instance.OnTroopRecruited(mobileParty.LeaderHero, null, null, troop, num);
			this.ApplyPrisonerRecruitmentEffects(mobileParty, troop, num);
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001203FB File Offset: 0x0011E5FB
		private bool IsPrisonerRecruitable(MobileParty mobileParty, CharacterObject character, out int conformityNeeded)
		{
			return Campaign.Current.Models.PrisonerRecruitmentCalculationModel.IsPrisonerRecruitable(mobileParty.Party, character, out conformityNeeded);
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0012041C File Offset: 0x0011E61C
		private void OnMainPartyPrisonerRecruited(FlattenedTroopRoster flattenedTroopRosters)
		{
			foreach (CharacterObject characterObject in flattenedTroopRosters.Troops)
			{
				CampaignEventDispatcher.Instance.OnUnitRecruited(characterObject, 1);
				this.ApplyPrisonerRecruitmentEffects(MobileParty.MainParty, characterObject, 1);
			}
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0012047C File Offset: 0x0011E67C
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
