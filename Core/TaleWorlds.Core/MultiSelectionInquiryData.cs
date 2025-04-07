using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000B6 RID: 182
	public class MultiSelectionInquiryData
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0001F224 File Offset: 0x0001D424
		public MultiSelectionInquiryData(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction, string soundEventPath = "", bool isSeachAvailable = false)
		{
			this.TitleText = titleText;
			this.DescriptionText = descriptionText;
			this.InquiryElements = inquiryElements;
			this.IsExitShown = isExitShown;
			this.AffirmativeText = affirmativeText;
			this.NegativeText = negativeText;
			this.AffirmativeAction = affirmativeAction;
			this.NegativeAction = negativeAction;
			this.MinSelectableOptionCount = minSelectableOptionCount;
			this.MaxSelectableOptionCount = maxSelectableOptionCount;
			this.SoundEventPath = soundEventPath;
			this.IsSeachAvailable = isSeachAvailable;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001F294 File Offset: 0x0001D494
		public bool HasSameContentWith(object other)
		{
			MultiSelectionInquiryData multiSelectionInquiryData;
			if ((multiSelectionInquiryData = (other as MultiSelectionInquiryData)) != null)
			{
				bool flag = true;
				if (this.InquiryElements.Count == multiSelectionInquiryData.InquiryElements.Count)
				{
					for (int i = 0; i < this.InquiryElements.Count; i++)
					{
						if (!this.InquiryElements[i].HasSameContentWith(multiSelectionInquiryData.InquiryElements[i]))
						{
							flag = false;
						}
					}
				}
				else
				{
					flag = false;
				}
				return this.TitleText == multiSelectionInquiryData.TitleText && this.DescriptionText == multiSelectionInquiryData.DescriptionText && flag && this.IsExitShown == multiSelectionInquiryData.IsExitShown && this.AffirmativeText == multiSelectionInquiryData.AffirmativeText && this.NegativeText == multiSelectionInquiryData.NegativeText && this.AffirmativeAction == multiSelectionInquiryData.AffirmativeAction && this.NegativeAction == multiSelectionInquiryData.NegativeAction && this.MinSelectableOptionCount == multiSelectionInquiryData.MinSelectableOptionCount && this.MaxSelectableOptionCount == multiSelectionInquiryData.MaxSelectableOptionCount && this.SoundEventPath == multiSelectionInquiryData.SoundEventPath;
			}
			return false;
		}

		// Token: 0x04000564 RID: 1380
		public readonly string TitleText;

		// Token: 0x04000565 RID: 1381
		public readonly string DescriptionText;

		// Token: 0x04000566 RID: 1382
		public readonly List<InquiryElement> InquiryElements;

		// Token: 0x04000567 RID: 1383
		public readonly bool IsExitShown;

		// Token: 0x04000568 RID: 1384
		public readonly int MaxSelectableOptionCount;

		// Token: 0x04000569 RID: 1385
		public readonly int MinSelectableOptionCount;

		// Token: 0x0400056A RID: 1386
		public readonly string SoundEventPath;

		// Token: 0x0400056B RID: 1387
		public readonly string AffirmativeText;

		// Token: 0x0400056C RID: 1388
		public readonly string NegativeText;

		// Token: 0x0400056D RID: 1389
		public readonly Action<List<InquiryElement>> AffirmativeAction;

		// Token: 0x0400056E RID: 1390
		public readonly Action<List<InquiryElement>> NegativeAction;

		// Token: 0x0400056F RID: 1391
		public readonly bool IsSeachAvailable;
	}
}
