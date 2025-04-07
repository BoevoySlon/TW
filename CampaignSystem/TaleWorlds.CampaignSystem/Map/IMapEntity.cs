using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C3 RID: 195
	public interface IMapEntity
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x0600127D RID: 4733
		Vec2 InteractionPosition { get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x0600127E RID: 4734
		TextObject Name { get; }

		// Token: 0x0600127F RID: 4735
		bool OnMapClick(bool followModifierUsed);

		// Token: 0x06001280 RID: 4736
		void OnHover();

		// Token: 0x06001281 RID: 4737
		void OnOpenEncyclopedia();

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001282 RID: 4738
		bool IsMobileEntity { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001283 RID: 4739
		bool ShowCircleAroundEntity { get; }

		// Token: 0x06001284 RID: 4740
		bool IsEnemyOf(IFaction faction);

		// Token: 0x06001285 RID: 4741
		bool IsAllyOf(IFaction faction);

		// Token: 0x06001286 RID: 4742
		void GetMountAndHarnessVisualIdsForPartyIcon(out string mountStringId, out string harnessStringId);

		// Token: 0x06001287 RID: 4743
		void OnPartyInteraction(MobileParty mobileParty);
	}
}
