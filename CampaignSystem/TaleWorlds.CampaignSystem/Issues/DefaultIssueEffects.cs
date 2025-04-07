using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000320 RID: 800
	public class DefaultIssueEffects
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002DF9 RID: 11769 RVA: 0x000C0F28 File Offset: 0x000BF128
		private static DefaultIssueEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultIssueEffects;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002DFA RID: 11770 RVA: 0x000C0F34 File Offset: 0x000BF134
		public static IssueEffect SettlementLoyalty
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementLoyalty;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002DFB RID: 11771 RVA: 0x000C0F40 File Offset: 0x000BF140
		public static IssueEffect SettlementSecurity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementSecurity;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002DFC RID: 11772 RVA: 0x000C0F4C File Offset: 0x000BF14C
		public static IssueEffect SettlementMilitia
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementMilitia;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000C0F58 File Offset: 0x000BF158
		public static IssueEffect SettlementProsperity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementProsperity;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002DFE RID: 11774 RVA: 0x000C0F64 File Offset: 0x000BF164
		public static IssueEffect VillageHearth
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectVillageHearth;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002DFF RID: 11775 RVA: 0x000C0F70 File Offset: 0x000BF170
		public static IssueEffect SettlementFood
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementFood;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x000C0F7C File Offset: 0x000BF17C
		public static IssueEffect SettlementTax
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementTax;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000C0F88 File Offset: 0x000BF188
		public static IssueEffect SettlementGarrison
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementGarrison;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002E02 RID: 11778 RVA: 0x000C0F94 File Offset: 0x000BF194
		public static IssueEffect HalfVillageProduction
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectHalfVillageProduction;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x000C0FA0 File Offset: 0x000BF1A0
		public static IssueEffect IssueOwnerPower
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectIssueOwnerPower;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002E04 RID: 11780 RVA: 0x000C0FAC File Offset: 0x000BF1AC
		public static IssueEffect ClanInfluence
		{
			get
			{
				return DefaultIssueEffects.Instance._clanInfluence;
			}
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000C0FB8 File Offset: 0x000BF1B8
		public DefaultIssueEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000C0FC8 File Offset: 0x000BF1C8
		private void RegisterAll()
		{
			this._issueEffectSettlementLoyalty = this.Create("issue_effect_settlement_loyalty");
			this._issueEffectSettlementSecurity = this.Create("issue_effect_settlement_security");
			this._issueEffectSettlementMilitia = this.Create("issue_effect_settlement_militia");
			this._issueEffectSettlementProsperity = this.Create("issue_effect_settlement_prosperity");
			this._issueEffectVillageHearth = this.Create("issue_effect_village_hearth");
			this._issueEffectSettlementFood = this.Create("issue_effect_settlement_food");
			this._issueEffectSettlementTax = this.Create("issue_effect_settlement_tax");
			this._issueEffectSettlementGarrison = this.Create("issue_effect_settlement_garrison");
			this._issueEffectHalfVillageProduction = this.Create("issue_effect_half_village_production");
			this._issueEffectIssueOwnerPower = this.Create("issue_effect_issue_owner_power");
			this._clanInfluence = this.Create("issue_effect_clan_influence");
			this.InitializeAll();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000C1096 File Offset: 0x000BF296
		private IssueEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<IssueEffect>(new IssueEffect(stringId));
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000C10B0 File Offset: 0x000BF2B0
		private void InitializeAll()
		{
			this._issueEffectSettlementLoyalty.Initialize(new TextObject("{=YO0x7ZAo}Loyalty", null), new TextObject("{=xAWvm25T}Effects settlement's loyalty.", null));
			this._issueEffectSettlementSecurity.Initialize(new TextObject("{=MqCH7R4A}Security", null), new TextObject("{=h117Qj3E}Effects settlement's security.", null));
			this._issueEffectSettlementMilitia.Initialize(new TextObject("{=gsVtO9A7}Militia", null), new TextObject("{=dTmPV82D}Effects settlement's militia.", null));
			this._issueEffectSettlementProsperity.Initialize(new TextObject("{=IagYTD5O}Prosperity", null), new TextObject("{=ETye0JMY}Effects settlement's prosperity.", null));
			this._issueEffectVillageHearth.Initialize(new TextObject("{=f5X5uU0m}Village Hearth", null), new TextObject("{=7TbVhbT9}Effects village's hearth.", null));
			this._issueEffectSettlementFood.Initialize(new TextObject("{=qSi4DlT4}Food", null), new TextObject("{=onDsUkUl}Effects settlement's food.", null));
			this._issueEffectSettlementTax.Initialize(new TextObject("{=2awf1tei}Tax", null), new TextObject("{=q2Ovtr1s}Effects settlement's tax.", null));
			this._issueEffectSettlementGarrison.Initialize(new TextObject("{=jlgjLDo7}Garrison", null), new TextObject("{=WJ7SnBgN}Effects settlement's garrison.", null));
			this._issueEffectHalfVillageProduction.Initialize(new TextObject("{=bGyrPe8c}Production", null), new TextObject("{=arbaXvQf}Effects village's production.", null));
			this._issueEffectIssueOwnerPower.Initialize(new TextObject("{=gGXelWQX}Issue owner power", null), new TextObject("{=tjudHtDB}Effects the power of issue owner in the settlement.", null));
			this._clanInfluence.Initialize(new TextObject("{=KN6khbSl}Clan Influence", null), new TextObject("{=y2aLOwOs}Effects the influence of clan.", null));
		}

		// Token: 0x04000DB6 RID: 3510
		private IssueEffect _issueEffectSettlementGarrison;

		// Token: 0x04000DB7 RID: 3511
		private IssueEffect _issueEffectSettlementLoyalty;

		// Token: 0x04000DB8 RID: 3512
		private IssueEffect _issueEffectSettlementSecurity;

		// Token: 0x04000DB9 RID: 3513
		private IssueEffect _issueEffectSettlementMilitia;

		// Token: 0x04000DBA RID: 3514
		private IssueEffect _issueEffectSettlementProsperity;

		// Token: 0x04000DBB RID: 3515
		private IssueEffect _issueEffectVillageHearth;

		// Token: 0x04000DBC RID: 3516
		private IssueEffect _issueEffectSettlementFood;

		// Token: 0x04000DBD RID: 3517
		private IssueEffect _issueEffectSettlementTax;

		// Token: 0x04000DBE RID: 3518
		private IssueEffect _issueEffectHalfVillageProduction;

		// Token: 0x04000DBF RID: 3519
		private IssueEffect _issueEffectIssueOwnerPower;

		// Token: 0x04000DC0 RID: 3520
		private IssueEffect _clanInfluence;
	}
}
