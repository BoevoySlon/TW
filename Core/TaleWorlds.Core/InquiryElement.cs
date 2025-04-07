using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008D RID: 141
	public class InquiryElement
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x0001B2C5 File Offset: 0x000194C5
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = true;
			this.Hint = null;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001B2F0 File Offset: 0x000194F0
		public InquiryElement(object identifier, string title, ImageIdentifier imageIdentifier, bool isEnabled, string hint)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.ImageIdentifier = imageIdentifier;
			this.IsEnabled = isEnabled;
			this.Hint = hint;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001B320 File Offset: 0x00019520
		public bool HasSameContentWith(object other)
		{
			InquiryElement inquiryElement;
			if ((inquiryElement = (other as InquiryElement)) != null)
			{
				if (this.Title == inquiryElement.Title)
				{
					if (this.ImageIdentifier != null || inquiryElement.ImageIdentifier != null)
					{
						ImageIdentifier imageIdentifier = this.ImageIdentifier;
						if (imageIdentifier == null || !imageIdentifier.Equals(inquiryElement.ImageIdentifier))
						{
							return false;
						}
					}
					if (this.Identifier == inquiryElement.Identifier && this.IsEnabled == inquiryElement.IsEnabled)
					{
						return this.Hint == inquiryElement.Hint;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04000404 RID: 1028
		public readonly string Title;

		// Token: 0x04000405 RID: 1029
		public readonly ImageIdentifier ImageIdentifier;

		// Token: 0x04000406 RID: 1030
		public readonly object Identifier;

		// Token: 0x04000407 RID: 1031
		public readonly bool IsEnabled;

		// Token: 0x04000408 RID: 1032
		public readonly string Hint;
	}
}
