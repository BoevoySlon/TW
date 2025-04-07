using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013C RID: 316
	public class DefaultSiegeLordsHallFightModel : SiegeLordsHallFightModel
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x000771D7 File Offset: 0x000753D7
		public override float AreaLostRatio
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x000771DE File Offset: 0x000753DE
		public override float AttackerDefenderTroopCountRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x000771E5 File Offset: 0x000753E5
		public override float DefenderMaxArcherRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x000771EC File Offset: 0x000753EC
		public override int MaxDefenderSideTroopCount
		{
			get
			{
				return 27;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x000771F0 File Offset: 0x000753F0
		public override int MaxDefenderArcherCount
		{
			get
			{
				return MathF.Round((float)this.MaxDefenderSideTroopCount * this.DefenderMaxArcherRatio);
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x00077205 File Offset: 0x00075405
		public override int MaxAttackerSideTroopCount
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x00077209 File Offset: 0x00075409
		public override int DefenderTroopNumberForSuccessfulPullBack
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00077210 File Offset: 0x00075410
		public override FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount)
		{
			List<MapEventParty> list = (from x in playerMapEvent.PartiesOnSide(side)
			where x.Party.IsMobile
			select x).ToList<MapEventParty>();
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(list.Sum((MapEventParty x) => x.Party.MemberRoster.TotalHealthyCount));
			foreach (MapEventParty mapEventParty in list)
			{
				flattenedTroopRoster.Add(mapEventParty.Party.MemberRoster.GetTroopRoster());
			}
			List<FlattenedTroopRosterElement> list2 = (from x in flattenedTroopRoster
			where !x.Troop.IsHero && x.Troop.IsRanged && !x.IsWounded
			select x).ToList<FlattenedTroopRosterElement>();
			list2.Shuffle<FlattenedTroopRosterElement>();
			List<FlattenedTroopRosterElement> list3 = (from x in flattenedTroopRoster
			where !x.Troop.IsHero && !x.Troop.IsRanged && !x.IsWounded
			select x).ToList<FlattenedTroopRosterElement>();
			list3.Shuffle<FlattenedTroopRosterElement>();
			flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => !x.Troop.IsHero || x.IsWounded);
			int num = troopCount - flattenedTroopRoster.Count<FlattenedTroopRosterElement>();
			if (num > 0)
			{
				int count = list2.Count;
				int count2 = list3.Count;
				int num2 = MathF.Min(count, Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderArcherCount);
				int num3 = 0;
				int num4 = 0;
				while (num > 0 && (num3 < num2 || num4 < count2))
				{
					if (num3 < num2)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement = list2[num3];
						flattenedTroopRoster.Add(flattenedTroopRosterElement.Troop, false, flattenedTroopRosterElement.Xp);
						num--;
					}
					if (num4 < count2 && num > 0)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement2 = list3[num4];
						flattenedTroopRoster.Add(flattenedTroopRosterElement2.Troop, false, flattenedTroopRosterElement2.Xp);
						num--;
					}
					num3++;
					num4++;
				}
			}
			return flattenedTroopRoster;
		}
	}
}
