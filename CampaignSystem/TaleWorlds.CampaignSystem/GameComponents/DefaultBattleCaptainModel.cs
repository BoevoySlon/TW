using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EE RID: 238
	public class DefaultBattleCaptainModel : BattleCaptainModel
	{
		// Token: 0x060014A2 RID: 5282 RVA: 0x0005CB88 File Offset: 0x0005AD88
		public override float GetCaptainRatingForTroopUsages(Hero hero, TroopUsageFlags flag, out List<PerkObject> compatiblePerks)
		{
			float num = 0f;
			compatiblePerks = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkHelper.GetCaptainPerksForTroopUsages(flag))
			{
				if (hero.GetPerkValue(perkObject))
				{
					num += perkObject.RequiredSkillValue;
					compatiblePerks.Add(perkObject);
				}
			}
			num /= 1650f;
			return num;
		}
	}
}
