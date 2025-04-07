using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x0200034C RID: 844
	public class DefaultCulturalFeats
	{
		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002FAD RID: 12205 RVA: 0x000C405C File Offset: 0x000C225C
		private static DefaultCulturalFeats Instance
		{
			get
			{
				return Campaign.Current.DefaultFeats;
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000C4068 File Offset: 0x000C2268
		public DefaultCulturalFeats()
		{
			this.RegisterAll();
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000C4078 File Offset: 0x000C2278
		private void RegisterAll()
		{
			this._aseraiTraderFeat = this.Create("aserai_cheaper_caravans");
			this._aseraiDesertSpeedFeat = this.Create("aserai_desert_speed");
			this._aseraiWageFeat = this.Create("aserai_increased_wages");
			this._battaniaForestSpeedFeat = this.Create("battanian_forest_speed");
			this._battaniaMilitiaFeat = this.Create("battanian_militia_production");
			this._battaniaConstructionFeat = this.Create("battanian_slower_construction");
			this._empireGarrisonWageFeat = this.Create("empire_decreased_garrison_wage");
			this._empireArmyInfluenceFeat = this.Create("empire_army_influence");
			this._empireVillageHearthFeat = this.Create("empire_slower_hearth_production");
			this._khuzaitCheaperRecruitsFeat = this.Create("khuzait_cheaper_recruits_mounted");
			this._khuzaitAnimalProductionFeat = this.Create("khuzait_increased_animal_production");
			this._khuzaitDecreasedTaxFeat = this.Create("khuzait_decreased_town_tax");
			this._sturgianCheaperRecruitsFeat = this.Create("sturgian_cheaper_recruits_infantry");
			this._sturgianArmyCohesionFeat = this.Create("sturgian_decreased_cohesion_rate");
			this._sturgianDecisionPenaltyFeat = this.Create("sturgian_increased_decision_penalty");
			this._vlandianRenownIncomeFeat = this.Create("vlandian_renown_mercenary_income");
			this._vlandianVillageProductionFeat = this.Create("vlandian_villages_production_bonus");
			this._vlandianArmyInfluenceCostFeat = this.Create("vlandian_increased_army_influence_cost");
			this.InitializeAll();
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000C41BD File Offset: 0x000C23BD
		private FeatObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<FeatObject>(new FeatObject(stringId));
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000C41D4 File Offset: 0x000C23D4
		private void InitializeAll()
		{
			this._aseraiTraderFeat.Initialize("{=!}aserai_cheaper_caravans", "{=7kGGgkro}Caravans are 30% cheaper to build. 10% less trade penalty.", 0.7f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiDesertSpeedFeat.Initialize("{=!}aserai_desert_speed", "{=6aFTN1Nb}No speed penalty on desert.", 1f, true, FeatObject.AdditionType.AddFactor);
			this._aseraiWageFeat.Initialize("{=!}aserai_increased_wages", "{=GacrZ1Jl}Daily wages of troops in the party are increased by 5%.", 0.05f, false, FeatObject.AdditionType.AddFactor);
			this._battaniaForestSpeedFeat.Initialize("{=!}battanian_forest_speed", "{=38W2WloI}50% less speed penalty and 15% sight range bonus in forests.", 0.5f, true, FeatObject.AdditionType.AddFactor);
			this._battaniaMilitiaFeat.Initialize("{=!}battanian_militia_production", "{=1qUFMK28}Towns owned by Battanian rulers have +1 militia production.", 1f, true, FeatObject.AdditionType.Add);
			this._battaniaConstructionFeat.Initialize("{=!}battanian_slower_construction", "{=ruP9jbSq}10% slower build rate for town projects in settlements.", -0.1f, false, FeatObject.AdditionType.AddFactor);
			this._empireGarrisonWageFeat.Initialize("{=!}empire_decreased_garrison_wage", "{=a2eM0QUb}20% less garrison troop wage.", -0.2f, true, FeatObject.AdditionType.AddFactor);
			this._empireArmyInfluenceFeat.Initialize("{=!}empire_army_influence", "{=xgPNGOa8}Being in army brings 25% more influence.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._empireVillageHearthFeat.Initialize("{=!}empire_slower_hearth_production", "{=UWiqIFUb}Village hearths increase 20% less.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._khuzaitCheaperRecruitsFeat.Initialize("{=!}khuzait_cheaper_recruits_mounted", "{=JUpZuals}Recruiting and upgrading mounted troops are 10% cheaper.", -0.1f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitAnimalProductionFeat.Initialize("{=!}khuzait_increased_animal_production", "{=Xaw2CoCG}25% production bonus to horse, mule, cow and sheep in villages owned by Khuzait rulers.", 0.25f, true, FeatObject.AdditionType.AddFactor);
			this._khuzaitDecreasedTaxFeat.Initialize("{=!}khuzait_decreased_town_tax", "{=8PsaGhI8}20% less tax income from towns.", -0.2f, false, FeatObject.AdditionType.AddFactor);
			this._sturgianCheaperRecruitsFeat.Initialize("{=!}sturgian_cheaper_recruits_infantry", "{=CJ5pLHaL}Recruiting and upgrading infantry troops are 25% cheaper.", -0.25f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianArmyCohesionFeat.Initialize("{=!}sturgian_decreased_cohesion_rate", "{=QiHaWd75}Armies lose 20% less daily cohesion.", -0.2f, true, FeatObject.AdditionType.AddFactor);
			this._sturgianDecisionPenaltyFeat.Initialize("{=!}sturgian_increased_decision_penalty", "{=fB7kS9Cx}20% more relationship penalty from kingdom decisions.", 0.2f, false, FeatObject.AdditionType.AddFactor);
			this._vlandianRenownIncomeFeat.Initialize("{=!}vlandian_renown_mercenary_income", "{=ppdrgOL8}5% more renown from the battles, 15% more income while serving as a mercenary.", 0.05f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianVillageProductionFeat.Initialize("{=!}vlandian_villages_production_bonus", "{=3GsZXXOi}10% production bonus to villages that are bound to castles.", 0.1f, true, FeatObject.AdditionType.AddFactor);
			this._vlandianArmyInfluenceCostFeat.Initialize("{=!}vlandian_increased_army_influence_cost", "{=O1XCNeZr}Recruiting lords to armies costs 20% more influence.", 0.2f, false, FeatObject.AdditionType.AddFactor);
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x000C43D9 File Offset: 0x000C25D9
		public static FeatObject AseraiTraderFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiTraderFeat;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x000C43E5 File Offset: 0x000C25E5
		public static FeatObject AseraiDesertFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiDesertSpeedFeat;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000C43F1 File Offset: 0x000C25F1
		public static FeatObject AseraiIncreasedWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._aseraiWageFeat;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000C43FD File Offset: 0x000C25FD
		public static FeatObject BattanianForestSpeedFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaForestSpeedFeat;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x000C4409 File Offset: 0x000C2609
		public static FeatObject BattanianMilitiaFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaMilitiaFeat;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000C4415 File Offset: 0x000C2615
		public static FeatObject BattanianConstructionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._battaniaConstructionFeat;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000C4421 File Offset: 0x000C2621
		public static FeatObject EmpireGarrisonWageFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireGarrisonWageFeat;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x000C442D File Offset: 0x000C262D
		public static FeatObject EmpireArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireArmyInfluenceFeat;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000C4439 File Offset: 0x000C2639
		public static FeatObject EmpireVillageHearthFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._empireVillageHearthFeat;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06002FBB RID: 12219 RVA: 0x000C4445 File Offset: 0x000C2645
		public static FeatObject KhuzaitRecruitUpgradeFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitCheaperRecruitsFeat;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000C4451 File Offset: 0x000C2651
		public static FeatObject KhuzaitAnimalProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitAnimalProductionFeat;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000C445D File Offset: 0x000C265D
		public static FeatObject KhuzaitDecreasedTaxFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._khuzaitDecreasedTaxFeat;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000C4469 File Offset: 0x000C2669
		public static FeatObject SturgianRecruitUpgradeFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianCheaperRecruitsFeat;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x000C4475 File Offset: 0x000C2675
		public static FeatObject SturgianArmyCohesionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianArmyCohesionFeat;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000C4481 File Offset: 0x000C2681
		public static FeatObject SturgianDecisionPenaltyFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._sturgianDecisionPenaltyFeat;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002FC1 RID: 12225 RVA: 0x000C448D File Offset: 0x000C268D
		public static FeatObject VlandianRenownMercenaryFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianRenownIncomeFeat;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000C4499 File Offset: 0x000C2699
		public static FeatObject VlandianCastleVillageProductionFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianVillageProductionFeat;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000C44A5 File Offset: 0x000C26A5
		public static FeatObject VlandianArmyInfluenceFeat
		{
			get
			{
				return DefaultCulturalFeats.Instance._vlandianArmyInfluenceCostFeat;
			}
		}

		// Token: 0x04000E1D RID: 3613
		private FeatObject _aseraiTraderFeat;

		// Token: 0x04000E1E RID: 3614
		private FeatObject _aseraiDesertSpeedFeat;

		// Token: 0x04000E1F RID: 3615
		private FeatObject _aseraiWageFeat;

		// Token: 0x04000E20 RID: 3616
		private FeatObject _battaniaForestSpeedFeat;

		// Token: 0x04000E21 RID: 3617
		private FeatObject _battaniaMilitiaFeat;

		// Token: 0x04000E22 RID: 3618
		private FeatObject _battaniaConstructionFeat;

		// Token: 0x04000E23 RID: 3619
		private FeatObject _empireGarrisonWageFeat;

		// Token: 0x04000E24 RID: 3620
		private FeatObject _empireArmyInfluenceFeat;

		// Token: 0x04000E25 RID: 3621
		private FeatObject _empireVillageHearthFeat;

		// Token: 0x04000E26 RID: 3622
		private FeatObject _khuzaitCheaperRecruitsFeat;

		// Token: 0x04000E27 RID: 3623
		private FeatObject _khuzaitAnimalProductionFeat;

		// Token: 0x04000E28 RID: 3624
		private FeatObject _khuzaitDecreasedTaxFeat;

		// Token: 0x04000E29 RID: 3625
		private FeatObject _sturgianCheaperRecruitsFeat;

		// Token: 0x04000E2A RID: 3626
		private FeatObject _sturgianArmyCohesionFeat;

		// Token: 0x04000E2B RID: 3627
		private FeatObject _sturgianDecisionPenaltyFeat;

		// Token: 0x04000E2C RID: 3628
		private FeatObject _vlandianRenownIncomeFeat;

		// Token: 0x04000E2D RID: 3629
		private FeatObject _vlandianVillageProductionFeat;

		// Token: 0x04000E2E RID: 3630
		private FeatObject _vlandianArmyInfluenceCostFeat;
	}
}
