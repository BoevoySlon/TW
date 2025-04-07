using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017A RID: 378
	public abstract class CombatXpModel : GameModel
	{
		// Token: 0x06001985 RID: 6533
		public abstract SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit);

		// Token: 0x06001986 RID: 6534
		public abstract void GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType, out int xpAmount);

		// Token: 0x06001987 RID: 6535
		public abstract float GetXpMultiplierFromShotDifficulty(float shotDifficulty);

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001988 RID: 6536
		public abstract float CaptainRadius { get; }

		// Token: 0x02000559 RID: 1369
		public enum MissionTypeEnum
		{
			// Token: 0x04001695 RID: 5781
			Battle,
			// Token: 0x04001696 RID: 5782
			PracticeFight,
			// Token: 0x04001697 RID: 5783
			Tournament,
			// Token: 0x04001698 RID: 5784
			SimulationBattle,
			// Token: 0x04001699 RID: 5785
			NoXp
		}
	}
}
