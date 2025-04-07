using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000374 RID: 884
	public class AgingCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060033F3 RID: 13299 RVA: 0x000D7C34 File Offset: 0x000D5E34
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.OnCharacterCreationIsOver));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.HeroReachesTeenAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroReachesTeenAge));
			CampaignEvents.HeroGrowsOutOfInfancyEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroGrowsOutOfInfancy));
			CampaignEvents.PerkOpenedEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkOpened));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000D7D10 File Offset: 0x000D5F10
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, int>>("_extraLivesContainer", ref this._extraLivesContainer);
			dataStore.SyncData<Dictionary<Hero, int>>("_heroesYoungerThanHeroComesOfAge", ref this._heroesYoungerThanHeroComesOfAge);
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000D7D38 File Offset: 0x000D5F38
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			int num = (int)hero.Age;
			if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000D7D71 File Offset: 0x000D5F71
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (this._heroesYoungerThanHeroComesOfAge.ContainsKey(victim))
			{
				this._heroesYoungerThanHeroComesOfAge.Remove(victim);
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000D7D90 File Offset: 0x000D5F90
		private void AddExtraLife(Hero hero)
		{
			if (hero.IsAlive)
			{
				if (this._extraLivesContainer.ContainsKey(hero))
				{
					Dictionary<Hero, int> extraLivesContainer = this._extraLivesContainer;
					extraLivesContainer[hero]++;
					return;
				}
				this._extraLivesContainer.Add(hero, 1);
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000D7DDC File Offset: 0x000D5FDC
		private void OnPerkOpened(Hero hero, PerkObject perk)
		{
			if (perk == DefaultPerks.Medicine.CheatDeath)
			{
				this.AddExtraLife(hero);
			}
			if (perk == DefaultPerks.Medicine.HealthAdvise)
			{
				Clan clan = hero.Clan;
				if (((clan != null) ? clan.Leader : null) == hero)
				{
					foreach (Hero hero2 in from x in hero.Clan.Heroes
					where x.IsAlive
					select x)
					{
						this.AddExtraLife(hero2);
					}
				}
			}
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000D7E80 File Offset: 0x000D6080
		private void DailyTickHero(Hero hero)
		{
			bool flag = (int)CampaignTime.Now.ToDays == this._gameStartDay;
			if (!CampaignOptions.IsLifeDeathCycleDisabled && !flag && !hero.IsTemplate)
			{
				if (hero.IsAlive && hero.CanDie(KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge))
				{
					if (hero.DeathMark != KillCharacterAction.KillCharacterActionDetail.None && (hero.PartyBelongedTo == null || (hero.PartyBelongedTo.MapEvent == null && hero.PartyBelongedTo.SiegeEvent == null)))
					{
						KillCharacterAction.ApplyByDeathMark(hero, false);
					}
					else
					{
						this.IsItTimeOfDeath(hero);
					}
				}
				int num;
				if (this._heroesYoungerThanHeroComesOfAge.TryGetValue(hero, out num))
				{
					int num2 = (int)hero.Age;
					if (num != num2)
					{
						if (num2 >= Campaign.Current.Models.AgeModel.HeroComesOfAge)
						{
							this._heroesYoungerThanHeroComesOfAge.Remove(hero);
							CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);
						}
						else
						{
							this._heroesYoungerThanHeroComesOfAge[hero] = num2;
							if (num2 == Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
							{
								CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
							}
							else if (num2 == Campaign.Current.Models.AgeModel.BecomeChildAge)
							{
								CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
							}
						}
					}
				}
				if (hero == Hero.MainHero && Hero.IsMainHeroIll && Hero.MainHero.HeroState != Hero.CharacterStates.Dead)
				{
					Campaign.Current.MainHeroIllDays++;
					if (Campaign.Current.MainHeroIllDays > 3)
					{
						Hero.MainHero.HitPoints -= MathF.Ceiling((float)Hero.MainHero.HitPoints * (0.05f * (float)Campaign.Current.MainHeroIllDays));
						if (Hero.MainHero.HitPoints <= 1 && Hero.MainHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
						{
							int num3;
							if (this._extraLivesContainer.TryGetValue(Hero.MainHero, out num3))
							{
								if (num3 == 0)
								{
									this.KillMainHeroWithIllness();
									return;
								}
								Campaign.Current.MainHeroIllDays = -1;
								this._extraLivesContainer[Hero.MainHero] = num3 - 1;
								if (this._extraLivesContainer[Hero.MainHero] == 0)
								{
									this._extraLivesContainer.Remove(Hero.MainHero);
									return;
								}
							}
							else
							{
								this.KillMainHeroWithIllness();
							}
						}
					}
				}
			}
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000D80AB File Offset: 0x000D62AB
		private void KillMainHeroWithIllness()
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Hero.MainHero.AddDeathMark(null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge);
			KillCharacterAction.ApplyByOldAge(Hero.MainHero, true);
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000D80CF File Offset: 0x000D62CF
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this.CheckYoungHeroes();
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000D80D8 File Offset: 0x000D62D8
		private void OnCharacterCreationIsOver()
		{
			this._gameStartDay = (int)CampaignTime.Now.ToDays;
			if (!CampaignOptions.IsLifeDeathCycleDisabled)
			{
				this.InitializeHeroesYoungerThanHeroComesOfAge();
			}
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000D8106 File Offset: 0x000D6306
		private void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper(true, null);
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000D8124 File Offset: 0x000D6324
		private void OnHeroReachesTeenAge(Hero hero)
		{
			MBEquipmentRoster randomElementInefficiently = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForHeroReachesTeenAge(hero).GetRandomElementInefficiently<MBEquipmentRoster>();
			if (randomElementInefficiently != null)
			{
				Equipment randomElementInefficiently2 = randomElementInefficiently.GetCivilianEquipments().GetRandomElementInefficiently<Equipment>();
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElementInefficiently2);
				new Equipment(false).FillFrom(randomElementInefficiently2, false);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElementInefficiently2);
			}
			else
			{
				Debug.FailedAssert("Cant find child equipment template for " + hero.Name, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AgingCampaignBehavior.cs", "OnHeroReachesTeenAge", 217);
			}
			if (hero.Clan != Clan.PlayerClan)
			{
				foreach (TraitObject traitObject in DefaultTraits.Personality)
				{
					int num = hero.GetTraitLevel(traitObject);
					if (hero.Father == null && hero.Mother == null && hero.Template != null)
					{
						hero.SetTraitLevel(traitObject, hero.Template.GetTraitLevel(traitObject));
					}
					else
					{
						float randomFloat = MBRandom.RandomFloat;
						float randomFloat2 = MBRandom.RandomFloat;
						if ((double)randomFloat < 0.2 && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && !hero.CharacterObject.IsFemale && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.7 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat2 < 0.3)
						{
							num--;
						}
						else if ((double)randomFloat2 >= 0.7)
						{
							num++;
						}
						num = MBMath.ClampInt(num, traitObject.MinValue, traitObject.MaxValue);
						if (num != hero.GetTraitLevel(traitObject))
						{
							hero.SetTraitLevel(traitObject, num);
						}
					}
				}
				hero.HeroDeveloper.InitializeHeroDeveloper(true, null);
			}
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D8348 File Offset: 0x000D6548
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero.HeroState != Hero.CharacterStates.Active)
			{
				bool flag = !hero.IsFemale || hero.Clan == Hero.MainHero.Clan || (hero.Mother != null && !hero.Mother.IsNoncombatant) || (hero.RandomIntWithSeed(17U, 0, 1) == 0 && hero.GetTraitLevel(DefaultTraits.Valor) == 1);
				if (hero.Clan != Clan.PlayerClan)
				{
					foreach (TraitObject trait in DefaultTraits.SkillCategories)
					{
						hero.SetTraitLevel(trait, 0);
					}
					if (flag)
					{
						hero.SetTraitLevel(DefaultTraits.CavalryFightingSkills, 5);
						int value = MathF.Max(DefaultTraits.Commander.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Valor) + hero.GetTraitLevel(DefaultTraits.Generosity) + hero.RandomIntWithSeed(18U, -1, 2));
						hero.SetTraitLevel(DefaultTraits.Commander, value);
					}
					int value2 = MathF.Max(DefaultTraits.Manager.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Honor) + hero.RandomIntWithSeed(19U, -1, 2));
					hero.SetTraitLevel(DefaultTraits.Manager, value2);
					int value3 = MathF.Max(DefaultTraits.Politician.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Calculating) + hero.RandomIntWithSeed(20U, -1, 2));
					hero.SetTraitLevel(DefaultTraits.Politician, value3);
					hero.HeroDeveloper.InitializeHeroDeveloper(true, null);
				}
				else
				{
					hero.HeroDeveloper.SetInitialLevel(hero.Level);
				}
				MBList<MBEquipmentRoster> equipmentRostersForHeroComeOfAge = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForHeroComeOfAge(hero, false);
				MBList<MBEquipmentRoster> equipmentRostersForHeroComeOfAge2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForHeroComeOfAge(hero, true);
				MBEquipmentRoster randomElement = equipmentRostersForHeroComeOfAge.GetRandomElement<MBEquipmentRoster>();
				MBEquipmentRoster randomElement2 = equipmentRostersForHeroComeOfAge2.GetRandomElement<MBEquipmentRoster>();
				Equipment randomElement3 = randomElement.AllEquipments.GetRandomElement<Equipment>();
				Equipment randomElement4 = randomElement2.AllEquipments.GetRandomElement<Equipment>();
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElement3);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElement4);
			}
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000D8548 File Offset: 0x000D6748
		private void IsItTimeOfDeath(Hero hero)
		{
			if (hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.BecomeOldAge && !CampaignOptions.IsLifeDeathCycleDisabled && hero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && MBRandom.RandomFloat < hero.ProbabilityOfDeath)
			{
				int num;
				if (this._extraLivesContainer.TryGetValue(hero, out num) && num > 0)
				{
					this._extraLivesContainer[hero] = num - 1;
					if (this._extraLivesContainer[hero] == 0)
					{
						this._extraLivesContainer.Remove(hero);
						return;
					}
				}
				else
				{
					if (hero == Hero.MainHero && !Hero.IsMainHeroIll)
					{
						Campaign.Current.MainHeroIllDays++;
						Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=2duoimiP}Caught Illness", null).ToString(), new TextObject("{=vo3MqtMn}You are at death's door, wracked by fever, drifting in and out of consciousness. The healers do not believe that you can recover. You should resolve your final affairs and determine a heir for your clan while you still have the strength to speak.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), "", null, null, "event:/ui/notification/quest_fail", 0f, null, null, null), false, false);
						return;
					}
					if (hero != Hero.MainHero)
					{
						KillCharacterAction.ApplyByOldAge(hero, true);
					}
				}
			}
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000D8678 File Offset: 0x000D6878
		private void MainHeroHealCheck()
		{
			if (MBRandom.RandomFloat <= 0.05f && Hero.MainHero.IsAlive)
			{
				Campaign.Current.MainHeroIllDays = -1;
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=M5eUjgQl}Cured", null).ToString(), new TextObject("{=T5H3L9Kw}The fever has broken. You are weak but you feel you will recover. You rise from your bed from the first time in days, blinking in the sunlight.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), "", null, null, "event:/ui/notification/quest_finished", 0f, null, null, null), false, false);
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			}
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000D8708 File Offset: 0x000D6908
		private void InitializeHeroesYoungerThanHeroComesOfAge()
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				int num = (int)hero.Age;
				if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (!hero2.IsDead && hero2.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero2))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero2, (int)hero2.Age);
				}
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000D8810 File Offset: 0x000D6A10
		private void CheckYoungHeroes()
		{
			foreach (Hero hero in Hero.FindAll((Hero x) => !x.IsDead && x.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(x)))
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, (int)hero.Age);
				if (!hero.IsDisabled && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeChildAge)
					{
						CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
					}
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
					{
						CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
					}
				}
			}
		}

		// Token: 0x040010EF RID: 4335
		private Dictionary<Hero, int> _extraLivesContainer = new Dictionary<Hero, int>();

		// Token: 0x040010F0 RID: 4336
		private Dictionary<Hero, int> _heroesYoungerThanHeroComesOfAge = new Dictionary<Hero, int>();

		// Token: 0x040010F1 RID: 4337
		private int _gameStartDay;
	}
}
