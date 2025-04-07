using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008E RID: 142
	[Flags]
	public enum ItemFlags : uint
	{
		// Token: 0x0400040A RID: 1034
		ForceAttachOffHandPrimaryItemBone = 256U,
		// Token: 0x0400040B RID: 1035
		ForceAttachOffHandSecondaryItemBone = 512U,
		// Token: 0x0400040C RID: 1036
		AttachmentMask = 768U,
		// Token: 0x0400040D RID: 1037
		NotUsableByFemale = 1024U,
		// Token: 0x0400040E RID: 1038
		NotUsableByMale = 2048U,
		// Token: 0x0400040F RID: 1039
		DropOnWeaponChange = 4096U,
		// Token: 0x04000410 RID: 1040
		DropOnAnyAction = 8192U,
		// Token: 0x04000411 RID: 1041
		CannotBePickedUp = 16384U,
		// Token: 0x04000412 RID: 1042
		CanBePickedUpFromCorpse = 32768U,
		// Token: 0x04000413 RID: 1043
		QuickFadeOut = 65536U,
		// Token: 0x04000414 RID: 1044
		WoodenAttack = 131072U,
		// Token: 0x04000415 RID: 1045
		WoodenParry = 262144U,
		// Token: 0x04000416 RID: 1046
		HeldInOffHand = 524288U,
		// Token: 0x04000417 RID: 1047
		HasToBeHeldUp = 1048576U,
		// Token: 0x04000418 RID: 1048
		UseTeamColor = 2097152U,
		// Token: 0x04000419 RID: 1049
		Civilian = 4194304U,
		// Token: 0x0400041A RID: 1050
		DoNotScaleBodyAccordingToWeaponLength = 8388608U,
		// Token: 0x0400041B RID: 1051
		DoesNotHideChest = 16777216U,
		// Token: 0x0400041C RID: 1052
		NotStackable = 33554432U
	}
}
