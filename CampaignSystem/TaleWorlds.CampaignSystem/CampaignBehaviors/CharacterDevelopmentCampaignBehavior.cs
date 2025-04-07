using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200037E RID: 894
	public class CharacterDevelopmentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060034CD RID: 13517 RVA: 0x000E0FD7 File Offset: 0x000DF1D7
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000E0FF0 File Offset: 0x000DF1F0
		private void DailyTickHero(Hero hero)
		{
			if (!hero.IsChild && hero.IsAlive && (hero.Clan != Clan.PlayerClan || (hero != Hero.MainHero && CampaignOptions.AutoAllocateClanMemberPerks)))
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					hero.HeroDeveloper.DevelopCharacterStats();
				}
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000E1048 File Offset: 0x000DF248
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
