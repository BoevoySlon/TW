using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000090 RID: 144
	public interface ISaveManager
	{
		// Token: 0x06001100 RID: 4352
		int GetAutoSaveInterval();

		// Token: 0x06001101 RID: 4353
		void OnSaveOver(bool isSuccessful, string newSaveGameName);
	}
}
