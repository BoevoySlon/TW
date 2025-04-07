using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000091 RID: 145
	public interface ITrackableBase
	{
		// Token: 0x060007F6 RID: 2038
		TextObject GetName();

		// Token: 0x060007F7 RID: 2039
		Vec3 GetPosition();

		// Token: 0x060007F8 RID: 2040
		float GetTrackDistanceToMainAgent();

		// Token: 0x060007F9 RID: 2041
		bool CheckTracked(BasicCharacterObject basicCharacter);
	}
}
