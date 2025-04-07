using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C5 RID: 453
	public abstract class DelayedTeleportationModel : GameModel
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001BBD RID: 7101
		public abstract float DefaultTeleportationSpeed { get; }

		// Token: 0x06001BBE RID: 7102
		public abstract ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target);
	}
}
