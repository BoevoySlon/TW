using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000022 RID: 34
	public class BattleSimulationResult
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000EBB7 File Offset: 0x0000CDB7
		// (set) Token: 0x0600014E RID: 334 RVA: 0x0000EBBF File Offset: 0x0000CDBF
		public UniqueTroopDescriptor TroopDescriptor { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000EBD0 File Offset: 0x0000CDD0
		public BattleSideEnum Side { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000EBD9 File Offset: 0x0000CDD9
		// (set) Token: 0x06000152 RID: 338 RVA: 0x0000EBE1 File Offset: 0x0000CDE1
		public TroopProperty TroopProperty { get; private set; }

		// Token: 0x06000153 RID: 339 RVA: 0x0000EBEA File Offset: 0x0000CDEA
		public BattleSimulationResult(UniqueTroopDescriptor troopDescriptor, BattleSideEnum side, TroopProperty troopProperty)
		{
			this.TroopDescriptor = troopDescriptor;
			this.Side = side;
			this.TroopProperty = troopProperty;
		}
	}
}
