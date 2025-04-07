using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace Helpers
{
	// Token: 0x02000010 RID: 16
	public static class MapEventHelper
	{
		// Token: 0x0600008B RID: 139 RVA: 0x000075D8 File Offset: 0x000057D8
		public static PartyBase GetSallyOutDefenderLeader()
		{
			PartyBase result;
			if (MobileParty.MainParty.CurrentSettlement.Town.GarrisonParty != null)
			{
				result = MobileParty.MainParty.CurrentSettlement.Town.GarrisonParty.MapEvent.DefenderSide.LeaderParty;
			}
			else
			{
				PartyBase party = MobileParty.MainParty.CurrentSettlement.Party;
				if (((party != null) ? party.MapEvent : null) != null)
				{
					result = MobileParty.MainParty.CurrentSettlement.Party.MapEvent.DefenderSide.LeaderParty;
				}
				else
				{
					result = MobileParty.MainParty.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party;
				}
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007680 File Offset: 0x00005880
		public static bool CanLeaveBattle(MobileParty mobileParty)
		{
			return mobileParty.MapEvent.DefenderSide.LeaderParty != mobileParty.Party && (!mobileParty.MapEvent.DefenderSide.LeaderParty.IsSettlement || mobileParty.CurrentSettlement != mobileParty.MapEvent.DefenderSide.LeaderParty.Settlement || mobileParty.MapFaction != mobileParty.MapEvent.DefenderSide.LeaderParty.MapFaction) && (mobileParty.MapEvent.PartiesOnSide(BattleSideEnum.Attacker).FindIndexQ((MapEventParty party) => party.Party == mobileParty.Party) < 0 || !mobileParty.MapEvent.IsRaid || mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty) && (mobileParty.MapEvent.PartiesOnSide(BattleSideEnum.Defender).FindIndexQ((MapEventParty party) => party.Party == mobileParty.Party) < 0 || mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000077D8 File Offset: 0x000059D8
		public static void OnConversationEnd()
		{
			if (PlayerEncounter.Current != null && ((PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.MapFaction != null && !PlayerEncounter.EncounteredMobileParty.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) || (PlayerEncounter.EncounteredParty != null && PlayerEncounter.EncounteredParty.MapFaction != null && !PlayerEncounter.EncounteredParty.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))))
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}
	}
}
