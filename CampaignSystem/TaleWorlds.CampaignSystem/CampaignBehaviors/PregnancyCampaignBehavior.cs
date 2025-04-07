using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C7 RID: 967
	public class PregnancyCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B5C RID: 15196 RVA: 0x0011A504 File Offset: 0x00118704
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.ChildConceived));
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0011A558 File Offset: 0x00118758
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsFemale && !CampaignOptions.IsLifeDeathCycleDisabled && hero.IsAlive && hero.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (hero.Clan == null || !hero.Clan.IsRebelClan))
			{
				if (hero.Age > 18f && hero.Spouse != null && hero.Spouse.IsAlive && !hero.IsPregnant)
				{
					this.RefreshSpouseVisit(hero);
				}
				if (hero.IsPregnant)
				{
					this.CheckOffspringsToDeliver(hero);
				}
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0011A5F4 File Offset: 0x001187F4
		private void CheckOffspringsToDeliver(Hero hero)
		{
			PregnancyCampaignBehavior.Pregnancy pregnancy = this._heroPregnancies.Find((PregnancyCampaignBehavior.Pregnancy x) => x.Mother == hero);
			if (pregnancy == null)
			{
				hero.IsPregnant = false;
				return;
			}
			this.CheckOffspringsToDeliver(pregnancy);
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0011A63D File Offset: 0x0011883D
		private void RefreshSpouseVisit(Hero hero)
		{
			if (this.CheckAreNearby(hero, hero.Spouse) && MBRandom.RandomFloat <= Campaign.Current.Models.PregnancyModel.GetDailyChanceOfPregnancyForHero(hero))
			{
				MakePregnantAction.Apply(hero);
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0011A670 File Offset: 0x00118870
		private bool CheckAreNearby(Hero hero, Hero spouse)
		{
			Settlement settlement;
			MobileParty mobileParty;
			this.GetLocation(hero, out settlement, out mobileParty);
			Settlement settlement2;
			MobileParty mobileParty2;
			this.GetLocation(spouse, out settlement2, out mobileParty2);
			return (settlement != null && settlement == settlement2) || (mobileParty != null && mobileParty == mobileParty2) || (hero.Clan != Hero.MainHero.Clan && MBRandom.RandomFloat < 0.2f);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0011A6C4 File Offset: 0x001188C4
		private void GetLocation(Hero hero, out Settlement heroSettlement, out MobileParty heroParty)
		{
			heroSettlement = hero.CurrentSettlement;
			heroParty = hero.PartyBelongedTo;
			MobileParty mobileParty = heroParty;
			if (((mobileParty != null) ? mobileParty.AttachedTo : null) != null)
			{
				heroParty = heroParty.AttachedTo;
			}
			if (heroSettlement == null)
			{
				MobileParty mobileParty2 = heroParty;
				heroSettlement = ((mobileParty2 != null) ? mobileParty2.CurrentSettlement : null);
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x0011A704 File Offset: 0x00118904
		private void CheckOffspringsToDeliver(PregnancyCampaignBehavior.Pregnancy pregnancy)
		{
			PregnancyModel pregnancyModel = Campaign.Current.Models.PregnancyModel;
			if (!pregnancy.DueDate.IsFuture && pregnancy.Mother.IsAlive)
			{
				Hero mother = pregnancy.Mother;
				bool flag = MBRandom.RandomFloat <= pregnancyModel.DeliveringTwinsProbability;
				List<Hero> list = new List<Hero>();
				int num = flag ? 2 : 1;
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					if (MBRandom.RandomFloat > pregnancyModel.StillbirthProbability)
					{
						bool isOffspringFemale = MBRandom.RandomFloat <= pregnancyModel.DeliveringFemaleOffspringProbability;
						Hero item = HeroCreator.DeliverOffSpring(mother, pregnancy.Father, isOffspringFemale);
						list.Add(item);
					}
					else
					{
						TextObject textObject = new TextObject("{=pw4cUPEn}{MOTHER.LINK} has delivered stillborn.", null);
						StringHelpers.SetCharacterProperties("MOTHER", mother.CharacterObject, textObject, false);
						InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
						num2++;
					}
				}
				CampaignEventDispatcher.Instance.OnGivenBirth(mother, list, num2);
				mother.IsPregnant = false;
				this._heroPregnancies.Remove(pregnancy);
				if (mother != Hero.MainHero && MBRandom.RandomFloat <= pregnancyModel.MaternalMortalityProbabilityInLabor)
				{
					KillCharacterAction.ApplyInLabor(mother, true);
				}
			}
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0011A83C File Offset: 0x00118A3C
		private void ChildConceived(Hero mother)
		{
			this._heroPregnancies.Add(new PregnancyCampaignBehavior.Pregnancy(mother, mother.Spouse, CampaignTime.DaysFromNow(Campaign.Current.Models.PregnancyModel.PregnancyDurationInDays)));
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0011A870 File Offset: 0x00118A70
		public void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.IsFemale && this._heroPregnancies.Any((PregnancyCampaignBehavior.Pregnancy pregnancy) => pregnancy.Mother == victim))
			{
				this._heroPregnancies.RemoveAll((PregnancyCampaignBehavior.Pregnancy pregnancy) => pregnancy.Mother == victim);
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0011A8C8 File Offset: 0x00118AC8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<PregnancyCampaignBehavior.Pregnancy>>("_heroPregnancies", ref this._heroPregnancies);
		}

		// Token: 0x040011CF RID: 4559
		private List<PregnancyCampaignBehavior.Pregnancy> _heroPregnancies = new List<PregnancyCampaignBehavior.Pregnancy>();

		// Token: 0x0200071F RID: 1823
		public class PregnancyCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005915 RID: 22805 RVA: 0x00183A2B File Offset: 0x00181C2B
			public PregnancyCampaignBehaviorTypeDefiner() : base(110000)
			{
			}

			// Token: 0x06005916 RID: 22806 RVA: 0x00183A38 File Offset: 0x00181C38
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(PregnancyCampaignBehavior.Pregnancy), 2, null);
			}

			// Token: 0x06005917 RID: 22807 RVA: 0x00183A4C File Offset: 0x00181C4C
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<PregnancyCampaignBehavior.Pregnancy>));
			}
		}

		// Token: 0x02000720 RID: 1824
		internal class Pregnancy
		{
			// Token: 0x06005918 RID: 22808 RVA: 0x00183A5E File Offset: 0x00181C5E
			public Pregnancy(Hero pregnantHero, Hero father, CampaignTime dueDate)
			{
				this.Mother = pregnantHero;
				this.Father = father;
				this.DueDate = dueDate;
			}

			// Token: 0x06005919 RID: 22809 RVA: 0x00183A7B File Offset: 0x00181C7B
			internal static void AutoGeneratedStaticCollectObjectsPregnancy(object o, List<object> collectedObjects)
			{
				((PregnancyCampaignBehavior.Pregnancy)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600591A RID: 22810 RVA: 0x00183A89 File Offset: 0x00181C89
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Mother);
				collectedObjects.Add(this.Father);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.DueDate, collectedObjects);
			}

			// Token: 0x0600591B RID: 22811 RVA: 0x00183AB4 File Offset: 0x00181CB4
			internal static object AutoGeneratedGetMemberValueMother(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).Mother;
			}

			// Token: 0x0600591C RID: 22812 RVA: 0x00183AC1 File Offset: 0x00181CC1
			internal static object AutoGeneratedGetMemberValueFather(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).Father;
			}

			// Token: 0x0600591D RID: 22813 RVA: 0x00183ACE File Offset: 0x00181CCE
			internal static object AutoGeneratedGetMemberValueDueDate(object o)
			{
				return ((PregnancyCampaignBehavior.Pregnancy)o).DueDate;
			}

			// Token: 0x04001E03 RID: 7683
			[SaveableField(1)]
			public readonly Hero Mother;

			// Token: 0x04001E04 RID: 7684
			[SaveableField(2)]
			public readonly Hero Father;

			// Token: 0x04001E05 RID: 7685
			[SaveableField(3)]
			public readonly CampaignTime DueDate;
		}
	}
}
