using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A1 RID: 417
	public abstract class MobilePartyMoraleModel : GameModel
	{
		// Token: 0x06001ACB RID: 6859
		public abstract float CalculateMoraleChange(MobileParty party);

		// Token: 0x06001ACC RID: 6860
		public abstract TextObject GetMoraleTooltipText(MobileParty party);
	}
}
