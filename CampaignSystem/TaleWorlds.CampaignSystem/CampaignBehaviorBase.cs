using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002B RID: 43
	public abstract class CampaignBehaviorBase : ICampaignBehavior
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x000124DC File Offset: 0x000106DC
		public CampaignBehaviorBase(string stringId)
		{
			this.StringId = stringId;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000124EB File Offset: 0x000106EB
		public CampaignBehaviorBase()
		{
			this.StringId = base.GetType().Name;
		}

		// Token: 0x060002C8 RID: 712
		public abstract void RegisterEvents();

		// Token: 0x060002C9 RID: 713 RVA: 0x00012504 File Offset: 0x00010704
		public static T GetCampaignBehavior<T>()
		{
			return Campaign.Current.GetCampaignBehavior<T>();
		}

		// Token: 0x060002CA RID: 714
		public abstract void SyncData(IDataStore dataStore);

		// Token: 0x040000D7 RID: 215
		public readonly string StringId;
	}
}
