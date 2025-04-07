using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008A RID: 138
	public interface IMissionSiegeWeapon
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060007EC RID: 2028
		int Index { get; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060007ED RID: 2029
		SiegeEngineType Type { get; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060007EE RID: 2030
		float Health { get; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060007EF RID: 2031
		float InitialHealth { get; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060007F0 RID: 2032
		float MaxHealth { get; }
	}
}
