using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012F RID: 303
	public class DefaultRomanceModel : RomanceModel
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x00072294 File Offset: 0x00070494
		public override int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest)
		{
			return MathF.Abs((potentiallyInterestedCharacter.StaticBodyProperties.GetHashCode() + heroOfInterest.StaticBodyProperties.GetHashCode()) % 100);
		}
	}
}
