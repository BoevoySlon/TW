using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000045 RID: 69
	public struct PieceData
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00013F80 File Offset: 0x00012180
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00013F88 File Offset: 0x00012188
		public CraftingPiece.PieceTypes PieceType { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00013F91 File Offset: 0x00012191
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00013F99 File Offset: 0x00012199
		public int Order { get; private set; }

		// Token: 0x0600058B RID: 1419 RVA: 0x00013FA2 File Offset: 0x000121A2
		public PieceData(CraftingPiece.PieceTypes pieceType, int order)
		{
			this.PieceType = pieceType;
			this.Order = order;
		}
	}
}
