using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F5 RID: 501
	public class DrinkingInTavernTag : ConversationTag
	{
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x00088542 File Offset: 0x00086742
		public override string StringId
		{
			get
			{
				return "DrinkingInTavernTag";
			}
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0008854C File Offset: 0x0008674C
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (LocationComplex.Current != null && character.IsHero)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(character.HeroObject);
				Location locationWithId = LocationComplex.Current.GetLocationWithId("tavern");
				if (character.HeroObject.IsWanderer && Settlement.CurrentSettlement != null && locationWithId == locationOfCharacter)
				{
					return true;
				}
			}
			else if (character.HeroObject == null && LocationComplex.Current != null && Settlement.CurrentSettlement != null && LocationComplex.Current.GetLocationWithId("tavern") == CampaignMission.Current.Location)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0400097B RID: 2427
		public const string Id = "DrinkingInTavernTag";
	}
}
