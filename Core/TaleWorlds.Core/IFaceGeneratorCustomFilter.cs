using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000080 RID: 128
	public interface IFaceGeneratorCustomFilter
	{
		// Token: 0x060007CD RID: 1997
		int[] GetHaircutIndices(BasicCharacterObject character);

		// Token: 0x060007CE RID: 1998
		int[] GetFacialHairIndices(BasicCharacterObject character);

		// Token: 0x060007CF RID: 1999
		FaceGeneratorStage[] GetAvailableStages();
	}
}
