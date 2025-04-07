using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000335 RID: 821
	public class GameOverState : GameState
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002E99 RID: 11929 RVA: 0x000C1F70 File Offset: 0x000C0170
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x000C1F73 File Offset: 0x000C0173
		// (set) Token: 0x06002E9B RID: 11931 RVA: 0x000C1F7B File Offset: 0x000C017B
		public IGameOverStateHandler Handler
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

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x000C1F84 File Offset: 0x000C0184
		// (set) Token: 0x06002E9D RID: 11933 RVA: 0x000C1F8C File Offset: 0x000C018C
		public GameOverState.GameOverReason Reason { get; private set; }

		// Token: 0x06002E9E RID: 11934 RVA: 0x000C1F95 File Offset: 0x000C0195
		public GameOverState()
		{
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000C1F9D File Offset: 0x000C019D
		public GameOverState(GameOverState.GameOverReason reason)
		{
			this.Reason = reason;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000C1FAC File Offset: 0x000C01AC
		public static GameOverState CreateForVictory()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[]
			{
				GameOverState.GameOverReason.Victory
			});
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000C1FD2 File Offset: 0x000C01D2
		public static GameOverState CreateForRetirement()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[]
			{
				GameOverState.GameOverReason.Retirement
			});
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000C1FF8 File Offset: 0x000C01F8
		public static GameOverState CreateForClanDestroyed()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[]
			{
				GameOverState.GameOverReason.ClanDestroyed
			});
		}

		// Token: 0x04000DEE RID: 3566
		private IGameOverStateHandler _handler;

		// Token: 0x02000686 RID: 1670
		public enum GameOverReason
		{
			// Token: 0x04001B7A RID: 7034
			Retirement,
			// Token: 0x04001B7B RID: 7035
			ClanDestroyed,
			// Token: 0x04001B7C RID: 7036
			Victory
		}
	}
}
