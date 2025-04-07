using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007A RID: 122
	public class DialogFlow
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x00048319 File Offset: 0x00046519
		private DialogFlow(string startingToken, int priority = 100)
		{
			this._currentToken = startingToken;
			this.Priority = priority;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0004833C File Offset: 0x0004653C
		private DialogFlow Line(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false)
		{
			string text2 = Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, this._currentToken, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, false);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00048377 File Offset: 0x00046577
		public DialogFlow Variation(string text, params object[] propertiesAndWeights)
		{
			return this.Variation(new TextObject(text, null), propertiesAndWeights);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00048388 File Offset: 0x00046588
		public DialogFlow Variation(TextObject text, params object[] propertiesAndWeights)
		{
			for (int i = 0; i < propertiesAndWeights.Length; i += 2)
			{
				string tagName = (string)propertiesAndWeights[i];
				int weight = Convert.ToInt32(propertiesAndWeights[i + 1]);
				List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
				list.Add(new GameTextManager.ChoiceTag(tagName, weight));
				this.Lines[this.Lines.Count - 1].AddVariation(text, list);
			}
			return this;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000483EA File Offset: 0x000465EA
		public DialogFlow NpcLine(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.NpcLine(new TextObject(npcText, null), speakerDelegate, listenerDelegate);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000483FB File Offset: 0x000465FB
		public DialogFlow NpcLine(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.Line(npcText, false, speakerDelegate, listenerDelegate, false);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00048408 File Offset: 0x00046608
		public DialogFlow NpcLineWithVariation(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			DialogFlow result = this.Line(TextObject.Empty, false, speakerDelegate, listenerDelegate, false);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(new TextObject(npcText, null), list);
			return result;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00048460 File Offset: 0x00046660
		public DialogFlow NpcLineWithVariation(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			DialogFlow result = this.Line(TextObject.Empty, false, speakerDelegate, listenerDelegate, false);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(npcText, list);
			return result;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x000484B2 File Offset: 0x000466B2
		public DialogFlow PlayerLine(string playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.Line(new TextObject(playerText, null), true, null, listenerDelegate, false);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000484C5 File Offset: 0x000466C5
		public DialogFlow PlayerLine(TextObject playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.Line(playerText, true, null, listenerDelegate, false);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000484D2 File Offset: 0x000466D2
		private DialogFlow BeginOptions(bool byPlayer)
		{
			this._curDialogFlowContext = new DialogFlowContext(this._currentToken, byPlayer, this._curDialogFlowContext);
			return this;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x000484ED File Offset: 0x000466ED
		public DialogFlow BeginPlayerOptions()
		{
			return this.BeginOptions(true);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x000484F6 File Offset: 0x000466F6
		public DialogFlow BeginNpcOptions()
		{
			return this.BeginOptions(false);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00048500 File Offset: 0x00046700
		private DialogFlow Option(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false, bool isSpecialOption = false)
		{
			string text2 = Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, this._curDialogFlowContext.Token, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, isSpecialOption);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00048541 File Offset: 0x00046741
		public DialogFlow PlayerOption(string text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.PlayerOption(new TextObject(text, null), listenerDelegate);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00048551 File Offset: 0x00046751
		public DialogFlow PlayerOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(text, true, null, listenerDelegate, false, false);
			return this;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00048561 File Offset: 0x00046761
		public DialogFlow PlayerSpecialOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(text, true, null, listenerDelegate, false, true);
			return this;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00048571 File Offset: 0x00046771
		public DialogFlow PlayerRepeatableOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(text, true, null, listenerDelegate, true, false);
			return this;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00048581 File Offset: 0x00046781
		public DialogFlow NpcOption(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(new TextObject(text, null), false, speakerDelegate, listenerDelegate, false, false);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000485A4 File Offset: 0x000467A4
		public DialogFlow NpcOption(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(text, false, speakerDelegate, listenerDelegate, false, false);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x000485C1 File Offset: 0x000467C1
		public DialogFlow NpcOptionWithVariation(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.NpcOptionWithVariation(new TextObject(text, null), conditionDelegate, speakerDelegate, listenerDelegate);
			return this;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x000485D8 File Offset: 0x000467D8
		public DialogFlow NpcOptionWithVariation(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			this.Option(TextObject.Empty, false, speakerDelegate, listenerDelegate, false, false);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this._lastLine.AddVariation(text, list);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00048628 File Offset: 0x00046828
		private DialogFlow EndOptions(bool byPlayer)
		{
			this._curDialogFlowContext = this._curDialogFlowContext.Parent;
			return this;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0004863C File Offset: 0x0004683C
		public DialogFlow EndPlayerOptions()
		{
			return this.EndOptions(true);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00048645 File Offset: 0x00046845
		public DialogFlow EndNpcOptions()
		{
			return this.EndOptions(false);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0004864E File Offset: 0x0004684E
		public DialogFlow Condition(ConversationSentence.OnConditionDelegate conditionDelegate)
		{
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0004865D File Offset: 0x0004685D
		public DialogFlow ClickableCondition(ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate)
		{
			this._lastLine.ClickableConditionDelegate = clickableConditionDelegate;
			return this;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0004866C File Offset: 0x0004686C
		public DialogFlow Consequence(ConversationSentence.OnConsequenceDelegate consequenceDelegate)
		{
			this._lastLine.ConsequenceDelegate = consequenceDelegate;
			return this;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0004867B File Offset: 0x0004687B
		public static DialogFlow CreateDialogFlow(string inputToken = null, int priority = 100)
		{
			return new DialogFlow(inputToken ?? Campaign.Current.ConversationManager.CreateToken(), priority);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00048698 File Offset: 0x00046898
		private DialogFlowLine AddLine(TextObject text, string inputToken, string outputToken, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate, bool isRepeatable, bool isSpecialOption = false)
		{
			DialogFlowLine dialogFlowLine = new DialogFlowLine();
			dialogFlowLine.Text = text;
			dialogFlowLine.InputToken = inputToken;
			dialogFlowLine.OutputToken = outputToken;
			dialogFlowLine.ByPlayer = byPlayer;
			dialogFlowLine.SpeakerDelegate = speakerDelegate;
			dialogFlowLine.ListenerDelegate = listenerDelegate;
			dialogFlowLine.IsRepeatable = isRepeatable;
			dialogFlowLine.IsSpecialOption = isSpecialOption;
			this.Lines.Add(dialogFlowLine);
			this._lastLine = dialogFlowLine;
			return dialogFlowLine;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000486FC File Offset: 0x000468FC
		public DialogFlow NpcDefaultOption(string text)
		{
			return this.NpcOption(text, null, null, null);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00048708 File Offset: 0x00046908
		public DialogFlow GotoDialogState(string input)
		{
			this._lastLine.OutputToken = input;
			this._currentToken = input;
			return this;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0004871E File Offset: 0x0004691E
		public DialogFlow GetOutputToken(out string oState)
		{
			oState = this._lastLine.OutputToken;
			return this;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0004872E File Offset: 0x0004692E
		public DialogFlow GoBackToDialogState(string iState)
		{
			this._currentToken = iState;
			return this;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00048738 File Offset: 0x00046938
		public DialogFlow CloseDialog()
		{
			this.GotoDialogState("close_window");
			return this;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00048747 File Offset: 0x00046947
		private ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			Campaign.Current.ConversationManager.AddDialogLine(dialogLine);
			return dialogLine;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004875C File Offset: 0x0004695C
		public ConversationSentence AddPlayerLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 1U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, persuasionOptionDelegate));
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00048798 File Offset: 0x00046998
		public ConversationSentence AddDialogLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, null));
		}

		// Token: 0x0400051E RID: 1310
		internal readonly List<DialogFlowLine> Lines = new List<DialogFlowLine>();

		// Token: 0x0400051F RID: 1311
		internal readonly int Priority;

		// Token: 0x04000520 RID: 1312
		private string _currentToken;

		// Token: 0x04000521 RID: 1313
		private DialogFlowLine _lastLine;

		// Token: 0x04000522 RID: 1314
		private DialogFlowContext _curDialogFlowContext;
	}
}
