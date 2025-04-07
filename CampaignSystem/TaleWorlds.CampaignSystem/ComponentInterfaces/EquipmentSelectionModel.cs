using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C9 RID: 457
	public abstract class EquipmentSelectionModel : GameModel
	{
		// Token: 0x06001BD4 RID: 7124
		public abstract MBList<MBEquipmentRoster> GetEquipmentRostersForHeroComeOfAge(Hero hero, bool isCivilian);

		// Token: 0x06001BD5 RID: 7125
		public abstract MBList<MBEquipmentRoster> GetEquipmentRostersForHeroReachesTeenAge(Hero hero);

		// Token: 0x06001BD6 RID: 7126
		public abstract MBList<MBEquipmentRoster> GetEquipmentRostersForInitialChildrenGeneration(Hero hero);

		// Token: 0x06001BD7 RID: 7127
		public abstract MBList<MBEquipmentRoster> GetEquipmentRostersForDeliveredOffspring(Hero hero);

		// Token: 0x06001BD8 RID: 7128
		public abstract MBList<MBEquipmentRoster> GetEquipmentRostersForCompanion(Hero companionHero, bool isCivilian);
	}
}
