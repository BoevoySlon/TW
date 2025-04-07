using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000203 RID: 515
	public class PlayerIsSisterTag : ConversationTag
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0008886F File Offset: 0x00086A6F
		public override string StringId
		{
			get
			{
				return "PlayerIsSisterTag";
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x00088876 File Offset: 0x00086A76
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000989 RID: 2441
		public const string Id = "PlayerIsSisterTag";
	}
}
