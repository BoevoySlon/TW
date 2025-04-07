using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C3 RID: 451
	public abstract class ExecutionRelationModel : GameModel
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001BAC RID: 7084
		public abstract int HeroKillingHeroClanRelationPenalty { get; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001BAD RID: 7085
		public abstract int HeroKillingHeroFriendRelationPenalty { get; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001BAE RID: 7086
		public abstract int PlayerExecutingHeroFactionRelationPenaltyDishonorable { get; }

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001BAF RID: 7087
		public abstract int PlayerExecutingHeroClanRelationPenaltyDishonorable { get; }

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001BB0 RID: 7088
		public abstract int PlayerExecutingHeroFriendRelationPenaltyDishonorable { get; }

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001BB1 RID: 7089
		public abstract int PlayerExecutingHeroHonorPenalty { get; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001BB2 RID: 7090
		public abstract int PlayerExecutingHeroFactionRelationPenalty { get; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001BB3 RID: 7091
		public abstract int PlayerExecutingHeroHonorableNobleRelationPenalty { get; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001BB4 RID: 7092
		public abstract int PlayerExecutingHeroClanRelationPenalty { get; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001BB5 RID: 7093
		public abstract int PlayerExecutingHeroFriendRelationPenalty { get; }

		// Token: 0x06001BB6 RID: 7094
		public abstract int GetRelationChangeForExecutingHero(Hero victim, Hero hero, out bool showQuickNotification);
	}
}
