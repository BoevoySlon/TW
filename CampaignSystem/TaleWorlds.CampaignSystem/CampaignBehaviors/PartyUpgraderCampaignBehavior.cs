using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003BD RID: 957
	public class PartyUpgraderCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A63 RID: 14947 RVA: 0x001136F4 File Offset: 0x001118F4
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x00113724 File Offset: 0x00111924
		private void MapEventEnded(MapEvent mapEvent)
		{
			foreach (PartyBase party in mapEvent.InvolvedParties)
			{
				this.UpgradeReadyTroops(party);
			}
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x00113774 File Offset: 0x00111974
		public void DailyTickParty(MobileParty party)
		{
			if (party.MapEvent == null)
			{
				this.UpgradeReadyTroops(party.Party);
			}
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x0011378A File Offset: 0x0011198A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x0011378C File Offset: 0x0011198C
		private PartyUpgraderCampaignBehavior.TroopUpgradeArgs SelectPossibleUpgrade(List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> possibleUpgrades)
		{
			PartyUpgraderCampaignBehavior.TroopUpgradeArgs result = possibleUpgrades[0];
			if (possibleUpgrades.Count > 1)
			{
				float num = 0f;
				foreach (PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs in possibleUpgrades)
				{
					num += troopUpgradeArgs.UpgradeChance;
				}
				float num2 = num * MBRandom.RandomFloat;
				foreach (PartyUpgraderCampaignBehavior.TroopUpgradeArgs troopUpgradeArgs2 in possibleUpgrades)
				{
					num2 -= troopUpgradeArgs2.UpgradeChance;
					if (num2 <= 0f)
					{
						result = troopUpgradeArgs2;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00113850 File Offset: 0x00111A50
		private List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> GetPossibleUpgradeTargets(PartyBase party, TroopRosterElement element)
		{
			PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
			List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> list = new List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs>();
			CharacterObject character = element.Character;
			int num = element.Number - element.WoundedNumber;
			if (num > 0)
			{
				PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
				int i = 0;
				while (i < character.UpgradeTargets.Length)
				{
					CharacterObject characterObject = character.UpgradeTargets[i];
					int upgradeXpCost = character.GetUpgradeXpCost(party, i);
					if (upgradeXpCost <= 0)
					{
						goto IL_7F;
					}
					num = MathF.Min(num, element.Xp / upgradeXpCost);
					if (num != 0)
					{
						goto IL_7F;
					}
					IL_19C:
					i++;
					continue;
					IL_7F:
					if (characterObject.Tier > character.Tier && party.MobileParty.HasLimitedWage() && party.MobileParty.TotalWage + num * (partyWageModel.GetCharacterWage(characterObject) - partyWageModel.GetCharacterWage(character)) > party.MobileParty.PaymentLimit)
					{
						num = MathF.Max(0, MathF.Min(num, (party.MobileParty.PaymentLimit - party.MobileParty.TotalWage) / (partyWageModel.GetCharacterWage(characterObject) - partyWageModel.GetCharacterWage(character))));
						if (num == 0)
						{
							goto IL_19C;
						}
					}
					int upgradeGoldCost = character.GetUpgradeGoldCost(party, i);
					if (party.LeaderHero != null && upgradeGoldCost != 0 && num * upgradeGoldCost > party.LeaderHero.Gold)
					{
						num = party.LeaderHero.Gold / upgradeGoldCost;
						if (num == 0)
						{
							goto IL_19C;
						}
					}
					if ((!party.Culture.IsBandit || characterObject.Culture.IsBandit) && (character.Occupation != Occupation.Bandit || partyTroopUpgradeModel.CanPartyUpgradeTroopToTarget(party, character, characterObject)))
					{
						float upgradeChanceForTroopUpgrade = Campaign.Current.Models.PartyTroopUpgradeModel.GetUpgradeChanceForTroopUpgrade(party, character, i);
						list.Add(new PartyUpgraderCampaignBehavior.TroopUpgradeArgs(character, characterObject, num, upgradeGoldCost, upgradeXpCost, upgradeChanceForTroopUpgrade));
						goto IL_19C;
					}
					goto IL_19C;
				}
			}
			return list;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00113A10 File Offset: 0x00111C10
		private void ApplyEffects(PartyBase party, PartyUpgraderCampaignBehavior.TroopUpgradeArgs upgradeArgs)
		{
			if (party.Owner != null && party.Owner.IsAlive)
			{
				SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
				GiveGoldAction.ApplyBetweenCharacters(party.Owner, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
				return;
			}
			if (party.LeaderHero != null && party.LeaderHero.IsAlive)
			{
				SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
				GiveGoldAction.ApplyBetweenCharacters(party.LeaderHero, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x00113AAC File Offset: 0x00111CAC
		private void UpgradeTroop(PartyBase party, int rosterIndex, PartyUpgraderCampaignBehavior.TroopUpgradeArgs upgradeArgs)
		{
			TroopRoster memberRoster = party.MemberRoster;
			CharacterObject upgradeTarget = upgradeArgs.UpgradeTarget;
			int possibleUpgradeCount = upgradeArgs.PossibleUpgradeCount;
			int num = upgradeArgs.UpgradeXpCost * possibleUpgradeCount;
			memberRoster.SetElementXp(rosterIndex, memberRoster.GetElementXp(rosterIndex) - num);
			memberRoster.AddToCounts(upgradeArgs.Target, -possibleUpgradeCount, false, 0, 0, true, -1);
			memberRoster.AddToCounts(upgradeTarget, possibleUpgradeCount, false, 0, 0, true, -1);
			if (possibleUpgradeCount > 0)
			{
				this.ApplyEffects(party, upgradeArgs);
			}
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00113B18 File Offset: 0x00111D18
		public void UpgradeReadyTroops(PartyBase party)
		{
			if (party != PartyBase.MainParty && party.IsActive)
			{
				TroopRoster memberRoster = party.MemberRoster;
				PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
				for (int i = 0; i < memberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
					if (partyTroopUpgradeModel.IsTroopUpgradeable(party, elementCopyAtIndex.Character))
					{
						List<PartyUpgraderCampaignBehavior.TroopUpgradeArgs> possibleUpgradeTargets = this.GetPossibleUpgradeTargets(party, elementCopyAtIndex);
						if (possibleUpgradeTargets.Count > 0)
						{
							PartyUpgraderCampaignBehavior.TroopUpgradeArgs upgradeArgs = this.SelectPossibleUpgrade(possibleUpgradeTargets);
							this.UpgradeTroop(party, i, upgradeArgs);
						}
					}
				}
			}
		}

		// Token: 0x0200071B RID: 1819
		private readonly struct TroopUpgradeArgs
		{
			// Token: 0x060058D3 RID: 22739 RVA: 0x00183673 File Offset: 0x00181873
			public TroopUpgradeArgs(CharacterObject target, CharacterObject upgradeTarget, int possibleUpgradeCount, int upgradeGoldCost, int upgradeXpCost, float upgradeChance)
			{
				this.Target = target;
				this.UpgradeTarget = upgradeTarget;
				this.PossibleUpgradeCount = possibleUpgradeCount;
				this.UpgradeGoldCost = upgradeGoldCost;
				this.UpgradeXpCost = upgradeXpCost;
				this.UpgradeChance = upgradeChance;
			}

			// Token: 0x04001DBF RID: 7615
			public readonly CharacterObject Target;

			// Token: 0x04001DC0 RID: 7616
			public readonly CharacterObject UpgradeTarget;

			// Token: 0x04001DC1 RID: 7617
			public readonly int PossibleUpgradeCount;

			// Token: 0x04001DC2 RID: 7618
			public readonly int UpgradeGoldCost;

			// Token: 0x04001DC3 RID: 7619
			public readonly int UpgradeXpCost;

			// Token: 0x04001DC4 RID: 7620
			public readonly float UpgradeChance;
		}
	}
}
