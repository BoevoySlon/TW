using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200032D RID: 813
	public class CharacterDeveloperState : GameState
	{
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06002E6F RID: 11887 RVA: 0x000C1DCE File Offset: 0x000BFFCE
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06002E70 RID: 11888 RVA: 0x000C1DD1 File Offset: 0x000BFFD1
		// (set) Token: 0x06002E71 RID: 11889 RVA: 0x000C1DD9 File Offset: 0x000BFFD9
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x06002E72 RID: 11890 RVA: 0x000C1DE2 File Offset: 0x000BFFE2
		public CharacterDeveloperState()
		{
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000C1DEA File Offset: 0x000BFFEA
		public CharacterDeveloperState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06002E74 RID: 11892 RVA: 0x000C1DF9 File Offset: 0x000BFFF9
		// (set) Token: 0x06002E75 RID: 11893 RVA: 0x000C1E01 File Offset: 0x000C0001
		public ICharacterDeveloperStateHandler Handler
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

		// Token: 0x04000DE3 RID: 3555
		private ICharacterDeveloperStateHandler _handler;
	}
}
