using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007D RID: 125
	public interface ICommunicator
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060007B6 RID: 1974
		VirtualPlayer VirtualPlayer { get; }

		// Token: 0x060007B7 RID: 1975
		void OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component);

		// Token: 0x060007B8 RID: 1976
		void OnAddComponent(PeerComponent component);

		// Token: 0x060007B9 RID: 1977
		void OnRemoveComponent(PeerComponent component);

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060007BA RID: 1978
		bool IsNetworkActive { get; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060007BB RID: 1979
		bool IsConnectionActive { get; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060007BC RID: 1980
		bool IsServerPeer { get; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060007BD RID: 1981
		// (set) Token: 0x060007BE RID: 1982
		bool IsSynchronized { get; set; }
	}
}
