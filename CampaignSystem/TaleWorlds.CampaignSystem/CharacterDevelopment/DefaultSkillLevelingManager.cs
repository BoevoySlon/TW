using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x0200034E RID: 846
	public class DefaultSkillLevelingManager : ISkillLevelingManager
	{
		// Token: 0x06002FCB RID: 12235 RVA: 0x000CC1CC File Offset: 0x000CA3CC
		public void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge)
		{
			if (isTeamKill)
			{
				return;
			}
			float num = 1f;
			if (affectorCharacter.IsHero)
			{
				Hero heroObject = affectorCharacter.HeroObject;
				CombatXpModel combatXpModel = Campaign.Current.Models.CombatXpModel;
				CharacterObject characterObject = heroObject.CharacterObject;
				MobileParty partyBelongedTo = heroObject.PartyBelongedTo;
				int num2;
				combatXpModel.GetXpFromHit(characterObject, captain, affectedCharacter, (partyBelongedTo != null) ? partyBelongedTo.Party : null, (int)damageAmount, isFatal, missionType, out num2);
				num = (float)num2;
				SkillObject skillObject;
				if (affectorWeapon != null)
				{
					skillObject = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(affectorWeapon, isSiegeEngineHit);
					float num3 = (skillObject == DefaultSkills.Bow) ? 0.5f : 1f;
					if (shotDifficulty > 0f)
					{
						num += (float)MathF.Floor(num * num3 * Campaign.Current.Models.CombatXpModel.GetXpMultiplierFromShotDifficulty(shotDifficulty));
					}
				}
				else
				{
					skillObject = (isHorseCharge ? DefaultSkills.Riding : DefaultSkills.Athletics);
				}
				heroObject.AddSkillXp(skillObject, (float)MBRandom.RoundRandomized(num));
				if (!isSiegeEngineHit && !isHorseCharge)
				{
					float num4 = shotDifficulty * 0.15f;
					if (isAffectorMounted)
					{
						float num5 = 0.5f;
						if (num4 > 0f)
						{
							num5 += num4;
						}
						if (speedBonusFromMovement > 0f)
						{
							num5 *= 1f + speedBonusFromMovement;
						}
						if (num5 > 0f)
						{
							DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(num5 * num), heroObject.CharacterObject.Equipment.Horse.Item);
						}
					}
					else
					{
						float num6 = 1f;
						if (num4 > 0f)
						{
							num6 += num4;
						}
						if (speedBonusFromMovement > 0f)
						{
							num6 += 1.5f * speedBonusFromMovement;
						}
						if (num6 > 0f)
						{
							heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(num6 * num));
						}
					}
				}
			}
			if (commander != null && commander != affectorCharacter.HeroObject && commander.PartyBelongedTo != null)
			{
				this.OnTacticsUsed(commander.PartyBelongedTo, (float)MathF.Ceiling(0.02f * num));
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000CC3AC File Offset: 0x000CA5AC
		public void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				float skillXp = (float)destroyedSiegeEngine.ManDayCost * 20f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, skillXp, SkillEffect.PerkRole.Engineer);
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000CC3E4 File Offset: 0x000CA5E4
		public void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty)
		{
			int xpReward = Campaign.Current.Models.PartyTrainingModel.GetXpReward(affectedCharacter);
			if (affectorCharacter.IsHero)
			{
				ItemObject defaultWeapon = CharacterHelper.GetDefaultWeapon(affectorCharacter);
				Hero heroObject = affectorCharacter.HeroObject;
				if (defaultWeapon != null)
				{
					SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(defaultWeapon.GetWeaponWithUsageIndex(0), false);
					heroObject.AddSkillXp(skillForWeapon, (float)xpReward);
				}
				if (affectorCharacter.IsMounted)
				{
					float f = (float)xpReward * 0.3f;
					DefaultSkillLevelingManager.OnGainingRidingExperience(heroObject, (float)MBRandom.RoundRandomized(f), heroObject.CharacterObject.Equipment.Horse.Item);
				}
				else
				{
					float f2 = (float)xpReward * 0.3f;
					heroObject.AddSkillXp(DefaultSkills.Athletics, (float)MBRandom.RoundRandomized(f2));
				}
			}
			if (commanderParty != null && commanderParty.IsMobile && commanderParty.LeaderHero != null && commanderParty.LeaderHero != affectedCharacter.HeroObject)
			{
				this.OnTacticsUsed(commanderParty.MobileParty, (float)MathF.Ceiling(0.02f * (float)xpReward));
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000CC4E4 File Offset: 0x000CA6E4
		public void OnTradeProfitMade(PartyBase party, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float skillXp = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPartySkillExercised(party.MobileParty, DefaultSkills.Trade, skillXp, SkillEffect.PerkRole.PartyLeader);
			}
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000CC510 File Offset: 0x000CA710
		public void OnTradeProfitMade(Hero hero, int tradeProfit)
		{
			if (tradeProfit > 0)
			{
				float skillXp = (float)tradeProfit * 0.5f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Trade, skillXp, hero == Hero.MainHero);
			}
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000CC53E File Offset: 0x000CA73E
		public void OnSettlementProjectFinished(Settlement settlement)
		{
			DefaultSkillLevelingManager.OnSettlementSkillExercised(settlement, DefaultSkills.Steward, 1000f);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000CC550 File Offset: 0x000CA750
		public void OnSettlementGoverned(Hero governor, Settlement settlement)
		{
			float prosperityChange = settlement.Town.ProsperityChange;
			if (prosperityChange > 0f)
			{
				float skillXp = prosperityChange * 30f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(governor, DefaultSkills.Steward, skillXp, true);
			}
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000CC588 File Offset: 0x000CA788
		public void OnInfluenceSpent(Hero hero, float amountSpent)
		{
			if (hero.PartyBelongedTo != null)
			{
				float skillXp = 10f * amountSpent;
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Steward, skillXp, SkillEffect.PerkRole.PartyLeader);
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000CC5B8 File Offset: 0x000CA7B8
		public void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default)
		{
			if ((hero.PartyBelongedTo == null && detail != ChangeRelationAction.ChangeRelationDetail.Emissary) || relationChange <= 0f)
			{
				return;
			}
			int charmExperienceFromRelationGain = Campaign.Current.Models.DiplomacyModel.GetCharmExperienceFromRelationGain(gainedRelationWith, relationChange, detail);
			if (hero.PartyBelongedTo != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, SkillEffect.PerkRole.PartyLeader);
				return;
			}
			DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Charm, (float)charmExperienceFromRelationGain, true);
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000CC620 File Offset: 0x000CA820
		public void OnTroopRecruited(Hero hero, int amount, int tier)
		{
			if (amount > 0)
			{
				int num = amount * tier * 2;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(hero, DefaultSkills.Leadership, (float)num, true);
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000CC648 File Offset: 0x000CA848
		public void OnBribeGiven(int amount)
		{
			if (amount > 0)
			{
				float skillXp = (float)amount * 0.1f;
				DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, skillXp, SkillEffect.PerkRole.PartyLeader);
			}
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000CC673 File Offset: 0x000CA873
		public void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count)
		{
			if (count > 0)
			{
				DefaultSkillLevelingManager.OnPersonalSkillExercised(mobileParty.LeaderHero, DefaultSkills.Roguery, (float)(count * 2 * bandit.Tier), true);
			}
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000CC698 File Offset: 0x000CA898
		public void OnMainHeroReleasedFromCaptivity(float captivityTime)
		{
			float skillXp = captivityTime * 0.5f;
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, skillXp, true);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000CC6C0 File Offset: 0x000CA8C0
		public void OnMainHeroTortured()
		{
			float skillXp = MBRandom.RandomFloatRanged(50f, 100f);
			DefaultSkillLevelingManager.OnPersonalSkillExercised(Hero.MainHero, DefaultSkills.Roguery, skillXp, true);
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000CC6F0 File Offset: 0x000CA8F0
		public void OnMainHeroDisguised(bool isNotCaught)
		{
			float skillXp = isNotCaught ? MBRandom.RandomFloatRanged(10f, 25f) : MBRandom.RandomFloatRanged(1f, 10f);
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Roguery, skillXp, SkillEffect.PerkRole.PartyLeader);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000CC734 File Offset: 0x000CA934
		public void OnRaid(MobileParty attackerParty, ItemRoster lootedItems)
		{
			if (attackerParty.LeaderHero != null)
			{
				float skillXp = (float)lootedItems.TradeGoodsTotalValue * 0.5f;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, skillXp, true);
			}
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000CC76C File Offset: 0x000CA96C
		public void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = 0f;
				if (forcedParty.IsVillager)
				{
					num = (attacked ? 0.75f : 0.5f);
				}
				else if (forcedParty.IsCaravan)
				{
					num = (attacked ? 0.15f : 0.1f);
				}
				float skillXp = (float)lootedItems.TradeGoodsTotalValue * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, skillXp, true);
			}
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000CC7D8 File Offset: 0x000CA9D8
		public void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			int num = 0;
			for (int i = 0; i < prisonerRoster.Count; i++)
			{
				num += prisonerRoster.data[i].Character.Tier * prisonerRoster.data[i].Number;
			}
			int num2 = num * 2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Roguery, (float)num2, SkillEffect.PerkRole.PartyLeader);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000CC838 File Offset: 0x000CAA38
		public void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier)
		{
			float skillXp = (float)(surgerySuccess ? (10 * troopTier) : (5 * troopTier));
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Medicine, skillXp, SkillEffect.PerkRole.Surgeon);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000CC860 File Offset: 0x000CAA60
		public void OnTacticsUsed(MobileParty party, float xp)
		{
			if (xp > 0f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Tactics, xp, SkillEffect.PerkRole.PartyLeader);
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000CC877 File Offset: 0x000CAA77
		public void OnHideoutSpotted(MobileParty party, PartyBase spottedParty)
		{
			DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Scouting, 100f, SkillEffect.PerkRole.Scout);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000CC88C File Offset: 0x000CAA8C
		public void OnTrackDetected(Track track)
		{
			float skillFromTrackDetected = Campaign.Current.Models.MapTrackModel.GetSkillFromTrackDetected(track);
			DefaultSkillLevelingManager.OnPartySkillExercised(MobileParty.MainParty, DefaultSkills.Scouting, skillFromTrackDetected, SkillEffect.PerkRole.Scout);
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000CC8C1 File Offset: 0x000CAAC1
		public void OnTravelOnFoot(Hero hero, float speed)
		{
			hero.AddSkillXp(DefaultSkills.Athletics, (float)(MBRandom.RoundRandomized(0.2f * speed) + 1));
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000CC8E0 File Offset: 0x000CAAE0
		public void OnTravelOnHorse(Hero hero, float speed)
		{
			ItemObject item = hero.CharacterObject.Equipment.Horse.Item;
			DefaultSkillLevelingManager.OnGainingRidingExperience(hero, (float)MBRandom.RoundRandomized(0.3f * speed), item);
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000CC91C File Offset: 0x000CAB1C
		public void OnHeroHealedWhileWaiting(Hero hero, int healingAmount)
		{
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.EffectiveSurgeon != null)
			{
				float num = (float)Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(hero.PartyBelongedTo.Party);
				float num2 = (hero.PartyBelongedTo.CurrentSettlement != null && !hero.PartyBelongedTo.CurrentSettlement.IsCastle) ? 0.2f : 0.1f;
				num *= (float)healingAmount * num2 * (1f + (float)hero.PartyBelongedTo.EffectiveSurgeon.Level * 0.1f);
				DefaultSkillLevelingManager.OnPartySkillExercised(hero.PartyBelongedTo, DefaultSkills.Medicine, num, SkillEffect.PerkRole.Surgeon);
			}
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000CC9C8 File Offset: 0x000CABC8
		public void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier)
		{
			float num = (float)(Campaign.Current.Models.PartyHealingModel.GetSkillXpFromHealingTroop(mobileParty.Party) * healedTroopCount) * averageTier;
			float num2 = (mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsCastle) ? 2f : 1f;
			num *= num2;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Medicine, num, SkillEffect.PerkRole.Surgeon);
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x000CCA28 File Offset: 0x000CAC28
		public void OnLeadingArmy(MobileParty mobileParty)
		{
			float skillXp = mobileParty.GetTotalStrengthWithFollowers(true) * 0.0004f * mobileParty.Army.Morale;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Leadership, skillXp, SkillEffect.PerkRole.PartyLeader);
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x000CCA5C File Offset: 0x000CAC5C
		public void OnSieging(MobileParty mobileParty)
		{
			int num = mobileParty.MemberRoster.TotalManCount;
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
			{
				foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
				{
					if (mobileParty2 != mobileParty)
					{
						num += mobileParty2.MemberRoster.TotalManCount;
					}
				}
			}
			float skillXp = 0.25f * MathF.Sqrt((float)num);
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, skillXp, SkillEffect.PerkRole.Engineer);
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x000CCAFC File Offset: 0x000CACFC
		public void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine)
		{
			float skillXp = 30f + 2f * (float)siegeEngine.Difficulty;
			DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Engineering, skillXp, SkillEffect.PerkRole.Engineer);
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000CCB2C File Offset: 0x000CAD2C
		public void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			if (hero != null)
			{
				SkillObject skill = DefaultSkills.Leadership;
				float num = 0.025f;
				if (troop.Occupation == Occupation.Bandit)
				{
					skill = DefaultSkills.Roguery;
					num = 0.05f;
				}
				float xpAmount = (float)Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(party, troop, upgrade) * num * (float)numberOfTroops;
				hero.AddSkillXp(skill, xpAmount);
			}
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000CCB98 File Offset: 0x000CAD98
		public void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			float num = (float)Campaign.Current.Models.PersuasionModel.GetSkillXpFromPersuasion(difficulty, argumentDifficultyBonusCoefficient);
			if (num > 0f)
			{
				targetHero.AddSkillXp(skill, num);
			}
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000CCBD0 File Offset: 0x000CADD0
		public void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded)
		{
			float rogueryRewardOnPrisonBreak = Campaign.Current.Models.PrisonBreakModel.GetRogueryRewardOnPrisonBreak(prisonerHero, isSucceeded);
			if (rogueryRewardOnPrisonBreak > 0f)
			{
				Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, rogueryRewardOnPrisonBreak);
			}
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000CCC0C File Offset: 0x000CAE0C
		public void OnWallBreached(MobileParty party)
		{
			if (((party != null) ? party.EffectiveEngineer : null) != null)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(party, DefaultSkills.Engineering, 250f, SkillEffect.PerkRole.Engineer);
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000CCC30 File Offset: 0x000CAE30
		public void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty)
		{
			if (attackerParty.LeaderHero != null)
			{
				int num = MathF.Ceiling(forcedParty.Settlement.Village.Hearth / 10f);
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, (float)num, true);
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000CCC74 File Offset: 0x000CAE74
		public void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked)
		{
			if (attackerParty.LeaderHero != null)
			{
				float num = attacked ? 0.75f : 0.5f;
				float skillXp = (float)lootedItems.TradeGoodsTotalValue * num;
				DefaultSkillLevelingManager.OnPersonalSkillExercised(attackerParty.LeaderHero, DefaultSkills.Roguery, skillXp, true);
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000CCCB8 File Offset: 0x000CAEB8
		public void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType)
		{
			int num = (currentTerrainType == TerrainType.Forest) ? MBRandom.RoundRandomized(5f) : MBRandom.RoundRandomized(3f);
			hero.AddSkillXp(DefaultSkills.Scouting, isCaravanParty ? ((float)num / 2f) : ((float)num));
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000CCCFC File Offset: 0x000CAEFC
		public void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType)
		{
			float num = 0f;
			float speed = mobileParty.Speed;
			if (speed > 1f)
			{
				bool flag = currentTerrainType == TerrainType.Desert || currentTerrainType == TerrainType.Dune || currentTerrainType == TerrainType.Forest || currentTerrainType == TerrainType.Snow;
				num = speed * (1f + MathF.Pow((float)mobileParty.MemberRoster.TotalManCount, 0.66f)) * (flag ? 0.25f : 0.15f);
			}
			if (mobileParty.IsCaravan)
			{
				num *= 0.5f;
			}
			if (num >= 5f)
			{
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Scouting, num, SkillEffect.PerkRole.Scout);
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000CCD88 File Offset: 0x000CAF88
		public void OnBattleEnd(PartyBase party, FlattenedTroopRoster flattenedTroopRoster)
		{
			Hero hero = party.LeaderHero ?? party.Owner;
			if (hero != null && hero.IsAlive)
			{
				Dictionary<SkillObject, float> dictionary = new Dictionary<SkillObject, float>();
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in flattenedTroopRoster)
				{
					CharacterObject troop = flattenedTroopRosterElement.Troop;
					int num;
					bool flag = MobilePartyHelper.CanTroopGainXp(party, troop, out num);
					if (!flattenedTroopRosterElement.IsKilled && flattenedTroopRosterElement.XpGained > 0 && !flag)
					{
						float num2 = (troop.Occupation == Occupation.Bandit) ? 0.05f : 0.025f;
						float num3 = (float)flattenedTroopRosterElement.XpGained * num2;
						SkillObject key = (troop.Occupation == Occupation.Bandit) ? DefaultSkills.Roguery : DefaultSkills.Leadership;
						float num4;
						if (dictionary.TryGetValue(key, out num4))
						{
							dictionary[key] = num4 + num3;
						}
						else
						{
							dictionary[key] = num3;
						}
					}
				}
				foreach (SkillObject skillObject in dictionary.Keys)
				{
					if (dictionary[skillObject] > 0f)
					{
						hero.AddSkillXp(skillObject, dictionary[skillObject]);
					}
				}
			}
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000CCEE4 File Offset: 0x000CB0E4
		public void OnFoodConsumed(MobileParty mobileParty, bool wasStarving)
		{
			if (!wasStarving && mobileParty.ItemRoster.FoodVariety > 3 && mobileParty.EffectiveQuartermaster != null)
			{
				float skillXp = (float)(MathF.Round(-mobileParty.BaseFoodChange * 100f) * (mobileParty.ItemRoster.FoodVariety - 2) / 3);
				DefaultSkillLevelingManager.OnPartySkillExercised(mobileParty, DefaultSkills.Steward, skillXp, SkillEffect.PerkRole.Quartermaster);
			}
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000CCF3C File Offset: 0x000CB13C
		public void OnAlleyCleared(Alley alley)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetInitialXpGainForMainHero());
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000CCF64 File Offset: 0x000CB164
		public void OnDailyAlleyTick(Alley alley, Hero alleyLeader)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForMainHero());
			if (alleyLeader != null && !alleyLeader.IsDead)
			{
				alleyLeader.AddSkillXp(DefaultSkills.Roguery, Campaign.Current.Models.AlleyModel.GetDailyXpGainForAssignedClanMember(alleyLeader));
			}
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000CCFC0 File Offset: 0x000CB1C0
		public void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain)
		{
			switch (difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20f);
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50f);
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 100f);
				break;
			}
			if (extraXpGain)
			{
				lord.AddSkillXp(DefaultSkills.Steward, 100f);
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000CD034 File Offset: 0x000CB234
		public void OnWarehouseProduction(EquipmentElement production)
		{
			Hero.MainHero.AddSkillXp(DefaultSkills.Trade, Campaign.Current.Models.WorkshopModel.GetTradeXpPerWarehouseProduction(production));
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000CD05A File Offset: 0x000CB25A
		private static void OnPersonalSkillExercised(Hero hero, SkillObject skill, float skillXp, bool shouldNotify = true)
		{
			if (hero != null)
			{
				hero.HeroDeveloper.AddSkillXp(skill, skillXp, true, shouldNotify);
			}
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000CD070 File Offset: 0x000CB270
		private static void OnSettlementSkillExercised(Settlement settlement, SkillObject skill, float skillXp)
		{
			Town town = settlement.Town;
			Hero hero = ((town != null) ? town.Governor : null) ?? ((settlement.OwnerClan.Leader.CurrentSettlement == settlement) ? settlement.OwnerClan.Leader : null);
			if (hero == null)
			{
				return;
			}
			hero.AddSkillXp(skill, skillXp);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000CD0C0 File Offset: 0x000CB2C0
		private static void OnGainingRidingExperience(Hero hero, float baseXpAmount, ItemObject horse)
		{
			if (horse != null)
			{
				float num = 1f + (float)horse.Difficulty * 0.02f;
				hero.AddSkillXp(DefaultSkills.Riding, baseXpAmount * num);
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000CD0F2 File Offset: 0x000CB2F2
		private static void OnPartySkillExercised(MobileParty party, SkillObject skill, float skillXp, SkillEffect.PerkRole perkRole = SkillEffect.PerkRole.PartyLeader)
		{
			Hero effectiveRoleHolder = party.GetEffectiveRoleHolder(perkRole);
			if (effectiveRoleHolder == null)
			{
				return;
			}
			effectiveRoleHolder.AddSkillXp(skill, skillXp);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000CD10F File Offset: 0x000CB30F
		void ISkillLevelingManager.OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			this.OnPrisonerSell(mobileParty, prisonerRoster);
		}

		// Token: 0x04000FA6 RID: 4006
		private const float TacticsXpCoefficient = 0.02f;
	}
}
