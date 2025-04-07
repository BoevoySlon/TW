using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000B8 RID: 184
	public abstract class PlayerGameState : GameState
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001F413 File Offset: 0x0001D613
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0001F41B File Offset: 0x0001D61B
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			private set
			{
				this._peer = value;
			}
		}

		// Token: 0x04000572 RID: 1394
		private VirtualPlayer _peer;
	}
}
