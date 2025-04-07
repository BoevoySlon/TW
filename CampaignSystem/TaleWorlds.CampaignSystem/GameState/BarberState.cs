using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200032C RID: 812
	public class BarberState : GameState
	{
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06002E6A RID: 11882 RVA: 0x000C1D9C File Offset: 0x000BFF9C
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06002E6B RID: 11883 RVA: 0x000C1D9F File Offset: 0x000BFF9F
		// (set) Token: 0x06002E6C RID: 11884 RVA: 0x000C1DA7 File Offset: 0x000BFFA7
		public IFaceGeneratorCustomFilter Filter { get; private set; }

		// Token: 0x06002E6D RID: 11885 RVA: 0x000C1DB0 File Offset: 0x000BFFB0
		public BarberState()
		{
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000C1DB8 File Offset: 0x000BFFB8
		public BarberState(BasicCharacterObject character, IFaceGeneratorCustomFilter filter)
		{
			this.Character = character;
			this.Filter = filter;
		}

		// Token: 0x04000DE0 RID: 3552
		public BasicCharacterObject Character;
	}
}
