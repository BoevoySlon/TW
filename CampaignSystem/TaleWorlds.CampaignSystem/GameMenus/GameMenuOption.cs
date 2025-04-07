using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E3 RID: 227
	public class GameMenuOption
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00059AA5 File Offset: 0x00057CA5
		// (set) Token: 0x06001413 RID: 5139 RVA: 0x00059AAD File Offset: 0x00057CAD
		public GameMenu.MenuAndOptionType Type { get; private set; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00059AB6 File Offset: 0x00057CB6
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x00059ABE File Offset: 0x00057CBE
		public GameMenuOption.LeaveType OptionLeaveType { get; set; }

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00059AC7 File Offset: 0x00057CC7
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00059ACF File Offset: 0x00057CCF
		public GameMenuOption.IssueQuestFlags OptionQuestData { get; set; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00059AD8 File Offset: 0x00057CD8
		// (set) Token: 0x06001419 RID: 5145 RVA: 0x00059AE0 File Offset: 0x00057CE0
		public string IdString { get; private set; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00059AE9 File Offset: 0x00057CE9
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x00059AF1 File Offset: 0x00057CF1
		public TextObject Text { get; private set; }

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00059AFA File Offset: 0x00057CFA
		// (set) Token: 0x0600141D RID: 5149 RVA: 0x00059B02 File Offset: 0x00057D02
		public TextObject Text2 { get; private set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00059B0B File Offset: 0x00057D0B
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x00059B13 File Offset: 0x00057D13
		public TextObject Tooltip { get; private set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00059B1C File Offset: 0x00057D1C
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x00059B24 File Offset: 0x00057D24
		public bool IsLeave { get; private set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00059B2D File Offset: 0x00057D2D
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00059B35 File Offset: 0x00057D35
		public bool IsRepeatable { get; private set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00059B3E File Offset: 0x00057D3E
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00059B46 File Offset: 0x00057D46
		public bool IsEnabled { get; private set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x00059B4F File Offset: 0x00057D4F
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x00059B57 File Offset: 0x00057D57
		public object RelatedObject { get; private set; }

		// Token: 0x06001428 RID: 5160 RVA: 0x00059B60 File Offset: 0x00057D60
		internal GameMenuOption()
		{
			this.Text = TextObject.Empty;
			this.Tooltip = TextObject.Empty;
			this.IsEnabled = true;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00059B88 File Offset: 0x00057D88
		public GameMenuOption(GameMenu.MenuAndOptionType type, string idString, TextObject text, TextObject text2, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, bool isLeave = false, bool isRepeatable = false, object relatedObject = null)
		{
			this.Type = type;
			this.IdString = idString;
			this.Text = text;
			this.Text2 = text2;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Tooltip = TextObject.Empty;
			this.IsRepeatable = isRepeatable;
			this.IsEnabled = true;
			this.IsLeave = isLeave;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00059BF4 File Offset: 0x00057DF4
		public bool GetConditionsHold(Game game, MenuContext menuContext)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.Text);
				bool result = this.OnCondition(menuCallbackArgs);
				this.IsEnabled = menuCallbackArgs.IsEnabled;
				this.Tooltip = menuCallbackArgs.Tooltip;
				this.OptionQuestData = menuCallbackArgs.OptionQuestData;
				this.OptionLeaveType = menuCallbackArgs.optionLeaveType;
				return result;
			}
			return true;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00059C54 File Offset: 0x00057E54
		public void RunConsequence(MenuContext menuContext)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs args = new MenuCallbackArgs(menuContext, this.Text);
				this.OnConsequence(args);
			}
			menuContext.OnConsequence(this);
			if (Campaign.Current != null)
			{
				CampaignEventDispatcher.Instance.OnGameMenuOptionSelected(this);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00059C9B File Offset: 0x00057E9B
		public void SetEnable(bool isEnable)
		{
			this.IsEnabled = isEnable;
		}

		// Token: 0x040006E5 RID: 1765
		public static GameMenuOption.IssueQuestFlags[] IssueQuestFlagsValues = (GameMenuOption.IssueQuestFlags[])Enum.GetValues(typeof(GameMenuOption.IssueQuestFlags));

		// Token: 0x040006ED RID: 1773
		public GameMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x040006EE RID: 1774
		public GameMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x020004EF RID: 1263
		// (Invoke) Token: 0x06004388 RID: 17288
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x020004F0 RID: 1264
		// (Invoke) Token: 0x0600438C RID: 17292
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);

		// Token: 0x020004F1 RID: 1265
		public enum LeaveType
		{
			// Token: 0x04001529 RID: 5417
			Default,
			// Token: 0x0400152A RID: 5418
			Mission,
			// Token: 0x0400152B RID: 5419
			Submenu,
			// Token: 0x0400152C RID: 5420
			BribeAndEscape,
			// Token: 0x0400152D RID: 5421
			Escape,
			// Token: 0x0400152E RID: 5422
			Craft,
			// Token: 0x0400152F RID: 5423
			ForceToGiveGoods,
			// Token: 0x04001530 RID: 5424
			ForceToGiveTroops,
			// Token: 0x04001531 RID: 5425
			Bribe,
			// Token: 0x04001532 RID: 5426
			LeaveTroopsAndFlee,
			// Token: 0x04001533 RID: 5427
			OrderTroopsToAttack,
			// Token: 0x04001534 RID: 5428
			Raid,
			// Token: 0x04001535 RID: 5429
			HostileAction,
			// Token: 0x04001536 RID: 5430
			Recruit,
			// Token: 0x04001537 RID: 5431
			Trade,
			// Token: 0x04001538 RID: 5432
			Wait,
			// Token: 0x04001539 RID: 5433
			Leave,
			// Token: 0x0400153A RID: 5434
			Continue,
			// Token: 0x0400153B RID: 5435
			Manage,
			// Token: 0x0400153C RID: 5436
			TroopSelection,
			// Token: 0x0400153D RID: 5437
			WaitQuest,
			// Token: 0x0400153E RID: 5438
			Surrender,
			// Token: 0x0400153F RID: 5439
			Conversation,
			// Token: 0x04001540 RID: 5440
			DefendAction,
			// Token: 0x04001541 RID: 5441
			Devastate,
			// Token: 0x04001542 RID: 5442
			Pillage,
			// Token: 0x04001543 RID: 5443
			ShowMercy,
			// Token: 0x04001544 RID: 5444
			Leaderboard,
			// Token: 0x04001545 RID: 5445
			OpenStash,
			// Token: 0x04001546 RID: 5446
			ManageGarrison,
			// Token: 0x04001547 RID: 5447
			StagePrisonBreak,
			// Token: 0x04001548 RID: 5448
			ManagePrisoners,
			// Token: 0x04001549 RID: 5449
			Ransom,
			// Token: 0x0400154A RID: 5450
			PracticeFight,
			// Token: 0x0400154B RID: 5451
			BesiegeTown,
			// Token: 0x0400154C RID: 5452
			SneakIn,
			// Token: 0x0400154D RID: 5453
			LeadAssault,
			// Token: 0x0400154E RID: 5454
			DonateTroops,
			// Token: 0x0400154F RID: 5455
			DonatePrisoners,
			// Token: 0x04001550 RID: 5456
			SiegeAmbush,
			// Token: 0x04001551 RID: 5457
			Warehouse
		}

		// Token: 0x020004F2 RID: 1266
		[Flags]
		public enum IssueQuestFlags
		{
			// Token: 0x04001553 RID: 5459
			None = 0,
			// Token: 0x04001554 RID: 5460
			AvailableIssue = 1,
			// Token: 0x04001555 RID: 5461
			ActiveIssue = 2,
			// Token: 0x04001556 RID: 5462
			ActiveStoryQuest = 4,
			// Token: 0x04001557 RID: 5463
			TrackedIssue = 8,
			// Token: 0x04001558 RID: 5464
			TrackedStoryQuest = 16
		}
	}
}
