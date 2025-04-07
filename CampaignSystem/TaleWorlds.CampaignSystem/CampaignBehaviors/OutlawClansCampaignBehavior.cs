using System;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003B5 RID: 949
	public class OutlawClansCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A1B RID: 14875 RVA: 0x001116F0 File Offset: 0x0010F8F0
		private static void MakeOutlawFactionsEnemyToKingdomFactions()
		{
			foreach (Clan clan in Clan.All)
			{
				if (clan.IsMinorFaction && clan.IsOutlaw)
				{
					foreach (Kingdom kingdom in Kingdom.All)
					{
						if (kingdom.Culture == clan.Culture)
						{
							FactionManager.DeclareWar(kingdom, clan, true);
						}
					}
				}
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x0011179C File Offset: 0x0010F99C
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x001117B5 File Offset: 0x0010F9B5
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			OutlawClansCampaignBehavior.MakeOutlawFactionsEnemyToKingdomFactions();
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x001117BC File Offset: 0x0010F9BC
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
