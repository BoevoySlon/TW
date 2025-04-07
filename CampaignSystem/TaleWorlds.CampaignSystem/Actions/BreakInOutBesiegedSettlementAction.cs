using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000428 RID: 1064
	public static class BreakInOutBesiegedSettlementAction
	{
		// Token: 0x06004050 RID: 16464 RVA: 0x0013CF84 File Offset: 0x0013B184
		public static void ApplyBreakIn(out TroopRoster casualties, out int armyCasualtiesCount)
		{
			BreakInOutBesiegedSettlementAction.ApplyInternal(true, out casualties, out armyCasualtiesCount);
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0013CF8E File Offset: 0x0013B18E
		public static void ApplyBreakOut(out TroopRoster casualties, out int armyCasualtiesCount)
		{
			BreakInOutBesiegedSettlementAction.ApplyInternal(false, out casualties, out armyCasualtiesCount);
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0013CF98 File Offset: 0x0013B198
		private static void ApplyInternal(bool breakIn, out TroopRoster casualties, out int armyCasualtiesCount)
		{
			casualties = TroopRoster.CreateDummyTroopRoster();
			armyCasualtiesCount = -1;
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int num;
			if (breakIn)
			{
				num = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent);
			}
			else
			{
				num = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent);
			}
			if (mainParty.Army == null || mainParty.Army.LeaderParty != mainParty)
			{
				TroopRoster memberRoster = mainParty.MemberRoster;
				for (int i = 0; i < num; i++)
				{
					int index = MBRandom.RandomInt(memberRoster.Count);
					CharacterObject characterAtIndex = memberRoster.GetCharacterAtIndex(index);
					if (!characterAtIndex.IsRegular || memberRoster.GetElementNumber(index) == 0)
					{
						i--;
					}
					else
					{
						memberRoster.AddToCountsAtIndex(index, -1, 0, 0, true);
						casualties.AddToCounts(characterAtIndex, 1, false, 0, 0, true, -1);
					}
				}
				if (mainParty.Army != null && mainParty.Army.LeaderParty != MobileParty.MainParty)
				{
					TroopSacrificeModel troopSacrificeModel = Campaign.Current.Models.TroopSacrificeModel;
					ChangeRelationAction.ApplyPlayerRelation(mainParty.Army.LeaderParty.LeaderHero, troopSacrificeModel.BreakOutArmyLeaderRelationPenalty, true, true);
					foreach (MobileParty mobileParty in mainParty.Army.LeaderParty.AttachedParties)
					{
						if (mobileParty.LeaderHero != null && mobileParty != mainParty)
						{
							ChangeRelationAction.ApplyPlayerRelation(mobileParty.LeaderHero, troopSacrificeModel.BreakOutArmyMemberRelationPenalty, true, true);
						}
					}
					MobileParty.MainParty.Army = null;
					return;
				}
			}
			else
			{
				armyCasualtiesCount = 0;
				Army army = mainParty.Army;
				int num2 = 0;
				foreach (MobileParty mobileParty2 in army.Parties)
				{
					num2 += mobileParty2.MemberRoster.TotalManCount - mobileParty2.MemberRoster.TotalHeroes;
				}
				for (int j = 0; j < num; j++)
				{
					float num3 = MBRandom.RandomFloat * (float)num2;
					foreach (MobileParty mobileParty3 in army.Parties)
					{
						num3 -= (float)(mobileParty3.MemberRoster.TotalManCount - mobileParty3.MemberRoster.TotalHeroes);
						if (num3 < 0f)
						{
							num3 += (float)(mobileParty3.MemberRoster.TotalManCount - mobileParty3.MemberRoster.TotalHeroes);
							int num4 = -1;
							for (int k = 0; k < mobileParty3.MemberRoster.Count; k++)
							{
								if (!mobileParty3.MemberRoster.GetCharacterAtIndex(k).IsHero)
								{
									num3 -= (float)(mobileParty3.MemberRoster.GetElementNumber(k) + mobileParty3.MemberRoster.GetElementWoundedNumber(k));
									if (num3 < 0f)
									{
										num4 = k;
										break;
									}
								}
							}
							if (num4 >= 0)
							{
								CharacterObject characterAtIndex2 = mobileParty3.MemberRoster.GetCharacterAtIndex(num4);
								mobileParty3.MemberRoster.AddToCountsAtIndex(num4, -1, 0, 0, true);
								num2--;
								if (mobileParty3 == MobileParty.MainParty)
								{
									casualties.AddToCounts(characterAtIndex2, 1, false, 0, 0, true, -1);
									break;
								}
								armyCasualtiesCount++;
								break;
							}
						}
					}
				}
			}
		}
	}
}
