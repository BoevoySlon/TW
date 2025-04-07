using System;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002A8 RID: 680
	public struct TroopTradeDifference
	{
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x000A8A24 File Offset: 0x000A6C24
		// (set) Token: 0x06002771 RID: 10097 RVA: 0x000A8A2C File Offset: 0x000A6C2C
		public CharacterObject Troop { get; set; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x000A8A35 File Offset: 0x000A6C35
		// (set) Token: 0x06002773 RID: 10099 RVA: 0x000A8A3D File Offset: 0x000A6C3D
		public bool IsPrisoner { get; set; }

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x000A8A46 File Offset: 0x000A6C46
		// (set) Token: 0x06002775 RID: 10101 RVA: 0x000A8A4E File Offset: 0x000A6C4E
		public int FromCount { get; set; }

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x000A8A57 File Offset: 0x000A6C57
		// (set) Token: 0x06002777 RID: 10103 RVA: 0x000A8A5F File Offset: 0x000A6C5F
		public int ToCount { get; set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000A8A68 File Offset: 0x000A6C68
		public int DifferenceCount
		{
			get
			{
				return this.FromCount - this.ToCount;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x000A8A77 File Offset: 0x000A6C77
		// (set) Token: 0x0600277A RID: 10106 RVA: 0x000A8A7F File Offset: 0x000A6C7F
		public bool IsEmpty { get; private set; }

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000A8A88 File Offset: 0x000A6C88
		public static TroopTradeDifference Empty
		{
			get
			{
				return new TroopTradeDifference
				{
					IsEmpty = true
				};
			}
		}
	}
}
