﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200031B RID: 795
	public class ScoutEnemyGarrisonsIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x06002DD9 RID: 11737 RVA: 0x000C07C3 File Offset: 0x000BE9C3
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000C07DC File Offset: 0x000BE9DC
		public void OnCheckForIssue(Hero hero)
		{
			List<Settlement> relatedObject;
			if (this.ConditionsHold(hero, out relatedObject))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon, relatedObject));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000C0844 File Offset: 0x000BEA44
		private bool ConditionsHold(Hero issueGiver, out List<Settlement> settlements)
		{
			settlements = new List<Settlement>();
			if (issueGiver.MapFaction.IsKingdomFaction && issueGiver.IsFactionLeader && !issueGiver.IsMinorFactionHero && !issueGiver.IsPrisoner && !issueGiver.IsFugitive)
			{
				if (issueGiver.GetMapPoint() != null)
				{
					Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate((Kingdom x) => x.IsAtWarWith(issueGiver.MapFaction));
					if (randomElementWithPredicate != null)
					{
						List<Settlement> list = (from x in randomElementWithPredicate.Settlements
						where ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(x, issueGiver)
						select x).ToList<Settlement>();
						if (list.Count >= 5)
						{
							list = (from y in list
							orderby issueGiver.GetMapPoint().Position2D.Distance(y.Position2D)
							select y).ToList<Settlement>();
							settlements = list.Take(3).ToList<Settlement>();
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000C0930 File Offset: 0x000BEB30
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue(issueOwner, potentialIssueData.RelatedObject as List<Settlement>);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000C0956 File Offset: 0x000BEB56
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000C0958 File Offset: 0x000BEB58
		private static bool SuitableSettlementCondition(Settlement settlement, Hero issueGiver)
		{
			return settlement.IsFortification && settlement.MapFaction.IsAtWarWith(issueGiver.MapFaction) && (!settlement.IsUnderSiege || settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapFaction != Hero.MainHero.MapFaction);
		}

		// Token: 0x04000DB0 RID: 3504
		private const IssueBase.IssueFrequency ScoutEnemyGarrisonsIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000DB1 RID: 3505
		private const int QuestDurationInDays = 15;

		// Token: 0x0200066E RID: 1646
		public class ScoutEnemyGarrisonsIssue : IssueBase
		{
			// Token: 0x06005314 RID: 21268 RVA: 0x001784B3 File Offset: 0x001766B3
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsIssue(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005315 RID: 21269 RVA: 0x001784C1 File Offset: 0x001766C1
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._settlement1);
				collectedObjects.Add(this._settlement2);
				collectedObjects.Add(this._settlement3);
			}

			// Token: 0x06005316 RID: 21270 RVA: 0x001784EE File Offset: 0x001766EE
			internal static object AutoGeneratedGetMemberValue_settlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement1;
			}

			// Token: 0x06005317 RID: 21271 RVA: 0x001784FB File Offset: 0x001766FB
			internal static object AutoGeneratedGetMemberValue_settlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement2;
			}

			// Token: 0x06005318 RID: 21272 RVA: 0x00178508 File Offset: 0x00176708
			internal static object AutoGeneratedGetMemberValue_settlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement3;
			}

			// Token: 0x17001198 RID: 4504
			// (get) Token: 0x06005319 RID: 21273 RVA: 0x00178515 File Offset: 0x00176715
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001199 RID: 4505
			// (get) Token: 0x0600531A RID: 21274 RVA: 0x00178518 File Offset: 0x00176718
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700119A RID: 4506
			// (get) Token: 0x0600531B RID: 21275 RVA: 0x0017851B File Offset: 0x0017671B
			protected override int RewardGold
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x1700119B RID: 4507
			// (get) Token: 0x0600531C RID: 21276 RVA: 0x0017851E File Offset: 0x0017671E
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=rrCkJgtd}We don't know enough about the enemy, [ib:closed][if:convo_thinking]where they are strong and where they are weak. I don't want to lead a huge army through their territory on a wild goose hunt. We need someone to ride through there swiftly, scouting out their garrisons. Can you do this?", null);
				}
			}

			// Token: 0x1700119C RID: 4508
			// (get) Token: 0x0600531D RID: 21277 RVA: 0x0017852C File Offset: 0x0017672C
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=dGakGflE}Yes, your {?QUEST_GIVER.GENDER}ladyship{?}lordship{\\?}, I'll gladly do it.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700119D RID: 4509
			// (get) Token: 0x0600531E RID: 21278 RVA: 0x00178560 File Offset: 0x00176760
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=seEyGLMz}Go deep into {ENEMY} territory, to {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}. [ib:hip][if:convo_normal]I want to know every detail about them, what sort of fortifications they have, whether the walls are well-manned or undergarrisoned, and any other enemy forces in the vicinity.", null);
					textObject.SetTextVariable("ENEMY", this._settlement1.MapFaction.Name);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x1700119E RID: 4510
			// (get) Token: 0x0600531F RID: 21279 RVA: 0x001785D9 File Offset: 0x001767D9
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=g6P6nKIf}Consider it done, commander.", null);
				}
			}

			// Token: 0x1700119F RID: 4511
			// (get) Token: 0x06005320 RID: 21280 RVA: 0x001785E6 File Offset: 0x001767E6
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x170011A0 RID: 4512
			// (get) Token: 0x06005321 RID: 21281 RVA: 0x001785F4 File Offset: 0x001767F4
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=AdoaDR26}{QUEST_GIVER.LINK} asks you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x06005322 RID: 21282 RVA: 0x0017866B File Offset: 0x0017686B
			public ScoutEnemyGarrisonsIssue(Hero issueOwner, List<Settlement> settlements) : base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this._settlement1 = settlements[0];
				this._settlement2 = settlements[1];
				this._settlement3 = settlements[2];
			}

			// Token: 0x06005323 RID: 21283 RVA: 0x001786A5 File Offset: 0x001768A5
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005324 RID: 21284 RVA: 0x001786A7 File Offset: 0x001768A7
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005325 RID: 21285 RVA: 0x001786A9 File Offset: 0x001768A9
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest(questId, base.IssueOwner, this._settlement1, this._settlement2, this._settlement3);
			}

			// Token: 0x06005326 RID: 21286 RVA: 0x001786C9 File Offset: 0x001768C9
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x06005327 RID: 21287 RVA: 0x001786CC File Offset: 0x001768CC
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill)
			{
				relationHero = null;
				skill = null;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Hero.MainHero.IsKingdomLeader)
				{
					flag |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				if (Clan.PlayerClan.Tier < 2)
				{
					flag |= IssueBase.PreconditionFlags.ClanTier;
				}
				if (Hero.MainHero.GetSkillValue(DefaultSkills.Scouting) < 30)
				{
					flag |= IssueBase.PreconditionFlags.Skill;
					skill = DefaultSkills.Scouting;
				}
				if (Hero.MainHero.MapFaction != base.IssueOwner.MapFaction)
				{
					flag |= IssueBase.PreconditionFlags.NotInSameFaction;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005328 RID: 21288 RVA: 0x0017878C File Offset: 0x0017698C
			public override bool IssueStayAliveConditions()
			{
				bool flag = this._settlement1.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement2.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement3.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction);
				if (!flag)
				{
					flag = this.TryToUpdateSettlements();
				}
				return flag && base.IssueOwner.MapFaction.IsKingdomFaction;
			}

			// Token: 0x06005329 RID: 21289 RVA: 0x00178811 File Offset: 0x00176A11
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x0600532A RID: 21290 RVA: 0x00178828 File Offset: 0x00176A28
			private bool TryToUpdateSettlements()
			{
				Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate((Kingdom x) => x.IsAtWarWith(base.IssueOwner.MapFaction));
				if (randomElementWithPredicate != null)
				{
					List<Settlement> list = (from x in randomElementWithPredicate.Settlements
					where ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(x, base.IssueOwner)
					select x).ToList<Settlement>();
					if (list.Count >= 5)
					{
						list = list.Take(3).ToList<Settlement>();
						this._settlement1 = list[0];
						this._settlement2 = list[1];
						this._settlement3 = list[2];
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600532B RID: 21291 RVA: 0x001788AC File Offset: 0x00176AAC
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001B14 RID: 6932
			private const int MinimumRelationToTakeQuest = -10;

			// Token: 0x04001B15 RID: 6933
			[SaveableField(10)]
			private Settlement _settlement1;

			// Token: 0x04001B16 RID: 6934
			[SaveableField(20)]
			private Settlement _settlement2;

			// Token: 0x04001B17 RID: 6935
			[SaveableField(30)]
			private Settlement _settlement3;
		}

		// Token: 0x0200066F RID: 1647
		public class ScoutEnemyGarrisonsQuest : QuestBase
		{
			// Token: 0x0600532E RID: 21294 RVA: 0x001788CF File Offset: 0x00176ACF
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsQuest(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600532F RID: 21295 RVA: 0x001788DD File Offset: 0x00176ADD
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questSettlement1);
				collectedObjects.Add(this._questSettlement2);
				collectedObjects.Add(this._questSettlement3);
				collectedObjects.Add(this._startQuestLog);
			}

			// Token: 0x06005330 RID: 21296 RVA: 0x00178916 File Offset: 0x00176B16
			internal static object AutoGeneratedGetMemberValue_questSettlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement1;
			}

			// Token: 0x06005331 RID: 21297 RVA: 0x00178923 File Offset: 0x00176B23
			internal static object AutoGeneratedGetMemberValue_questSettlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement2;
			}

			// Token: 0x06005332 RID: 21298 RVA: 0x00178930 File Offset: 0x00176B30
			internal static object AutoGeneratedGetMemberValue_questSettlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement3;
			}

			// Token: 0x06005333 RID: 21299 RVA: 0x0017893D File Offset: 0x00176B3D
			internal static object AutoGeneratedGetMemberValue_scoutedSettlementCount(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._scoutedSettlementCount;
			}

			// Token: 0x06005334 RID: 21300 RVA: 0x0017894F File Offset: 0x00176B4F
			internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._startQuestLog;
			}

			// Token: 0x170011A1 RID: 4513
			// (get) Token: 0x06005335 RID: 21301 RVA: 0x0017895C File Offset: 0x00176B5C
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011A2 RID: 4514
			// (get) Token: 0x06005336 RID: 21302 RVA: 0x0017895F File Offset: 0x00176B5F
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x170011A3 RID: 4515
			// (get) Token: 0x06005337 RID: 21303 RVA: 0x0017896C File Offset: 0x00176B6C
			private TextObject _playerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8avwit9N}{QUEST_GIVER.LINK}, the army commander of {FACTION} has told you that they need detailed information about enemy fortifications and troop numbers of the enemy. {?QUEST_GIVER.GENDER}She{?}He{\\?} wanted you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("FACTION", base.QuestGiver.MapFaction.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_1", this._questSettlement1.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_2", this._questSettlement2.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_3", this._questSettlement3.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170011A4 RID: 4516
			// (get) Token: 0x06005338 RID: 21304 RVA: 0x00178A0E File Offset: 0x00176C0E
			private TextObject _settlementBecomeNeutralLogText
			{
				get
				{
					return new TextObject("{=wgX2nL5Z}{SETTLEMENT} is no longer in control of enemy. There is no need to scout that settlement.", null);
				}
			}

			// Token: 0x170011A5 RID: 4517
			// (get) Token: 0x06005339 RID: 21305 RVA: 0x00178A1B File Offset: 0x00176C1B
			private TextObject _armyDisbandedQuestCancelLogText
			{
				get
				{
					return new TextObject("{=JiHaL6IV}Army has disbanded and your mission has been canceled.", null);
				}
			}

			// Token: 0x170011A6 RID: 4518
			// (get) Token: 0x0600533A RID: 21306 RVA: 0x00178A28 File Offset: 0x00176C28
			private TextObject _noLongerAllyQuestCancelLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vTnSa9rr}You are no longer allied with {QUEST_GIVER.LINK}'s faction. Your agreement with {QUEST_GIVER.LINK} was terminated.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011A7 RID: 4519
			// (get) Token: 0x0600533B RID: 21307 RVA: 0x00178A5A File Offset: 0x00176C5A
			private TextObject _allTargetsAreNeutral
			{
				get
				{
					return new TextObject("{=LC2F84GR}None of the target settlements are in control of the enemy. Army Commander has canceled the mission.", null);
				}
			}

			// Token: 0x170011A8 RID: 4520
			// (get) Token: 0x0600533C RID: 21308 RVA: 0x00178A67 File Offset: 0x00176C67
			private TextObject _scoutFinishedForSettlementWallLevel1LogText
			{
				get
				{
					return new TextObject("{=5kxDhBWk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are not high enough but can be useful with sufficient garrison support.", null);
				}
			}

			// Token: 0x170011A9 RID: 4521
			// (get) Token: 0x0600533D RID: 21309 RVA: 0x00178A74 File Offset: 0x00176C74
			private TextObject _scoutFinishedForSettlementWallLevel2LogText
			{
				get
				{
					return new TextObject("{=GUqjL6xk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are high enough to defend against invaders.", null);
				}
			}

			// Token: 0x170011AA RID: 4522
			// (get) Token: 0x0600533E RID: 21310 RVA: 0x00178A81 File Offset: 0x00176C81
			private TextObject _scoutFinishedForSettlementWallLevel3LogText
			{
				get
				{
					return new TextObject("{=YErURO5l}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are too high and hard to breach.", null);
				}
			}

			// Token: 0x170011AB RID: 4523
			// (get) Token: 0x0600533F RID: 21311 RVA: 0x00178A8E File Offset: 0x00176C8E
			private TextObject _questSuccess
			{
				get
				{
					return new TextObject("{=Qy7Zmmvk}You have successfully scouted the target settlements.", null);
				}
			}

			// Token: 0x170011AC RID: 4524
			// (get) Token: 0x06005340 RID: 21312 RVA: 0x00178A9B File Offset: 0x00176C9B
			private TextObject _questTimedOut
			{
				get
				{
					return new TextObject("{=GzodT3vS}You have failed to scout the enemy settlements in time.", null);
				}
			}

			// Token: 0x06005341 RID: 21313 RVA: 0x00178AA8 File Offset: 0x00176CA8
			public ScoutEnemyGarrisonsQuest(string questId, Hero questGiver, Settlement settlement1, Settlement settlement2, Settlement settlement3) : base(questId, questGiver, CampaignTime.DaysFromNow(15f), 0)
			{
				this._questSettlement1 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement1, 0);
				this._questSettlement2 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement2, 0);
				this._questSettlement3 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement3, 0);
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06005342 RID: 21314 RVA: 0x00178AFD File Offset: 0x00176CFD
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06005343 RID: 21315 RVA: 0x00178B08 File Offset: 0x00176D08
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=lyGvyZK4}Very well. When you reach one of their fortresses, spend some time observing. Don't move on to the next one at once. You don't need to find me to report back the details, just send your messengers.", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=x3TO0gkN}Is there any progress on the task I gave you?[ib:closed][if:convo_normal]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
				}).BeginPlayerOptions().PlayerOption(new TextObject("{=W5ab31gQ}Soon, commander. We are still working on it.", null), null).NpcLine(new TextObject("{=U3LR7dyK}Good. I'll be waiting for your messengers.[if:convo_thinking]", null), null, null).CloseDialog().PlayerOption(new TextObject("{=v75k1FoT}Not yet. We need to make more preparations.", null), null).NpcLine(new TextObject("{=zYKeYZAo}All right. Don't rush this but also don't wait too long.", null), null, null).CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x06005344 RID: 21316 RVA: 0x00178C18 File Offset: 0x00176E18
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddTrackedObject(this._questSettlement1.Settlement);
				base.AddTrackedObject(this._questSettlement2.Settlement);
				base.AddTrackedObject(this._questSettlement3.Settlement);
				this._scoutedSettlementCount = 0;
				this._startQuestLog = base.AddDiscreteLog(this._playerStartsQuestLogText, new TextObject("{=jpBpwgAs}Settlements", null), this._scoutedSettlementCount, 3, null, false);
			}

			// Token: 0x06005345 RID: 21317 RVA: 0x00178C8C File Offset: 0x00176E8C
			protected override void RegisterEvents()
			{
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x06005346 RID: 21318 RVA: 0x00178CE0 File Offset: 0x00176EE0
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement>
					{
						this._questSettlement1,
						this._questSettlement2,
						this._questSettlement3
					};
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => !x.Settlement.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						base.AddLog(this._allTargetsAreNeutral, false);
						base.CompleteQuestWithCancel(null);
						return;
					}
					foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
					{
						if (!questSettlement.IsScoutingCompleted())
						{
							if (Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, questSettlement.Settlement) <= MobileParty.MainParty.SeeingRange)
							{
								questSettlement.CurrentScoutProgress++;
								if (questSettlement.CurrentScoutProgress == 1)
								{
									TextObject textObject = new TextObject("{=qfjRGjM4}Your scouts started to gather information about {SETTLEMENT}.", null);
									textObject.SetTextVariable("SETTLEMENT", questSettlement.Settlement.Name);
									MBInformationManager.AddQuickInformation(textObject, 0, null, "");
								}
								else if (questSettlement.IsScoutingCompleted())
								{
									JournalLog startQuestLog = this._startQuestLog;
									int num = this._scoutedSettlementCount + 1;
									this._scoutedSettlementCount = num;
									startQuestLog.UpdateCurrentProgress(num);
									base.RemoveTrackedObject(questSettlement.Settlement);
									TextObject textObject2 = TextObject.Empty;
									if (questSettlement.Settlement.Town.GetWallLevel() == 1)
									{
										textObject2 = this._scoutFinishedForSettlementWallLevel1LogText;
									}
									else if (questSettlement.Settlement.Town.GetWallLevel() == 2)
									{
										textObject2 = this._scoutFinishedForSettlementWallLevel2LogText;
									}
									else
									{
										textObject2 = this._scoutFinishedForSettlementWallLevel3LogText;
									}
									textObject2.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
									MobileParty garrisonParty = questSettlement.Settlement.Town.GarrisonParty;
									int num2 = (garrisonParty != null) ? garrisonParty.MemberRoster.TotalHealthyCount : 0;
									int num3 = (int)questSettlement.Settlement.Militia;
									textObject2.SetTextVariable("GARRISON_SIZE", num2 + num3);
									base.AddLog(textObject2, false);
								}
							}
							else
							{
								questSettlement.ResetCurrentProgress();
							}
						}
					}
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsScoutingCompleted()))
					{
						this.AllScoutingDone();
					}
				}
			}

			// Token: 0x06005347 RID: 21319 RVA: 0x00178F30 File Offset: 0x00177130
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement>
				{
					this._questSettlement1,
					this._questSettlement2,
					this._questSettlement3
				};
				foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
				{
					if (settlement == questSettlement.Settlement && (newOwner.MapFaction == base.QuestGiver.MapFaction || !newOwner.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						questSettlement.IsCompletedThroughBeingNeutral = true;
						questSettlement.SetScoutingCompleted();
						JournalLog startQuestLog = this._startQuestLog;
						int num = this._scoutedSettlementCount + 1;
						this._scoutedSettlementCount = num;
						startQuestLog.UpdateCurrentProgress(num);
						if (base.IsTracked(questSettlement.Settlement))
						{
							base.RemoveTrackedObject(questSettlement.Settlement);
						}
						TextObject settlementBecomeNeutralLogText = this._settlementBecomeNeutralLogText;
						settlementBecomeNeutralLogText.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
						base.AddLog(settlementBecomeNeutralLogText, false);
						if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsCompletedThroughBeingNeutral))
						{
							base.AddLog(this._allTargetsAreNeutral, false);
							base.CompleteQuestWithCancel(null);
							break;
						}
						break;
					}
				}
			}

			// Token: 0x06005348 RID: 21320 RVA: 0x00179098 File Offset: 0x00177298
			private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
			{
				if (army.ArmyOwner == base.QuestGiver)
				{
					base.AddLog(this._armyDisbandedQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005349 RID: 21321 RVA: 0x001790BD File Offset: 0x001772BD
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == base.QuestGiver.MapFaction)
				{
					base.AddLog(this._noLongerAllyQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x0600534A RID: 21322 RVA: 0x001790EA File Offset: 0x001772EA
			private void AllScoutingDone()
			{
				base.AddLog(this._questSuccess, false);
				GainRenownAction.Apply(Hero.MainHero, 3f, false);
				GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, 10f);
				this.RelationshipChangeWithQuestGiver = 3;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600534B RID: 21323 RVA: 0x00179126 File Offset: 0x00177326
			protected override void OnTimedOut()
			{
				base.AddLog(this._questTimedOut, false);
				this.RelationshipChangeWithQuestGiver = -2;
			}

			// Token: 0x04001B18 RID: 6936
			[SaveableField(10)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement1;

			// Token: 0x04001B19 RID: 6937
			[SaveableField(20)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement2;

			// Token: 0x04001B1A RID: 6938
			[SaveableField(30)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement3;

			// Token: 0x04001B1B RID: 6939
			[SaveableField(40)]
			private int _scoutedSettlementCount;

			// Token: 0x04001B1C RID: 6940
			[SaveableField(50)]
			private JournalLog _startQuestLog;
		}

		// Token: 0x02000670 RID: 1648
		public class QuestSettlement
		{
			// Token: 0x0600534F RID: 21327 RVA: 0x0017917C File Offset: 0x0017737C
			internal static void AutoGeneratedStaticCollectObjectsQuestSettlement(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005350 RID: 21328 RVA: 0x0017918A File Offset: 0x0017738A
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Settlement);
			}

			// Token: 0x06005351 RID: 21329 RVA: 0x00179198 File Offset: 0x00177398
			internal static object AutoGeneratedGetMemberValueSettlement(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).Settlement;
			}

			// Token: 0x06005352 RID: 21330 RVA: 0x001791A5 File Offset: 0x001773A5
			internal static object AutoGeneratedGetMemberValueCurrentScoutProgress(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).CurrentScoutProgress;
			}

			// Token: 0x06005353 RID: 21331 RVA: 0x001791B7 File Offset: 0x001773B7
			public QuestSettlement(Settlement settlement, int currentScoutProgress)
			{
				this.Settlement = settlement;
				this.CurrentScoutProgress = currentScoutProgress;
				this.IsCompletedThroughBeingNeutral = false;
			}

			// Token: 0x06005354 RID: 21332 RVA: 0x001791D4 File Offset: 0x001773D4
			public bool IsScoutingCompleted()
			{
				return this.CurrentScoutProgress >= 8;
			}

			// Token: 0x06005355 RID: 21333 RVA: 0x001791E2 File Offset: 0x001773E2
			public void SetScoutingCompleted()
			{
				this.CurrentScoutProgress = 8;
			}

			// Token: 0x06005356 RID: 21334 RVA: 0x001791EB File Offset: 0x001773EB
			public void ResetCurrentProgress()
			{
				this.CurrentScoutProgress = 0;
			}

			// Token: 0x04001B1D RID: 6941
			private const int CompleteScoutAfterHours = 8;

			// Token: 0x04001B1E RID: 6942
			[SaveableField(10)]
			public Settlement Settlement;

			// Token: 0x04001B1F RID: 6943
			[SaveableField(20)]
			public int CurrentScoutProgress;

			// Token: 0x04001B20 RID: 6944
			public bool IsCompletedThroughBeingNeutral;
		}

		// Token: 0x02000671 RID: 1649
		public class ScoutEnemyGarrisonsIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005357 RID: 21335 RVA: 0x001791F4 File Offset: 0x001773F4
			public ScoutEnemyGarrisonsIssueTypeDefiner() : base(97600)
			{
			}

			// Token: 0x06005358 RID: 21336 RVA: 0x00179201 File Offset: 0x00177401
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), 1, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest), 2, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.QuestSettlement), 3, null);
			}
		}
	}
}
