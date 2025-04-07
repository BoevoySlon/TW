using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000219 RID: 537
	public class RomanticallyInvolvedTag : ConversationTag
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x00088E7F File Offset: 0x0008707F
		public override string StringId
		{
			get
			{
				return "RomanticallyInvolvedTag";
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x00088E86 File Offset: 0x00087086
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, CharacterObject.PlayerCharacter.HeroObject) >= Romance.RomanceLevelEnum.CourtshipStarted;
		}

		// Token: 0x0400099F RID: 2463
		public const string Id = "RomanticallyInvolvedTag";
	}
}
