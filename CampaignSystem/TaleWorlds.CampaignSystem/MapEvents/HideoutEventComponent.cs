﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.MapEvents
{
	// Token: 0x020002BF RID: 703
	public class HideoutEventComponent : MapEventComponent
	{
		// Token: 0x060029AE RID: 10670 RVA: 0x000B31CD File Offset: 0x000B13CD
		internal static void AutoGeneratedStaticCollectObjectsHideoutEventComponent(object o, List<object> collectedObjects)
		{
			((HideoutEventComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000B31DB File Offset: 0x000B13DB
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000B31E4 File Offset: 0x000B13E4
		protected HideoutEventComponent(MapEvent mapEvent) : base(mapEvent)
		{
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000B31F0 File Offset: 0x000B13F0
		public static HideoutEventComponent CreateHideoutEvent(PartyBase attackerParty, PartyBase defenderParty)
		{
			MapEvent mapEvent = new MapEvent();
			HideoutEventComponent hideoutEventComponent = new HideoutEventComponent(mapEvent);
			mapEvent.Initialize(attackerParty, defenderParty, hideoutEventComponent, MapEvent.BattleTypes.Hideout);
			Campaign.Current.MapEventManager.OnMapEventCreated(mapEvent);
			return hideoutEventComponent;
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000B3225 File Offset: 0x000B1425
		public static HideoutEventComponent CreateComponentForOldSaves(MapEvent mapEvent)
		{
			return new HideoutEventComponent(mapEvent);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000B3230 File Offset: 0x000B1430
		protected override void OnFinalize()
		{
			BattleSideEnum winnerSide = (base.MapEvent.BattleState == BattleState.AttackerVictory) ? BattleSideEnum.Attacker : BattleSideEnum.Defender;
			CampaignEventDispatcher.Instance.OnHideoutBattleCompleted(winnerSide, this);
		}
	}
}
