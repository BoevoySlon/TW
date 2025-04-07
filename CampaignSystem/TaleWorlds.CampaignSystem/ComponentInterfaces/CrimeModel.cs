using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000199 RID: 409
	public abstract class CrimeModel : GameModel
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001A78 RID: 6776
		public abstract int DeclareWarCrimeRatingThreshold { get; }

		// Token: 0x06001A79 RID: 6777
		public abstract float GetMaxCrimeRating();

		// Token: 0x06001A7A RID: 6778
		public abstract float GetMinAcceptableCrimeRating(IFaction faction);

		// Token: 0x06001A7B RID: 6779
		public abstract bool DoesPlayerHaveAnyCrimeRating(IFaction faction);

		// Token: 0x06001A7C RID: 6780
		public abstract bool IsPlayerCrimeRatingSevere(IFaction faction);

		// Token: 0x06001A7D RID: 6781
		public abstract bool IsPlayerCrimeRatingModerate(IFaction faction);

		// Token: 0x06001A7E RID: 6782
		public abstract bool IsPlayerCrimeRatingMild(IFaction faction);

		// Token: 0x06001A7F RID: 6783
		public abstract float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating);

		// Token: 0x06001A80 RID: 6784
		public abstract ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false);

		// Token: 0x0200055C RID: 1372
		[Flags]
		public enum PaymentMethod : uint
		{
			// Token: 0x040016A4 RID: 5796
			ExMachina = 4096U,
			// Token: 0x040016A5 RID: 5797
			Gold = 1U,
			// Token: 0x040016A6 RID: 5798
			Influence = 2U,
			// Token: 0x040016A7 RID: 5799
			Punishment = 4U,
			// Token: 0x040016A8 RID: 5800
			Execution = 8U
		}
	}
}
