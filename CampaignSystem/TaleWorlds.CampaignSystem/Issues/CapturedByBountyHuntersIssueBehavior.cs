﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000300 RID: 768
	public class CapturedByBountyHuntersIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x06002CE9 RID: 11497 RVA: 0x000BC51C File Offset: 0x000BA71C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000BC535 File Offset: 0x000BA735
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000BC538 File Offset: 0x000BA738
		private bool ConditionsHold(Hero issueGiver, out Settlement selectedHideout)
		{
			selectedHideout = null;
			if (issueGiver.IsLord || (issueGiver.IsNotable && issueGiver.CurrentSettlement == null))
			{
				return false;
			}
			if (issueGiver.IsGangLeader)
			{
				selectedHideout = this.FindSuitableHideout(issueGiver);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("looter");
				return selectedHideout != null && @object != null;
			}
			return false;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000BC590 File Offset: 0x000BA790
		private Settlement FindSuitableHideout(Hero issueGiver)
		{
			Settlement result = null;
			float num = float.MaxValue;
			foreach (Hideout hideout in from t in Campaign.Current.AllHideouts
			where t.IsInfested
			select t)
			{
				float num2;
				if (!Campaign.Current.BusyHideouts.Contains(hideout.Settlement) && Campaign.Current.Models.MapDistanceModel.GetDistance(issueGiver.GetMapPoint(), hideout.Settlement, (55f < num) ? 55f : num, out num2) && num2 < num)
				{
					num = num2;
					result = hideout.Settlement;
				}
			}
			return result;
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000BC664 File Offset: 0x000BA864
		public void OnCheckForIssue(Hero hero)
		{
			Settlement relatedObject;
			if (this.ConditionsHold(hero, out relatedObject))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue), IssueBase.IssueFrequency.Common, relatedObject));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000BC6CC File Offset: 0x000BA8CC
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x04000D79 RID: 3449
		private const IssueBase.IssueFrequency CapturedByBountyHuntersIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x04000D7A RID: 3450
		private const float ValidHideoutDistance = 55f;

		// Token: 0x0200060D RID: 1549
		public class CapturedByBountyHuntersIssue : IssueBase
		{
			// Token: 0x0600496F RID: 18799 RVA: 0x00153AA0 File Offset: 0x00151CA0
			internal static void AutoGeneratedStaticCollectObjectsCapturedByBountyHuntersIssue(object o, List<object> collectedObjects)
			{
				((CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004970 RID: 18800 RVA: 0x00153AAE File Offset: 0x00151CAE
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._hideout);
			}

			// Token: 0x06004971 RID: 18801 RVA: 0x00153AC3 File Offset: 0x00151CC3
			internal static object AutoGeneratedGetMemberValue_hideout(object o)
			{
				return ((CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue)o)._hideout;
			}

			// Token: 0x17000E85 RID: 3717
			// (get) Token: 0x06004972 RID: 18802 RVA: 0x00153AD0 File Offset: 0x00151CD0
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 10;
				}
			}

			// Token: 0x17000E86 RID: 3718
			// (get) Token: 0x06004973 RID: 18803 RVA: 0x00153AD4 File Offset: 0x00151CD4
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return Math.Max(5, 4 + MathF.Ceiling(6f * base.IssueDifficultyMultiplier));
				}
			}

			// Token: 0x17000E87 RID: 3719
			// (get) Token: 0x06004974 RID: 18804 RVA: 0x00153AEF File Offset: 0x00151CEF
			protected override int RewardGold
			{
				get
				{
					return 3000;
				}
			}

			// Token: 0x17000E88 RID: 3720
			// (get) Token: 0x06004975 RID: 18805 RVA: 0x00153AF6 File Offset: 0x00151CF6
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17000E89 RID: 3721
			// (get) Token: 0x06004976 RID: 18806 RVA: 0x00153AFA File Offset: 0x00151CFA
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=QtmPWQ5a}Some of my lads have gone missing. I've got a witness who says they'd gotten themselves dead drunk drinking with another band in these parts who turned out to be filthy bounty hunters. Now my boys are all trussed up, and these treacherous animals aim to turn them in for the bounty.[if:convo_annoyed][ib:closed]", null);
				}
			}

			// Token: 0x17000E8A RID: 3722
			// (get) Token: 0x06004977 RID: 18807 RVA: 0x00153B07 File Offset: 0x00151D07
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=tZqbrlV9}How can I help you?", null);
				}
			}

			// Token: 0x17000E8B RID: 3723
			// (get) Token: 0x06004978 RID: 18808 RVA: 0x00153B14 File Offset: 0x00151D14
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=MiVYmiBc}Raid the bounty hunters' hideout and rescue my associates from them. I will make it worth your while, say {GOLD_AMOUNT} denars.[if:convo_mocking_revenge][ib:closed2]", null);
					textObject.SetTextVariable("GOLD_AMOUNT", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x17000E8C RID: 3724
			// (get) Token: 0x06004979 RID: 18809 RVA: 0x00153B33 File Offset: 0x00151D33
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=GIkvhuCC}Maybe one of your men who knows a thing or two about scouting, with {TROOP_AMOUNT} good men can deal with these scum. So what do you say?[if:convo_undecided_open]", null);
					textObject.SetTextVariable("TROOP_AMOUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x17000E8D RID: 3725
			// (get) Token: 0x0600497A RID: 18810 RVA: 0x00153B52 File Offset: 0x00151D52
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=cvWxXGo5}I can do the job.", null);
				}
			}

			// Token: 0x17000E8E RID: 3726
			// (get) Token: 0x0600497B RID: 18811 RVA: 0x00153B5F File Offset: 0x00151D5F
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=AvBNKK5y}Alright, I will have one of my companions go and rescue your associates.", null);
				}
			}

			// Token: 0x17000E8F RID: 3727
			// (get) Token: 0x0600497C RID: 18812 RVA: 0x00153B6C File Offset: 0x00151D6C
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=9u9OEZ9Y}Splendid. My men will guide your companion to the hideout.", null);
				}
			}

			// Token: 0x17000E90 RID: 3728
			// (get) Token: 0x0600497D RID: 18813 RVA: 0x00153B79 File Offset: 0x00151D79
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=zwNjgdbi}My boys are getting ready for the battle. I'm pretty sure your men will tip the balance of that fight in our favor. Thank you.", null);
				}
			}

			// Token: 0x17000E91 RID: 3729
			// (get) Token: 0x0600497E RID: 18814 RVA: 0x00153B86 File Offset: 0x00151D86
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000E92 RID: 3730
			// (get) Token: 0x0600497F RID: 18815 RVA: 0x00153B89 File Offset: 0x00151D89
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000E93 RID: 3731
			// (get) Token: 0x06004980 RID: 18816 RVA: 0x00153B8C File Offset: 0x00151D8C
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=TQyB9rAs}{ISSUE_OWNER.NAME}'s associates captured by bounty hunters.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000E94 RID: 3732
			// (get) Token: 0x06004981 RID: 18817 RVA: 0x00153BC0 File Offset: 0x00151DC0
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=HhTTzgLj}{ISSUE_OWNER.LINK}, a gang leader in {ISSUE_SETTLEMENT}, wants us to raid some bounty hunters' hideout and rescue {?ISSUE_OWNER.GENDER}her{?}his{\\?} associates.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("ISSUE_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x06004982 RID: 18818 RVA: 0x00153C0E File Offset: 0x00151E0E
			public CapturedByBountyHuntersIssue(Hero issueOwner, Settlement hideout) : base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this._hideout = hideout;
				Campaign.Current.BusyHideouts.Add(this._hideout);
			}

			// Token: 0x06004983 RID: 18819 RVA: 0x00153C3D File Offset: 0x00151E3D
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return 1f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x06004984 RID: 18820 RVA: 0x00153C60 File Offset: 0x00151E60
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Scouting) >= hero.GetSkillValue(DefaultSkills.Riding)) ? DefaultSkills.Scouting : DefaultSkills.Riding, 120);
			}

			// Token: 0x06004985 RID: 18821 RVA: 0x00153C8D File Offset: 0x00151E8D
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, false);
			}

			// Token: 0x06004986 RID: 18822 RVA: 0x00153CA5 File Offset: 0x00151EA5
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, false);
			}

			// Token: 0x17000E95 RID: 3733
			// (get) Token: 0x06004987 RID: 18823 RVA: 0x00153CC6 File Offset: 0x00151EC6
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(750f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06004988 RID: 18824 RVA: 0x00153CDB File Offset: 0x00151EDB
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueOwner.AddPower(10f);
				base.IssueOwner.CurrentSettlement.Town.Security -= 5f;
			}

			// Token: 0x06004989 RID: 18825 RVA: 0x00153D18 File Offset: 0x00151F18
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				TraitLevelingHelper.OnIssueFailed(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -10)
				});
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueOwner.AddPower(-10f);
				base.IssueOwner.CurrentSettlement.Town.Security += 5f;
			}

			// Token: 0x17000E96 RID: 3734
			// (get) Token: 0x0600498A RID: 18826 RVA: 0x00153D80 File Offset: 0x00151F80
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=U7sTASN4}{ISSUE_OWNER.LINK}, a gang leader from {QUEST_SETTLEMENT}, has told you that some bounty hunters captured some of {?ISSUE_OWNER.GENDER}her{?}his{\\?} gang members and are holding them in their hideout. {?ISSUE_OWNER.GENDER}She{?}He{\\?} wants them found and rescued. You agreed to send {TROOP_COUNT} of your men along with a {COMPANION.LINK} to find these bounty hunters and rescue {?ISSUE_OWNER.GENDER}her{?}his{\\?} associates. They should be back in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					return textObject;
				}
			}

			// Token: 0x0600498B RID: 18827 RVA: 0x00153E11 File Offset: 0x00152011
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x0600498C RID: 18828 RVA: 0x00153E14 File Offset: 0x00152014
			public override bool IssueStayAliveConditions()
			{
				return this._hideout.Hideout.IsInfested;
			}

			// Token: 0x0600498D RID: 18829 RVA: 0x00153E26 File Offset: 0x00152026
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill)
			{
				skill = null;
				relationHero = null;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600498E RID: 18830 RVA: 0x00153E50 File Offset: 0x00152050
			protected override void OnGameLoad()
			{
				if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.9", 66233) && !Campaign.Current.BusyHideouts.Contains(this._hideout))
				{
					Campaign.Current.BusyHideouts.Add(this._hideout);
				}
			}

			// Token: 0x0600498F RID: 18831 RVA: 0x00153EAB File Offset: 0x001520AB
			protected override void HourlyTick()
			{
			}

			// Token: 0x06004990 RID: 18832 RVA: 0x00153EAD File Offset: 0x001520AD
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), this.RewardGold, this._hideout);
			}

			// Token: 0x06004991 RID: 18833 RVA: 0x00153ED1 File Offset: 0x001520D1
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x06004992 RID: 18834 RVA: 0x00153ED3 File Offset: 0x001520D3
			protected override void OnIssueFinalized()
			{
				Campaign.Current.BusyHideouts.Remove(this._hideout);
			}

			// Token: 0x0400190B RID: 6411
			private const int CompanionRequiredSkillLevel = 120;

			// Token: 0x0400190C RID: 6412
			private const int IssueDuration = 15;

			// Token: 0x0400190D RID: 6413
			private const int QuestTimeLimit = 30;

			// Token: 0x0400190E RID: 6414
			[SaveableField(100)]
			private Settlement _hideout;
		}

		// Token: 0x0200060E RID: 1550
		public class CapturedByBountyHuntersIssueQuest : QuestBase
		{
			// Token: 0x06004993 RID: 18835 RVA: 0x00153EEB File Offset: 0x001520EB
			internal static void AutoGeneratedStaticCollectObjectsCapturedByBountyHuntersIssueQuest(object o, List<object> collectedObjects)
			{
				((CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004994 RID: 18836 RVA: 0x00153EF9 File Offset: 0x001520F9
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questHideout);
			}

			// Token: 0x06004995 RID: 18837 RVA: 0x00153F0E File Offset: 0x0015210E
			internal static object AutoGeneratedGetMemberValue_questHideout(object o)
			{
				return ((CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssueQuest)o)._questHideout;
			}

			// Token: 0x17000E97 RID: 3735
			// (get) Token: 0x06004996 RID: 18838 RVA: 0x00153F1C File Offset: 0x0015211C
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=TQyB9rAs}{ISSUE_OWNER.NAME}'s associates captured by bounty hunters.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000E98 RID: 3736
			// (get) Token: 0x06004997 RID: 18839 RVA: 0x00153F4E File Offset: 0x0015214E
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000E99 RID: 3737
			// (get) Token: 0x06004998 RID: 18840 RVA: 0x00153F54 File Offset: 0x00152154
			private TextObject _playerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=P7MZ0hZb}{QUEST_GIVER.LINK}, a gang leader from {QUEST_SETTLEMENT}, has told you that some bounty hunters captured some of {?QUEST_GIVER.GENDER}her{?}his{\\?} gang members and are holding them in their hideout. You told {?QUEST_GIVER.GENDER}her{?}him{\\?} you would find them yourself.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", Settlement.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000E9A RID: 3738
			// (get) Token: 0x06004999 RID: 18841 RVA: 0x00153F9C File Offset: 0x0015219C
			private TextObject _successQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=rNDRyFP4}You cleared the hideout and rescued the {QUEST_GIVER.LINK}'s associates. {?QUEST_GIVER.GENDER}She{?}He{\\?} sends you the following letter. \"Thank you for rescuing my men. I'll remember this.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000E9B RID: 3739
			// (get) Token: 0x0600499A RID: 18842 RVA: 0x00153FD0 File Offset: 0x001521D0
			private TextObject _playerLostTheFightLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=nq0qLQ1x}You lost the fight against bounty hunters and failed to rescue the {QUEST_GIVER.LINK}'s men. {?QUEST_GIVER.GENDER}She{?}He{\\?} sends you the following letter. \"I appreciate your effort but it wasn't good enough...\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000E9C RID: 3740
			// (get) Token: 0x0600499B RID: 18843 RVA: 0x00154004 File Offset: 0x00152204
			private TextObject _hideoutClearedBySomeoneElseLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=4Bub0GY6}Hideout was cleared by someone else. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000E9D RID: 3741
			// (get) Token: 0x0600499C RID: 18844 RVA: 0x00154038 File Offset: 0x00152238
			private TextObject _timeOutLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=JPAGzEhe}You failed to rescue the {QUEST_GIVER.LINK}'s men in time. {?QUEST_GIVER.GENDER}She{?}He{\\?} sends you the following letter. \"You sat on your heels doing nothing and my men will pay the price. I won't forget this...\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600499D RID: 18845 RVA: 0x0015406A File Offset: 0x0015226A
			public CapturedByBountyHuntersIssueQuest(string questId, Hero giverHero, CampaignTime duration, int rewardGold, Settlement hideout) : base(questId, giverHero, duration, rewardGold)
			{
				this._questHideout = hideout;
				Campaign.Current.BusyHideouts.Add(this._questHideout);
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600499E RID: 18846 RVA: 0x001540A0 File Offset: 0x001522A0
			private bool DialogCondition()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x0600499F RID: 18847 RVA: 0x001540B0 File Offset: 0x001522B0
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=BUM63VJq}That's the spirit. My men will tell you how to find the hideout. Rescue those poor captives, and a large sack of silver will be on your way![if:convo_approving][ib:hip]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=vYCY931w}Any news about my men?", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=DJcMau0U}Not yet. We are still looking for them.", null), null).NpcLine(new TextObject("{=VZhs6rpG}Well, try to speed it up. Once the bounty hunters turn them in, it'll be too late.", null), null, null).CloseDialog().PlayerOption(new TextObject("{=LvNTjCtQ}We need more time.", null), null).NpcLine(new TextObject("{=15wCjIBY}Take too much time, and my men will swing from the gallows. Speed it along, will you?", null), null, null).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x060049A0 RID: 18848 RVA: 0x00154197 File Offset: 0x00152397
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this._playerStartsQuestLogText, false);
			}

			// Token: 0x060049A1 RID: 18849 RVA: 0x001541AD File Offset: 0x001523AD
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x060049A2 RID: 18850 RVA: 0x001541B5 File Offset: 0x001523B5
			protected override void HourlyTick()
			{
			}

			// Token: 0x060049A3 RID: 18851 RVA: 0x001541B7 File Offset: 0x001523B7
			protected override void OnFinalize()
			{
				Campaign.Current.BusyHideouts.Remove(this._questHideout);
			}

			// Token: 0x060049A4 RID: 18852 RVA: 0x001541D0 File Offset: 0x001523D0
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			}

			// Token: 0x060049A5 RID: 18853 RVA: 0x00154224 File Offset: 0x00152424
			private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
			{
				if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.9", 66233))
				{
					if (!Campaign.Current.BusyHideouts.Contains(this._questHideout))
					{
						Campaign.Current.BusyHideouts.Add(this._questHideout);
						return;
					}
					base.CompleteQuestWithCancel(null);
					Campaign.Current.BusyHideouts.Add(this._questHideout);
				}
			}

			// Token: 0x060049A6 RID: 18854 RVA: 0x0015429C File Offset: 0x0015249C
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party == MobileParty.MainParty && settlement == base.QuestGiver.CurrentSettlement)
				{
					this._questHideout.Hideout.IsSpotted = true;
					this._questHideout.IsVisible = true;
					base.AddTrackedObject(this._questHideout);
					TextObject textObject = new TextObject("{=R9R6imnU}Scouts working for {QUEST_GIVER.NAME} marked the hideout on your map", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
				}
			}

			// Token: 0x060049A7 RID: 18855 RVA: 0x0015431C File Offset: 0x0015251C
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (mapEvent.IsPlayerMapEvent)
				{
					if (mapEvent.MapEventSettlement == this._questHideout)
					{
						if (mapEvent.DefeatedSide == mapEvent.PlayerSide || mapEvent.DefeatedSide == BattleSideEnum.None)
						{
							base.AddLog(this._playerLostTheFightLogText, false);
							this.FailConsequences(false);
							return;
						}
						base.AddLog(this._successQuestLogText, false);
						this.SuccessConsequences();
						return;
					}
				}
				else if (this._questHideout.Parties.Count == 0)
				{
					base.AddLog(this._hideoutClearedBySomeoneElseLogText, false);
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x060049A8 RID: 18856 RVA: 0x001543A8 File Offset: 0x001525A8
			private void SuccessConsequences()
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 5;
				if (base.QuestGiver.CurrentSettlement != null && base.QuestGiver.CurrentSettlement.Town != null)
				{
					base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				}
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 100)
				});
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060049A9 RID: 18857 RVA: 0x00154444 File Offset: 0x00152644
			private void FailConsequences(bool isTimedOut)
			{
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -10)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-10f);
				if (base.QuestGiver.CurrentSettlement != null && base.QuestGiver.CurrentSettlement.Town != null)
				{
					base.QuestGiver.CurrentSettlement.Town.Security += 5f;
				}
				if (!isTimedOut)
				{
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x060049AA RID: 18858 RVA: 0x001544D3 File Offset: 0x001526D3
			protected override void OnTimedOut()
			{
				base.AddLog(this._timeOutLogText, false);
				this.FailConsequences(true);
			}

			// Token: 0x0400190F RID: 6415
			[SaveableField(102)]
			private Settlement _questHideout;
		}

		// Token: 0x0200060F RID: 1551
		public class CapturedByBountyHuntersIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060049AB RID: 18859 RVA: 0x001544EA File Offset: 0x001526EA
			public CapturedByBountyHuntersIssueTypeDefiner() : base(580000)
			{
			}

			// Token: 0x060049AC RID: 18860 RVA: 0x001544F7 File Offset: 0x001526F7
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssue), 1, null);
				base.AddClassDefinition(typeof(CapturedByBountyHuntersIssueBehavior.CapturedByBountyHuntersIssueQuest), 2, null);
			}
		}
	}
}
