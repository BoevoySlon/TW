using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200032F RID: 815
	public class ClanState : GameState
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000C1E0A File Offset: 0x000C000A
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06002E77 RID: 11895 RVA: 0x000C1E0D File Offset: 0x000C000D
		// (set) Token: 0x06002E78 RID: 11896 RVA: 0x000C1E15 File Offset: 0x000C0015
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x000C1E1E File Offset: 0x000C001E
		// (set) Token: 0x06002E7A RID: 11898 RVA: 0x000C1E26 File Offset: 0x000C0026
		public PartyBase InitialSelectedParty { get; private set; }

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x000C1E2F File Offset: 0x000C002F
		// (set) Token: 0x06002E7C RID: 11900 RVA: 0x000C1E37 File Offset: 0x000C0037
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000C1E40 File Offset: 0x000C0040
		// (set) Token: 0x06002E7E RID: 11902 RVA: 0x000C1E48 File Offset: 0x000C0048
		public Workshop InitialSelectedWorkshop { get; private set; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002E7F RID: 11903 RVA: 0x000C1E51 File Offset: 0x000C0051
		// (set) Token: 0x06002E80 RID: 11904 RVA: 0x000C1E59 File Offset: 0x000C0059
		public Alley InitialSelectedAlley { get; private set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x000C1E62 File Offset: 0x000C0062
		// (set) Token: 0x06002E82 RID: 11906 RVA: 0x000C1E6A File Offset: 0x000C006A
		public IClanStateHandler Handler
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

		// Token: 0x06002E83 RID: 11907 RVA: 0x000C1E73 File Offset: 0x000C0073
		public ClanState()
		{
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000C1E7B File Offset: 0x000C007B
		public ClanState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000C1E8A File Offset: 0x000C008A
		public ClanState(PartyBase initialSelectedParty)
		{
			this.InitialSelectedParty = initialSelectedParty;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000C1E99 File Offset: 0x000C0099
		public ClanState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000C1EA8 File Offset: 0x000C00A8
		public ClanState(Workshop initialSelectedWorkshop)
		{
			this.InitialSelectedWorkshop = initialSelectedWorkshop;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000C1EB7 File Offset: 0x000C00B7
		public ClanState(Alley initialSelectedAlley)
		{
			this.InitialSelectedAlley = initialSelectedAlley;
		}

		// Token: 0x04000DE9 RID: 3561
		private IClanStateHandler _handler;
	}
}
