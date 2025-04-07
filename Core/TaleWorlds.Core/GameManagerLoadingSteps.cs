using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000065 RID: 101
	public enum GameManagerLoadingSteps
	{
		// Token: 0x0400039F RID: 927
		None = -1,
		// Token: 0x040003A0 RID: 928
		PreInitializeZerothStep,
		// Token: 0x040003A1 RID: 929
		FirstInitializeFirstStep,
		// Token: 0x040003A2 RID: 930
		WaitSecondStep,
		// Token: 0x040003A3 RID: 931
		SecondInitializeThirdState,
		// Token: 0x040003A4 RID: 932
		PostInitializeFourthState,
		// Token: 0x040003A5 RID: 933
		FinishLoadingFifthStep,
		// Token: 0x040003A6 RID: 934
		LoadingIsOver
	}
}
