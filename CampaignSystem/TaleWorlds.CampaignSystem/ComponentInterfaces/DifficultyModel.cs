using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C0 RID: 448
	public abstract class DifficultyModel : GameModel
	{
		// Token: 0x06001B9D RID: 7069
		public abstract float GetPlayerTroopsReceivedDamageMultiplier();

		// Token: 0x06001B9E RID: 7070
		public abstract float GetDamageToPlayerMultiplier();

		// Token: 0x06001B9F RID: 7071
		public abstract int GetPlayerRecruitSlotBonus();

		// Token: 0x06001BA0 RID: 7072
		public abstract float GetPlayerMapMovementSpeedBonusMultiplier();

		// Token: 0x06001BA1 RID: 7073
		public abstract float GetCombatAIDifficultyMultiplier();

		// Token: 0x06001BA2 RID: 7074
		public abstract float GetPersuasionBonusChance();

		// Token: 0x06001BA3 RID: 7075
		public abstract float GetClanMemberDeathChanceMultiplier();
	}
}
