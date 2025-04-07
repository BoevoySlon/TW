using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F5 RID: 245
	public class DefaultCharacterStatsModel : CharacterStatsModel
	{
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x0005E95D File Offset: 0x0005CB5D
		public override int MaxCharacterTier
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0005E960 File Offset: 0x0005CB60
		public override int GetTier(CharacterObject character)
		{
			if (character.IsHero)
			{
				return 0;
			}
			return MathF.Min(MathF.Max(MathF.Ceiling(((float)character.Level - 5f) / 5f), 0), Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0005E9B0 File Offset: 0x0005CBB0
		public override ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(100f, includeDescriptions, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Trainer, character, true, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.UnwaveringDefense, character, true, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.ThickHides, character, true, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.WellBuilt, character, true, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.PreventiveMedicine, character, true, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.DoctorsOath, character, false, ref result);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.FortitudeTonic, character, false, ref result);
			if (character.IsHero && character.HeroObject.PartyBelongedTo != null && character.HeroObject.PartyBelongedTo.LeaderHero != character.HeroObject && character.HeroObject.PartyBelongedTo.HasPerk(DefaultPerks.Medicine.FortitudeTonic, false))
			{
				result.Add(DefaultPerks.Medicine.FortitudeTonic.PrimaryBonus, DefaultPerks.Medicine.FortitudeTonic.Name, null);
			}
			if (character.GetPerkValue(DefaultPerks.Athletics.MightyBlow))
			{
				int num = character.GetSkillValue(DefaultSkills.Athletics) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
				result.Add((float)num, DefaultPerks.Athletics.MightyBlow.Name, null);
			}
			return result;
		}
	}
}
