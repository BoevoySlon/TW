using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003AF RID: 943
	public class MilitiasCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x0011028C File Offset: 0x0010E48C
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x001102A8 File Offset: 0x0010E4A8
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			int count = Town.AllTowns.Count;
			int count2 = Town.AllCastles.Count;
			int count3 = Village.All.Count;
			int num = count / 100 + ((count % 100 > i) ? 1 : 0);
			int num2 = count2 / 100 + ((count2 % 100 > i) ? 1 : 0);
			int num3 = count3 / 100 + ((count3 % 100 > i) ? 1 : 0);
			int num4 = count / 100 * i;
			int num5 = count2 / 100 * i;
			int num6 = count3 / 100 * i;
			for (int j = 0; j < i; j++)
			{
				num4 += ((count % 100 > j) ? 1 : 0);
				num5 += ((count2 % 100 > j) ? 1 : 0);
				num6 += ((count3 % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num; k++)
			{
				Town.AllTowns[num4 + k].Settlement.Militia = Town.AllTowns[num4 + k].Settlement.Town.MilitiaChange * 45f;
			}
			for (int l = 0; l < num2; l++)
			{
				Town.AllCastles[num5 + l].Settlement.Militia = Town.AllCastles[num5 + l].Settlement.Town.MilitiaChange * 45f;
			}
			for (int m = 0; m < num3; m++)
			{
				Village.All[num6 + m].Settlement.Militia = Village.All[num6 + m].Settlement.Village.MilitiaChange * 45f;
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0011044F File Offset: 0x0010E64F
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
