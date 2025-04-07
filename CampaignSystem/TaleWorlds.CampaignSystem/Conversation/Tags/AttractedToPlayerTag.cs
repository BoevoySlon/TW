using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021A RID: 538
	public class AttractedToPlayerTag : ConversationTag
	{
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001EAB RID: 7851 RVA: 0x00088EB5 File Offset: 0x000870B5
		public override string StringId
		{
			get
			{
				return "AttractedToPlayerTag";
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00088EBC File Offset: 0x000870BC
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			return heroObject != null && Hero.MainHero.IsFemale != heroObject.IsFemale && !FactionManager.IsAtWarAgainstFaction(heroObject.MapFaction, Hero.MainHero.MapFaction) && Campaign.Current.Models.RomanceModel.GetAttractionValuePercentage(heroObject, Hero.MainHero) > 70 && heroObject.Spouse == null && Hero.MainHero.Spouse == null;
		}

		// Token: 0x040009A0 RID: 2464
		public const string Id = "AttractedToPlayerTag";

		// Token: 0x040009A1 RID: 2465
		private const int MinimumFlirtPercentageForComment = 70;
	}
}
