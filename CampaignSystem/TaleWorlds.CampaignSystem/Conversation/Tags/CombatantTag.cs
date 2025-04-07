using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F3 RID: 499
	public class CombatantTag : ConversationTag
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0008849A File Offset: 0x0008669A
		public override string StringId
		{
			get
			{
				return "CombatantTag";
			}
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000884A1 File Offset: 0x000866A1
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !character.IsHero || !character.HeroObject.IsNoncombatant;
		}

		// Token: 0x04000979 RID: 2425
		public const string Id = "CombatantTag";
	}
}
