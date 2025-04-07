using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000225 RID: 549
	public class MerchantNotableTypeTag : ConversationTag
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x00089149 File Offset: 0x00087349
		public override string StringId
		{
			get
			{
				return "MerchantNotableTypeTag";
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00089150 File Offset: 0x00087350
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Merchant;
		}

		// Token: 0x040009AC RID: 2476
		public const string Id = "MerchantNotableTypeTag";
	}
}
