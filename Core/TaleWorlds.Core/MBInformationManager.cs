using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000A6 RID: 166
	public static class MBInformationManager
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000838 RID: 2104 RVA: 0x0001BF48 File Offset: 0x0001A148
		// (remove) Token: 0x06000839 RID: 2105 RVA: 0x0001BF7C File Offset: 0x0001A17C
		public static event Action<string, int, BasicCharacterObject, string> FiringQuickInformation;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600083A RID: 2106 RVA: 0x0001BFB0 File Offset: 0x0001A1B0
		// (remove) Token: 0x0600083B RID: 2107 RVA: 0x0001BFE4 File Offset: 0x0001A1E4
		public static event Action<MultiSelectionInquiryData, bool, bool> OnShowMultiSelectionInquiry;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600083C RID: 2108 RVA: 0x0001C018 File Offset: 0x0001A218
		// (remove) Token: 0x0600083D RID: 2109 RVA: 0x0001C04C File Offset: 0x0001A24C
		public static event Action<InformationData> OnAddMapNotice;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600083E RID: 2110 RVA: 0x0001C080 File Offset: 0x0001A280
		// (remove) Token: 0x0600083F RID: 2111 RVA: 0x0001C0B4 File Offset: 0x0001A2B4
		public static event Action<InformationData> OnRemoveMapNotice;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000840 RID: 2112 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
		// (remove) Token: 0x06000841 RID: 2113 RVA: 0x0001C11C File Offset: 0x0001A31C
		public static event Action<SceneNotificationData> OnShowSceneNotification;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000842 RID: 2114 RVA: 0x0001C150 File Offset: 0x0001A350
		// (remove) Token: 0x06000843 RID: 2115 RVA: 0x0001C184 File Offset: 0x0001A384
		public static event Action OnHideSceneNotification;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000844 RID: 2116 RVA: 0x0001C1B8 File Offset: 0x0001A3B8
		// (remove) Token: 0x06000845 RID: 2117 RVA: 0x0001C1EC File Offset: 0x0001A3EC
		public static event Func<bool> IsAnySceneNotificationActive;

		// Token: 0x06000846 RID: 2118 RVA: 0x0001C21F File Offset: 0x0001A41F
		public static void AddQuickInformation(TextObject message, int extraTimeInMs = 0, BasicCharacterObject announcerCharacter = null, string soundEventPath = "")
		{
			Action<string, int, BasicCharacterObject, string> firingQuickInformation = MBInformationManager.FiringQuickInformation;
			if (firingQuickInformation != null)
			{
				firingQuickInformation(message.ToString(), extraTimeInMs, announcerCharacter, soundEventPath);
			}
			Debug.Print(message.ToString(), 0, Debug.DebugColor.White, 1125899906842624UL);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001C251 File Offset: 0x0001A451
		public static void ShowMultiSelectionInquiry(MultiSelectionInquiryData data, bool pauseGameActiveState = false, bool prioritize = false)
		{
			Action<MultiSelectionInquiryData, bool, bool> onShowMultiSelectionInquiry = MBInformationManager.OnShowMultiSelectionInquiry;
			if (onShowMultiSelectionInquiry == null)
			{
				return;
			}
			onShowMultiSelectionInquiry(data, pauseGameActiveState, prioritize);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001C265 File Offset: 0x0001A465
		public static void AddNotice(InformationData data)
		{
			Action<InformationData> onAddMapNotice = MBInformationManager.OnAddMapNotice;
			if (onAddMapNotice == null)
			{
				return;
			}
			onAddMapNotice(data);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001C277 File Offset: 0x0001A477
		public static void MapNoticeRemoved(InformationData data)
		{
			Action<InformationData> onRemoveMapNotice = MBInformationManager.OnRemoveMapNotice;
			if (onRemoveMapNotice == null)
			{
				return;
			}
			onRemoveMapNotice(data);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001C289 File Offset: 0x0001A489
		public static void ShowHint(string hint)
		{
			InformationManager.ShowTooltip(typeof(string), new object[]
			{
				hint
			});
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		public static void HideInformations()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001C2AB File Offset: 0x0001A4AB
		public static void ShowSceneNotification(SceneNotificationData data)
		{
			Action<SceneNotificationData> onShowSceneNotification = MBInformationManager.OnShowSceneNotification;
			if (onShowSceneNotification == null)
			{
				return;
			}
			onShowSceneNotification(data);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001C2BD File Offset: 0x0001A4BD
		public static void HideSceneNotification()
		{
			Action onHideSceneNotification = MBInformationManager.OnHideSceneNotification;
			if (onHideSceneNotification == null)
			{
				return;
			}
			onHideSceneNotification();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001C2D0 File Offset: 0x0001A4D0
		public static bool? GetIsAnySceneNotificationActive()
		{
			Func<bool> isAnySceneNotificationActive = MBInformationManager.IsAnySceneNotificationActive;
			if (isAnySceneNotificationActive == null)
			{
				return null;
			}
			return new bool?(isAnySceneNotificationActive());
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001C2FA File Offset: 0x0001A4FA
		public static void Clear()
		{
			MBInformationManager.FiringQuickInformation = null;
			MBInformationManager.OnShowMultiSelectionInquiry = null;
			MBInformationManager.OnAddMapNotice = null;
			MBInformationManager.OnRemoveMapNotice = null;
			MBInformationManager.OnShowSceneNotification = null;
			MBInformationManager.OnHideSceneNotification = null;
		}
	}
}
