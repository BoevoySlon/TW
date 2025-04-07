using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001C RID: 28
	public class TooltipHelper
	{
		// Token: 0x060000EF RID: 239 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		public static TextObject GetSendTroopsPowerContextTooltipForMapEvent()
		{
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			MapEvent.PowerCalculationContext simulationContext = playerMapEvent.SimulationContext;
			string text = simulationContext.ToString();
			if (simulationContext == MapEvent.PowerCalculationContext.Village || simulationContext == MapEvent.PowerCalculationContext.RiverCrossingBattle || simulationContext == MapEvent.PowerCalculationContext.Siege)
			{
				text += ((playerMapEvent.PlayerSide == playerMapEvent.AttackerSide.MissionSide) ? "Attacker" : "Defender");
			}
			return GameTexts.FindText("str_simulation_tooltip", text);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000C33E File Offset: 0x0000A53E
		public static TextObject GetSendTroopsPowerContextTooltipForSiege()
		{
			return GameTexts.FindText("str_simulation_tooltip", (PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? "SiegeAttacker" : "SiegeDefender");
		}
	}
}
