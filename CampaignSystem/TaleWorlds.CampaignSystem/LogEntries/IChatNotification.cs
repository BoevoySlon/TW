using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002F3 RID: 755
	public interface IChatNotification
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06002BE6 RID: 11238
		bool IsVisibleNotification { get; }

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06002BE7 RID: 11239
		ChatNotificationType NotificationType { get; }

		// Token: 0x06002BE8 RID: 11240
		TextObject GetNotificationText();
	}
}
