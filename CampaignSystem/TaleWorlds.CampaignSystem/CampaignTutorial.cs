using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200006F RID: 111
	public class CampaignTutorial
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00046243 File Offset: 0x00044443
		public TextObject Description
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_description", this.TutorialTypeId);
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00046255 File Offset: 0x00044455
		public TextObject Title
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_title", this.TutorialTypeId);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00046267 File Offset: 0x00044467
		public CampaignTutorial(string tutorialType, int priority)
		{
			this.TutorialTypeId = tutorialType;
			this.Priority = priority;
		}

		// Token: 0x04000450 RID: 1104
		public readonly string TutorialTypeId;

		// Token: 0x04000451 RID: 1105
		public readonly int Priority;
	}
}
