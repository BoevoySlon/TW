using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FF RID: 255
	public class DefaultDelayedTeleportationModel : DelayedTeleportationModel
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x00062A07 File Offset: 0x00060C07
		public override float DefaultTeleportationSpeed
		{
			get
			{
				return 0.24f;
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00062A10 File Offset: 0x00060C10
		public override ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target)
		{
			float num = 300f;
			IMapPoint mapPoint = teleportingHero.GetMapPoint();
			if (mapPoint != null)
			{
				if (target.IsSettlement)
				{
					if (teleportingHero.CurrentSettlement != null && teleportingHero.CurrentSettlement == target.Settlement)
					{
						num = 0f;
					}
					else
					{
						Campaign.Current.Models.MapDistanceModel.GetDistance(mapPoint, target.Settlement, 300f, out num);
					}
				}
				else if (target.IsMobile)
				{
					Campaign.Current.Models.MapDistanceModel.GetDistance(mapPoint, target.MobileParty, 300f, out num);
				}
			}
			return new ExplainedNumber(num * this.DefaultTeleportationSpeed, false, null);
		}

		// Token: 0x04000770 RID: 1904
		private const float MaximumDistanceForDelay = 300f;
	}
}
