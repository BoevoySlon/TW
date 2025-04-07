using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core
{
	// Token: 0x02000052 RID: 82
	public class DummyCommunicator : ICommunicator
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000167C9 File Offset: 0x000149C9
		public VirtualPlayer VirtualPlayer { get; }

		// Token: 0x0600063A RID: 1594 RVA: 0x000167D1 File Offset: 0x000149D1
		public void OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component)
		{
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000167D3 File Offset: 0x000149D3
		public void OnAddComponent(PeerComponent component)
		{
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000167D5 File Offset: 0x000149D5
		public void OnRemoveComponent(PeerComponent component)
		{
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x000167D7 File Offset: 0x000149D7
		public bool IsNetworkActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x000167DA File Offset: 0x000149DA
		public bool IsConnectionActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x000167DD File Offset: 0x000149DD
		public bool IsServerPeer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000167E0 File Offset: 0x000149E0
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x000167E3 File Offset: 0x000149E3
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000167E5 File Offset: 0x000149E5
		private DummyCommunicator(int index, string name)
		{
			this.VirtualPlayer = new VirtualPlayer(index, name, PlayerId.Empty, this);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00016800 File Offset: 0x00014A00
		public static DummyCommunicator CreateAsServer(int index, string name)
		{
			return new DummyCommunicator(index, name);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00016809 File Offset: 0x00014A09
		public static DummyCommunicator CreateAsClient(string name, int index)
		{
			return new DummyCommunicator(index, name);
		}
	}
}
