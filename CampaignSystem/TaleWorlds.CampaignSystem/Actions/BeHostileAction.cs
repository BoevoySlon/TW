using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000427 RID: 1063
	public static class BeHostileAction
	{
		// Token: 0x0600404A RID: 16458 RVA: 0x0013CAEC File Offset: 0x0013ACEC
		private static void ApplyInternal(PartyBase attackerParty, PartyBase defenderParty, float value)
		{
			if (defenderParty.IsMobile && defenderParty.MobileParty.ActualClan == null)
			{
				return;
			}
			int num = (int)(-1f * value);
			int relationChange = (int)(-5f * value);
			int relationChange2 = (int)(-1f * value);
			int num2 = (int)(-4f * value);
			int relationChange3 = (int)(-4f * value);
			int num3 = (int)(-10f * value);
			int num4 = (int)(-2f * value);
			bool flag = attackerParty.MapFaction.IsAtWarWith(defenderParty.MapFaction);
			Hero leaderHero = attackerParty.LeaderHero;
			if (defenderParty.IsSettlement)
			{
				if (defenderParty.Settlement.IsVillage && !flag)
				{
					if (num2 < 0)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, defenderParty.Settlement.OwnerClan.Leader, num2, true);
						foreach (Hero gainedRelationWith in defenderParty.Settlement.Notables)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, gainedRelationWith, relationChange3, true);
						}
					}
					BeHostileAction.ApplyGeneralConsequencesOnPeace(attackerParty, defenderParty, value);
					return;
				}
			}
			else if (defenderParty.MobileParty != null)
			{
				if (defenderParty.MobileParty.IsVillager)
				{
					if (flag)
					{
						using (List<Hero>.Enumerator enumerator = defenderParty.MobileParty.HomeSettlement.Notables.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Hero gainedRelationWith2 = enumerator.Current;
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, gainedRelationWith2, relationChange2, true);
							}
							return;
						}
					}
					if (num < 0)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, defenderParty.MobileParty.HomeSettlement.OwnerClan.Leader, num, true);
						foreach (Hero gainedRelationWith3 in defenderParty.MobileParty.HomeSettlement.Notables)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, gainedRelationWith3, relationChange, true);
						}
					}
					BeHostileAction.ApplyGeneralConsequencesOnPeace(attackerParty, defenderParty, value);
					return;
				}
				if (defenderParty.MobileParty.IsCaravan)
				{
					if (flag)
					{
						if (num4 < 0 && defenderParty.Owner != null)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, defenderParty.Owner, num4, true);
							return;
						}
					}
					else
					{
						if (num3 < 0 && defenderParty.Owner != null)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, defenderParty.Owner, num3, true);
						}
						BeHostileAction.ApplyGeneralConsequencesOnPeace(attackerParty, defenderParty, value);
					}
				}
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0013CD48 File Offset: 0x0013AF48
		private static void ApplyGeneralConsequencesOnPeace(PartyBase attackerParty, PartyBase defenderParty, float value)
		{
			float num = -25f * value;
			float num2 = 10f * value;
			int num3 = (int)(-2f * value);
			float num4 = -50f * value;
			bool isClan = attackerParty.MapFaction.IsClan;
			bool isKingdomLeader = attackerParty.LeaderHero.IsKingdomLeader;
			bool isUnderMercenaryService = attackerParty.LeaderHero.Clan.IsUnderMercenaryService;
			Hero leaderHero = attackerParty.LeaderHero;
			if (leaderHero.Equals(Hero.MainHero))
			{
				if (num < 0f)
				{
					TraitLevelingHelper.OnHostileAction((int)num);
				}
				if (num2 > 0f)
				{
					ChangeCrimeRatingAction.Apply(defenderParty.MapFaction, num2, true);
				}
			}
			if (!isClan)
			{
				if (isKingdomLeader)
				{
					if (num4 < 0f)
					{
						GainKingdomInfluenceAction.ApplyForDefault(attackerParty.MobileParty.LeaderHero, num4);
						return;
					}
				}
				else if (isUnderMercenaryService)
				{
					if (num3 < 0)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, leaderHero.MapFaction.Leader, num3, true);
					}
					if (value == 6f)
					{
						ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(leaderHero.Clan, true);
						return;
					}
				}
				else
				{
					if (num3 < 0 && attackerParty.MapFaction != null && defenderParty.MapFaction != null)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leaderHero, defenderParty.MapFaction.Leader, num3, true);
					}
					if (num4 < 0f)
					{
						GainKingdomInfluenceAction.ApplyForDefault(attackerParty.MobileParty.LeaderHero, num4);
					}
				}
			}
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0013CE74 File Offset: 0x0013B074
		public static void ApplyHostileAction(PartyBase attackerParty, PartyBase defenderParty, float value)
		{
			if (attackerParty == null || defenderParty == null || value == 0f)
			{
				Debug.FailedAssert("BeHostileAction, attackerParty and/or defenderParty is null or value is 0.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\BeHostileAction.cs", "ApplyHostileAction", 199);
				return;
			}
			BeHostileAction.ApplyInternal(attackerParty, defenderParty, value);
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0013CEA6 File Offset: 0x0013B0A6
		public static void ApplyMinorCoercionHostileAction(PartyBase attackerParty, PartyBase defenderParty)
		{
			if (attackerParty == null || defenderParty == null)
			{
				Debug.FailedAssert("BeHostileAction, attackerParty and/or defenderParty is null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\BeHostileAction.cs", "ApplyMinorCoercionHostileAction", 211);
				return;
			}
			BeHostileAction.ApplyInternal(attackerParty, defenderParty, 1f);
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0013CED4 File Offset: 0x0013B0D4
		public static void ApplyMajorCoercionHostileAction(PartyBase attackerParty, PartyBase defenderParty)
		{
			if (attackerParty == null || defenderParty == null)
			{
				Debug.FailedAssert("BeHostileAction, attackerParty and/or defenderParty is null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\BeHostileAction.cs", "ApplyMajorCoercionHostileAction", 223);
				return;
			}
			BeHostileAction.ApplyInternal(attackerParty, defenderParty, 2f);
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0013CF04 File Offset: 0x0013B104
		public static void ApplyEncounterHostileAction(PartyBase attackerParty, PartyBase defenderParty)
		{
			if (Campaign.Current.Models.EncounterModel.IsEncounterExemptFromHostileActions(attackerParty, defenderParty))
			{
				return;
			}
			BeHostileAction.ApplyInternal(attackerParty, defenderParty, 6f);
			if (attackerParty == PartyBase.MainParty && attackerParty.MapFaction != defenderParty.MapFaction && !FactionManager.IsAtWarAgainstFaction(attackerParty.MapFaction, defenderParty.MapFaction))
			{
				ChangeRelationAction.ApplyPlayerRelation(defenderParty.MapFaction.Leader, -10, true, true);
				DeclareWarAction.ApplyByPlayerHostility(attackerParty.MapFaction, defenderParty.MapFaction);
			}
		}

		// Token: 0x040012BC RID: 4796
		private const float MinorCoercionValue = 1f;

		// Token: 0x040012BD RID: 4797
		private const float MajorCoercionValue = 2f;

		// Token: 0x040012BE RID: 4798
		private const float EncounterValue = 6f;
	}
}
