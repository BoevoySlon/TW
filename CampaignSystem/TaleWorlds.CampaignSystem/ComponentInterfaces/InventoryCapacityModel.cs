using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000192 RID: 402
	public abstract class InventoryCapacityModel : GameModel
	{
		// Token: 0x06001A57 RID: 6743
		public abstract ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool includeDescriptions = false, int additionalManOnFoot = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false);

		// Token: 0x06001A58 RID: 6744
		public abstract int GetItemAverageWeight();
	}
}
