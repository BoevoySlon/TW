using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004D RID: 77
	public class CampaignGameStarter : IGameStarter
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x000227AA File Offset: 0x000209AA
		public ICollection<CampaignBehaviorBase> CampaignBehaviors
		{
			get
			{
				return this._campaignBehaviors;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000227B2 File Offset: 0x000209B2
		public IEnumerable<GameModel> Models
		{
			get
			{
				return this._models;
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000227BA File Offset: 0x000209BA
		public CampaignGameStarter(GameMenuManager gameMenuManager, ConversationManager conversationManager, GameTextManager gameTextManager)
		{
			this._conversationManager = conversationManager;
			this._gameTextManager = gameTextManager;
			this._gameMenuManager = gameMenuManager;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000227ED File Offset: 0x000209ED
		public void UnregisterNonReadyObjects()
		{
			Game.Current.ObjectManager.UnregisterNonReadyObjects();
			this._gameMenuManager.UnregisterNonReadyObjects();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00022809 File Offset: 0x00020A09
		public void AddBehavior(CampaignBehaviorBase campaignBehavior)
		{
			if (campaignBehavior != null)
			{
				this._campaignBehaviors.Add(campaignBehavior);
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002281C File Offset: 0x00020A1C
		public void RemoveBehaviors<T>() where T : CampaignBehaviorBase
		{
			for (int i = this._campaignBehaviors.Count - 1; i >= 0; i--)
			{
				if (this._campaignBehaviors[i] is T)
				{
					this._campaignBehaviors.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00022860 File Offset: 0x00020A60
		public bool RemoveBehavior<T>(T behavior) where T : CampaignBehaviorBase
		{
			return this._campaignBehaviors.Remove(behavior);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00022874 File Offset: 0x00020A74
		public void AddModel(GameModel model)
		{
			if (model != null)
			{
				if (this._models.FindIndex((GameModel x) => x.GetType() == model.GetType()) >= 0)
				{
					throw new ArgumentException();
				}
				this._models.Add(model);
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000228C7 File Offset: 0x00020AC7
		public void AddGameMenu(string menuId, string menuText, OnInitDelegate initDelegate, GameOverlays.MenuOverlayType overlay = GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags menuFlags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.GetPresumedGameMenu(menuId).Initialize(new TextObject(menuText, null), initDelegate, overlay, menuFlags, relatedObject);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x000228E4 File Offset: 0x00020AE4
		public void AddWaitGameMenu(string idString, string text, OnInitDelegate initDelegate, OnConditionDelegate condition, OnConsequenceDelegate consequence, OnTickDelegate tick, GameMenu.MenuAndOptionType type, GameOverlays.MenuOverlayType overlay = GameOverlays.MenuOverlayType.None, float targetWaitHours = 0f, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.GetPresumedGameMenu(idString).Initialize(new TextObject(text, null), initDelegate, condition, consequence, tick, type, overlay, targetWaitHours, flags, relatedObject);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00022918 File Offset: 0x00020B18
		public void AddGameMenuOption(string menuId, string optionId, string optionText, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, bool isLeave = false, int index = -1, bool isRepeatable = false, object relatedObject = null)
		{
			this.GetPresumedGameMenu(menuId).AddOption(optionId, new TextObject(optionText, null), condition, consequence, index, isLeave, isRepeatable, relatedObject);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00022948 File Offset: 0x00020B48
		public GameMenu GetPresumedGameMenu(string stringId)
		{
			GameMenu gameMenu = this._gameMenuManager.GetGameMenu(stringId);
			if (gameMenu == null)
			{
				gameMenu = new GameMenu(stringId);
				this._gameMenuManager.AddGameMenu(gameMenu);
			}
			return gameMenu;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00022979 File Offset: 0x00020B79
		private ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			this._conversationManager.AddDialogLine(dialogLine);
			return dialogLine;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002298C File Offset: 0x00020B8C
		public ConversationSentence AddPlayerLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 1U, priority, 0, 0, null, false, null, null, persuasionOptionDelegate));
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000229C0 File Offset: 0x00020BC0
		public ConversationSentence AddRepeatablePlayerLine(string id, string inputToken, string outputToken, string text, string continueListingRepeatedObjectsText, string continueListingOptionOutputToken, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			ConversationSentence result = this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 3U, priority, 0, 0, null, false, null, null, null));
			this.AddDialogLine(new ConversationSentence(id + "_continue", new TextObject(continueListingRepeatedObjectsText, null), inputToken, continueListingOptionOutputToken, new ConversationSentence.OnConditionDelegate(ConversationManager.IsThereMultipleRepeatablePages), null, new ConversationSentence.OnConsequenceDelegate(ConversationManager.DialogRepeatContinueListing), 1U, priority, 0, 0, null, false, null, null, null));
			return result;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00022A38 File Offset: 0x00020C38
		public ConversationSentence AddDialogLineWithVariation(string id, string inputToken, string outputToken, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, string idleActionId = "", string idleFaceAnimId = "", string reactionId = "", string reactionFaceAnimId = "", ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject("{=7AyjDt96}{VARIATION_TEXT_TAGGED_LINE}", null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, null, true, null, null, null));
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00022A70 File Offset: 0x00020C70
		public ConversationSentence AddDialogLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, null, false, null, null, null));
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00022AA4 File Offset: 0x00020CA4
		public ConversationSentence AddDialogLineMultiAgent(string id, string inputToken, string outputToken, TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int agentIndex, int nextAgentIndex, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, text, inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, agentIndex, nextAgentIndex, null, false, null, null, null));
		}

		// Token: 0x04000289 RID: 649
		private readonly GameMenuManager _gameMenuManager;

		// Token: 0x0400028A RID: 650
		private readonly GameTextManager _gameTextManager;

		// Token: 0x0400028B RID: 651
		private readonly ConversationManager _conversationManager;

		// Token: 0x0400028C RID: 652
		private readonly List<CampaignBehaviorBase> _campaignBehaviors = new List<CampaignBehaviorBase>();

		// Token: 0x0400028D RID: 653
		private readonly List<GameModel> _models = new List<GameModel>();
	}
}
