using System;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A9 RID: 425
	public abstract class BuildingEffectModel : GameModel
	{
		// Token: 0x06001B02 RID: 6914
		public abstract float GetBuildingEffectAmount(Building building, BuildingEffectEnum effect);
	}
}
