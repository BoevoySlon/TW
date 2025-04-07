using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200001C RID: 28
	public static class BattleSideEnumExtensions
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000063A4 File Offset: 0x000045A4
		public static bool IsValid(this BattleSideEnum battleSide)
		{
			return battleSide >= BattleSideEnum.Defender && battleSide < BattleSideEnum.NumSides;
		}
	}
}
