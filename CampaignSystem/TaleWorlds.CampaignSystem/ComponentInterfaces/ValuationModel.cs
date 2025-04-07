using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000190 RID: 400
	public abstract class ValuationModel : GameModel
	{
		// Token: 0x06001A4E RID: 6734
		public abstract float GetValueOfTroop(CharacterObject troop);

		// Token: 0x06001A4F RID: 6735
		public abstract float GetMilitaryValueOfParty(MobileParty party);

		// Token: 0x06001A50 RID: 6736
		public abstract float GetValueOfHero(Hero hero);
	}
}
