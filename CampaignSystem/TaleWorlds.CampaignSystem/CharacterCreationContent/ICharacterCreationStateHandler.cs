using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001E0 RID: 480
	public interface ICharacterCreationStateHandler
	{
		// Token: 0x06001C82 RID: 7298
		void OnCharacterCreationFinalized();

		// Token: 0x06001C83 RID: 7299
		void OnRefresh();

		// Token: 0x06001C84 RID: 7300
		void OnStageCreated(CharacterCreationStageBase stage);
	}
}
