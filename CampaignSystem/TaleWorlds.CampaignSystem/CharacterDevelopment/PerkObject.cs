﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x0200034A RID: 842
	public sealed class PerkObject : PropertyObject
	{
		// Token: 0x06002F87 RID: 12167 RVA: 0x000C3C29 File Offset: 0x000C1E29
		internal static void AutoGeneratedStaticCollectObjectsPerkObject(object o, List<object> collectedObjects)
		{
			((PerkObject)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000C3C37 File Offset: 0x000C1E37
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002F89 RID: 12169 RVA: 0x000C3C40 File Offset: 0x000C1E40
		public static MBReadOnlyList<PerkObject> All
		{
			get
			{
				return Campaign.Current.AllPerks;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x000C3C4C File Offset: 0x000C1E4C
		// (set) Token: 0x06002F8B RID: 12171 RVA: 0x000C3C54 File Offset: 0x000C1E54
		public SkillObject Skill { get; private set; }

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x000C3C5D File Offset: 0x000C1E5D
		// (set) Token: 0x06002F8D RID: 12173 RVA: 0x000C3C65 File Offset: 0x000C1E65
		public float RequiredSkillValue { get; private set; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x000C3C6E File Offset: 0x000C1E6E
		// (set) Token: 0x06002F8F RID: 12175 RVA: 0x000C3C76 File Offset: 0x000C1E76
		public PerkObject AlternativePerk { get; private set; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000C3C7F File Offset: 0x000C1E7F
		// (set) Token: 0x06002F91 RID: 12177 RVA: 0x000C3C87 File Offset: 0x000C1E87
		public SkillEffect.PerkRole PrimaryRole { get; private set; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x000C3C90 File Offset: 0x000C1E90
		// (set) Token: 0x06002F93 RID: 12179 RVA: 0x000C3C98 File Offset: 0x000C1E98
		public SkillEffect.PerkRole SecondaryRole { get; private set; }

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x000C3CA1 File Offset: 0x000C1EA1
		// (set) Token: 0x06002F95 RID: 12181 RVA: 0x000C3CA9 File Offset: 0x000C1EA9
		public float PrimaryBonus { get; private set; }

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x000C3CB2 File Offset: 0x000C1EB2
		// (set) Token: 0x06002F97 RID: 12183 RVA: 0x000C3CBA File Offset: 0x000C1EBA
		public float SecondaryBonus { get; private set; }

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06002F98 RID: 12184 RVA: 0x000C3CC3 File Offset: 0x000C1EC3
		// (set) Token: 0x06002F99 RID: 12185 RVA: 0x000C3CCB File Offset: 0x000C1ECB
		public SkillEffect.EffectIncrementType PrimaryIncrementType { get; private set; }

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x000C3CD4 File Offset: 0x000C1ED4
		// (set) Token: 0x06002F9B RID: 12187 RVA: 0x000C3CDC File Offset: 0x000C1EDC
		public SkillEffect.EffectIncrementType SecondaryIncrementType { get; private set; }

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000C3CE5 File Offset: 0x000C1EE5
		// (set) Token: 0x06002F9D RID: 12189 RVA: 0x000C3CED File Offset: 0x000C1EED
		public TroopUsageFlags PrimaryTroopUsageMask { get; private set; }

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000C3CF6 File Offset: 0x000C1EF6
		// (set) Token: 0x06002F9F RID: 12191 RVA: 0x000C3CFE File Offset: 0x000C1EFE
		public TroopUsageFlags SecondaryTroopUsageMask { get; private set; }

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000C3D07 File Offset: 0x000C1F07
		// (set) Token: 0x06002FA1 RID: 12193 RVA: 0x000C3D0F File Offset: 0x000C1F0F
		public TextObject PrimaryDescription { get; private set; }

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000C3D18 File Offset: 0x000C1F18
		// (set) Token: 0x06002FA3 RID: 12195 RVA: 0x000C3D20 File Offset: 0x000C1F20
		public TextObject SecondaryDescription { get; private set; }

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x000C3D29 File Offset: 0x000C1F29
		public bool IsTrash
		{
			get
			{
				return base.Name == null || base.Description == null || this.Skill == null;
			}
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000C3D46 File Offset: 0x000C1F46
		public PerkObject(string stringId) : base(stringId)
		{
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000C3D50 File Offset: 0x000C1F50
		public void Initialize(string name, SkillObject skill, int requiredSkillValue, PerkObject alternativePerk, string primaryDescription, SkillEffect.PerkRole primaryRole, float primaryBonus, SkillEffect.EffectIncrementType incrementType, string secondaryDescription = "", SkillEffect.PerkRole secondaryRole = SkillEffect.PerkRole.None, float secondaryBonus = 0f, SkillEffect.EffectIncrementType secondaryIncrementType = SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags primaryTroopUsageMask = TroopUsageFlags.Undefined, TroopUsageFlags secondaryTroopUsageMask = TroopUsageFlags.Undefined)
		{
			this.PrimaryDescription = new TextObject(primaryDescription, null);
			this.SecondaryDescription = new TextObject(secondaryDescription, null);
			PerkHelper.SetDescriptionTextVariable(this.PrimaryDescription, primaryBonus, incrementType);
			TextObject textObject;
			if (secondaryDescription != "")
			{
				PerkHelper.SetDescriptionTextVariable(this.SecondaryDescription, secondaryBonus, secondaryIncrementType);
				textObject = GameTexts.FindText("str_string_newline_newline_string", null);
				textObject.SetTextVariable("STR1", this.PrimaryDescription);
				textObject.SetTextVariable("STR2", this.SecondaryDescription);
			}
			else
			{
				textObject = this.PrimaryDescription.CopyTextObject();
			}
			textObject.SetTextVariable("newline", "\n");
			base.Initialize(new TextObject(name, null), textObject);
			this.Skill = skill;
			this.RequiredSkillValue = (float)requiredSkillValue;
			this.AlternativePerk = alternativePerk;
			if (alternativePerk != null)
			{
				alternativePerk.AlternativePerk = this;
			}
			this.PrimaryRole = primaryRole;
			this.SecondaryRole = secondaryRole;
			this.PrimaryBonus = primaryBonus;
			this.SecondaryBonus = secondaryBonus;
			this.PrimaryIncrementType = incrementType;
			this.SecondaryIncrementType = ((secondaryIncrementType == SkillEffect.EffectIncrementType.Invalid) ? this.PrimaryIncrementType : secondaryIncrementType);
			this.PrimaryTroopUsageMask = primaryTroopUsageMask;
			this.SecondaryTroopUsageMask = secondaryTroopUsageMask;
			base.AfterInitialized();
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000C3E78 File Offset: 0x000C2078
		public override string ToString()
		{
			return base.Name.ToString();
		}
	}
}
