using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E4 RID: 228
	public class MenuCallbackArgs
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00059CBF File Offset: 0x00057EBF
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x00059CC7 File Offset: 0x00057EC7
		public MenuContext MenuContext { get; private set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00059CD0 File Offset: 0x00057ED0
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x00059CD8 File Offset: 0x00057ED8
		public MapState MapState { get; private set; }

		// Token: 0x06001432 RID: 5170 RVA: 0x00059CE1 File Offset: 0x00057EE1
		public MenuCallbackArgs(MenuContext menuContext, TextObject text)
		{
			this.MenuContext = menuContext;
			this.Text = text;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00059D14 File Offset: 0x00057F14
		public MenuCallbackArgs(MapState mapState, TextObject text)
		{
			this.MapState = mapState;
			this.Text = text;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00059D47 File Offset: 0x00057F47
		public MenuCallbackArgs(MapState mapState, TextObject text, float dt)
		{
			this.MapState = mapState;
			this.Text = text;
			this.DeltaTime = dt;
		}

		// Token: 0x040006F3 RID: 1779
		public float DeltaTime;

		// Token: 0x040006F4 RID: 1780
		public bool IsEnabled = true;

		// Token: 0x040006F5 RID: 1781
		public TextObject Text;

		// Token: 0x040006F6 RID: 1782
		public TextObject Tooltip = TextObject.Empty;

		// Token: 0x040006F7 RID: 1783
		public GameMenuOption.IssueQuestFlags OptionQuestData;

		// Token: 0x040006F8 RID: 1784
		public GameMenuOption.LeaveType optionLeaveType;

		// Token: 0x040006F9 RID: 1785
		public TextObject MenuTitle = TextObject.Empty;
	}
}
