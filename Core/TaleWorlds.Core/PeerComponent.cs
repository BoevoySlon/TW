using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000B7 RID: 183
	public abstract class PeerComponent : IEntityComponent
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0001F3BC File Offset: 0x0001D5BC
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			set
			{
				this._peer = value;
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001F3CD File Offset: 0x0001D5CD
		public virtual void Initialize()
		{
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0001F3CF File Offset: 0x0001D5CF
		public string Name
		{
			get
			{
				return this.Peer.UserName;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0001F3DC File Offset: 0x0001D5DC
		public bool IsMine
		{
			get
			{
				return this.Peer.IsMine;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001F3E9 File Offset: 0x0001D5E9
		public T GetComponent<T>() where T : PeerComponent
		{
			return this.Peer.GetComponent<T>();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001F3F6 File Offset: 0x0001D5F6
		public virtual void OnInitialize()
		{
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		public virtual void OnFinalize()
		{
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0001F3FA File Offset: 0x0001D5FA
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0001F402 File Offset: 0x0001D602
		public uint TypeId { get; set; }

		// Token: 0x04000570 RID: 1392
		private VirtualPlayer _peer;
	}
}
