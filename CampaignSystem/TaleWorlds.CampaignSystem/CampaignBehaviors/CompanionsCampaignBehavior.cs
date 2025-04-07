using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000382 RID: 898
	public class CompanionsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06003505 RID: 13573 RVA: 0x000E4005 File Offset: 0x000E2205
		private float _desiredTotalCompanionCount
		{
			get
			{
				return (float)Town.AllTowns.Count * 0.6f;
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000E4018 File Offset: 0x000E2218
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroOccupationChangedEvent.AddNonSerializedListener(this, new Action<Hero, Occupation>(this.OnHeroOccupationChanged));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000E40C8 File Offset: 0x000E22C8
		private void OnGameLoadFinished()
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000E417C File Offset: 0x000E237C
		private void DailyTick()
		{
			this.TryKillCompanion();
			this.SwapCompanions();
			this.TrySpawnNewCompanion();
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000E4190 File Offset: 0x000E2390
		private void WeeklyTick()
		{
			foreach (Hero hero in Hero.DeadOrDisabledHeroes.ToList<Hero>())
			{
				if (hero.IsWanderer && hero.DeathDay.ElapsedDaysUntilNow >= 40f)
				{
					Campaign.Current.CampaignObjectManager.UnregisterDeadHero(hero);
				}
			}
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000E4210 File Offset: 0x000E2410
		private void RemoveFromAliveCompanions(Hero companion)
		{
			CharacterObject template = companion.Template;
			if (this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Remove(template);
			}
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000E4240 File Offset: 0x000E2440
		private void AddToAliveCompanions(Hero companion, bool isTemplateControlled = false)
		{
			CharacterObject template = companion.Template;
			bool flag = true;
			if (!isTemplateControlled)
			{
				flag = this.IsTemplateKnown(template);
			}
			if (flag && !this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Add(template);
			}
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000E427F File Offset: 0x000E247F
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.RemoveFromAliveCompanions(victim);
			if (victim.IsWanderer && !victim.HasMet)
			{
				Campaign.Current.CampaignObjectManager.UnregisterDeadHero(victim);
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000E42A8 File Offset: 0x000E24A8
		private void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			if (oldOccupation == Occupation.Wanderer)
			{
				this.RemoveFromAliveCompanions(hero);
				return;
			}
			if (hero.Occupation == Occupation.Wanderer)
			{
				this.AddToAliveCompanions(hero, false);
			}
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000E42C9 File Offset: 0x000E24C9
		private void OnHeroCreated(Hero hero, bool showNotification = true)
		{
			if (hero.IsAlive && hero.IsWanderer)
			{
				this.AddToAliveCompanions(hero, true);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000E42E4 File Offset: 0x000E24E4
		private void TryKillCompanion()
		{
			if (MBRandom.RandomFloat <= 0.1f && this._aliveCompanionTemplates.Count > 0)
			{
				CharacterObject randomElementInefficiently = this._aliveCompanionTemplates.GetRandomElementInefficiently<CharacterObject>();
				Hero hero = null;
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (hero2.Template == randomElementInefficiently && hero2.IsWanderer)
					{
						hero = hero2;
						break;
					}
				}
				if (hero != null && hero.CompanionOf == null && (hero.CurrentSettlement == null || hero.CurrentSettlement != Hero.MainHero.CurrentSettlement))
				{
					KillCharacterAction.ApplyByRemove(hero, false, true);
				}
			}
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000E439C File Offset: 0x000E259C
		private void TrySpawnNewCompanion()
		{
			if ((float)this._aliveCompanionTemplates.Count < this._desiredTotalCompanionCount)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate(delegate(Town x)
				{
					if (x.Settlement != Hero.MainHero.CurrentSettlement && x.Settlement.SiegeEvent == null)
					{
						return x.Settlement.HeroesWithoutParty.AllQ((Hero y) => !y.IsWanderer || y.CompanionOf != null);
					}
					return false;
				});
				Settlement settlement = (randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null;
				if (settlement != null)
				{
					this.CreateCompanionAndAddToSettlement(settlement);
				}
			}
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000E4400 File Offset: 0x000E2600
		private void SwapCompanions()
		{
			int num = Town.AllTowns.Count / 2;
			int num2 = MBRandom.RandomInt(Town.AllTowns.Count % 2);
			Town town = Town.AllTowns[num2 + MBRandom.RandomInt(num)];
			Hero hero = (from x in town.Settlement.HeroesWithoutParty
			where x.IsWanderer && x.CompanionOf == null
			select x).GetRandomElementInefficiently<Hero>();
			for (int i = 1; i < 2; i++)
			{
				Town town2 = Town.AllTowns[i * num + num2 + MBRandom.RandomInt(num)];
				IEnumerable<Hero> enumerable = from x in town2.Settlement.HeroesWithoutParty
				where x.IsWanderer && x.CompanionOf == null
				select x;
				Hero hero2 = null;
				if (enumerable.Any<Hero>())
				{
					hero2 = enumerable.GetRandomElementInefficiently<Hero>();
					LeaveSettlementAction.ApplyForCharacterOnly(hero2);
				}
				if (hero != null)
				{
					EnterSettlementAction.ApplyForCharacterOnly(hero, town2.Settlement);
				}
				hero = hero2;
			}
			if (hero != null)
			{
				EnterSettlementAction.ApplyForCharacterOnly(hero, town.Settlement);
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000E4513 File Offset: 0x000E2713
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000E4518 File Offset: 0x000E2718
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			this.InitializeCompanionTemplateList();
			List<Town> list = Town.AllTowns.ToListQ<Town>();
			list.Shuffle<Town>();
			int num = 0;
			while ((float)num < this._desiredTotalCompanionCount)
			{
				this.CreateCompanionAndAddToSettlement(list[num].Settlement);
				num++;
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000E4560 File Offset: 0x000E2760
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000E4614 File Offset: 0x000E2814
		private void AdjustEquipment(Hero hero)
		{
			this.AdjustEquipmentImp(hero.BattleEquipment);
			this.AdjustEquipmentImp(hero.CivilianEquipment);
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000E4630 File Offset: 0x000E2830
		private void AdjustEquipmentImp(Equipment equipment)
		{
			ItemModifier @object = MBObjectManager.Instance.GetObject<ItemModifier>("companion_armor");
			ItemModifier object2 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_weapon");
			ItemModifier object3 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_horse");
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				EquipmentElement equipmentElement = equipment[equipmentIndex];
				if (equipmentElement.Item != null)
				{
					if (equipmentElement.Item.ArmorComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, @object, null, false);
					}
					else if (equipmentElement.Item.HorseComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object3, null, false);
					}
					else if (equipmentElement.Item.WeaponComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object2, null, false);
					}
				}
			}
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000E4704 File Offset: 0x000E2904
		private void InitializeCompanionTemplateList()
		{
			foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
			{
				foreach (CharacterObject characterObject in cultureObject.NotableAndWandererTemplates)
				{
					if (characterObject.Occupation == Occupation.Wanderer)
					{
						this._companionsOfTemplates[this.GetTemplateTypeOfCompanion(characterObject)].Add(characterObject);
					}
				}
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000E47B0 File Offset: 0x000E29B0
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeOfCompanion(CharacterObject character)
		{
			CompanionsCampaignBehavior.CompanionTemplateType result = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			int num = 20;
			foreach (SkillObject skill in Skills.All)
			{
				int skillValue = character.GetSkillValue(skill);
				if (skillValue > num)
				{
					CompanionsCampaignBehavior.CompanionTemplateType templateTypeForSkill = this.GetTemplateTypeForSkill(skill);
					if (templateTypeForSkill != CompanionsCampaignBehavior.CompanionTemplateType.Combat)
					{
						num = skillValue;
						result = templateTypeForSkill;
					}
				}
			}
			foreach (Tuple<SkillObject, int> tuple in Campaign.Current.Models.CharacterDevelopmentModel.GetSkillsDerivedFromTraits(null, character, false))
			{
				int item = tuple.Item2;
				if (item > num)
				{
					CompanionsCampaignBehavior.CompanionTemplateType templateTypeForSkill2 = this.GetTemplateTypeForSkill(tuple.Item1);
					if (templateTypeForSkill2 != CompanionsCampaignBehavior.CompanionTemplateType.Combat)
					{
						num = item;
						result = templateTypeForSkill2;
					}
				}
			}
			return result;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000E489C File Offset: 0x000E2A9C
		private void CreateCompanionAndAddToSettlement(Settlement settlement)
		{
			CharacterObject companionTemplate = this.GetCompanionTemplateToSpawn();
			if (companionTemplate != null)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate((Town x) => x.Culture == companionTemplate.Culture);
				Settlement settlement2 = (randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null;
				if (settlement2 == null)
				{
					settlement2 = Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				Hero hero = HeroCreator.CreateSpecialHero(companionTemplate, settlement2, null, null, Campaign.Current.Models.AgeModel.HeroComesOfAge + 5 + MBRandom.RandomInt(27));
				this.AdjustEquipment(hero);
				hero.ChangeState(Hero.CharacterStates.Active);
				EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000E493C File Offset: 0x000E2B3C
		private CompanionsCampaignBehavior.CompanionTemplateType GetCompanionTemplateTypeToSpawn()
		{
			CompanionsCampaignBehavior.CompanionTemplateType result = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			float num = -1f;
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				float templateTypeScore = this.GetTemplateTypeScore(keyValuePair.Key);
				if (templateTypeScore > 0f)
				{
					int num2 = 0;
					foreach (CharacterObject item in keyValuePair.Value)
					{
						if (this._aliveCompanionTemplates.Contains(item))
						{
							num2++;
						}
					}
					float num3 = (float)num2 / this._desiredTotalCompanionCount;
					float num4 = (templateTypeScore - num3) / templateTypeScore;
					if (num2 < keyValuePair.Value.Count && num4 > num)
					{
						num = num4;
						result = keyValuePair.Key;
					}
				}
			}
			return result;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000E4A3C File Offset: 0x000E2C3C
		private bool IsTemplateKnown(CharacterObject companionTemplate)
		{
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					if (companionTemplate == keyValuePair.Value[i])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000E4AB0 File Offset: 0x000E2CB0
		private CharacterObject GetCompanionTemplateToSpawn()
		{
			List<CharacterObject> list = this._companionsOfTemplates[this.GetCompanionTemplateTypeToSpawn()];
			list.Shuffle<CharacterObject>();
			CharacterObject result = null;
			foreach (CharacterObject characterObject in list)
			{
				if (!this._aliveCompanionTemplates.Contains(characterObject))
				{
					result = characterObject;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000E4B24 File Offset: 0x000E2D24
		private float GetTemplateTypeScore(CompanionsCampaignBehavior.CompanionTemplateType templateType)
		{
			switch (templateType)
			{
			case CompanionsCampaignBehavior.CompanionTemplateType.Engineering:
				return 0.05882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Tactics:
				return 0.11764706f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Leadership:
				return 0.0882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Steward:
				return 0.0882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Trade:
				return 0.0882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Roguery:
				return 0.11764706f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Medicine:
				return 0.0882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Smithing:
				return 0.05882353f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Scouting:
				return 0.14705883f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Combat:
				return 0.14705883f;
			default:
				return 0f;
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000E4BA4 File Offset: 0x000E2DA4
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeForSkill(SkillObject skill)
		{
			CompanionsCampaignBehavior.CompanionTemplateType result = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			if (skill == DefaultSkills.Engineering)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Engineering;
			}
			else if (skill == DefaultSkills.Tactics)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Tactics;
			}
			else if (skill == DefaultSkills.Leadership)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Leadership;
			}
			else if (skill == DefaultSkills.Steward)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Steward;
			}
			else if (skill == DefaultSkills.Trade)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Trade;
			}
			else if (skill == DefaultSkills.Roguery)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Roguery;
			}
			else if (skill == DefaultSkills.Medicine)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Medicine;
			}
			else if (skill == DefaultSkills.Crafting)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Smithing;
			}
			else if (skill == DefaultSkills.Scouting)
			{
				result = CompanionsCampaignBehavior.CompanionTemplateType.Scouting;
			}
			return result;
		}

		// Token: 0x04001118 RID: 4376
		private const int CompanionMoveRandomIndex = 2;

		// Token: 0x04001119 RID: 4377
		private const float DesiredCompanionPerTown = 0.6f;

		// Token: 0x0400111A RID: 4378
		private const float KillChance = 0.1f;

		// Token: 0x0400111B RID: 4379
		private const int SkillThresholdValue = 20;

		// Token: 0x0400111C RID: 4380
		private const int RemoveWandererAfterDays = 40;

		// Token: 0x0400111D RID: 4381
		private IReadOnlyDictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> _companionsOfTemplates = new Dictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>>
		{
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Engineering,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Tactics,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Leadership,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Steward,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Trade,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Roguery,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Medicine,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Smithing,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Scouting,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Combat,
				new List<CharacterObject>()
			}
		};

		// Token: 0x0400111E RID: 4382
		private HashSet<CharacterObject> _aliveCompanionTemplates = new HashSet<CharacterObject>();

		// Token: 0x0400111F RID: 4383
		private const float EngineerScore = 2f;

		// Token: 0x04001120 RID: 4384
		private const float TacticsScore = 4f;

		// Token: 0x04001121 RID: 4385
		private const float LeadershipScore = 3f;

		// Token: 0x04001122 RID: 4386
		private const float StewardScore = 3f;

		// Token: 0x04001123 RID: 4387
		private const float TradeScore = 3f;

		// Token: 0x04001124 RID: 4388
		private const float RogueryScore = 4f;

		// Token: 0x04001125 RID: 4389
		private const float MedicineScore = 3f;

		// Token: 0x04001126 RID: 4390
		private const float SmithingScore = 2f;

		// Token: 0x04001127 RID: 4391
		private const float ScoutingScore = 5f;

		// Token: 0x04001128 RID: 4392
		private const float CombatScore = 5f;

		// Token: 0x04001129 RID: 4393
		private const float AllScore = 34f;

		// Token: 0x020006D3 RID: 1747
		private enum CompanionTemplateType
		{
			// Token: 0x04001C31 RID: 7217
			Engineering,
			// Token: 0x04001C32 RID: 7218
			Tactics,
			// Token: 0x04001C33 RID: 7219
			Leadership,
			// Token: 0x04001C34 RID: 7220
			Steward,
			// Token: 0x04001C35 RID: 7221
			Trade,
			// Token: 0x04001C36 RID: 7222
			Roguery,
			// Token: 0x04001C37 RID: 7223
			Medicine,
			// Token: 0x04001C38 RID: 7224
			Smithing,
			// Token: 0x04001C39 RID: 7225
			Scouting,
			// Token: 0x04001C3A RID: 7226
			Combat
		}
	}
}
