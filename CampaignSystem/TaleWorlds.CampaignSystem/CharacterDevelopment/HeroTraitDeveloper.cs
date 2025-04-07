﻿using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x02000347 RID: 839
	public class HeroTraitDeveloper : PropertyOwner<PropertyObject>
	{
		// Token: 0x06002F3C RID: 12092 RVA: 0x000C2E38 File Offset: 0x000C1038
		internal static void AutoGeneratedStaticCollectObjectsHeroTraitDeveloper(object o, List<object> collectedObjects)
		{
			((HeroTraitDeveloper)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000C2E46 File Offset: 0x000C1046
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Hero);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000C2E5B File Offset: 0x000C105B
		internal static object AutoGeneratedGetMemberValueHero(object o)
		{
			return ((HeroTraitDeveloper)o).Hero;
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x000C2E68 File Offset: 0x000C1068
		// (set) Token: 0x06002F40 RID: 12096 RVA: 0x000C2E70 File Offset: 0x000C1070
		[SaveableProperty(0)]
		internal Hero Hero { get; private set; }

		// Token: 0x06002F41 RID: 12097 RVA: 0x000C2E79 File Offset: 0x000C1079
		internal HeroTraitDeveloper(Hero hero)
		{
			this.Hero = hero;
			this.UpdateTraitXPAccordingToTraitLevels();
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000C2E90 File Offset: 0x000C1090
		public void AddTraitXp(TraitObject trait, int xpAmount)
		{
			xpAmount += base.GetPropertyValue(trait);
			int num;
			int value;
			Campaign.Current.Models.CharacterDevelopmentModel.GetTraitLevelForTraitXp(this.Hero, trait, xpAmount, out num, out value);
			base.SetPropertyValue(trait, value);
			if (num != this.Hero.GetTraitLevel(trait))
			{
				this.Hero.SetTraitLevel(trait, num);
			}
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000C2EEC File Offset: 0x000C10EC
		public void UpdateTraitXPAccordingToTraitLevels()
		{
			foreach (TraitObject traitObject in TraitObject.All)
			{
				int traitLevel = this.Hero.GetTraitLevel(traitObject);
				if (traitLevel != 0)
				{
					int traitXpRequiredForTraitLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetTraitXpRequiredForTraitLevel(traitObject, traitLevel);
					base.SetPropertyValue(traitObject, traitXpRequiredForTraitLevel);
				}
			}
		}
	}
}
