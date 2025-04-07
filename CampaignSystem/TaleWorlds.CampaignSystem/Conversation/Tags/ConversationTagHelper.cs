using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024C RID: 588
	public static class ConversationTagHelper
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x000897C9 File Offset: 0x000879C9
		public static bool UsesHighRegister(CharacterObject character)
		{
			return ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000897DE File Offset: 0x000879DE
		public static bool UsesLowRegister(CharacterObject character)
		{
			return !ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000897F4 File Offset: 0x000879F4
		public static bool TribalVoiceGroup(CharacterObject character)
		{
			return character.Culture.StringId == "sturgia" || character.Culture.StringId == "aserai" || character.Culture.StringId == "khuzait" || character.Culture.StringId == "battania" || character.Culture.StringId == "vlandia" || character.Culture.StringId == "nord" || character.Culture.StringId == "vakken";
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000898A8 File Offset: 0x00087AA8
		public static bool EducatedClass(CharacterObject character)
		{
			bool result = false;
			if (character.HeroObject != null)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					result = true;
				}
				if (character.HeroObject.IsMerchant)
				{
					result = true;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Siegecraft) >= 5 || character.HeroObject.GetTraitLevel(DefaultTraits.Surgery) >= 5)
				{
					result = true;
				}
				if (character.HeroObject.IsGangLeader)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x00089924 File Offset: 0x00087B24
		public static int TraitCompatibility(Hero hero1, Hero hero2, TraitObject trait)
		{
			int traitLevel = hero1.GetTraitLevel(trait);
			int traitLevel2 = hero2.GetTraitLevel(trait);
			if (traitLevel > 0 && traitLevel2 > 0)
			{
				return 1;
			}
			if (traitLevel < 0 || traitLevel2 < 0)
			{
				return MathF.Abs(traitLevel - traitLevel2) * -1;
			}
			return 0;
		}
	}
}
