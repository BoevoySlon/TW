using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200033F RID: 831
	public interface IPartyScreenLogicHandler
	{
		// Token: 0x06002F12 RID: 12050
		void RequestUserInput(string text, Action accept, Action cancel);
	}
}
