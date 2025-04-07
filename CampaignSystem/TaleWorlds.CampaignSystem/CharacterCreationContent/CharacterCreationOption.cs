using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001CF RID: 463
	public class CharacterCreationOption
	{
		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x0007E8C7 File Offset: 0x0007CAC7
		public int Id { get; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x0007E8CF File Offset: 0x0007CACF
		public MBReadOnlyList<SkillObject> AffectedSkills
		{
			get
			{
				return this._affectedSkills;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0007E8D7 File Offset: 0x0007CAD7
		public MBReadOnlyList<TraitObject> AffectedTraits
		{
			get
			{
				return this._affectedTraits;
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0007E8E0 File Offset: 0x0007CAE0
		public CharacterCreationOption(int id, MBList<SkillObject> affectedSkills, CharacterAttribute effectedAttribute, int focusToAdd, int skillLevelToAdd, int attributeLevelToAdd, TextObject text, CharacterCreationOnCondition onCondition, CharacterCreationOnSelect onSelect, CharacterCreationApplyFinalEffects applyFinalEffects, TextObject description = null, MBList<TraitObject> affectedTraits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocusPoint = 0, int unspentAttributePoint = 0)
		{
			this.Id = id;
			this.EffectedAttribute = effectedAttribute;
			this.Text = text;
			this.OnCondition = onCondition;
			this.OnSelect = onSelect;
			this.ApplyFinalEffects = applyFinalEffects;
			this.DescriptionText = (description ?? TextObject.Empty);
			this.FocusToAdd = focusToAdd;
			this.UnspentFocusToAdd = unspentFocusPoint;
			this.SkillLevelToAdd = skillLevelToAdd;
			this.AttributeLevelToAdd = attributeLevelToAdd;
			this.TraitLevelToAdd = traitLevelToAdd;
			this.RenownToAdd = renownToAdd;
			this.GoldToAdd = goldToAdd;
			this.UnspentAttributeToAdd = unspentAttributePoint;
			this._affectedSkills = affectedSkills;
			if (affectedTraits != null)
			{
				this._affectedTraits = affectedTraits;
			}
			this.PositiveEffectText = this.SetTextVariables(affectedSkills, this.EffectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, affectedTraits, this.TraitLevelToAdd, this.RenownToAdd, this.GoldToAdd, this.UnspentFocusToAdd, this.UnspentAttributeToAdd);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0007E9CC File Offset: 0x0007CBCC
		private TextObject SetTextVariables(MBList<SkillObject> skills, CharacterAttribute attribute, int focusToAdd = 0, int skillLevelToAdd = 0, int attributeLevelToAdd = 0, MBList<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocustoAdd = 0, int unspentAttributeToAdd = 0)
		{
			TextObject textObject = TextObject.Empty;
			if (skills.Count == 3)
			{
				textObject = new TextObject("{=jeWV2uV3}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}, {SKILL_TWO} and {SKILL_THREE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt(1).Name);
				textObject.SetTextVariable("SKILL_THREE", skills.ElementAt(2).Name);
			}
			else if (skills.Count == 2)
			{
				textObject = new TextObject("{=5JTEvvaO}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE} and {SKILL_TWO}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt(1).Name);
			}
			else if (skills.Count == 1)
			{
				textObject = new TextObject("{=uw2kKrQk}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt(0).Name);
			}
			else
			{
				textObject = new TextObject("{=NDWdnpI5}{UNSPENT_FOCUS_VALUE} unspent Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?}{NEWLINE}{UNSPENT_ATTR_VALUE} unspent Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?}", null);
			}
			textObject.SetTextVariable("NEWLINE", "\n");
			if (skills.Count > 0)
			{
				textObject.SetTextVariable("FOCUS_VALUE", focusToAdd);
				textObject.SetTextVariable("EXP_VALUE", skillLevelToAdd);
				textObject.SetTextVariable("ATTR_VALUE", attributeLevelToAdd);
				textObject.SetTextVariable("IS_PLURAL_SKILL", (skillLevelToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (focusToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (attributeLevelToAdd > 1) ? 1 : 0);
			}
			else
			{
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (unspentFocustoAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (unspentAttributeToAdd > 1) ? 1 : 0);
			}
			if (attribute != null)
			{
				textObject.SetTextVariable("ATTR_NAME", attribute.Name);
			}
			textObject.SetTextVariable("UNSPENT_FOCUS_VALUE", unspentFocustoAdd);
			textObject.SetTextVariable("UNSPENT_ATTR_VALUE", unspentAttributeToAdd);
			if (traits != null && traits.Count > 0 && traits.Count < 4)
			{
				TextObject textObject2 = TextObject.Empty;
				if (traits.Count == 1)
				{
					textObject2 = new TextObject("{=DuQvj7zd}\n+{VALUE} to {TRAIT_NAME}", null);
					textObject2.SetTextVariable("TRAIT_NAME", traits.ElementAt(0).Name);
				}
				else if (traits.Count == 2)
				{
					textObject2 = new TextObject("{=F1syZDs4}\n+{VALUE} to {TRAIT_NAME_ONE} and {TRAIT_NAME_TWO}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt(1).Name);
				}
				else if (traits.Count == 3)
				{
					textObject2 = new TextObject("{=i20baAus}\n+{VALUE} to {TRAIT_NAME_ONE}, {TRAIT_NAME_TWO} and {TRAIT_NAME_THREE}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt(1).Name);
					textObject2.SetTextVariable("TRAIT_NAME_THREE", traits.ElementAt(2).Name);
				}
				if (textObject2 != TextObject.Empty)
				{
					textObject.SetTextVariable("TRAIT_DESC", textObject2);
					textObject2.SetTextVariable("VALUE", traitLevelToAdd);
				}
			}
			else
			{
				textObject.SetTextVariable("TRAIT_DESC", TextObject.Empty);
			}
			if (renownToAdd > 0)
			{
				TextObject textObject3 = new TextObject("{=KXtaJNo4}\n+{VALUE} renown", null);
				textObject3.SetTextVariable("VALUE", renownToAdd);
				textObject.SetTextVariable("RENOWN_DESC", textObject3);
			}
			else
			{
				textObject.SetTextVariable("RENOWN_DESC", TextObject.Empty);
			}
			if (goldToAdd > 0)
			{
				TextObject textObject4 = new TextObject("{=YBqmnNGv}\n+{VALUE} gold", null);
				textObject4.SetTextVariable("VALUE", goldToAdd);
				textObject.SetTextVariable("GOLD_DESC", textObject4);
			}
			else
			{
				textObject.SetTextVariable("GOLD_DESC", TextObject.Empty);
			}
			return textObject;
		}

		// Token: 0x040008B0 RID: 2224
		public TextObject Text;

		// Token: 0x040008B1 RID: 2225
		public TextObject PositiveEffectText;

		// Token: 0x040008B2 RID: 2226
		public TextObject DescriptionText;

		// Token: 0x040008B3 RID: 2227
		public CharacterCreationOnCondition OnCondition;

		// Token: 0x040008B4 RID: 2228
		public CharacterCreationOnSelect OnSelect;

		// Token: 0x040008B5 RID: 2229
		public CharacterCreationApplyFinalEffects ApplyFinalEffects;

		// Token: 0x040008B6 RID: 2230
		private readonly MBList<SkillObject> _affectedSkills;

		// Token: 0x040008B7 RID: 2231
		private readonly MBList<TraitObject> _affectedTraits;

		// Token: 0x040008B8 RID: 2232
		public readonly CharacterAttribute EffectedAttribute;

		// Token: 0x040008B9 RID: 2233
		public readonly int FocusToAdd;

		// Token: 0x040008BA RID: 2234
		public readonly int UnspentFocusToAdd;

		// Token: 0x040008BB RID: 2235
		public readonly int UnspentAttributeToAdd;

		// Token: 0x040008BC RID: 2236
		public readonly int SkillLevelToAdd;

		// Token: 0x040008BD RID: 2237
		public readonly int AttributeLevelToAdd;

		// Token: 0x040008BE RID: 2238
		public readonly int TraitLevelToAdd;

		// Token: 0x040008BF RID: 2239
		public readonly int RenownToAdd;

		// Token: 0x040008C0 RID: 2240
		public readonly int GoldToAdd;
	}
}
