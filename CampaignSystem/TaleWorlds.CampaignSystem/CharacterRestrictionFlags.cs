using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000074 RID: 116
	[Flags]
	public enum CharacterRestrictionFlags : uint
	{
		// Token: 0x040004AD RID: 1197
		None = 0U,
		// Token: 0x040004AE RID: 1198
		NotTransferableInPartyScreen = 1U,
		// Token: 0x040004AF RID: 1199
		CanNotGoInHideout = 2U
	}
}
