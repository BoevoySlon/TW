using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000220 RID: 544
	public class PlayerIsLiegeTag : ConversationTag
	{
		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x00089053 File Offset: 0x00087253
		public override string StringId
		{
			get
			{
				return "PlayerIsLiegeTag";
			}
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0008905C File Offset: 0x0008725C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction.IsKingdomFaction && character.HeroObject.MapFaction == Hero.MainHero.MapFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero;
		}

		// Token: 0x040009A7 RID: 2471
		public const string Id = "PlayerIsLiegeTag";
	}
}
