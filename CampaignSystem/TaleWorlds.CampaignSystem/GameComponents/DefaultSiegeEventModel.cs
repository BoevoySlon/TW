using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013B RID: 315
	public class DefaultSiegeEventModel : SiegeEventModel
	{
		// Token: 0x060017C0 RID: 6080 RVA: 0x000761F0 File Offset: 0x000743F0
		public override string GetSiegeEngineMapPrefabName(SiegeEngineType type, int wallLevel, BattleSideEnum side)
		{
			string result = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				result = "mangonel_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				result = "mangonel_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				result = "mangonel_a_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				result = "mangonel_b_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon" : "ballista_b_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon" : "ballista_b_fire_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				result = "trebuchet_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				result = "trebuchet_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ram)
			{
				result = "batteringram_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.SiegeTower)
			{
				switch (wallLevel)
				{
				case 1:
					result = "siegetower_5m_mapicon";
					break;
				case 2:
					result = "siegetower_9m_mapicon";
					break;
				case 3:
					result = "siegetower_12m_mapicon";
					break;
				}
			}
			return result;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000762E8 File Offset: 0x000744E8
		public override string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType type)
		{
			string result = null;
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.Trebuchet || type == DefaultSiegeEngineTypes.Bricole)
			{
				result = "mangonel_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				result = "mangonel_fire_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				result = "ballista_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				result = "ballista_fire_mapicon_projectile";
			}
			return result;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00076358 File Offset: 0x00074558
		public override string GetSiegeEngineMapReloadAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string result = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				result = "mangonel_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				result = "mangonel_b_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				result = "mangonel_a_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				result = "mangonel_b_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_reload" : "ballista_b_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_reload" : "ballista_b_fire_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				result = "trebuchet_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				result = "trebuchet_b_mapicon_reload";
			}
			return result;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00076400 File Offset: 0x00074600
		public override string GetSiegeEngineMapFireAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string result = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				result = "mangonel_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				result = "mangonel_b_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				result = "mangonel_a_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				result = "mangonel_b_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_fire" : "ballista_b_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				result = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_fire" : "ballista_b_fire_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				result = "trebuchet_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				result = "trebuchet_b_mapicon_fire";
			}
			return result;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x000764A8 File Offset: 0x000746A8
		public override sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType type, BattleSideEnum side)
		{
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.FireOnager)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Ballista || type == DefaultSiegeEngineTypes.FireBallista)
			{
				return 7;
			}
			if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				return 4;
			}
			if (type == DefaultSiegeEngineTypes.Bricole)
			{
				return 20;
			}
			return -1;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00076504 File Offset: 0x00074704
		public override MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum battleSide)
		{
			MobileParty result = null;
			if (battleSide == BattleSideEnum.Attacker)
			{
				result = siegeEvent.BesiegerCamp.LeaderParty;
			}
			else
			{
				int num = 0;
				int num2 = -1;
				for (PartyBase nextInvolvedPartyForEventType = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege); nextInvolvedPartyForEventType != null; nextInvolvedPartyForEventType = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege))
				{
					if (nextInvolvedPartyForEventType.LeaderHero != null)
					{
						Hero effectiveEngineer = nextInvolvedPartyForEventType.MobileParty.EffectiveEngineer;
						int num3 = (effectiveEngineer != null) ? effectiveEngineer.GetSkillValue(DefaultSkills.Engineering) : 0;
						if (num3 > num)
						{
							num = num3;
							result = nextInvolvedPartyForEventType.MobileParty;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00076584 File Offset: 0x00074784
		public override float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side)
		{
			float num = 1f;
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Engineering.CampBuilding, true))
			{
				num += DefaultPerks.Engineering.CampBuilding.SecondaryBonus;
			}
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, true))
			{
				num -= DefaultPerks.Medicine.SiegeMedic.SecondaryBonus;
			}
			if (side == BattleSideEnum.Defender)
			{
				Town town = siegeEvent.BesiegedSettlement.Town;
				if (((town != null) ? town.Governor : null) != null && siegeEvent.BesiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Medicine.BattleHardened))
				{
					num += DefaultPerks.Medicine.BattleHardened.SecondaryBonus;
				}
			}
			return num;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00076619 File Offset: 0x00074819
		public override int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine)
		{
			return 2;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0007661C File Offset: 0x0007481C
		public override int GetColleteralDamageCasualties(SiegeEngineType siegeEngineType, MobileParty party)
		{
			int num = 1;
			if (party != null && party.HasPerk(DefaultPerks.Crossbow.Terror, false) && MBRandom.RandomFloat < DefaultPerks.Crossbow.Terror.PrimaryBonus)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00076654 File Offset: 0x00074854
		public override float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town)
		{
			float baseNumber;
			if (target - SiegeBombardTargets.Wall > 1)
			{
				if (target != SiegeBombardTargets.People)
				{
					throw new ArgumentOutOfRangeException("target", target, null);
				}
				baseNumber = siegeEngineType.AntiPersonnelHitChance;
			}
			else
			{
				baseNumber = siegeEngineType.HitChance;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(baseNumber, false, null);
			if (battleSide == BattleSideEnum.Attacker && target == SiegeBombardTargets.RangedEngines)
			{
				float num = 0f;
				switch (town.GetWallLevel())
				{
				case 1:
					num = 0.05f;
					break;
				case 2:
					num = 0.1f;
					break;
				case 3:
					num = 0.15f;
					break;
				}
				explainedNumber.Add(-num, new TextObject("{=b9NaTqyr}Extra Defender Defense", null), null);
			}
			if (battleSide == BattleSideEnum.Defender)
			{
				if (target == SiegeBombardTargets.RangedEngines && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.DreadfulSieger))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.DreadfulSieger.PrimaryBonus, DefaultPerks.Engineering.DreadfulSieger.Name);
				}
				if (siegeEngineType == DefaultSiegeEngineTypes.Ballista)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.Pavise, town, ref explainedNumber);
				}
			}
			SiegeEvent siegeEvent = town.Settlement.SiegeEvent;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			MobileParty effectiveSiegePartyForSide2 = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide.GetOppositeSide());
			if (effectiveSiegePartyForSide != null)
			{
				if ((siegeEngineType == DefaultSiegeEngineTypes.Trebuchet || siegeEngineType == DefaultSiegeEngineTypes.Onager || siegeEngineType == DefaultSiegeEngineTypes.FireOnager) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Foreman, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Foreman.PrimaryBonus, DefaultPerks.Engineering.Foreman.Name);
				}
				if ((siegeEngineType == DefaultSiegeEngineTypes.Ballista || siegeEngineType == DefaultSiegeEngineTypes.FireBallista) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Salvager, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Salvager.PrimaryBonus, DefaultPerks.Engineering.Salvager.Name);
				}
			}
			if (battleSide == BattleSideEnum.Defender && effectiveSiegePartyForSide2 != null && target == SiegeBombardTargets.RangedEngines && effectiveSiegePartyForSide2.HasPerk(DefaultPerks.Engineering.DungeonArchitect, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.DungeonArchitect.PrimaryBonus, DefaultPerks.Engineering.DungeonArchitect.Name);
			}
			if (explainedNumber.ResultNumber < 0f)
			{
				explainedNumber = new ExplainedNumber(0f, false, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00076850 File Offset: 0x00074A50
		public override float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy)
		{
			if (strategy == DefaultSiegeStrategies.PreserveStrength)
			{
				return -9000f;
			}
			if (strategy != DefaultSiegeStrategies.Custom)
			{
				return MBRandom.RandomFloat;
			}
			if (siege == PlayerSiege.PlayerSiegeEvent && side == PlayerSiege.PlayerSide && siege.BesiegerCamp != null && siege.BesiegerCamp.LeaderParty == MobileParty.MainParty)
			{
				return 9000f;
			}
			return -100f;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000768B0 File Offset: 0x00074AB0
		public override float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			float availableManDayPower = this.GetAvailableManDayPower(side);
			float num = (float)type.ManDayCost;
			explainedNumber.Add(1f / (num / availableManDayPower * 24f), this._baseConstructionSpeedText, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side.BattleSide);
			if (effectiveSiegePartyForSide != null)
			{
				int? num2;
				if (effectiveSiegePartyForSide == null)
				{
					num2 = null;
				}
				else
				{
					Hero effectiveEngineer = effectiveSiegePartyForSide.EffectiveEngineer;
					num2 = ((effectiveEngineer != null) ? new int?(effectiveEngineer.GetSkillValue(DefaultSkills.Engineering)) : null);
				}
				if ((num2 ?? 0) > 0)
				{
					SkillHelper.AddSkillBonusForParty(DefaultSkills.Engineering, DefaultSkillEffects.SiegeEngineProductionBonus, effectiveSiegePartyForSide, ref explainedNumber);
				}
			}
			float num3 = 0f;
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				foreach (Building building in siegeEvent.BesiegedSettlement.Town.Buildings)
				{
					num3 += building.GetBuildingEffectAmount(BuildingEffectEnum.SiegeEngineSpeed);
				}
				explainedNumber.AddFactor(num3 * 0.01f, this._constructionSpeedProjectBonusText);
				Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
				if (((governor != null) ? governor.CurrentSettlement : null) != null && governor.CurrentSettlement == siegeEvent.BesiegedSettlement)
				{
					SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Engineering, DefaultSkillEffects.SiegeEngineProductionBonus, governor.CharacterObject, ref explainedNumber, -1, true, 0);
				}
			}
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty : null) != null && siegeEvent.BesiegerCamp.LeaderParty.HasPerk(DefaultPerks.Steward.Sweatshops, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Steward.Sweatshops.SecondaryBonus, null);
			}
			if (effectiveSiegePartyForSide != null)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegePreparations = side.SiegeEngines.SiegePreparations;
				if (siegePreparations != null && !siegePreparations.IsConstructed && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.ImprovedTools, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.ImprovedTools.PrimaryBonus, DefaultPerks.Engineering.ImprovedTools.Name);
				}
				else
				{
					PerkObject perkObject = type.IsRanged ? DefaultPerks.Engineering.TorsionEngines : DefaultPerks.Engineering.Scaffolds;
					if (effectiveSiegePartyForSide.HasPerk(perkObject, false))
					{
						explainedNumber.AddFactor(perkObject.PrimaryBonus, perkObject.Name);
					}
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
				PerkObject salvager = DefaultPerks.Engineering.Salvager;
				if (PerkHelper.GetPerkValueForTown(salvager, besiegedSettlement.Town))
				{
					explainedNumber.AddFactor(salvager.SecondaryBonus * besiegedSettlement.Militia, salvager.Name);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00076B3C File Offset: 0x00074D3C
		public override float GetAvailableManDayPower(ISiegeEventSide side)
		{
			int num = -1;
			PartyBase nextInvolvedPartyForEventType = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			int num2 = 0;
			while (nextInvolvedPartyForEventType != null)
			{
				num2 += nextInvolvedPartyForEventType.NumberOfHealthyMembers;
				nextInvolvedPartyForEventType = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			}
			return MathF.Sqrt((float)num2);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00076B78 File Offset: 0x00074D78
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				Building building2 = town.Buildings.Find((Building building) => building.BuildingType == DefaultBuildingTypes.SettlementSiegeWorkshop);
				if (building2 != null)
				{
					switch (building2.CurrentLevel)
					{
					case 0:
						goto IL_B7;
					case 1:
						goto IL_91;
					case 2:
						break;
					case 3:
						list.Add(DefaultSiegeEngineTypes.Ballista);
						list.Add(DefaultSiegeEngineTypes.Catapult);
						break;
					default:
						Debug.FailedAssert("Invalid building level", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSiegeEventCalculationModel.cs", "GetPrebuiltSiegeEnginesOfSettlement", 568);
						goto IL_B7;
					}
					list.Add(DefaultSiegeEngineTypes.Ballista);
					list.Add(DefaultSiegeEngineTypes.Catapult);
					IL_91:
					list.Add(DefaultSiegeEngineTypes.Ballista);
				}
				IL_B7:
				if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeWorks))
				{
					list.Add(DefaultSiegeEngineTypes.Catapult);
				}
			}
			return list;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00076C64 File Offset: 0x00074E64
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp besiegerCamp)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			if (besiegerCamp.LeaderParty.HasPerk(DefaultPerks.Engineering.Battlements, false))
			{
				list.Add(DefaultSiegeEngineTypes.Ballista);
			}
			return list;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00076C98 File Offset: 0x00074E98
		public override float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.BaseHitPoints, false, null);
			Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (battleSide == BattleSideEnum.Defender && besiegedSettlement.Town.Governor != null && besiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeEngineer))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.SiegeEngineer.PrimaryBonus, DefaultPerks.Engineering.SiegeEngineer.Name);
			}
			if (siegeEngine.IsRanged)
			{
				if (effectiveSiegePartyForSide != null && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.SiegeWorks, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.SiegeWorks.PrimaryBonus, DefaultPerks.Engineering.SiegeWorks.Name);
				}
			}
			else if (battleSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide != null && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Carpenters, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.Carpenters.PrimaryBonus, DefaultPerks.Engineering.Carpenters.Name);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00076D74 File Offset: 0x00074F74
		public override float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.Damage, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (effectiveSiegePartyForSide != null)
			{
				if (battleSide == BattleSideEnum.Attacker)
				{
					if (target == SiegeBombardTargets.Wall && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.WallBreaker, false))
					{
						explainedNumber.AddFactor(DefaultPerks.Engineering.WallBreaker.PrimaryBonus, DefaultPerks.Engineering.WallBreaker.Name);
					}
					if (target == SiegeBombardTargets.RangedEngines && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Tactics.MakeThemPay, false))
					{
						explainedNumber.AddFactor(DefaultPerks.Tactics.MakeThemPay.PrimaryBonus, DefaultPerks.Tactics.MakeThemPay.Name);
					}
				}
				if ((target == SiegeBombardTargets.RangedEngines || target == SiegeBombardTargets.Wall) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Masterwork, false))
				{
					int num = effectiveSiegePartyForSide.LeaderHero.GetSkillValue(DefaultSkills.Engineering) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
					if (num > 0)
					{
						float value = (float)num * DefaultPerks.Engineering.Masterwork.PrimaryBonus;
						explainedNumber.AddFactor(value, DefaultPerks.Engineering.Masterwork.Name);
					}
				}
			}
			if (battleSide == BattleSideEnum.Defender && target == SiegeBombardTargets.RangedEngines)
			{
				Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
				if (governor != null && governor.GetPerkValue(DefaultPerks.Tactics.MakeThemPay))
				{
					explainedNumber.AddFactor(DefaultPerks.Tactics.MakeThemPay.SecondaryBonus, DefaultPerks.Tactics.MakeThemPay.Name);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00076EB0 File Offset: 0x000750B0
		public override int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(siegeEngine.CampaignRateOfFirePerDay, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side);
			if (effectiveSiegePartyForSide != null)
			{
				if ((siegeEngine == DefaultSiegeEngineTypes.Ballista || siegeEngine == DefaultSiegeEngineTypes.FireBallista) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Clockwork, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Clockwork.PrimaryBonus, DefaultPerks.Engineering.Clockwork.Name);
				}
				else if ((siegeEngine == DefaultSiegeEngineTypes.Onager || siegeEngine == DefaultSiegeEngineTypes.Trebuchet || siegeEngine == DefaultSiegeEngineTypes.FireOnager) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.ArchitecturalCommisions, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.ArchitecturalCommisions.PrimaryBonus, DefaultPerks.Engineering.ArchitecturalCommisions.Name);
				}
			}
			return MathF.Round(1440f / explainedNumber.ResultNumber);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00076F66 File Offset: 0x00075166
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party)
		{
			bool hasFirePerks = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, true) || party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, true);
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Onager;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireOnager;
			}
			yield return DefaultSiegeEngineTypes.Trebuchet;
			yield break;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00076F76 File Offset: 0x00075176
		public override IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party)
		{
			bool hasFirePerks = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, true) || party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, true);
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Catapult;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireCatapult;
			}
			yield break;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00076F86 File Offset: 0x00075186
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.Ram;
			yield break;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00076F8F File Offset: 0x0007518F
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.SiegeTower;
			yield break;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00076F98 File Offset: 0x00075198
		public override FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush()
		{
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (TroopRosterElement troop in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				if (this.IsPriorityTroopForSallyOutAmbush(troop))
				{
					flattenedTroopRoster.Add(troop);
				}
			}
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			if (playerSiegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty != null && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.Count > 0)
			{
				foreach (TroopRosterElement troop2 in playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					if (this.IsPriorityTroopForSallyOutAmbush(troop2))
					{
						flattenedTroopRoster.Add(troop2);
					}
				}
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				foreach (PartyBase partyBase in playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
				{
					if (partyBase != PartyBase.MainParty)
					{
						foreach (TroopRosterElement troop3 in partyBase.MemberRoster.GetTroopRoster())
						{
							if (this.IsPriorityTroopForSallyOutAmbush(troop3))
							{
								flattenedTroopRoster.Add(troop3);
							}
						}
					}
				}
			}
			return flattenedTroopRoster;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00077178 File Offset: 0x00075378
		private bool IsPriorityTroopForSallyOutAmbush(TroopRosterElement troop)
		{
			CharacterObject character = troop.Character;
			return character.IsHero || character.HasMount();
		}

		// Token: 0x04000866 RID: 2150
		private readonly TextObject _baseConstructionSpeedText = new TextObject("{=MhGbcXJ4}Base construction speed", null);

		// Token: 0x04000867 RID: 2151
		private readonly TextObject _constructionSpeedProjectBonusText = new TextObject("{=xoTWC8Sm}Project Bonus", null);

		// Token: 0x04000868 RID: 2152
		private readonly TextObject _weatherConstructionPenalty = new TextObject("{=J6RjCKbk}Weather", null);
	}
}
