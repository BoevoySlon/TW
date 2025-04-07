using System;

namespace Helpers
{
	// Token: 0x02000015 RID: 21
	public static class BoardGameHelper
	{
		// Token: 0x0200046E RID: 1134
		public enum AIDifficulty
		{
			// Token: 0x04001321 RID: 4897
			Easy,
			// Token: 0x04001322 RID: 4898
			Normal,
			// Token: 0x04001323 RID: 4899
			Hard,
			// Token: 0x04001324 RID: 4900
			NumTypes
		}

		// Token: 0x0200046F RID: 1135
		public enum BoardGameState
		{
			// Token: 0x04001326 RID: 4902
			None,
			// Token: 0x04001327 RID: 4903
			Win,
			// Token: 0x04001328 RID: 4904
			Loss,
			// Token: 0x04001329 RID: 4905
			Draw
		}
	}
}
