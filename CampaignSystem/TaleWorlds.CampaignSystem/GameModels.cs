using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007E RID: 126
	public sealed class GameModels : GameModelsManager
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00049982 File Offset: 0x00047B82
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x0004998A File Offset: 0x00047B8A
		public MapVisibilityModel MapVisibilityModel { get; private set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00049993 File Offset: 0x00047B93
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x0004999B File Offset: 0x00047B9B
		public InformationRestrictionModel InformationRestrictionModel { get; private set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x000499A4 File Offset: 0x00047BA4
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x000499AC File Offset: 0x00047BAC
		public PartySpeedModel PartySpeedCalculatingModel { get; private set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000499B5 File Offset: 0x00047BB5
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x000499BD File Offset: 0x00047BBD
		public PartyHealingModel PartyHealingModel { get; private set; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x000499C6 File Offset: 0x00047BC6
		// (set) Token: 0x06000F84 RID: 3972 RVA: 0x000499CE File Offset: 0x00047BCE
		public PartyTrainingModel PartyTrainingModel { get; private set; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x000499D7 File Offset: 0x00047BD7
		// (set) Token: 0x06000F86 RID: 3974 RVA: 0x000499DF File Offset: 0x00047BDF
		public BarterModel BarterModel { get; private set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x000499E8 File Offset: 0x00047BE8
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x000499F0 File Offset: 0x00047BF0
		public PersuasionModel PersuasionModel { get; private set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x000499F9 File Offset: 0x00047BF9
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x00049A01 File Offset: 0x00047C01
		public CombatSimulationModel CombatSimulationModel { get; private set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00049A0A File Offset: 0x00047C0A
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x00049A12 File Offset: 0x00047C12
		public CombatXpModel CombatXpModel { get; private set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00049A1B File Offset: 0x00047C1B
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x00049A23 File Offset: 0x00047C23
		public GenericXpModel GenericXpModel { get; private set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00049A2C File Offset: 0x00047C2C
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x00049A34 File Offset: 0x00047C34
		public SmithingModel SmithingModel { get; private set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00049A3D File Offset: 0x00047C3D
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x00049A45 File Offset: 0x00047C45
		public PartyTradeModel PartyTradeModel { get; private set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00049A4E File Offset: 0x00047C4E
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x00049A56 File Offset: 0x00047C56
		public RansomValueCalculationModel RansomValueCalculationModel { get; private set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00049A5F File Offset: 0x00047C5F
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x00049A67 File Offset: 0x00047C67
		public RaidModel RaidModel { get; private set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x00049A70 File Offset: 0x00047C70
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x00049A78 File Offset: 0x00047C78
		public MobilePartyFoodConsumptionModel MobilePartyFoodConsumptionModel { get; private set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00049A81 File Offset: 0x00047C81
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x00049A89 File Offset: 0x00047C89
		public PartyFoodBuyingModel PartyFoodBuyingModel { get; private set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x00049A92 File Offset: 0x00047C92
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x00049A9A File Offset: 0x00047C9A
		public PartyImpairmentModel PartyImpairmentModel { get; private set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x00049AA3 File Offset: 0x00047CA3
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x00049AAB File Offset: 0x00047CAB
		public PartyMoraleModel PartyMoraleModel { get; private set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00049AB4 File Offset: 0x00047CB4
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x00049ABC File Offset: 0x00047CBC
		public PartyDesertionModel PartyDesertionModel { get; private set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00049AC5 File Offset: 0x00047CC5
		// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x00049ACD File Offset: 0x00047CCD
		public DiplomacyModel DiplomacyModel { get; private set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00049AD6 File Offset: 0x00047CD6
		// (set) Token: 0x06000FA4 RID: 4004 RVA: 0x00049ADE File Offset: 0x00047CDE
		public MinorFactionsModel MinorFactionsModel { get; private set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00049AE7 File Offset: 0x00047CE7
		// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x00049AEF File Offset: 0x00047CEF
		public KingdomCreationModel KingdomCreationModel { get; private set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00049AF8 File Offset: 0x00047CF8
		// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x00049B00 File Offset: 0x00047D00
		public KingdomDecisionPermissionModel KingdomDecisionPermissionModel { get; private set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x00049B09 File Offset: 0x00047D09
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x00049B11 File Offset: 0x00047D11
		public EmissaryModel EmissaryModel { get; private set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00049B1A File Offset: 0x00047D1A
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x00049B22 File Offset: 0x00047D22
		public CharacterDevelopmentModel CharacterDevelopmentModel { get; private set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00049B2B File Offset: 0x00047D2B
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x00049B33 File Offset: 0x00047D33
		public CharacterStatsModel CharacterStatsModel { get; private set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00049B3C File Offset: 0x00047D3C
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x00049B44 File Offset: 0x00047D44
		public EncounterModel EncounterModel { get; private set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00049B4D File Offset: 0x00047D4D
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x00049B55 File Offset: 0x00047D55
		public ItemDiscardModel ItemDiscardModel { get; private set; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00049B5E File Offset: 0x00047D5E
		// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x00049B66 File Offset: 0x00047D66
		public ValuationModel ValuationModel { get; private set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00049B6F File Offset: 0x00047D6F
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x00049B77 File Offset: 0x00047D77
		public PartySizeLimitModel PartySizeLimitModel { get; private set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00049B80 File Offset: 0x00047D80
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x00049B88 File Offset: 0x00047D88
		public InventoryCapacityModel InventoryCapacityModel { get; private set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00049B91 File Offset: 0x00047D91
		// (set) Token: 0x06000FBA RID: 4026 RVA: 0x00049B99 File Offset: 0x00047D99
		public PartyWageModel PartyWageModel { get; private set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00049BA2 File Offset: 0x00047DA2
		// (set) Token: 0x06000FBC RID: 4028 RVA: 0x00049BAA File Offset: 0x00047DAA
		public VillageProductionCalculatorModel VillageProductionCalculatorModel { get; private set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00049BB3 File Offset: 0x00047DB3
		// (set) Token: 0x06000FBE RID: 4030 RVA: 0x00049BBB File Offset: 0x00047DBB
		public VolunteerModel VolunteerModel { get; private set; }

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00049BC4 File Offset: 0x00047DC4
		// (set) Token: 0x06000FC0 RID: 4032 RVA: 0x00049BCC File Offset: 0x00047DCC
		public RomanceModel RomanceModel { get; private set; }

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00049BD5 File Offset: 0x00047DD5
		// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x00049BDD File Offset: 0x00047DDD
		public ArmyManagementCalculationModel ArmyManagementCalculationModel { get; private set; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00049BE6 File Offset: 0x00047DE6
		// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x00049BEE File Offset: 0x00047DEE
		public BanditDensityModel BanditDensityModel { get; private set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x00049BF7 File Offset: 0x00047DF7
		// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x00049BFF File Offset: 0x00047DFF
		public EncounterGameMenuModel EncounterGameMenuModel { get; private set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00049C08 File Offset: 0x00047E08
		// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x00049C10 File Offset: 0x00047E10
		public BattleRewardModel BattleRewardModel { get; private set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00049C19 File Offset: 0x00047E19
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x00049C21 File Offset: 0x00047E21
		public MapTrackModel MapTrackModel { get; private set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00049C2A File Offset: 0x00047E2A
		// (set) Token: 0x06000FCC RID: 4044 RVA: 0x00049C32 File Offset: 0x00047E32
		public MapDistanceModel MapDistanceModel { get; private set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00049C3B File Offset: 0x00047E3B
		// (set) Token: 0x06000FCE RID: 4046 RVA: 0x00049C43 File Offset: 0x00047E43
		public MapWeatherModel MapWeatherModel { get; private set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00049C4C File Offset: 0x00047E4C
		// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x00049C54 File Offset: 0x00047E54
		public TargetScoreCalculatingModel TargetScoreCalculatingModel { get; private set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00049C5D File Offset: 0x00047E5D
		// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x00049C65 File Offset: 0x00047E65
		public TradeItemPriceFactorModel TradeItemPriceFactorModel { get; private set; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00049C6E File Offset: 0x00047E6E
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x00049C76 File Offset: 0x00047E76
		public SettlementEconomyModel SettlementConsumptionModel { get; private set; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00049C7F File Offset: 0x00047E7F
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00049C87 File Offset: 0x00047E87
		public SettlementFoodModel SettlementFoodModel { get; private set; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00049C90 File Offset: 0x00047E90
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00049C98 File Offset: 0x00047E98
		public SettlementValueModel SettlementValueModel { get; private set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00049CA1 File Offset: 0x00047EA1
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00049CA9 File Offset: 0x00047EA9
		public SettlementMilitiaModel SettlementMilitiaModel { get; private set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00049CB2 File Offset: 0x00047EB2
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x00049CBA File Offset: 0x00047EBA
		public SettlementLoyaltyModel SettlementLoyaltyModel { get; private set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00049CC3 File Offset: 0x00047EC3
		// (set) Token: 0x06000FDE RID: 4062 RVA: 0x00049CCB File Offset: 0x00047ECB
		public SettlementSecurityModel SettlementSecurityModel { get; private set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x00049CD4 File Offset: 0x00047ED4
		// (set) Token: 0x06000FE0 RID: 4064 RVA: 0x00049CDC File Offset: 0x00047EDC
		public SettlementProsperityModel SettlementProsperityModel { get; private set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x00049CE5 File Offset: 0x00047EE5
		// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x00049CED File Offset: 0x00047EED
		public SettlementGarrisonModel SettlementGarrisonModel { get; private set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x00049CF6 File Offset: 0x00047EF6
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x00049CFE File Offset: 0x00047EFE
		public ClanTierModel ClanTierModel { get; private set; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x00049D07 File Offset: 0x00047F07
		// (set) Token: 0x06000FE6 RID: 4070 RVA: 0x00049D0F File Offset: 0x00047F0F
		public VassalRewardsModel VassalRewardsModel { get; private set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x00049D18 File Offset: 0x00047F18
		// (set) Token: 0x06000FE8 RID: 4072 RVA: 0x00049D20 File Offset: 0x00047F20
		public ClanPoliticsModel ClanPoliticsModel { get; private set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x00049D29 File Offset: 0x00047F29
		// (set) Token: 0x06000FEA RID: 4074 RVA: 0x00049D31 File Offset: 0x00047F31
		public ClanFinanceModel ClanFinanceModel { get; private set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x00049D3A File Offset: 0x00047F3A
		// (set) Token: 0x06000FEC RID: 4076 RVA: 0x00049D42 File Offset: 0x00047F42
		public SettlementTaxModel SettlementTaxModel { get; private set; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x00049D4B File Offset: 0x00047F4B
		// (set) Token: 0x06000FEE RID: 4078 RVA: 0x00049D53 File Offset: 0x00047F53
		public HeirSelectionCalculationModel HeirSelectionCalculationModel { get; private set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00049D5C File Offset: 0x00047F5C
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x00049D64 File Offset: 0x00047F64
		public HeroDeathProbabilityCalculationModel HeroDeathProbabilityCalculationModel { get; private set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00049D6D File Offset: 0x00047F6D
		// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x00049D75 File Offset: 0x00047F75
		public BuildingConstructionModel BuildingConstructionModel { get; private set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00049D7E File Offset: 0x00047F7E
		// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x00049D86 File Offset: 0x00047F86
		public BuildingEffectModel BuildingEffectModel { get; private set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x00049D8F File Offset: 0x00047F8F
		// (set) Token: 0x06000FF6 RID: 4086 RVA: 0x00049D97 File Offset: 0x00047F97
		public WallHitPointCalculationModel WallHitPointCalculationModel { get; private set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x00049DA0 File Offset: 0x00047FA0
		// (set) Token: 0x06000FF8 RID: 4088 RVA: 0x00049DA8 File Offset: 0x00047FA8
		public MarriageModel MarriageModel { get; private set; }

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x00049DB1 File Offset: 0x00047FB1
		// (set) Token: 0x06000FFA RID: 4090 RVA: 0x00049DB9 File Offset: 0x00047FB9
		public AgeModel AgeModel { get; private set; }

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00049DC2 File Offset: 0x00047FC2
		// (set) Token: 0x06000FFC RID: 4092 RVA: 0x00049DCA File Offset: 0x00047FCA
		public PlayerProgressionModel PlayerProgressionModel { get; private set; }

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00049DD3 File Offset: 0x00047FD3
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x00049DDB File Offset: 0x00047FDB
		public DailyTroopXpBonusModel DailyTroopXpBonusModel { get; private set; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00049DE4 File Offset: 0x00047FE4
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x00049DEC File Offset: 0x00047FEC
		public PregnancyModel PregnancyModel { get; private set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x00049DF5 File Offset: 0x00047FF5
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x00049DFD File Offset: 0x00047FFD
		public NotablePowerModel NotablePowerModel { get; private set; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x00049E06 File Offset: 0x00048006
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x00049E0E File Offset: 0x0004800E
		public MilitaryPowerModel MilitaryPowerModel { get; private set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x00049E17 File Offset: 0x00048017
		// (set) Token: 0x06001006 RID: 4102 RVA: 0x00049E1F File Offset: 0x0004801F
		public PrisonerDonationModel PrisonerDonationModel { get; private set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x00049E28 File Offset: 0x00048028
		// (set) Token: 0x06001008 RID: 4104 RVA: 0x00049E30 File Offset: 0x00048030
		public NotableSpawnModel NotableSpawnModel { get; private set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00049E39 File Offset: 0x00048039
		// (set) Token: 0x0600100A RID: 4106 RVA: 0x00049E41 File Offset: 0x00048041
		public TournamentModel TournamentModel { get; private set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x00049E4A File Offset: 0x0004804A
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x00049E52 File Offset: 0x00048052
		public CrimeModel CrimeModel { get; private set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x00049E5B File Offset: 0x0004805B
		// (set) Token: 0x0600100E RID: 4110 RVA: 0x00049E63 File Offset: 0x00048063
		public DisguiseDetectionModel DisguiseDetectionModel { get; private set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x00049E6C File Offset: 0x0004806C
		// (set) Token: 0x06001010 RID: 4112 RVA: 0x00049E74 File Offset: 0x00048074
		public BribeCalculationModel BribeCalculationModel { get; private set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001011 RID: 4113 RVA: 0x00049E7D File Offset: 0x0004807D
		// (set) Token: 0x06001012 RID: 4114 RVA: 0x00049E85 File Offset: 0x00048085
		public TroopSacrificeModel TroopSacrificeModel { get; private set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x00049E8E File Offset: 0x0004808E
		// (set) Token: 0x06001014 RID: 4116 RVA: 0x00049E96 File Offset: 0x00048096
		public SiegeStrategyActionModel SiegeStrategyActionModel { get; private set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x00049E9F File Offset: 0x0004809F
		// (set) Token: 0x06001016 RID: 4118 RVA: 0x00049EA7 File Offset: 0x000480A7
		public SiegeEventModel SiegeEventModel { get; private set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x00049EB0 File Offset: 0x000480B0
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x00049EB8 File Offset: 0x000480B8
		public SiegeAftermathModel SiegeAftermathModel { get; private set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x00049EC1 File Offset: 0x000480C1
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x00049EC9 File Offset: 0x000480C9
		public SiegeLordsHallFightModel SiegeLordsHallFightModel { get; private set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00049ED2 File Offset: 0x000480D2
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x00049EDA File Offset: 0x000480DA
		public CompanionHiringPriceCalculationModel CompanionHiringPriceCalculationModel { get; private set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x00049EE3 File Offset: 0x000480E3
		// (set) Token: 0x0600101E RID: 4126 RVA: 0x00049EEB File Offset: 0x000480EB
		public BuildingScoreCalculationModel BuildingScoreCalculationModel { get; private set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x00049EF4 File Offset: 0x000480F4
		// (set) Token: 0x06001020 RID: 4128 RVA: 0x00049EFC File Offset: 0x000480FC
		public SettlementAccessModel SettlementAccessModel { get; private set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x00049F05 File Offset: 0x00048105
		// (set) Token: 0x06001022 RID: 4130 RVA: 0x00049F0D File Offset: 0x0004810D
		public IssueModel IssueModel { get; private set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x00049F16 File Offset: 0x00048116
		// (set) Token: 0x06001024 RID: 4132 RVA: 0x00049F1E File Offset: 0x0004811E
		public PrisonerRecruitmentCalculationModel PrisonerRecruitmentCalculationModel { get; private set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x00049F27 File Offset: 0x00048127
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x00049F2F File Offset: 0x0004812F
		public PartyTroopUpgradeModel PartyTroopUpgradeModel { get; private set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x00049F38 File Offset: 0x00048138
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x00049F40 File Offset: 0x00048140
		public TavernMercenaryTroopsModel TavernMercenaryTroopsModel { get; private set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x00049F49 File Offset: 0x00048149
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x00049F51 File Offset: 0x00048151
		public WorkshopModel WorkshopModel { get; private set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x00049F5A File Offset: 0x0004815A
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x00049F62 File Offset: 0x00048162
		public DifficultyModel DifficultyModel { get; private set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00049F6B File Offset: 0x0004816B
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x00049F73 File Offset: 0x00048173
		public LocationModel LocationModel { get; private set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x00049F7C File Offset: 0x0004817C
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x00049F84 File Offset: 0x00048184
		public PrisonBreakModel PrisonBreakModel { get; private set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x00049F8D File Offset: 0x0004818D
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x00049F95 File Offset: 0x00048195
		public BattleCaptainModel BattleCaptainModel { get; private set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00049F9E File Offset: 0x0004819E
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x00049FA6 File Offset: 0x000481A6
		public ExecutionRelationModel ExecutionRelationModel { get; private set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00049FAF File Offset: 0x000481AF
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x00049FB7 File Offset: 0x000481B7
		public BannerItemModel BannerItemModel { get; private set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x00049FC0 File Offset: 0x000481C0
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x00049FC8 File Offset: 0x000481C8
		public DelayedTeleportationModel DelayedTeleportationModel { get; private set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00049FD1 File Offset: 0x000481D1
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x00049FD9 File Offset: 0x000481D9
		public TroopSupplierProbabilityModel TroopSupplierProbabilityModel { get; private set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00049FE2 File Offset: 0x000481E2
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00049FEA File Offset: 0x000481EA
		public CutsceneSelectionModel CutsceneSelectionModel { get; private set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00049FF3 File Offset: 0x000481F3
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x00049FFB File Offset: 0x000481FB
		public EquipmentSelectionModel EquipmentSelectionModel { get; private set; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0004A004 File Offset: 0x00048204
		// (set) Token: 0x06001040 RID: 4160 RVA: 0x0004A00C File Offset: 0x0004820C
		public AlleyModel AlleyModel { get; private set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0004A015 File Offset: 0x00048215
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x0004A01D File Offset: 0x0004821D
		public VoiceOverModel VoiceOverModel { get; private set; }

		// Token: 0x06001043 RID: 4163 RVA: 0x0004A028 File Offset: 0x00048228
		private void GetSpecificGameBehaviors()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign || Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				this.CharacterDevelopmentModel = base.GetGameModel<CharacterDevelopmentModel>();
				this.CharacterStatsModel = base.GetGameModel<CharacterStatsModel>();
				this.EncounterModel = base.GetGameModel<EncounterModel>();
				this.ItemDiscardModel = base.GetGameModel<ItemDiscardModel>();
				this.ValuationModel = base.GetGameModel<ValuationModel>();
				this.MapVisibilityModel = base.GetGameModel<MapVisibilityModel>();
				this.InformationRestrictionModel = base.GetGameModel<InformationRestrictionModel>();
				this.PartySpeedCalculatingModel = base.GetGameModel<PartySpeedModel>();
				this.PartyHealingModel = base.GetGameModel<PartyHealingModel>();
				this.PartyTrainingModel = base.GetGameModel<PartyTrainingModel>();
				this.PartyTradeModel = base.GetGameModel<PartyTradeModel>();
				this.RansomValueCalculationModel = base.GetGameModel<RansomValueCalculationModel>();
				this.RaidModel = base.GetGameModel<RaidModel>();
				this.CombatSimulationModel = base.GetGameModel<CombatSimulationModel>();
				this.CombatXpModel = base.GetGameModel<CombatXpModel>();
				this.GenericXpModel = base.GetGameModel<GenericXpModel>();
				this.SmithingModel = base.GetGameModel<SmithingModel>();
				this.MobilePartyFoodConsumptionModel = base.GetGameModel<MobilePartyFoodConsumptionModel>();
				this.PartyImpairmentModel = base.GetGameModel<PartyImpairmentModel>();
				this.PartyFoodBuyingModel = base.GetGameModel<PartyFoodBuyingModel>();
				this.PartyMoraleModel = base.GetGameModel<PartyMoraleModel>();
				this.PartyDesertionModel = base.GetGameModel<PartyDesertionModel>();
				this.DiplomacyModel = base.GetGameModel<DiplomacyModel>();
				this.MinorFactionsModel = base.GetGameModel<MinorFactionsModel>();
				this.KingdomCreationModel = base.GetGameModel<KingdomCreationModel>();
				this.EmissaryModel = base.GetGameModel<EmissaryModel>();
				this.KingdomDecisionPermissionModel = base.GetGameModel<KingdomDecisionPermissionModel>();
				this.VillageProductionCalculatorModel = base.GetGameModel<VillageProductionCalculatorModel>();
				this.RomanceModel = base.GetGameModel<RomanceModel>();
				this.VolunteerModel = base.GetGameModel<VolunteerModel>();
				this.ArmyManagementCalculationModel = base.GetGameModel<ArmyManagementCalculationModel>();
				this.BanditDensityModel = base.GetGameModel<BanditDensityModel>();
				this.EncounterGameMenuModel = base.GetGameModel<EncounterGameMenuModel>();
				this.BattleRewardModel = base.GetGameModel<BattleRewardModel>();
				this.MapTrackModel = base.GetGameModel<MapTrackModel>();
				this.MapDistanceModel = base.GetGameModel<MapDistanceModel>();
				this.MapWeatherModel = base.GetGameModel<MapWeatherModel>();
				this.TargetScoreCalculatingModel = base.GetGameModel<TargetScoreCalculatingModel>();
				this.PartySizeLimitModel = base.GetGameModel<PartySizeLimitModel>();
				this.PartyWageModel = base.GetGameModel<PartyWageModel>();
				this.PlayerProgressionModel = base.GetGameModel<PlayerProgressionModel>();
				this.InventoryCapacityModel = base.GetGameModel<InventoryCapacityModel>();
				this.TradeItemPriceFactorModel = base.GetGameModel<TradeItemPriceFactorModel>();
				this.SettlementValueModel = base.GetGameModel<SettlementValueModel>();
				this.SettlementConsumptionModel = base.GetGameModel<SettlementEconomyModel>();
				this.SettlementMilitiaModel = base.GetGameModel<SettlementMilitiaModel>();
				this.SettlementFoodModel = base.GetGameModel<SettlementFoodModel>();
				this.SettlementLoyaltyModel = base.GetGameModel<SettlementLoyaltyModel>();
				this.SettlementSecurityModel = base.GetGameModel<SettlementSecurityModel>();
				this.SettlementProsperityModel = base.GetGameModel<SettlementProsperityModel>();
				this.SettlementGarrisonModel = base.GetGameModel<SettlementGarrisonModel>();
				this.SettlementTaxModel = base.GetGameModel<SettlementTaxModel>();
				this.BarterModel = base.GetGameModel<BarterModel>();
				this.PersuasionModel = base.GetGameModel<PersuasionModel>();
				this.ClanTierModel = base.GetGameModel<ClanTierModel>();
				this.VassalRewardsModel = base.GetGameModel<VassalRewardsModel>();
				this.ClanPoliticsModel = base.GetGameModel<ClanPoliticsModel>();
				this.ClanFinanceModel = base.GetGameModel<ClanFinanceModel>();
				this.HeirSelectionCalculationModel = base.GetGameModel<HeirSelectionCalculationModel>();
				this.HeroDeathProbabilityCalculationModel = base.GetGameModel<HeroDeathProbabilityCalculationModel>();
				this.BuildingConstructionModel = base.GetGameModel<BuildingConstructionModel>();
				this.BuildingEffectModel = base.GetGameModel<BuildingEffectModel>();
				this.WallHitPointCalculationModel = base.GetGameModel<WallHitPointCalculationModel>();
				this.MarriageModel = base.GetGameModel<MarriageModel>();
				this.AgeModel = base.GetGameModel<AgeModel>();
				this.DailyTroopXpBonusModel = base.GetGameModel<DailyTroopXpBonusModel>();
				this.PregnancyModel = base.GetGameModel<PregnancyModel>();
				this.NotablePowerModel = base.GetGameModel<NotablePowerModel>();
				this.NotableSpawnModel = base.GetGameModel<NotableSpawnModel>();
				this.TournamentModel = base.GetGameModel<TournamentModel>();
				this.SiegeStrategyActionModel = base.GetGameModel<SiegeStrategyActionModel>();
				this.SiegeEventModel = base.GetGameModel<SiegeEventModel>();
				this.SiegeAftermathModel = base.GetGameModel<SiegeAftermathModel>();
				this.SiegeLordsHallFightModel = base.GetGameModel<SiegeLordsHallFightModel>();
				this.CrimeModel = base.GetGameModel<CrimeModel>();
				this.DisguiseDetectionModel = base.GetGameModel<DisguiseDetectionModel>();
				this.BribeCalculationModel = base.GetGameModel<BribeCalculationModel>();
				this.CompanionHiringPriceCalculationModel = base.GetGameModel<CompanionHiringPriceCalculationModel>();
				this.TroopSacrificeModel = base.GetGameModel<TroopSacrificeModel>();
				this.BuildingScoreCalculationModel = base.GetGameModel<BuildingScoreCalculationModel>();
				this.SettlementAccessModel = base.GetGameModel<SettlementAccessModel>();
				this.IssueModel = base.GetGameModel<IssueModel>();
				this.PrisonerRecruitmentCalculationModel = base.GetGameModel<PrisonerRecruitmentCalculationModel>();
				this.PartyTroopUpgradeModel = base.GetGameModel<PartyTroopUpgradeModel>();
				this.TavernMercenaryTroopsModel = base.GetGameModel<TavernMercenaryTroopsModel>();
				this.WorkshopModel = base.GetGameModel<WorkshopModel>();
				this.DifficultyModel = base.GetGameModel<DifficultyModel>();
				this.LocationModel = base.GetGameModel<LocationModel>();
				this.MilitaryPowerModel = base.GetGameModel<MilitaryPowerModel>();
				this.PrisonerDonationModel = base.GetGameModel<PrisonerDonationModel>();
				this.PrisonBreakModel = base.GetGameModel<PrisonBreakModel>();
				this.BattleCaptainModel = base.GetGameModel<BattleCaptainModel>();
				this.ExecutionRelationModel = base.GetGameModel<ExecutionRelationModel>();
				this.BannerItemModel = base.GetGameModel<BannerItemModel>();
				this.DelayedTeleportationModel = base.GetGameModel<DelayedTeleportationModel>();
				this.TroopSupplierProbabilityModel = base.GetGameModel<TroopSupplierProbabilityModel>();
				this.CutsceneSelectionModel = base.GetGameModel<CutsceneSelectionModel>();
				this.EquipmentSelectionModel = base.GetGameModel<EquipmentSelectionModel>();
				this.AlleyModel = base.GetGameModel<AlleyModel>();
				this.VoiceOverModel = base.GetGameModel<VoiceOverModel>();
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004A502 File Offset: 0x00048702
		private void MakeGameComponentBindings()
		{
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0004A504 File Offset: 0x00048704
		public GameModels(IEnumerable<GameModel> inputComponents) : base(inputComponents)
		{
			this.GetSpecificGameBehaviors();
			this.MakeGameComponentBindings();
		}
	}
}
