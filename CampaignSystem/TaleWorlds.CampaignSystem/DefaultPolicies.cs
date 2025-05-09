﻿using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000076 RID: 118
	public class DefaultPolicies
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x000466C7 File Offset: 0x000448C7
		private static DefaultPolicies Instance
		{
			get
			{
				return Campaign.Current.DefaultPolicies;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x000466D3 File Offset: 0x000448D3
		public static PolicyObject LandTax
		{
			get
			{
				return DefaultPolicies.Instance._policyLandTax;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x000466DF File Offset: 0x000448DF
		public static PolicyObject StateMonopolies
		{
			get
			{
				return DefaultPolicies.Instance._policyStateMonopolies;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x000466EB File Offset: 0x000448EB
		public static PolicyObject SacredMajesty
		{
			get
			{
				return DefaultPolicies.Instance._policySacredMajesty;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x000466F7 File Offset: 0x000448F7
		public static PolicyObject Magistrates
		{
			get
			{
				return DefaultPolicies.Instance._policyMagistrates;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00046703 File Offset: 0x00044903
		public static PolicyObject DebasementOfTheCurrency
		{
			get
			{
				return DefaultPolicies.Instance._policyDebasementOfTheCurrency;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0004670F File Offset: 0x0004490F
		public static PolicyObject PrecarialLandTenure
		{
			get
			{
				return DefaultPolicies.Instance._policyPrecarialLandTenure;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0004671B File Offset: 0x0004491B
		public static PolicyObject CrownDuty
		{
			get
			{
				return DefaultPolicies.Instance._policyCrownDuty;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x00046727 File Offset: 0x00044927
		public static PolicyObject ImperialTowns
		{
			get
			{
				return DefaultPolicies.Instance._policyImperialTowns;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00046733 File Offset: 0x00044933
		public static PolicyObject RoyalCommissions
		{
			get
			{
				return DefaultPolicies.Instance._policyRoyalCommissions;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0004673F File Offset: 0x0004493F
		public static PolicyObject RoyalGuard
		{
			get
			{
				return DefaultPolicies.Instance._policyRoyalGuard;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x0004674B File Offset: 0x0004494B
		public static PolicyObject WarTax
		{
			get
			{
				return DefaultPolicies.Instance._policyWarTax;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x00046757 File Offset: 0x00044957
		public static PolicyObject RoyalPrivilege
		{
			get
			{
				return DefaultPolicies.Instance._policyRoyalPrivilege;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x00046763 File Offset: 0x00044963
		public static PolicyObject Senate
		{
			get
			{
				return DefaultPolicies.Instance._policySenate;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x0004676F File Offset: 0x0004496F
		public static PolicyObject LordsPrivyCouncil
		{
			get
			{
				return DefaultPolicies.Instance._policyLordsPrivyCouncil;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0004677B File Offset: 0x0004497B
		public static PolicyObject MilitaryCoronae
		{
			get
			{
				return DefaultPolicies.Instance._policyMilitaryCoronae;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x00046787 File Offset: 0x00044987
		public static PolicyObject FeudalInheritance
		{
			get
			{
				return DefaultPolicies.Instance._policyFeudalInheritance;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00046793 File Offset: 0x00044993
		public static PolicyObject Serfdom
		{
			get
			{
				return DefaultPolicies.Instance._policySerfdom;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0004679F File Offset: 0x0004499F
		public static PolicyObject NobleRetinues
		{
			get
			{
				return DefaultPolicies.Instance._policyNobleRetinues;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x000467AB File Offset: 0x000449AB
		public static PolicyObject CastleCharters
		{
			get
			{
				return DefaultPolicies.Instance._policyCastleCharters;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x000467B7 File Offset: 0x000449B7
		public static PolicyObject Bailiffs
		{
			get
			{
				return DefaultPolicies.Instance._policyBailiffs;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x000467C3 File Offset: 0x000449C3
		public static PolicyObject HuntingRights
		{
			get
			{
				return DefaultPolicies.Instance._policyHuntingRights;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x000467CF File Offset: 0x000449CF
		public static PolicyObject RoadTolls
		{
			get
			{
				return DefaultPolicies.Instance._policyRoadTolls;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x000467DB File Offset: 0x000449DB
		public static PolicyObject Marshals
		{
			get
			{
				return DefaultPolicies.Instance._policyMarshals;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x000467E7 File Offset: 0x000449E7
		public static PolicyObject CouncilOfTheCommons
		{
			get
			{
				return DefaultPolicies.Instance._policyCouncilOfTheCommons;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x000467F3 File Offset: 0x000449F3
		public static PolicyObject ForgivenessOfDebts
		{
			get
			{
				return DefaultPolicies.Instance._policyForgivenessOfDebts;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x000467FF File Offset: 0x000449FF
		public static PolicyObject Citizenship
		{
			get
			{
				return DefaultPolicies.Instance._policyCitizenship;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0004680B File Offset: 0x00044A0B
		public static PolicyObject TribunesOfThePeople
		{
			get
			{
				return DefaultPolicies.Instance._policyTribunesOfThePeople;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x00046817 File Offset: 0x00044A17
		public static PolicyObject GrazingRights
		{
			get
			{
				return DefaultPolicies.Instance._policyGrazingRights;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00046823 File Offset: 0x00044A23
		public static PolicyObject Lawspeakers
		{
			get
			{
				return DefaultPolicies.Instance._policyLawspeakers;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0004682F File Offset: 0x00044A2F
		public static PolicyObject TrialByJury
		{
			get
			{
				return DefaultPolicies.Instance._policyTrialByJury;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0004683B File Offset: 0x00044A3B
		public static PolicyObject Cantons
		{
			get
			{
				return DefaultPolicies.Instance._policyCantons;
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00046847 File Offset: 0x00044A47
		public DefaultPolicies()
		{
			this.RegisterAll();
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00046858 File Offset: 0x00044A58
		private void RegisterAll()
		{
			this._policyLandTax = this.Create("policy_land_tax");
			this._policyStateMonopolies = this.Create("policy_state_monopolies");
			this._policySacredMajesty = this.Create("policy_sacred_majesty");
			this._policyMagistrates = this.Create("policy_magistrates");
			this._policyDebasementOfTheCurrency = this.Create("policy_debasement_of_the_currency");
			this._policyPrecarialLandTenure = this.Create("policy_precarial_land_tenure");
			this._policyCrownDuty = this.Create("policy_crown_duty");
			this._policyImperialTowns = this.Create("policy_imperial_towns");
			this._policyRoyalCommissions = this.Create("policy_royal_commissions");
			this._policyRoyalGuard = this.Create("policy_royal_guard");
			this._policyWarTax = this.Create("policy_war_tax");
			this._policyRoyalPrivilege = this.Create("policy_royal_privilege");
			this._policySenate = this.Create("policy_senate");
			this._policyLordsPrivyCouncil = this.Create("policy_lords_privy_council");
			this._policyMilitaryCoronae = this.Create("policy_military_coronae");
			this._policyFeudalInheritance = this.Create("policy_feudal_inheritance");
			this._policySerfdom = this.Create("policy_serfdom");
			this._policyNobleRetinues = this.Create("policy_noble_retinues");
			this._policyCastleCharters = this.Create("policy_castle_charters");
			this._policyBailiffs = this.Create("policy_bailiffs");
			this._policyHuntingRights = this.Create("policy_hunting_rights");
			this._policyRoadTolls = this.Create("policy_road_tolls");
			this._policyMarshals = this.Create("policy_marshals");
			this._policyCouncilOfTheCommons = this.Create("policy_council_of_the_commons");
			this._policyCitizenship = this.Create("policy_citizenship");
			this._policyForgivenessOfDebts = this.Create("policy_forgiveness_of_debts");
			this._policyTribunesOfThePeople = this.Create("policy_tribunes_of_the_people");
			this._policyGrazingRights = this.Create("policy_grazing_rights");
			this._policyLawspeakers = this.Create("policy_lawspeakers");
			this._policyTrialByJury = this.Create("policy_trial_by_jury");
			this._policyCantons = this.Create("policy_cantons");
			this.InitializeAll();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00046A7A File Offset: 0x00044C7A
		private PolicyObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<PolicyObject>(new PolicyObject(stringId));
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00046A94 File Offset: 0x00044C94
		private void InitializeAll()
		{
			this._policyLandTax.Initialize(new TextObject("{=Tw2FaO0m}Land Tax", null), new TextObject("{=MWvzJSH1}A shift in the tax system that put more emphasis on property and less on the head tax charged to everyone could collect more from wealthy landowners.", null), new TextObject("{=0I6xdead}taxing landowners on their property", null), new TextObject("{=OWwPj500}5% of the village income is paid to the ruler clan as tax{newline}5% less village income for clans", null), 0.7f, 0.15f, -0.7f);
			this._policyStateMonopolies.Initialize(new TextObject("{=SXCudsWT}State Monopolies", null), new TextObject("{=2Qx4bL66}The ruler has a monopoly on certain goods, although practically he can license out production to merchants and collect a portion of the proceeds.", null), new TextObject("{=DIQF5bAX}giving the state monopolies on key goods", null), new TextObject("{=kIVfA5cv}Ruler clan gains 5% of settlement as tax per town{newline}Workshop production is decreased by 10%", null), 0.75f, 0.1f, -0.6f);
			this._policySacredMajesty.Initialize(new TextObject("{=rqHFujhr}Sacred Majesty", null), new TextObject("{=HQGUoRTW}The ruler is considered semi-divine and certain rituals are to be performed in his or her presence, increasing his or her air of authority.", null), new TextObject("{=00bd3Huo}performing new rituals that treat the ruler as semi-divine", null), new TextObject("{=RbjN7VQ2}Ruler clan earns 3 influence per day{newline}Non-ruler clans lose 0.5 influence per day", null), 0.85f, 0.1f, -0.9f);
			this._policyMagistrates.Initialize(new TextObject("{=ZhlpuMxb}Magistrates", null), new TextObject("{=QlZc9CNQ}Rulers could appoint magistrates to rule in disputes and solve crimes. This could cut down on gang activity and lawlessness, but was often greatly resented by communities.", null), new TextObject("{=E3b8iKjc}allowing the ruler to appoint magistrates", null), new TextObject("{=QLNfsk93}Town security is increased by 1 per day{newline}Town taxes are reduced by 5%", null), 0.6f, 0.35f, 0.1f);
			this._policyDebasementOfTheCurrency.Initialize(new TextObject("{=9lygvooT}Debasement Of The Currency", null), new TextObject("{=5WR5DsOz}Rulers could make money fast by debasing the currency and minting more, but this would cause prices to rise.", null), new TextObject("{=EmXai6dm}debasing the currency", null), new TextObject("{=6kn630ka}Ruler clan gains 100 denars per day for each town in the kingdom{newline}Settlement loyalty is decreased by 1 per day", null), 0.7f, 0.1f, -0.7f);
			this._policyPrecarialLandTenure.Initialize(new TextObject("{=ft08QZrA}Precarial Land Tenure", null), new TextObject("{=MF40cm9k}Land grants are considered to be temporary offices rather than the rightful inheritance of lords. In practice heirs tend to take over their family fiefs, but it's easier under Depositions to remove them.", null), new TextObject("{=Vl8BHCvF}allowing the ruler to easily revoke fiefs", null), new TextObject("{=6DQjKwt2}The influence cost of proposing settlement annexation is reduced by 50% for the ruler clan", null), 0.75f, 0f, -0.6f);
			this._policyCrownDuty.Initialize(new TextObject("{=dfyNVkFV}Crown Duty", null), new TextObject("{=elvb5y2I}The ruler is allowed to impose special taxes on trade in towns, payable directly to the royal or imperial treasury.", null), new TextObject("{=w3GIUD3u}allowing the ruler to collect special tariffs", null), new TextObject("{=bDANTRwA}5% tax on tariffs is paid to the ruler clan{newline}Higher trade penalty in towns{newline}Settlement prosperity is decreased by 1 per day", null), 0.75f, 0.15f, -0.4f);
			this._policyImperialTowns.Initialize(new TextObject("{=l5kDEI6N}Imperial Towns", null), new TextObject("{=ieIWHAtc}A ruler can grant towns special privileges based on their 'immediacy', special access to his person without going through lords or other vassals.", null), new TextObject("{=QoubP07U}allowing the ruler to grant special privileges to towns", null), new TextObject("{=8qwvZ15E}Towns held by the ruler clan gain 1 Loyalty and 1 Prosperity per day{newline}Towns held by non-ruler clans lose 0.3 Loyalty per day", null), 0.7f, 0.15f, -0.3f);
			this._policyRoyalCommissions.Initialize(new TextObject("{=XYDhkb6h}Royal Commissions", null), new TextObject("{=AzKq8AfI}In theory, the king or empire has the sole right to command men in the field. Anyone commanding an army does so in the king's name.", null), new TextObject("{=kPqs7EIf}giving the ruler more power to summon armies", null), new TextObject("{=L9n6yu1X}The influence cost of creating an army is reduced by 30% for the ruler{newline}Armies led by the ruler earn cohesion at 30% less cost{newline}Armies led by non-ruler nobles cost 10% more influence to create", null), 0.65f, 0f, -0.45f);
			this._policyRoyalGuard.Initialize(new TextObject("{=F41aPt80}Royal Guard", null), new TextObject("{=eibt105C}The ruler maintains a prestigious guard force. It attracts warriors who might otherwise serve their local lord.", null), new TextObject("{=bhoCBIWB}authorizing the ruler to have a large private bodyguard", null), new TextObject("{=GwAZMQ8b}Ruler's party size is increased by 60{newline}Non-ruling clans lose 0.2 influence per day.", null), 0.75f, 0f, -0.5f);
			this._policyWarTax.Initialize(new TextObject("{=AlZB8WIb}War Tax", null), new TextObject("{=b4TeiuoJ}Exceptional taxes were often applied in wartime.", null), new TextObject("{=76TgEba5}letting the ruler collect extra taxes in wartime", null), new TextObject("{=O4iki0FD}Ruler gains 5% tax from all settlements{newline}Towns lose 1 prosperity per day{newline}The influence cost of declaring war is doubled for the ruler clan", null), 0.7f, -0.1f, -0.65f);
			this._policyRoyalPrivilege.Initialize(new TextObject("{=Rl1AHKSp}Royal Privilege", null), new TextObject("{=ifnnu3g4}There is a long list of reasons why a ruler can reject a law passed by the council. A ruler does not need to search long to find an excuse for a veto.", null), new TextObject("{=aKLak7nn}giving the ruler broader powers to veto laws", null), new TextObject("{=DG3JbOa2}For kingdom decisions, the influence cost of the ruler overriding the popular decision outcome is reduced by 20%", null), 0.8f, -0.15f, -0.75f);
			this._policySenate.Initialize(new TextObject("{=8pjMAqOg}Senate", null), new TextObject("{=D3W9Qi0Z}All lords have a formal role on the council.", null), new TextObject("{=lXSeaba5}having the lords of the realm meet as a permanent council", null), new TextObject("{=TsvBHBdX}Tier 3+ clans gain 0.5 influence per day, influence cost of inviting lower tier clans to army are increased by 10%", null), -0.7f, 0.85f, 0.7f);
			this._policyLordsPrivyCouncil.Initialize(new TextObject("{=JaZ7T2Wj}Lords' Privy Council", null), new TextObject("{=on2EmlUT}A small council of the greatest lords of the realm. This gives the main clans extra influence, but prevents other clans from climbing into their ranks.", null), new TextObject("{=bxWITUaN}having the greatest lords of the realm meet as a small privy council", null), new TextObject("{=LpdAa1NY}Tier 5+ clans gain 0.5 influence per day, influence cost of inviting lower tier clans to army are increased by 20%", null), -0.5f, 0.7f, -0.15f);
			this._policyMilitaryCoronae.Initialize(new TextObject("{=IBlJ42MN}Military Coronae", null), new TextObject("{=ceq6ZMIx}Military achievements are favored and lords can vote to award each other decorations and distinctions (ie, the Roman corona.)", null), new TextObject("{=EG06bDxi}granting awards for deeds in the field", null), new TextObject("{=uCXGZ2YP}Military achievements grant 20% more influence{newline}Troop wages are increased by 10%", null), -0.15f, 0.6f, 0.35f);
			this._policyFeudalInheritance.Initialize(new TextObject("{=xbvei0Cb}Feudal Inheritance", null), new TextObject("{=AlFTInfU}States with strict and formal laws of inheritance make it more difficult to revoke land.", null), new TextObject("{=fj5mYvNE}recognizing the formal inheritance of fiefs", null), new TextObject("{=aWWzrwAw}The cost of revoking a fief from a clan is doubled{newline}Clans gain 0.1 influence for each fief they own", null), -0.75f, 0.75f, 0.65f);
			this._policySerfdom.Initialize(new TextObject("{=8FPCRv5L}Serfdom", null), new TextObject("{=0Qh7Pa9E}Tenants are forbidden from leaving the lands of their lords without notice.", null), new TextObject("{=5Wld45hP}forbidding tenants from leaving their lords' lands", null), new TextObject("{=H9Px95rR}Villages grant 0.2 influence per day to the owner clan{newline}Towns gain 1 security but lose 1 militia per day", null), -0.4f, 0.5f, -0.25f);
			this._policyNobleRetinues.Initialize(new TextObject("{=7Pk3bFPC}Noble Retinues", null), new TextObject("{=yXb8bphB}Nobles are expected to raise sizable retinues.", null), new TextObject("{=W3xUQxAa}encouraging lords to have large private armies", null), new TextObject("{=LIjN3rYD}Tier 5+ clans lose 1 influence per day and the party size of their leaders is increased by 40", null), -0.35f, 0.65f, -0.45f);
			this._policyCastleCharters.Initialize(new TextObject("{=W6XMWJ8R}Castle Charters", null), new TextObject("{=t8TfagYL}Nobles are encouraged to fortify their estates, and can requisition labor and materials to do so.", null), new TextObject("{=hsIvt3WC}encouraging lords to fortify their estates", null), new TextObject("{=RdbLbpgO}Castle upgrade costs are reduced by 20%", null), -0.65f, 0.45f, 0f);
			this._policyBailiffs.Initialize(new TextObject("{=HKT5MnjP}Bailiffs", null), new TextObject("{=nmnp9S7k}Nobles have the right to appoint bailiffs.", null), new TextObject("{=GFnqIuxy}encouraging lords to appoint bailiffs in their fiefs", null), new TextObject("{=XFcmlN1J}Town security is increased by 1 per day{newline}Towns with a security greater than 60 yield 1 additional influence to the owner clan.{newline}Tax from towns are reduced by 5%", null), 0f, 0.4f, -0.1f);
			this._policyHuntingRights.Initialize(new TextObject("{=0mKYUNb8}Hunting Rights", null), new TextObject("{=ZMTkq5TG}Nobles and other landowners have exclusive rights to hunt in forests.", null), new TextObject("{=gOn7a7if}granting lords exclusive hunting rights to nearby forests", null), new TextObject("{=agaSYd5t}Food production in towns and castles are increased by 2{newline}Town loyalty is decreased by 0.2", null), -0.2f, 0.35f, -0.15f);
			this._policyRoadTolls.Initialize(new TextObject("{=bOtYmSP8}Road Tolls", null), new TextObject("{=nP7KOISK}Local landowners have the right to collect tolls on commerce.", null), new TextObject("{=upK62aLR}allowing lords tolls on roads running through their lands", null), new TextObject("{=dLkfbU0a}Trade tax paid to the town owner is increased by 3%{newline}Town prosperity is decreased by 0.2", null), -0.5f, 0.45f, -0.35f);
			this._policyMarshals.Initialize(new TextObject("{=WSU35a7F}Marshals", null), new TextObject("{=7EP0NgzU}The highest ranking of nobles have the de facto right to assemble large armies.", null), new TextObject("{=cBAVR7e8}granting high-ranking nobles the right to summon large armies", null), new TextObject("{=0wxRZ9AV}Armies led by Tier 5+ nobles require 10% less influence{newline}Influence of the ruler clan is reduced by 1 per day", null), -0.45f, 0.5f, 0f);
			this._policyCouncilOfTheCommons.Initialize(new TextObject("{=bMSI9Bt3}Council of the Commons", null), new TextObject("{=55CsWKbg}Some kingdoms, especially those that evolved from a city-state or a tribe, had popular assemblies that most of its members had the right to attend. Its powers were often limited, since it could only meet periodically, but it still gave the public the right to participate in government.", null), new TextObject("{=srByX06Y}letting all citizens meet and vote on some issues", null), new TextObject("{=UZ0mPm8b}Each notable yields 0.1 influence per day to the settlement's owner clan{newline}Tax from fortifications 5% decreased", null), -0.5f, 0.1f, 0.7f);
			this._policyForgivenessOfDebts.Initialize(new TextObject("{=Vzsu5nZV}Forgiveness of Debts", null), new TextObject("{=Lgmisw4L}Limits the degree to which lords and merchants can lend to their tenants and employees and then demand repayment, or seize their assets or their freedom. Effectively bans serfdom.", null), new TextObject("{=9YKV4SNH}restricting what creditor may do to collect debts", null), new TextObject("{=xJ2uDcob}Settlement loyalty is increased by 2 per day{newline}Settlement production is reduced by 5%", null), -0.4f, -0.4f, 0.6f);
			this._policyCitizenship.Initialize(new TextObject("{=sYNFwOVg}Citizenship", null), new TextObject("{=O5sBO9sQ}Many empires granted their populations citizenship, which usually came with a series of rights. Of course, citizenship could not be granted immediately to conquered provinces until the population showed it was willing to adopt the ways of the empire, including the language, clothes, and religious cults.", null), new TextObject("{=dvEkfaab}recognizing the common folk in the realm as full citizens", null), new TextObject("{=qEOXka0Q}+0.5 Loyalty per day to settlements that have the same culture as their owner clan{newline}Settlement militia production is increased by 1{newline}-0.5 Loyalty per day to settlements with a different culture than its owner clan", null), -0.65f, -0.35f, 0.7f);
			this._policyTribunesOfThePeople.Initialize(new TextObject("{=IJdGTOAe}Tribunes of the People", null), new TextObject("{=VzmzX9Ln}Tribunes of the Plebs were Roman Republican offices. They were designed to give representation to families without patrician standing, and could veto legislation from the Senate.", null), new TextObject("{=auftprN9}allowing the common people to elect tribes to represent them", null), new TextObject("{=YOBeOFxY}Town taxes paid to the ruler are reduced by 5%{newline}Town loyalty is increased by 1 per day", null), -0.6f, -0.2f, 0.55f);
			this._policyGrazingRights.Initialize(new TextObject("{=mb25Ue3f}Grazing Rights", null), new TextObject("{=fjj0pJXV}Landowners could often assert legal rights to common areas and charge villages money to use them. If ordinary people petitioned a ruler, however, he might give them the right to use all common areas for hunting or grazing as members of the village.", null), new TextObject("{=1Y5p40uP}granting villagers the right to graze on land held in common", null), new TextObject("{=PG8anNca}Settlement loyalty is increased by 0.5 per day{newline}Daily hearth production at villages decreases by 0.25 per day", null), -0.75f, -0.3f, 0.7f);
			this._policyLawspeakers.Initialize(new TextObject("{=EBCV0LcU}Lawspeakers", null), new TextObject("{=U7s1LycQ}Refers to the Norse practice of appointing independent elders to remind the council of the law and past precedents. This tends to favor those with the education to make complex legal arguments.", null), new TextObject("{=7kNxogN8}appointing independent elders to uphold the law", null), new TextObject("{=bFEdxs6Y}All clans whose leader has high Charm gain 1 influence per day{newline}All clans whose leader has low Charm lose 1 influence per day", null), 0f, 0.25f, 0.45f);
			this._policyTrialByJury.Initialize(new TextObject("{=yNAzCfxc}Trial by Jury", null), new TextObject("{=InFseOAA}This limits the ability of magistrates to condemn those they consider criminals quickly. It prevents arbitrary abuse of power, but landowners or gang leaders can sometimes use threats or bribes to manipulate it.", null), new TextObject("{=L9aNOiJo}granting those accused of major crimes the right to trial by jury", null), new TextObject("{=ZJ2tJnJk}Settlement loyalty is increased by 0.5 per day{newline}Settlement security is decreased by 0.2 per day{newline}Clans lose 1 influence per day", null), -0.3f, 0.1f, 0.6f);
			this._policyCantons.Initialize(new TextObject("{=D6YLpUQa}Cantons", null), new TextObject("{=FMUlfbJf}Rulers organize farmers into groups of households responsible for supplying troops. This makes recruiting easier, but at the cost of their economic productivity.", null), new TextObject("{=PXhIFXbv}organizing households to supply military recruits", null), new TextObject("{=bPpdw81a}Daily militia production is increased by 1{newline}Recruits replenish 20% faster{newline}Tax income in settlements are reduced by 10%", null), -0.2f, -0.1f, 0.4f);
		}

		// Token: 0x040004C4 RID: 1220
		private PolicyObject _policyLandTax;

		// Token: 0x040004C5 RID: 1221
		private PolicyObject _policyStateMonopolies;

		// Token: 0x040004C6 RID: 1222
		private PolicyObject _policySacredMajesty;

		// Token: 0x040004C7 RID: 1223
		private PolicyObject _policyMagistrates;

		// Token: 0x040004C8 RID: 1224
		private PolicyObject _policyDebasementOfTheCurrency;

		// Token: 0x040004C9 RID: 1225
		private PolicyObject _policyPrecarialLandTenure;

		// Token: 0x040004CA RID: 1226
		private PolicyObject _policyCrownDuty;

		// Token: 0x040004CB RID: 1227
		private PolicyObject _policyImperialTowns;

		// Token: 0x040004CC RID: 1228
		private PolicyObject _policyRoyalCommissions;

		// Token: 0x040004CD RID: 1229
		private PolicyObject _policyRoyalGuard;

		// Token: 0x040004CE RID: 1230
		private PolicyObject _policyWarTax;

		// Token: 0x040004CF RID: 1231
		private PolicyObject _policyRoyalPrivilege;

		// Token: 0x040004D0 RID: 1232
		private PolicyObject _policySenate;

		// Token: 0x040004D1 RID: 1233
		private PolicyObject _policyLordsPrivyCouncil;

		// Token: 0x040004D2 RID: 1234
		private PolicyObject _policyMilitaryCoronae;

		// Token: 0x040004D3 RID: 1235
		private PolicyObject _policyFeudalInheritance;

		// Token: 0x040004D4 RID: 1236
		private PolicyObject _policySerfdom;

		// Token: 0x040004D5 RID: 1237
		private PolicyObject _policyNobleRetinues;

		// Token: 0x040004D6 RID: 1238
		private PolicyObject _policyCastleCharters;

		// Token: 0x040004D7 RID: 1239
		private PolicyObject _policyBailiffs;

		// Token: 0x040004D8 RID: 1240
		private PolicyObject _policyHuntingRights;

		// Token: 0x040004D9 RID: 1241
		private PolicyObject _policyRoadTolls;

		// Token: 0x040004DA RID: 1242
		private PolicyObject _policyMarshals;

		// Token: 0x040004DB RID: 1243
		private PolicyObject _policyCouncilOfTheCommons;

		// Token: 0x040004DC RID: 1244
		private PolicyObject _policyCitizenship;

		// Token: 0x040004DD RID: 1245
		private PolicyObject _policyForgivenessOfDebts;

		// Token: 0x040004DE RID: 1246
		private PolicyObject _policyTribunesOfThePeople;

		// Token: 0x040004DF RID: 1247
		private PolicyObject _policyGrazingRights;

		// Token: 0x040004E0 RID: 1248
		private PolicyObject _policyLawspeakers;

		// Token: 0x040004E1 RID: 1249
		private PolicyObject _policyTrialByJury;

		// Token: 0x040004E2 RID: 1250
		private PolicyObject _policyCantons;
	}
}
