using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000068 RID: 104
	public abstract class GameManagerComponent : IEntityComponent
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x000186FA File Offset: 0x000168FA
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x00018702 File Offset: 0x00016902
		public GameManagerBase GameManager { get; internal set; }

		// Token: 0x0600071C RID: 1820 RVA: 0x0001870B File Offset: 0x0001690B
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00018713 File Offset: 0x00016913
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00018715 File Offset: 0x00016915
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001871D File Offset: 0x0001691D
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001871F File Offset: 0x0001691F
		protected internal virtual void OnTick()
		{
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00018721 File Offset: 0x00016921
		protected internal virtual void OnPlayerDisconnect(VirtualPlayer peer)
		{
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00018723 File Offset: 0x00016923
		protected internal virtual void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00018725 File Offset: 0x00016925
		protected internal virtual void OnPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00018727 File Offset: 0x00016927
		protected internal virtual void OnGameNetworkBegin()
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00018729 File Offset: 0x00016929
		protected internal virtual void OnGameNetworkEnd()
		{
		}
	}
}
