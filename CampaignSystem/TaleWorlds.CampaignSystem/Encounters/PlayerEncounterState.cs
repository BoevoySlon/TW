using System;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000294 RID: 660
	public enum PlayerEncounterState
	{
		// Token: 0x04000B01 RID: 2817
		Begin,
		// Token: 0x04000B02 RID: 2818
		Wait,
		// Token: 0x04000B03 RID: 2819
		PrepareResults,
		// Token: 0x04000B04 RID: 2820
		ApplyResults,
		// Token: 0x04000B05 RID: 2821
		PlayerVictory,
		// Token: 0x04000B06 RID: 2822
		PlayerTotalDefeat,
		// Token: 0x04000B07 RID: 2823
		CaptureHeroes,
		// Token: 0x04000B08 RID: 2824
		FreeHeroes,
		// Token: 0x04000B09 RID: 2825
		LootParty,
		// Token: 0x04000B0A RID: 2826
		LootInventory,
		// Token: 0x04000B0B RID: 2827
		End
	}
}
