using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001FD RID: 509
	public class PlayerIsSpouseTag : ConversationTag
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x00088712 File Offset: 0x00086912
		public override string StringId
		{
			get
			{
				return "PlayerIsSpouseTag";
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00088719 File Offset: 0x00086919
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Hero.MainHero.Spouse == character.HeroObject;
		}

		// Token: 0x04000983 RID: 2435
		public const string Id = "PlayerIsSpouseTag";
	}
}
