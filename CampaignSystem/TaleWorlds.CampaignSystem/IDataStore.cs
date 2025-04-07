using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002C RID: 44
	public interface IDataStore
	{
		// Token: 0x060002CB RID: 715
		bool SyncData<T>(string key, ref T data);

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002CC RID: 716
		bool IsSaving { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002CD RID: 717
		bool IsLoading { get; }
	}
}
