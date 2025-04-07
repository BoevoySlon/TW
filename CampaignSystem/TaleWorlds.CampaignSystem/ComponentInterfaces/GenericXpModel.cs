using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000179 RID: 377
	public abstract class GenericXpModel : GameModel
	{
		// Token: 0x06001983 RID: 6531
		public abstract float GetXpMultiplier(Hero hero);
	}
}
