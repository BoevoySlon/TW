using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000064 RID: 100
	public abstract class GameHandler : IEntityComponent
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001824D File Offset: 0x0001644D
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00018255 File Offset: 0x00016455
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001825D File Offset: 0x0001645D
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001825F File Offset: 0x0001645F
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00018261 File Offset: 0x00016461
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00018263 File Offset: 0x00016463
		protected internal virtual void OnGameStart()
		{
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00018265 File Offset: 0x00016465
		protected internal virtual void OnGameEnd()
		{
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00018267 File Offset: 0x00016467
		protected internal virtual void OnGameNetworkBegin()
		{
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00018269 File Offset: 0x00016469
		protected internal virtual void OnGameNetworkEnd()
		{
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001826B File Offset: 0x0001646B
		protected internal virtual void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001826D File Offset: 0x0001646D
		protected internal virtual void OnPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001826F File Offset: 0x0001646F
		protected internal virtual void OnPlayerDisconnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060006EE RID: 1774
		public abstract void OnBeforeSave();

		// Token: 0x060006EF RID: 1775
		public abstract void OnAfterSave();
	}
}
