using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200033A RID: 826
	public class KingdomState : GameState
	{
		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x000C2054 File Offset: 0x000C0254
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000C2057 File Offset: 0x000C0257
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000C205F File Offset: 0x000C025F
		public Army InitialSelectedArmy { get; private set; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000C2068 File Offset: 0x000C0268
		// (set) Token: 0x06002EBC RID: 11964 RVA: 0x000C2070 File Offset: 0x000C0270
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000C2079 File Offset: 0x000C0279
		// (set) Token: 0x06002EBE RID: 11966 RVA: 0x000C2081 File Offset: 0x000C0281
		public Clan InitialSelectedClan { get; private set; }

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x000C208A File Offset: 0x000C028A
		// (set) Token: 0x06002EC0 RID: 11968 RVA: 0x000C2092 File Offset: 0x000C0292
		public PolicyObject InitialSelectedPolicy { get; private set; }

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x000C209B File Offset: 0x000C029B
		// (set) Token: 0x06002EC2 RID: 11970 RVA: 0x000C20A3 File Offset: 0x000C02A3
		public Kingdom InitialSelectedKingdom { get; private set; }

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000C20AC File Offset: 0x000C02AC
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x000C20B4 File Offset: 0x000C02B4
		public KingdomDecision InitialSelectedDecision { get; private set; }

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000C20BD File Offset: 0x000C02BD
		// (set) Token: 0x06002EC6 RID: 11974 RVA: 0x000C20C5 File Offset: 0x000C02C5
		public IKingdomStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000C20CE File Offset: 0x000C02CE
		public KingdomState()
		{
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000C20D6 File Offset: 0x000C02D6
		public KingdomState(KingdomDecision initialSelectedDecision)
		{
			this.InitialSelectedDecision = initialSelectedDecision;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000C20E5 File Offset: 0x000C02E5
		public KingdomState(Army initialSelectedArmy)
		{
			this.InitialSelectedArmy = initialSelectedArmy;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000C20F4 File Offset: 0x000C02F4
		public KingdomState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000C2104 File Offset: 0x000C0304
		public KingdomState(IFaction initialSelectedFaction)
		{
			Clan initialSelectedClan;
			if ((initialSelectedClan = (initialSelectedFaction as Clan)) != null)
			{
				this.InitialSelectedClan = initialSelectedClan;
				return;
			}
			Kingdom initialSelectedKingdom;
			if ((initialSelectedKingdom = (initialSelectedFaction as Kingdom)) != null)
			{
				this.InitialSelectedKingdom = initialSelectedKingdom;
			}
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000C213A File Offset: 0x000C033A
		public KingdomState(PolicyObject initialSelectedPolicy)
		{
			this.InitialSelectedPolicy = initialSelectedPolicy;
		}

		// Token: 0x04000DF8 RID: 3576
		private IKingdomStateHandler _handler;
	}
}
