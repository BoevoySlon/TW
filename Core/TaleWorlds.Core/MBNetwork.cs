using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A7 RID: 167
	public static class MBNetwork
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0001C320 File Offset: 0x0001A520
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x0001C327 File Offset: 0x0001A527
		public static INetworkCommunication NetworkViewCommunication { get; private set; }

		// Token: 0x06000852 RID: 2130 RVA: 0x0001C32F File Offset: 0x0001A52F
		public static void Initialize(INetworkCommunication networkCommunication)
		{
			MBNetwork.NetworkViewCommunication = networkCommunication;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0001C337 File Offset: 0x0001A537
		public static VirtualPlayer MyPeer
		{
			get
			{
				if (MBNetwork.NetworkViewCommunication != null)
				{
					return MBNetwork.NetworkViewCommunication.MyPeer;
				}
				return null;
			}
		}
	}
}
