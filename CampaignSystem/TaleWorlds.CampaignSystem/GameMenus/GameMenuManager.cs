using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E2 RID: 226
	public class GameMenuManager
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00059025 File Offset: 0x00057225
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x0005902D File Offset: 0x0005722D
		public string NextGameMenuId { get; private set; }

		// Token: 0x060013EC RID: 5100 RVA: 0x00059036 File Offset: 0x00057236
		public GameMenuManager()
		{
			this.NextGameMenuId = null;
			this._gameMenus = new Dictionary<string, GameMenu>();
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x00059064 File Offset: 0x00057264
		public GameMenu NextMenu
		{
			get
			{
				GameMenu result;
				this._gameMenus.TryGetValue(this.NextGameMenuId, out result);
				return result;
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00059086 File Offset: 0x00057286
		public void SetNextMenu(string name)
		{
			this.NextGameMenuId = name;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0005908F File Offset: 0x0005728F
		public void ExitToLast()
		{
			if (Campaign.Current.CurrentMenuContext != null)
			{
				Game.Current.GameStateManager.LastOrDefault<MapState>().ExitMenuMode();
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000590B1 File Offset: 0x000572B1
		internal object GetSelectedRepeatableObject(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.LastSelectedMenuObject;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetSelectedObject");
			}
			return 0;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000590E0 File Offset: 0x000572E0
		internal object ObjectGetCurrentRepeatableObject(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.CurrentRepeatableObject;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not return CurrentRepeatableIndex");
			}
			return null;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0005910A File Offset: 0x0005730A
		public void SetCurrentRepeatableIndex(MenuContext menuContext, int index)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.CurrentRepeatableIndex = index;
				return;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run SetCurrentRepeatableIndex");
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00059134 File Offset: 0x00057334
		public bool GetMenuOptionConditionsHold(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				if (Game.Current == null)
				{
					throw new MBNullParameterException("Game");
				}
				return menuContext.GameMenu.GetMenuOptionConditionsHold(Game.Current, menuContext, menuItemNumber);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionConditionsHold");
				}
				return false;
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00059184 File Offset: 0x00057384
		public void RefreshMenuOptions(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				if (Game.Current == null)
				{
					throw new MBNullParameterException("Game");
				}
				int virtualMenuOptionAmount = Campaign.Current.GameMenuManager.GetVirtualMenuOptionAmount(menuContext);
				for (int i = 0; i < virtualMenuOptionAmount; i++)
				{
					this.GetMenuOptionConditionsHold(menuContext, i);
				}
				return;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionConditionsHold");
				}
				return;
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000591E5 File Offset: 0x000573E5
		public string GetMenuOptionIdString(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionIdString(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionIdString");
			}
			return "";
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00059214 File Offset: 0x00057414
		internal bool GetMenuOptionIsLeave(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionIsLeave(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return false;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005923F File Offset: 0x0005743F
		public void RunConsequencesOfMenuOption(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				if (Game.Current == null)
				{
					throw new MBNullParameterException("Game");
				}
				menuContext.GameMenu.RunMenuOptionConsequence(menuContext, menuItemNumber);
				return;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run RunConsequencesOfMenuOption");
				}
				return;
			}
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0005927C File Offset: 0x0005747C
		internal void SetRepeatObjectList(MenuContext menuContext, IEnumerable<object> list)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.MenuRepeatObjects = list.ToList<object>();
				return;
			}
			Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "SetRepeatObjectList", 228);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x000592B4 File Offset: 0x000574B4
		public TextObject GetVirtualMenuOptionTooltip(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionTooltip(menuContext, 0);
				}
				return this.GetMenuOptionTooltip(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return TextObject.Empty;
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0005932F File Offset: 0x0005752F
		public GameOverlays.MenuOverlayType GetMenuOverlayType(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.OverlayType;
			}
			return GameOverlays.MenuOverlayType.SettlementWithCharacters;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00059348 File Offset: 0x00057548
		public TextObject GetVirtualMenuOptionText(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionText(menuContext, 0);
				}
				return this.GetMenuOptionText(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return TextObject.Empty;
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000593C4 File Offset: 0x000575C4
		public GameMenuOption GetVirtualGameMenuOption(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetGameMenuOption");
			}
			int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
			if (virtualMenuItemIndex < num)
			{
				return menuContext.GameMenu.GetGameMenuOption(0);
			}
			return menuContext.GameMenu.GetGameMenuOption(virtualMenuItemIndex + 1 - num);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0005942C File Offset: 0x0005762C
		public TextObject GetVirtualMenuOptionText2(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionText2(menuContext, 0);
				}
				return this.GetMenuOptionText2(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return TextObject.Empty;
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000594A7 File Offset: 0x000576A7
		public float GetVirtualMenuProgress(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.Progress;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return 0f;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x000594D5 File Offset: 0x000576D5
		public GameMenu.MenuAndOptionType GetVirtualMenuAndOptionType(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.Type;
			}
			return GameMenu.MenuAndOptionType.RegularMenuOption;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000594EC File Offset: 0x000576EC
		public bool GetVirtualMenuIsWaitActive(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.IsWaitActive;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return false;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00059516 File Offset: 0x00057716
		public float GetVirtualMenuTargetWaitHours(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.TargetWaitHours;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return 0f;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00059544 File Offset: 0x00057744
		public bool GetVirtualMenuOptionIsEnabled(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return menuContext.GameMenu.MenuOptions.ElementAt(0).IsEnabled;
				}
				return menuContext.GameMenu.MenuOptions.ElementAt(virtualMenuItemIndex + 1 - num).IsEnabled;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return false;
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000595D8 File Offset: 0x000577D8
		public int GetVirtualMenuOptionAmount(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				int count = menuContext.GameMenu.MenuRepeatObjects.Count;
				int menuItemAmount = menuContext.GameMenu.MenuItemAmount;
				if (count == 0)
				{
					return menuItemAmount;
				}
				return menuItemAmount - 1 + count;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionAmount");
				}
				return 0;
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0005962C File Offset: 0x0005782C
		public bool GetVirtualMenuOptionIsLeave(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionIsLeave(menuContext, 0);
				}
				return this.GetMenuOptionIsLeave(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return false;
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000596A3 File Offset: 0x000578A3
		public GameMenuOption GetLeaveMenuOption(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetLeaveMenuOption(Game.Current, menuContext);
			}
			return null;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000596C0 File Offset: 0x000578C0
		internal void RunConsequenceOfVirtualMenuOption(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					if (menuContext.GameMenu.MenuRepeatObjects.Count > 0)
					{
						menuContext.GameMenu.LastSelectedMenuObject = menuContext.GameMenu.MenuRepeatObjects[virtualMenuItemIndex];
					}
					this.RunConsequencesOfMenuOption(menuContext, 0);
					return;
				}
				this.RunConsequencesOfMenuOption(menuContext, virtualMenuItemIndex + 1 - num);
				return;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run RunVirtualMenuItemConsequence");
				}
				return;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00059758 File Offset: 0x00057958
		public bool GetVirtualMenuOptionConditionsHold(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = (menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1;
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionConditionsHold(menuContext, 0);
				}
				return this.GetMenuOptionConditionsHold(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionConditionsHold");
				}
				return false;
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000597CF File Offset: 0x000579CF
		public void OnFrameTick(MenuContext menuContext, float dt)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.RunOnTick(menuContext, dt);
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000597E6 File Offset: 0x000579E6
		public TextObject GetMenuText(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetText();
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuText");
			}
			return TextObject.Empty;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00059814 File Offset: 0x00057A14
		private TextObject GetMenuOptionText(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionText(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return TextObject.Empty;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00059843 File Offset: 0x00057A43
		private TextObject GetMenuOptionText2(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionText2(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return TextObject.Empty;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00059872 File Offset: 0x00057A72
		private TextObject GetMenuOptionTooltip(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				return menuContext.GameMenu.GetMenuOptionTooltip(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return TextObject.Empty;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x000598AE File Offset: 0x00057AAE
		public void AddGameMenu(GameMenu gameMenu)
		{
			this._gameMenus.Add(gameMenu.StringId, gameMenu);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x000598C4 File Offset: 0x00057AC4
		public void RemoveRelatedGameMenus(object relatedObject)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, GameMenu> keyValuePair in this._gameMenus)
			{
				if (keyValuePair.Value.RelatedObject == relatedObject)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (string key in list)
			{
				this._gameMenus.Remove(key);
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00059978 File Offset: 0x00057B78
		public void RemoveRelatedGameMenuOptions(object relatedObject)
		{
			foreach (KeyValuePair<string, GameMenu> keyValuePair in this._gameMenus.ToList<KeyValuePair<string, GameMenu>>())
			{
				foreach (GameMenuOption gameMenuOption in keyValuePair.Value.MenuOptions.ToList<GameMenuOption>())
				{
					if (gameMenuOption.RelatedObject == relatedObject)
					{
						keyValuePair.Value.RemoveMenuOption(gameMenuOption);
					}
				}
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00059A28 File Offset: 0x00057C28
		internal void UnregisterNonReadyObjects()
		{
			MBList<KeyValuePair<string, GameMenu>> mblist = this._gameMenus.ToMBList<KeyValuePair<string, GameMenu>>();
			for (int i = mblist.Count - 1; i >= 0; i--)
			{
				if (!mblist[i].Value.IsReady)
				{
					this._gameMenus.Remove(mblist[i].Key);
				}
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00059A88 File Offset: 0x00057C88
		public GameMenu GetGameMenu(string menuId)
		{
			GameMenu result;
			this._gameMenus.TryGetValue(menuId, out result);
			return result;
		}

		// Token: 0x040006DC RID: 1756
		private Dictionary<string, GameMenu> _gameMenus;

		// Token: 0x040006DE RID: 1758
		public int PreviouslySelectedGameMenuItem = -1;

		// Token: 0x040006DF RID: 1759
		public Location NextLocation;

		// Token: 0x040006E0 RID: 1760
		public Location PreviousLocation;

		// Token: 0x040006E1 RID: 1761
		public List<Location> MenuLocations = new List<Location>();

		// Token: 0x040006E2 RID: 1762
		public object PreviouslySelectedGameMenuObject;
	}
}
