using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000302 RID: 770
	public class EscortMerchantCaravanIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x000BC813 File Offset: 0x000BAA13
		private static EscortMerchantCaravanIssueBehavior Instance
		{
			get
			{
				return Campaign.Current.GetCampaignBehavior<EscortMerchantCaravanIssueBehavior>();
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000BC820 File Offset: 0x000BAA20
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000BC874 File Offset: 0x000BAA74
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeOnStart();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("e1.9.1", 66233))
			{
				for (int i = MobileParty.All.Count - 1; i >= 0; i--)
				{
					MobileParty mobileParty = MobileParty.All[i];
					if (mobileParty.StringId.Contains("defend_caravan_quest"))
					{
						if (mobileParty.MapEvent != null)
						{
							mobileParty.MapEvent.FinalizeEvent();
						}
						DestroyPartyAction.Apply(null, MobileParty.All[i]);
					}
				}
			}
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000BC904 File Offset: 0x000BAB04
		private void InitializeOnStart()
		{
			if (MBObjectManager.Instance.GetObject<ItemObject>("hardwood") == null || MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse") == null)
			{
				CampaignEventDispatcher.Instance.RemoveListeners(this);
				using (List<KeyValuePair<Hero, IssueBase>>.Enumerator enumerator = (from x in Campaign.Current.IssueManager.Issues
				where x.Value.GetType() == typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)
				select x).ToList<KeyValuePair<Hero, IssueBase>>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Hero, IssueBase> keyValuePair = enumerator.Current;
						keyValuePair.Value.CompleteIssueWithStayAliveConditionsFailed();
					}
					return;
				}
			}
			this.DefaultCaravanItems.Add(DefaultItems.Grain);
			foreach (string objectName in new string[]
			{
				"cotton",
				"velvet",
				"oil",
				"linen",
				"date_fruit"
			})
			{
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(objectName);
				if (@object != null)
				{
					this.DefaultCaravanItems.Add(@object);
				}
			}
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000BCA34 File Offset: 0x000BAC34
		private void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeOnStart();
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000BCA3C File Offset: 0x000BAC3C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000BCA40 File Offset: 0x000BAC40
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsMerchant && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.IsTown && issueGiver.CurrentSettlement.Town.Security <= 50f && issueGiver.OwnedCaravans.Count < 2;
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000BCA94 File Offset: 0x000BAC94
		public void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), IssueBase.IssueFrequency.VeryCommon, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000BCAF8 File Offset: 0x000BACF8
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			return new EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue(issueOwner);
		}

		// Token: 0x04000D7C RID: 3452
		private const IssueBase.IssueFrequency EscortMerchantCaravanIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000D7D RID: 3453
		internal readonly List<ItemObject> DefaultCaravanItems = new List<ItemObject>();

		// Token: 0x02000615 RID: 1557
		public class EscortMerchantCaravanIssue : IssueBase
		{
			// Token: 0x06004A10 RID: 18960 RVA: 0x00155C2E File Offset: 0x00153E2E
			internal static void AutoGeneratedStaticCollectObjectsEscortMerchantCaravanIssue(object o, List<object> collectedObjects)
			{
				((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004A11 RID: 18961 RVA: 0x00155C3C File Offset: 0x00153E3C
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004A12 RID: 18962 RVA: 0x00155C45 File Offset: 0x00153E45
			internal static object AutoGeneratedGetMemberValue_companionRewardRandom(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)o)._companionRewardRandom;
			}

			// Token: 0x17000EBE RID: 3774
			// (get) Token: 0x06004A13 RID: 18963 RVA: 0x00155C57 File Offset: 0x00153E57
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17000EBF RID: 3775
			// (get) Token: 0x06004A14 RID: 18964 RVA: 0x00155C5B File Offset: 0x00153E5B
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 10 + MathF.Ceiling(16f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000EC0 RID: 3776
			// (get) Token: 0x06004A15 RID: 18965 RVA: 0x00155C71 File Offset: 0x00153E71
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 6 + MathF.Ceiling(10f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000EC1 RID: 3777
			// (get) Token: 0x06004A16 RID: 18966 RVA: 0x00155C86 File Offset: 0x00153E86
			protected int DailyQuestRewardGold
			{
				get
				{
					return 250 + MathF.Ceiling(1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000EC2 RID: 3778
			// (get) Token: 0x06004A17 RID: 18967 RVA: 0x00155C9F File Offset: 0x00153E9F
			protected override int RewardGold
			{
				get
				{
					return Math.Min(this.DailyQuestRewardGold * this._companionRewardRandom, 8000);
				}
			}

			// Token: 0x17000EC3 RID: 3779
			// (get) Token: 0x06004A18 RID: 18968 RVA: 0x00155CB8 File Offset: 0x00153EB8
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject result = new TextObject("{=CSqaF7tz}There's been a real surge of banditry around here recently. I don't know if it's because the lords are away fighting or something else, but it's a miracle if a traveler can make three leagues beyond the gates without being set upon by highwaymen.[if:convo_annoyed][ib:hip]", null);
					if (base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt || base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaSoftspoken)
					{
						result = new TextObject("{=xwc9mJdC}Things have gotten a lot worse recently with the brigands on the roads around town. My caravans get looted as soon as they're out of sight of the gates.[if:convo_stern][ib:hip]", null);
					}
					return result;
				}
			}

			// Token: 0x17000EC4 RID: 3780
			// (get) Token: 0x06004A19 RID: 18969 RVA: 0x00155D0C File Offset: 0x00153F0C
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=TGYJUUn0}Go on.", null);
				}
			}

			// Token: 0x17000EC5 RID: 3781
			// (get) Token: 0x06004A1A RID: 18970 RVA: 0x00155D19 File Offset: 0x00153F19
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=8ym6UvxE}I'm of a mind to send out a new caravan but I fear it will be plundered before it can turn a profit. So I am looking for some good fighters who can escort it until it finds its footing and visits a couple of settlements.", null);
				}
			}

			// Token: 0x17000EC6 RID: 3782
			// (get) Token: 0x06004A1B RID: 18971 RVA: 0x00155D28 File Offset: 0x00153F28
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=ytdZutjw}I will be willing to pay generously {BASE_REWARD}{GOLD_ICON} for each day the caravan is on the road. It will be more than I usually pay for caravan guards, but you look like the type who send a message to these brigands, that my caravans aren't to be messed with.[if:convo_undecided_closed]", null);
					if (base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt || base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaSoftspoken)
					{
						textObject = new TextObject("{=YbbfaHqd}I will be willing to pay generously {BASE_REWARD}{GOLD_ICON} for each day the caravan is on the road. It will be more than I usually pay for guards, but figure maybe you can scare these bandits off. I'm sick of choosing between sending my men to the their deaths or letting them go because I've lost my goods and can't pay their wages.[if:convo_undecided_closed]", null);
					}
					textObject.SetTextVariable("BASE_REWARD", this.DailyQuestRewardGold);
					return textObject;
				}
			}

			// Token: 0x17000EC7 RID: 3783
			// (get) Token: 0x06004A1C RID: 18972 RVA: 0x00155D8E File Offset: 0x00153F8E
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=a7fEPW5Y}Don't worry, I'll escort the caravan myself.", null);
				}
			}

			// Token: 0x17000EC8 RID: 3784
			// (get) Token: 0x06004A1D RID: 18973 RVA: 0x00155D9B File Offset: 0x00153F9B
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=N4p2GCsG}I'll assign one of my companions and {NEEDED_MEN_COUNT} of my men to protect your caravan for {RETURN_DAYS} days.", null);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17000EC9 RID: 3785
			// (get) Token: 0x06004A1E RID: 18974 RVA: 0x00155DCC File Offset: 0x00153FCC
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=hU5j7b3e}I am sure your men are as capable as you are and will look after my caravan. Thanks again for your help, my friend.[if:convo_focused_happy]", null);
				}
			}

			// Token: 0x17000ECA RID: 3786
			// (get) Token: 0x06004A1F RID: 18975 RVA: 0x00155DD9 File Offset: 0x00153FD9
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=iny76Ifh}Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}, I think they will be enough.", null);
				}
			}

			// Token: 0x17000ECB RID: 3787
			// (get) Token: 0x06004A20 RID: 18976 RVA: 0x00155DE6 File Offset: 0x00153FE6
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000ECC RID: 3788
			// (get) Token: 0x06004A21 RID: 18977 RVA: 0x00155DE9 File Offset: 0x00153FE9
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000ECD RID: 3789
			// (get) Token: 0x06004A22 RID: 18978 RVA: 0x00155DEC File Offset: 0x00153FEC
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6y59FBgL}{ISSUEGIVER.LINK}, a merchant from {SETTLEMENT}, has told you about {?ISSUEGIVER.GENDER}her{?}his{\\?} recent problems with bandits. {?ISSUEGIVER.GENDER}She{?}he{\\?} asked you to guard {?ISSUEGIVER.GENDER}her{?}his{\\?} caravan for a while and deal with any attackers. In return {?ISSUEGIVER.GENDER}she{?}he{\\?} offered you {GOLD}{GOLD_ICON} for each day your troops spend on escort duty.{newline}You agreed to lend {?ISSUEGIVER.GENDER}her{?}him{\\?} {NEEDED_MEN_COUNT} men. They should be enough to turn away most of the bandits. Your troops should return after {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUEGIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", this.AlternativeSolutionSentTroops.TotalManCount);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					textObject.SetTextVariable("GOLD", this.DailyQuestRewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x17000ECE RID: 3790
			// (get) Token: 0x06004A23 RID: 18979 RVA: 0x00155E86 File Offset: 0x00154086
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=VpLzd69e}Escort Merchant Caravan", null);
				}
			}

			// Token: 0x17000ECF RID: 3791
			// (get) Token: 0x06004A24 RID: 18980 RVA: 0x00155E93 File Offset: 0x00154093
			public override TextObject Description
			{
				get
				{
					return new TextObject("{=8RNueEmy}A merchant caravan needs an escort for protection against bandits and brigands.", null);
				}
			}

			// Token: 0x17000ED0 RID: 3792
			// (get) Token: 0x06004A25 RID: 18981 RVA: 0x00155EA0 File Offset: 0x001540A0
			public override TextObject IssueAlternativeSolutionFailLog
			{
				get
				{
					return new TextObject("{=KLauwaRJ}The caravan was destroyed despite your companion's efforts. Quest failed.", null);
				}
			}

			// Token: 0x17000ED1 RID: 3793
			// (get) Token: 0x06004A26 RID: 18982 RVA: 0x00155EB0 File Offset: 0x001540B0
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=3NX8H4TJ}Your companion has protected the caravan that belongs to {ISSUE_GIVER.LINK} from {SETTLEMENT} as promised. {?ISSUE_GIVER.GENDER}She{?}He{\\?} was happy with your work.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x06004A27 RID: 18983 RVA: 0x00155EF9 File Offset: 0x001540F9
			public EscortMerchantCaravanIssue(Hero issueOwner) : base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._companionRewardRandom = MBRandom.RandomInt(3, 10);
			}

			// Token: 0x06004A28 RID: 18984 RVA: 0x00155F1A File Offset: 0x0015411A
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return -0.4f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x06004A29 RID: 18985 RVA: 0x00155F3D File Offset: 0x0015413D
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Scouting) >= hero.GetSkillValue(DefaultSkills.Riding)) ? DefaultSkills.Scouting : DefaultSkills.Riding, 120);
			}

			// Token: 0x06004A2A RID: 18986 RVA: 0x00155F6A File Offset: 0x0015416A
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x06004A2B RID: 18987 RVA: 0x00155F82 File Offset: 0x00154182
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x17000ED2 RID: 3794
			// (get) Token: 0x06004A2C RID: 18988 RVA: 0x00155FA3 File Offset: 0x001541A3
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06004A2D RID: 18989 RVA: 0x00155FB8 File Offset: 0x001541B8
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06004A2E RID: 18990 RVA: 0x00155FC6 File Offset: 0x001541C6
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x06004A2F RID: 18991 RVA: 0x00155FCC File Offset: 0x001541CC
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill)
			{
				skill = null;
				relationHero = null;
				flags = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 20)
				{
					flags |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06004A30 RID: 18992 RVA: 0x00156039 File Offset: 0x00154239
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.OwnedCaravans.Count < 2 && base.IssueOwner.CurrentSettlement.Town.Security <= 80f;
			}

			// Token: 0x06004A31 RID: 18993 RVA: 0x0015606F File Offset: 0x0015426F
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06004A32 RID: 18994 RVA: 0x00156071 File Offset: 0x00154271
			protected override void HourlyTick()
			{
			}

			// Token: 0x06004A33 RID: 18995 RVA: 0x00156073 File Offset: 0x00154273
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), base.IssueDifficultyMultiplier, this.DailyQuestRewardGold);
			}

			// Token: 0x06004A34 RID: 18996 RVA: 0x00156098 File Offset: 0x00154298
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				base.IssueOwner.AddPower(-5f);
				this.RelationshipChangeWithIssueOwner = -5;
				TraitLevelingHelper.OnIssueFailed(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.IssueSettlement.Town.Prosperity -= 20f;
			}

			// Token: 0x06004A35 RID: 18997 RVA: 0x001560F9 File Offset: 0x001542F9
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				base.IssueOwner.AddPower(10f);
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueSettlement.Town.Prosperity += 10f;
			}

			// Token: 0x06004A36 RID: 18998 RVA: 0x0015612E File Offset: 0x0015432E
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001936 RID: 6454
			private const int MinimumRequiredMenCount = 20;

			// Token: 0x04001937 RID: 6455
			private const int AlternativeSolutionTroopTierRequirement = 2;

			// Token: 0x04001938 RID: 6456
			private const int NeededCompanionSkillAmount = 120;

			// Token: 0x04001939 RID: 6457
			private const int QuestTimeLimit = 30;

			// Token: 0x0400193A RID: 6458
			private const int IssueDuration = 30;

			// Token: 0x0400193B RID: 6459
			[SaveableField(10)]
			private int _companionRewardRandom;
		}

		// Token: 0x02000616 RID: 1558
		public class EscortMerchantCaravanIssueQuest : QuestBase
		{
			// Token: 0x06004A37 RID: 18999 RVA: 0x00156130 File Offset: 0x00154330
			internal static void AutoGeneratedStaticCollectObjectsEscortMerchantCaravanIssueQuest(object o, List<object> collectedObjects)
			{
				((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004A38 RID: 19000 RVA: 0x00156140 File Offset: 0x00154340
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._visitedSettlements);
				collectedObjects.Add(this._questCaravanMobileParty);
				collectedObjects.Add(this._questBanditMobileParty);
				collectedObjects.Add(this._otherBanditParty);
				collectedObjects.Add(this._playerStartsQuestLog);
			}

			// Token: 0x06004A39 RID: 19001 RVA: 0x00156190 File Offset: 0x00154390
			internal static object AutoGeneratedGetMemberValue_requiredSettlementNumber(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._requiredSettlementNumber;
			}

			// Token: 0x06004A3A RID: 19002 RVA: 0x001561A2 File Offset: 0x001543A2
			internal static object AutoGeneratedGetMemberValue_visitedSettlements(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._visitedSettlements;
			}

			// Token: 0x06004A3B RID: 19003 RVA: 0x001561AF File Offset: 0x001543AF
			internal static object AutoGeneratedGetMemberValue_questCaravanMobileParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questCaravanMobileParty;
			}

			// Token: 0x06004A3C RID: 19004 RVA: 0x001561BC File Offset: 0x001543BC
			internal static object AutoGeneratedGetMemberValue_questBanditMobileParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditMobileParty;
			}

			// Token: 0x06004A3D RID: 19005 RVA: 0x001561C9 File Offset: 0x001543C9
			internal static object AutoGeneratedGetMemberValue_difficultyMultiplier(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._difficultyMultiplier;
			}

			// Token: 0x06004A3E RID: 19006 RVA: 0x001561DB File Offset: 0x001543DB
			internal static object AutoGeneratedGetMemberValue_isPlayerNotifiedForDanger(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._isPlayerNotifiedForDanger;
			}

			// Token: 0x06004A3F RID: 19007 RVA: 0x001561ED File Offset: 0x001543ED
			internal static object AutoGeneratedGetMemberValue_otherBanditParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._otherBanditParty;
			}

			// Token: 0x06004A40 RID: 19008 RVA: 0x001561FA File Offset: 0x001543FA
			internal static object AutoGeneratedGetMemberValue_questBanditPartyFollowDuration(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditPartyFollowDuration;
			}

			// Token: 0x06004A41 RID: 19009 RVA: 0x0015620C File Offset: 0x0015440C
			internal static object AutoGeneratedGetMemberValue_otherBanditPartyFollowDuration(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._otherBanditPartyFollowDuration;
			}

			// Token: 0x06004A42 RID: 19010 RVA: 0x0015621E File Offset: 0x0015441E
			internal static object AutoGeneratedGetMemberValue_daysSpentForEscorting(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._daysSpentForEscorting;
			}

			// Token: 0x06004A43 RID: 19011 RVA: 0x00156230 File Offset: 0x00154430
			internal static object AutoGeneratedGetMemberValue_questBanditPartyAlreadyAttacked(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditPartyAlreadyAttacked;
			}

			// Token: 0x06004A44 RID: 19012 RVA: 0x00156242 File Offset: 0x00154442
			internal static object AutoGeneratedGetMemberValue_playerStartsQuestLog(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._playerStartsQuestLog;
			}

			// Token: 0x17000ED3 RID: 3795
			// (get) Token: 0x06004A45 RID: 19013 RVA: 0x0015624F File Offset: 0x0015444F
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=VpLzd69e}Escort Merchant Caravan", null);
				}
			}

			// Token: 0x17000ED4 RID: 3796
			// (get) Token: 0x06004A46 RID: 19014 RVA: 0x0015625C File Offset: 0x0015445C
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000ED5 RID: 3797
			// (get) Token: 0x06004A47 RID: 19015 RVA: 0x0015625F File Offset: 0x0015445F
			private int BanditPartyTroopCount
			{
				get
				{
					return (int)MathF.Min(40f, (float)(MobileParty.MainParty.MemberRoster.TotalHealthyCount + this._questCaravanMobileParty.MemberRoster.TotalHealthyCount) * 0.7f);
				}
			}

			// Token: 0x17000ED6 RID: 3798
			// (get) Token: 0x06004A48 RID: 19016 RVA: 0x00156293 File Offset: 0x00154493
			private int CaravanPartyTroopCount
			{
				get
				{
					return MBRandom.RandomInt(10, 14);
				}
			}

			// Token: 0x17000ED7 RID: 3799
			// (get) Token: 0x06004A49 RID: 19017 RVA: 0x0015629E File Offset: 0x0015449E
			private bool CaravanIsInsideSettlement
			{
				get
				{
					return this._questCaravanMobileParty.CurrentSettlement != null;
				}
			}

			// Token: 0x17000ED8 RID: 3800
			// (get) Token: 0x06004A4A RID: 19018 RVA: 0x001562AE File Offset: 0x001544AE
			private int TotalRewardGold
			{
				get
				{
					return MathF.Min(8000, this.RewardGold * this._daysSpentForEscorting);
				}
			}

			// Token: 0x17000ED9 RID: 3801
			// (get) Token: 0x06004A4B RID: 19019 RVA: 0x001562C7 File Offset: 0x001544C7
			private CustomPartyComponent CaravanCustomPartyComponent
			{
				get
				{
					if (this._customPartyComponent == null)
					{
						this._customPartyComponent = (this._questCaravanMobileParty.PartyComponent as CustomPartyComponent);
					}
					return this._customPartyComponent;
				}
			}

			// Token: 0x17000EDA RID: 3802
			// (get) Token: 0x06004A4C RID: 19020 RVA: 0x001562F0 File Offset: 0x001544F0
			private TextObject _playerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=YXbKXUDu}{ISSUE_GIVER.LINK}, a merchant from {SETTLEMENT}, has told you about {?ISSUE_GIVER.GENDER}her{?}his{\\?} recent problems with bandits. {?ISSUE_GIVER.GENDER}She{?}He{\\?} asked you to guard {?ISSUE_GIVER.GENDER}her{?}his{\\?} caravan for a while and deal with any attackers. In return {?ISSUE_GIVER.GENDER}she{?}he{\\?} offered you {GOLD}{GOLD_ICON} denars for each day you spend on escort duty.{newline}You have agreed to guard it yourself until it visits {NUMBER_OF_SETTLEMENTS} settlements.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
					textObject.SetTextVariable("NUMBER_OF_SETTLEMENTS", this._requiredSettlementNumber);
					textObject.SetTextVariable("GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x17000EDB RID: 3803
			// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0015636D File Offset: 0x0015456D
			private TextObject _caravanDestroyedQuestLogText
			{
				get
				{
					return new TextObject("{=zk9QyKIz}The caravan was destroyed. Quest failed.", null);
				}
			}

			// Token: 0x17000EDC RID: 3804
			// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0015637C File Offset: 0x0015457C
			private TextObject _caravanLostTheTrackLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=y62dyzH6}You have lost the track of caravan. Your agreement with {ISSUE_GIVER.LINK} is failed.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000EDD RID: 3805
			// (get) Token: 0x06004A4F RID: 19023 RVA: 0x001563B0 File Offset: 0x001545B0
			private TextObject _caravanDestroyedByBanditsLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=MhvyTcrH}The caravan is destroyed by some bandits. Your agreement with {ISSUE_GIVER.LINK} is failed.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000EDE RID: 3806
			// (get) Token: 0x06004A50 RID: 19024 RVA: 0x001563E2 File Offset: 0x001545E2
			private TextObject _caravanDestroyedByPlayerQuestLogText
			{
				get
				{
					return new TextObject("{=Rd3m5kyk}You have attacked the caravan.", null);
				}
			}

			// Token: 0x17000EDF RID: 3807
			// (get) Token: 0x06004A51 RID: 19025 RVA: 0x001563F0 File Offset: 0x001545F0
			private TextObject _successQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=dKEADOhG}You have protected the caravan belonging to {QUEST_GIVER.LINK} from {SETTLEMENT} as promised. {?QUEST_GIVER.GENDER}She{?}He{\\?} was happy with your work.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x17000EE0 RID: 3808
			// (get) Token: 0x06004A52 RID: 19026 RVA: 0x00156440 File Offset: 0x00154640
			private TextObject _cancelByWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=KhNkBd9O}Your clan is now at war with the {QUEST_GIVER.LINK}’s lord. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06004A53 RID: 19027 RVA: 0x00156474 File Offset: 0x00154674
			public EscortMerchantCaravanIssueQuest(string questId, Hero giverHero, CampaignTime duration, float difficultyMultiplier, int rewardGold) : base(questId, giverHero, duration, rewardGold)
			{
				this._difficultyMultiplier = difficultyMultiplier;
				this._requiredSettlementNumber = MathF.Round(2f + 4f * this._difficultyMultiplier);
				this._visitedSettlements = new List<Settlement>();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06004A54 RID: 19028 RVA: 0x001564D0 File Offset: 0x001546D0
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=TdwKwExD}Thank you. You can find the caravan just outside the settlement.[if:convo_grateful]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=vtZYmAaR}I feel good knowing that you're looking after my caravan. Safe journeys, my friend![if:convo_grateful]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanPartyDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanGreetingDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanTradeDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanLootDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanFarewellDialogFlow(), this);
			}

			// Token: 0x17000EE1 RID: 3809
			// (get) Token: 0x06004A55 RID: 19029 RVA: 0x001565D0 File Offset: 0x001547D0
			private TextObject _caravanNoTargetLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=1FOmvEdf}All profitable trade routes of the caravan are blocked by recent wars. {QUEST_GIVER.LINK} decided to recall the caravan until the situation gets better. {?QUEST_GIVER.GENDER}She{?}He{\\?} was happy with your service and sent you {REWARD}{GOLD_ICON} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.TotalRewardGold);
					return textObject;
				}
			}

			// Token: 0x06004A56 RID: 19030 RVA: 0x00156614 File Offset: 0x00154814
			private DialogFlow GetCaravanPartyDialogFlow()
			{
				TextObject textObject = new TextObject("{=ZAqEJI9T}About the task {QUEST_GIVER.LINK} gave me.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions().PlayerOption(textObject, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition)).NpcLine("{=heWYa9Oq}I feel safe knowing that you're looking after us. Please continue to follow us my friend!", null, null).Consequence(delegate
				{
					PlayerEncounter.LeaveEncounter = true;
				}).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x06004A57 RID: 19031 RVA: 0x001566A8 File Offset: 0x001548A8
			private bool caravan_talk_on_condition()
			{
				bool flag = this._questCaravanMobileParty.MemberRoster.Contains(CharacterObject.OneToOneConversationCharacter) && this._questCaravanMobileParty == MobileParty.ConversationParty && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCustomParty && !CharacterObject.OneToOneConversationCharacter.IsHero && MobileParty.ConversationParty.Party.Owner != Hero.MainHero;
				if (flag)
				{
					MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
					StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				}
				return flag;
			}

			// Token: 0x06004A58 RID: 19032 RVA: 0x00156778 File Offset: 0x00154978
			private DialogFlow GetCaravanFarewellDialogFlow()
			{
				TextObject text = new TextObject("{=1IJouNaM}Carry on, then. Farewell.", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions().PlayerOption(text, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition)).NpcLine("{=heWYa9Oq}I feel safe knowing that you're looking after us. Please continue to follow us my friend!", null, null).Consequence(delegate
				{
					PlayerEncounter.LeaveEncounter = true;
				}).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x06004A59 RID: 19033 RVA: 0x001567F4 File Offset: 0x001549F4
			private DialogFlow GetCaravanLootDialogFlow()
			{
				TextObject text = new TextObject("{=WOBy5UfY}Hand over your goods, or die!", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions().PlayerOption(text, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_loot_on_condition)).NpcLine("{=QNaKmkt9}We're paid to guard this caravan. If you want to rob it, it's going to be over our dead bodies![if:convo_angry][ib:aggressive]", null, null).BeginPlayerOptions().PlayerOption("{=EhxS7NQ4}So be it. Attack!", null).Consequence(new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence)).CloseDialog().PlayerOption("{=bfPsE9M1}You must have misunderstood me. Go in peace.", null).Consequence(new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence)).CloseDialog().EndPlayerOptions().EndPlayerOptions();
			}

			// Token: 0x06004A5A RID: 19034 RVA: 0x00156893 File Offset: 0x00154A93
			private void conversation_caravan_fight_on_consequence()
			{
				PlayerEncounter.Current.IsEnemy = true;
			}

			// Token: 0x06004A5B RID: 19035 RVA: 0x001568A0 File Offset: 0x00154AA0
			private void caravan_talk_leave_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
			}

			// Token: 0x06004A5C RID: 19036 RVA: 0x001568B0 File Offset: 0x00154AB0
			private DialogFlow GetCaravanTradeDialogFlow()
			{
				TextObject text = new TextObject("{=t0UGXPV4}I'm interested in trading. What kind of products do you have?", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions().PlayerOption(text, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_buy_products_on_condition)).NpcLine("{=tlLDHAIu}Very well. A pleasure doing business with you.[if:convo_relaxed_happy][ib:demure]", null, null).Condition(new ConversationSentence.OnConditionDelegate(this.conversation_caravan_player_trade_end_on_condition)).NpcLine("{=DQBaaC0e}Is there anything else?", null, null).GotoDialogState("escort_caravan_talk").EndPlayerOptions();
			}

			// Token: 0x06004A5D RID: 19037 RVA: 0x0015692C File Offset: 0x00154B2C
			private bool caravan_buy_products_on_condition()
			{
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty == this._questCaravanMobileParty && !MobileParty.ConversationParty.IsCaravan)
				{
					for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
					{
						if (MobileParty.ConversationParty.ItemRoster.GetElementNumber(i) > 0)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06004A5E RID: 19038 RVA: 0x00156989 File Offset: 0x00154B89
			private bool conversation_caravan_player_trade_end_on_condition()
			{
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty == this._questCaravanMobileParty && !MobileParty.ConversationParty.IsCaravan)
				{
					InventoryManager.OpenTradeWithCaravanOrAlleyParty(MobileParty.ConversationParty, InventoryManager.InventoryCategoryType.None);
				}
				return true;
			}

			// Token: 0x06004A5F RID: 19039 RVA: 0x001569B8 File Offset: 0x00154BB8
			private DialogFlow GetCaravanGreetingDialogFlow()
			{
				TextObject npcText = new TextObject("{=FpUybbSk}Greetings. This caravan is owned by {MERCHANT.LINK}. We trade under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(npcText, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition)).GotoDialogState("escort_caravan_talk");
			}

			// Token: 0x06004A60 RID: 19040 RVA: 0x00156A00 File Offset: 0x00154C00
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this.SpawnCaravan();
				this._playerStartsQuestLog = base.AddDiscreteLog(this._playerStartsQuestLogText, new TextObject("{=r2y3n7dR}Visited Settlements", null), this._visitedSettlements.Count, this._requiredSettlementNumber, null, false);
			}

			// Token: 0x06004A61 RID: 19041 RVA: 0x00156A40 File Offset: 0x00154C40
			private bool caravan_loot_on_condition()
			{
				bool flag = MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.MapFaction != Hero.MainHero.MapFaction && !MobileParty.ConversationParty.IsCaravan && MobileParty.ConversationParty == this._questCaravanMobileParty;
				if (flag)
				{
					MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
					StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				}
				return flag;
			}

			// Token: 0x06004A62 RID: 19042 RVA: 0x00156AF0 File Offset: 0x00154CF0
			private void SpawnCaravan()
			{
				ItemRoster itemRoster = new ItemRoster();
				foreach (ItemObject item in EscortMerchantCaravanIssueBehavior.Instance.DefaultCaravanItems)
				{
					itemRoster.AddToCounts(item, 7);
				}
				string partyMountStringId;
				string partyHarnessStringId;
				this.GetAdditionalVisualsForParty(base.QuestGiver.Culture, out partyMountStringId, out partyHarnessStringId);
				TextObject textObject = GameTexts.FindText("str_caravan_party_name", null);
				textObject.SetCharacterProperties("OWNER", base.QuestGiver.CharacterObject, false);
				this._questCaravanMobileParty = CustomPartyComponent.CreateQuestParty(base.QuestGiver.CurrentSettlement.GatePosition, 0f, base.QuestGiver.CurrentSettlement, textObject, base.QuestGiver.Clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), base.QuestGiver, partyMountStringId, partyHarnessStringId, 4f, false);
				this.InitializeCaravanOnCreation(this._questCaravanMobileParty, base.QuestGiver, base.QuestGiver.CurrentSettlement, itemRoster, this.CaravanPartyTroopCount, false);
				base.AddTrackedObject(this._questCaravanMobileParty);
				this._questCaravanMobileParty.SetPartyUsedByQuest(true);
				this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(true);
				this._questCaravanMobileParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				this._caravanWaitedInSettlementForHours = 4;
			}

			// Token: 0x06004A63 RID: 19043 RVA: 0x00156C40 File Offset: 0x00154E40
			private bool ProperSettlementCondition(Settlement settlement)
			{
				return settlement != Settlement.CurrentSettlement && settlement.IsTown && !settlement.IsUnderSiege && !this._visitedSettlements.Contains(settlement);
			}

			// Token: 0x06004A64 RID: 19044 RVA: 0x00156C6C File Offset: 0x00154E6C
			private void InitializeCaravanOnCreation(MobileParty mobileParty, Hero owner, Settlement settlement, ItemRoster caravanItems, int troopToBeGiven, bool isElite)
			{
				mobileParty.Aggressiveness = 0f;
				if (troopToBeGiven == 0)
				{
					float num;
					if (MBRandom.RandomFloat < 0.67f)
					{
						num = (1f - MBRandom.RandomFloat * MBRandom.RandomFloat) * 0.5f + 0.5f;
					}
					else
					{
						num = 1f;
					}
					int num2 = (int)((float)mobileParty.Party.PartySizeLimit * num);
					if (num2 >= 10)
					{
						num2--;
					}
					troopToBeGiven = num2;
				}
				PartyTemplateObject pt = isElite ? settlement.Culture.EliteCaravanPartyTemplate : settlement.Culture.CaravanPartyTemplate;
				mobileParty.InitializeMobilePartyAtPosition(pt, settlement.GatePosition, troopToBeGiven);
				CharacterObject character2 = CharacterObject.All.First((CharacterObject character) => character.Occupation == Occupation.CaravanGuard && character.IsInfantry && character.Level == 26 && character.Culture == mobileParty.Party.Owner.Culture);
				mobileParty.MemberRoster.AddToCounts(character2, 1, true, 0, 0, true, -1);
				mobileParty.Party.SetVisualAsDirty();
				mobileParty.InitializePartyTrade(10000 + ((owner.Clan == Clan.PlayerClan) ? 5000 : 0));
				if (caravanItems != null)
				{
					mobileParty.ItemRoster.Add(caravanItems);
					return;
				}
				float num3 = 10000f;
				ItemObject itemObject = null;
				foreach (ItemObject itemObject2 in Items.All)
				{
					if (itemObject2.ItemCategory == DefaultItemCategories.PackAnimal && !itemObject2.NotMerchandise && (float)itemObject2.Value < num3)
					{
						itemObject = itemObject2;
						num3 = (float)itemObject2.Value;
					}
				}
				if (itemObject != null)
				{
					mobileParty.ItemRoster.Add(new ItemRosterElement(itemObject, (int)((float)mobileParty.MemberRoster.TotalManCount * 0.5f), null));
				}
			}

			// Token: 0x06004A65 RID: 19045 RVA: 0x00156E54 File Offset: 0x00155054
			private void GetAdditionalVisualsForParty(CultureObject culture, out string mountStringId, out string harnessStringId)
			{
				if (culture.StringId == "aserai" || culture.StringId == "khuzait")
				{
					mountStringId = "camel";
					harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "camel_saddle_a" : "camel_saddle_b");
					return;
				}
				mountStringId = "mule";
				harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "mule_load_a" : ((MBRandom.RandomFloat > 0.5f) ? "mule_load_b" : "mule_load_c"));
			}

			// Token: 0x06004A66 RID: 19046 RVA: 0x00156EDC File Offset: 0x001550DC
			protected override void RegisterEvents()
			{
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyHourlyTick));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x06004A67 RID: 19047 RVA: 0x00156FA1 File Offset: 0x001551A1
			private void OnPartyHourlyTick(MobileParty mobileParty)
			{
				this.CheckPartyAndMakeItAttackTheCaravan(mobileParty);
				this.CheckEncounterForBanditParty(this._questBanditMobileParty);
				this.CheckEncounterForBanditParty(this._otherBanditParty);
				this.CheckOtherBanditPartyDistance();
			}

			// Token: 0x06004A68 RID: 19048 RVA: 0x00156FC8 File Offset: 0x001551C8
			private void CheckOtherBanditPartyDistance()
			{
				if (base.IsOngoing)
				{
					if (this._otherBanditParty != null && this._otherBanditParty.IsActive && this._otherBanditParty.TargetParty == this._questCaravanMobileParty && this._otherBanditPartyFollowDuration < 0)
					{
						if (base.IsTracked(this._otherBanditParty))
						{
							base.RemoveTrackedObject(this._otherBanditParty);
						}
						this._otherBanditParty.Ai.SetMoveModeHold();
						this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
						this._otherBanditParty = null;
					}
					if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive && this._questBanditMobileParty.MapEvent == null && this._questBanditMobileParty.TargetParty == this._questCaravanMobileParty && this._questBanditPartyFollowDuration < 0 && !this._questBanditMobileParty.IsVisible)
					{
						if (base.IsTracked(this._questBanditMobileParty))
						{
							base.RemoveTrackedObject(this._questBanditMobileParty);
						}
						this._questBanditMobileParty.Ai.SetMoveModeHold();
						this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					}
				}
			}

			// Token: 0x06004A69 RID: 19049 RVA: 0x001570DC File Offset: 0x001552DC
			private void CheckEncounterForBanditParty(MobileParty mobileParty)
			{
				if (base.IsOngoing && mobileParty != null && mobileParty.IsActive && mobileParty.MapEvent == null && this._questCaravanMobileParty.IsActive && this._questCaravanMobileParty.MapEvent == null && this._questCaravanMobileParty.CurrentSettlement == null && mobileParty.Position2D.DistanceSquared(this._questCaravanMobileParty.Position2D) <= 1f)
				{
					EncounterManager.StartPartyEncounter(mobileParty.Party, this._questCaravanMobileParty.Party);
					MBInformationManager.AddQuickInformation(new TextObject("{=o8uAzFaJ}The caravan you are protecting is ambushed by raiders!", null), 0, null, "");
					this._questCaravanMobileParty.MapEvent.IsInvulnerable = true;
				}
			}

			// Token: 0x06004A6A RID: 19050 RVA: 0x00157198 File Offset: 0x00155398
			private void CheckPartyAndMakeItAttackTheCaravan(MobileParty mobileParty)
			{
				if (this._otherBanditParty == null && mobileParty.MapEvent == null && mobileParty.IsBandit && !mobileParty.IsCurrentlyUsedByAQuest && mobileParty != this._questBanditMobileParty && mobileParty.Party.NumberOfHealthyMembers > this._questCaravanMobileParty.Party.NumberOfHealthyMembers && (mobileParty.Speed > this._questCaravanMobileParty.Speed || mobileParty.Position2D.DistanceSquared(this._questCaravanMobileParty.Position2D) < 9f))
				{
					Settlement toSettlement = this._visitedSettlements.LastOrDefault<Settlement>() ?? this._questCaravanMobileParty.HomeSettlement;
					Settlement targetSettlement = this._questCaravanMobileParty.TargetSettlement;
					if (targetSettlement == null)
					{
						this.TryToFindAndSetTargetToNextSettlement();
						return;
					}
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, targetSettlement);
					float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, toSettlement);
					float num = mobileParty.Position2D.DistanceSquared(this._questCaravanMobileParty.Position2D);
					if (distance > 5f && distance2 > 5f && num < 64f)
					{
						SetPartyAiAction.GetActionForEngagingParty(mobileParty, this._questCaravanMobileParty);
						mobileParty.Ai.SetDoNotMakeNewDecisions(true);
						if (!base.IsTracked(mobileParty))
						{
							base.AddTrackedObject(mobileParty);
						}
						float num2 = mobileParty.Speed + this._questCaravanMobileParty.Speed;
						this._otherBanditPartyFollowDuration = (int)(num / num2) + 5;
						this._otherBanditParty = mobileParty;
					}
				}
			}

			// Token: 0x06004A6B RID: 19051 RVA: 0x00157320 File Offset: 0x00155520
			private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party == this._questCaravanMobileParty && settlement != this._questCaravanMobileParty.HomeSettlement && settlement.GatePosition.NearlyEquals(MobileParty.MainParty.Position2D, MobileParty.MainParty.SeeingRange + 2f) && settlement == this._questCaravanMobileParty.TargetSettlement)
				{
					this._visitedSettlements.Add(settlement);
					base.UpdateQuestTaskStage(this._playerStartsQuestLog, this._visitedSettlements.Count);
					TextObject textObject = new TextObject("{=0wj3HIbh}Caravan entered {SETTLEMENT_LINK}.", null);
					textObject.SetTextVariable("SETTLEMENT_LINK", settlement.EncyclopediaLinkWithName);
					base.AddLog(textObject, true);
					if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive)
					{
						if (base.IsTracked(this._questBanditMobileParty))
						{
							base.RemoveTrackedObject(this._questBanditMobileParty);
						}
						this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
						this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
						if (this._questBanditMobileParty.MapEvent == null)
						{
							SetPartyAiAction.GetActionForPatrollingAroundSettlement(this._questBanditMobileParty, settlement);
						}
					}
					if (this._otherBanditParty != null)
					{
						if (base.IsTracked(this._otherBanditParty))
						{
							base.RemoveTrackedObject(this._otherBanditParty);
						}
						this._otherBanditParty.Ai.SetMoveModeHold();
						this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
						this._otherBanditParty = null;
					}
					if (this._visitedSettlements.Count == this._requiredSettlementNumber)
					{
						this.SuccessConsequences(false);
					}
				}
			}

			// Token: 0x06004A6C RID: 19052 RVA: 0x0015749F File Offset: 0x0015569F
			private void OnDailyTick()
			{
				this._daysSpentForEscorting++;
			}

			// Token: 0x06004A6D RID: 19053 RVA: 0x001574AF File Offset: 0x001556AF
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				this.CheckWarDeclaration();
			}

			// Token: 0x06004A6E RID: 19054 RVA: 0x001574B7 File Offset: 0x001556B7
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				this.CheckWarDeclaration();
			}

			// Token: 0x06004A6F RID: 19055 RVA: 0x001574BF File Offset: 0x001556BF
			private void CheckWarDeclaration()
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this._cancelByWarQuestLogText);
				}
			}

			// Token: 0x06004A70 RID: 19056 RVA: 0x001574F0 File Offset: 0x001556F0
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				if (detail == DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility && (faction1 == this._questCaravanMobileParty.MapFaction || faction2 == this._questCaravanMobileParty.MapFaction) && PlayerEncounter.Battle != null && this._questCaravanMobileParty.MapEvent == PlayerEncounter.Battle)
				{
					this.FailByPlayerHostileConsequences();
				}
				else
				{
					this.CheckWarDeclaration();
				}
				if (this._questCaravanMobileParty != null && (this._questCaravanMobileParty.TargetSettlement == null || this._questCaravanMobileParty.TargetSettlement.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction)) && base.IsOngoing)
				{
					this.TryToFindAndSetTargetToNextSettlement();
				}
			}

			// Token: 0x06004A71 RID: 19057 RVA: 0x0015758C File Offset: 0x0015578C
			protected override void HourlyTick()
			{
				if (base.IsOngoing && this._questCaravanMobileParty.TargetSettlement == null)
				{
					this.TryToFindAndSetTargetToNextSettlement();
				}
				if (base.IsOngoing)
				{
					if (this.CaravanIsInsideSettlement)
					{
						this.SimulateSettlementWaitForCaravan();
					}
					else if (this._questCaravanMobileParty.MapEvent == null)
					{
						this.AdjustCaravansSpeed();
					}
					this.NotifyPlayerOrCancelTheQuestIfCaravanIsFar();
					if (base.IsOngoing)
					{
						this.ThinkAboutSpawningBanditParty();
						this.CheckCaravanMapEvent();
						this._otherBanditPartyFollowDuration--;
						this._questBanditPartyFollowDuration--;
					}
				}
			}

			// Token: 0x06004A72 RID: 19058 RVA: 0x00157618 File Offset: 0x00155818
			private void CheckCaravanMapEvent()
			{
				if (this._questCaravanMobileParty.MapEvent != null && this._questCaravanMobileParty.MapEvent.IsInvulnerable && this._questCaravanMobileParty.MapEvent.BattleStartTime.ElapsedHoursUntilNow > 3f)
				{
					this._questCaravanMobileParty.MapEvent.IsInvulnerable = false;
				}
			}

			// Token: 0x06004A73 RID: 19059 RVA: 0x00157674 File Offset: 0x00155874
			private void AdjustCaravansSpeed()
			{
				float speed = MobileParty.MainParty.Speed;
				float speed2 = this._questCaravanMobileParty.Speed;
				while (speed < speed2 || speed - speed2 > 1f)
				{
					if (speed2 >= speed)
					{
						this.CaravanCustomPartyComponent.SetBaseSpeed(this.CaravanCustomPartyComponent.BaseSpeed - 0.05f);
					}
					else if (speed - speed2 > 1f)
					{
						this.CaravanCustomPartyComponent.SetBaseSpeed(this.CaravanCustomPartyComponent.BaseSpeed + 0.05f);
					}
					speed = MobileParty.MainParty.Speed;
					speed2 = this._questCaravanMobileParty.Speed;
				}
			}

			// Token: 0x06004A74 RID: 19060 RVA: 0x00157708 File Offset: 0x00155908
			private void ThinkAboutSpawningBanditParty()
			{
				if (!this._questBanditPartyAlreadyAttacked && this._questBanditMobileParty == null)
				{
					Settlement targetSettlement = this._questCaravanMobileParty.TargetSettlement;
					if (targetSettlement != null)
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, targetSettlement);
						if (distance > 10f && distance < 80f)
						{
							this.ActivateBanditParty();
							float num = this._questBanditMobileParty.Speed + this._questCaravanMobileParty.Speed;
							this._questBanditPartyFollowDuration = (int)(80f / num) + 5;
							this._questBanditPartyAlreadyAttacked = true;
						}
					}
				}
			}

			// Token: 0x06004A75 RID: 19061 RVA: 0x00157795 File Offset: 0x00155995
			private void SimulateSettlementWaitForCaravan()
			{
				this._caravanWaitedInSettlementForHours++;
				if (this._caravanWaitedInSettlementForHours >= 5)
				{
					LeaveSettlementAction.ApplyForParty(this._questCaravanMobileParty);
					this._caravanWaitedInSettlementForHours = 0;
				}
			}

			// Token: 0x06004A76 RID: 19062 RVA: 0x001577C0 File Offset: 0x001559C0
			private void NotifyPlayerOrCancelTheQuestIfCaravanIsFar()
			{
				if (this._questCaravanMobileParty.IsActive && !this._questCaravanMobileParty.IsVisible)
				{
					float num = this._questCaravanMobileParty.Position2D.Distance(MobileParty.MainParty.Position2D);
					if (!this._isPlayerNotifiedForDanger && num >= MobileParty.MainParty.SeeingRange + 3f)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=2y9DhzCR}You are about to lose sight of the caravan. Find the caravan before they are in danger!", null), 0, null, "");
						this._isPlayerNotifiedForDanger = true;
						return;
					}
					if (num >= MobileParty.MainParty.SeeingRange + 20f)
					{
						base.AddLog(this._caravanLostTheTrackLogText, false);
						this.FailConsequences(false);
					}
				}
			}

			// Token: 0x06004A77 RID: 19063 RVA: 0x00157870 File Offset: 0x00155A70
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party == this._questCaravanMobileParty)
				{
					this.AdjustCaravansSpeed();
					if (party.TargetSettlement == null || party.TargetSettlement == settlement)
					{
						this.TryToFindAndSetTargetToNextSettlement();
					}
					this._caravanWaitedInSettlementForHours = 0;
					this._questBanditPartyAlreadyAttacked = false;
					this._questCaravanMobileParty.Party.SetAsCameraFollowParty();
					if (base.IsTracked(settlement))
					{
						base.RemoveTrackedObject(settlement);
					}
				}
			}

			// Token: 0x06004A78 RID: 19064 RVA: 0x001578D4 File Offset: 0x00155AD4
			private void TryToFindAndSetTargetToNextSettlement()
			{
				int num = 0;
				int num2 = -1;
				do
				{
					num2 = SettlementHelper.FindNextSettlementAroundMapPoint(this._questCaravanMobileParty, 150f, num2);
					if (num2 >= 0)
					{
						Settlement settlement = Settlement.All[num2];
						if (this.ProperSettlementCondition(settlement) && settlement != this._questCaravanMobileParty.HomeSettlement && (this._visitedSettlements.Count == 0 || settlement != this._visitedSettlements[this._visitedSettlements.Count - 1]) && !settlement.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction))
						{
							num++;
						}
					}
				}
				while (num2 >= 0);
				if (num > 0)
				{
					int num3 = MBRandom.RandomInt(num);
					num2 = -1;
					Settlement settlement2;
					for (;;)
					{
						num2 = SettlementHelper.FindNextSettlementAroundMapPoint(this._questCaravanMobileParty, 150f, num2);
						if (num2 >= 0)
						{
							settlement2 = Settlement.All[num2];
							if (this.ProperSettlementCondition(settlement2) && settlement2 != this._questCaravanMobileParty.HomeSettlement && (this._visitedSettlements.Count == 0 || settlement2 != this._visitedSettlements[this._visitedSettlements.Count - 1]) && !settlement2.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction))
							{
								num3--;
								if (num3 < 0)
								{
									break;
								}
							}
						}
						if (num2 < 0)
						{
							return;
						}
					}
					Settlement settlement3 = settlement2;
					SetPartyAiAction.GetActionForVisitingSettlement(this._questCaravanMobileParty, settlement3);
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(true);
					TextObject textObject = new TextObject("{=OjI8uGFa}We are traveling to {SETTLEMENT_NAME}.", null);
					textObject.SetTextVariable("SETTLEMENT_NAME", settlement3.Name);
					MBInformationManager.AddQuickInformation(textObject, 100, PartyBaseHelper.GetVisualPartyLeader(this._questCaravanMobileParty.Party), "");
					TextObject textObject2 = new TextObject("{=QDpfYm4c}The caravan is moving to {SETTLEMENT_NAME}.", null);
					textObject2.SetTextVariable("SETTLEMENT_NAME", settlement3.EncyclopediaLinkWithName);
					base.AddLog(textObject2, true);
					if (!base.IsTracked(settlement3))
					{
						base.AddTrackedObject(settlement3);
					}
					if (this._questBanditMobileParty != null && (this._questBanditMobileParty.Speed < this._questCaravanMobileParty.Speed || Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, this._questBanditMobileParty) > 10f))
					{
						this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
						this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
						if (base.IsTracked(this._questBanditMobileParty))
						{
							base.RemoveTrackedObject(this._questBanditMobileParty);
						}
						this._questBanditMobileParty = null;
						return;
					}
					return;
				}
				this.CaravanNoTargetQuestSuccess();
			}

			// Token: 0x06004A79 RID: 19065 RVA: 0x00157B45 File Offset: 0x00155D45
			private void CaravanNoTargetQuestSuccess()
			{
				this.SuccessConsequences(true);
			}

			// Token: 0x06004A7A RID: 19066 RVA: 0x00157B50 File Offset: 0x00155D50
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (this._questCaravanMobileParty != null && mapEvent.InvolvedParties.Contains(this._questCaravanMobileParty.Party))
				{
					if (mapEvent.HasWinner)
					{
						bool flag = this._questCaravanMobileParty.MapEventSide == MobileParty.MainParty.MapEventSide && mapEvent.IsPlayerMapEvent;
						bool flag2 = mapEvent.Winner == this._questCaravanMobileParty.MapEventSide;
						bool flag3 = mapEvent.InvolvedParties.Contains(PartyBase.MainParty);
						if (!flag2)
						{
							if (!flag3)
							{
								base.AddLog(this._caravanDestroyedByBanditsLogText, false);
								this.FailConsequences(true);
								return;
							}
							if (flag)
							{
								base.AddLog(this._caravanDestroyedQuestLogText, false);
								this.FailConsequences(true);
								return;
							}
							this.FailByPlayerHostileConsequences();
							return;
						}
						else
						{
							if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive && mapEvent.InvolvedParties.Contains(this._questBanditMobileParty.Party))
							{
								DestroyPartyAction.Apply(MobileParty.MainParty.Party, this._questBanditMobileParty);
							}
							if (this._otherBanditParty != null && this._otherBanditParty.IsActive && mapEvent.InvolvedParties.Contains(this._otherBanditParty.Party))
							{
								DestroyPartyAction.Apply(MobileParty.MainParty.Party, this._otherBanditParty);
							}
							if (this._questCaravanMobileParty.MemberRoster.TotalManCount <= 0)
							{
								this.FailConsequences(true);
							}
							if (this._questCaravanMobileParty.Speed < 2f)
							{
								this._questCaravanMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse"), 5);
								return;
							}
						}
					}
					else if (this._questCaravanMobileParty.MemberRoster.TotalManCount <= 0)
					{
						this.FailConsequences(true);
					}
				}
			}

			// Token: 0x06004A7B RID: 19067 RVA: 0x00157CFC File Offset: 0x00155EFC
			private void SuccessConsequences(bool isNoTargetLeftSuccess)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.TotalRewardGold, false);
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 5;
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 10f;
				if (isNoTargetLeftSuccess)
				{
					base.AddLog(this._caravanNoTargetLogText, false);
				}
				else
				{
					base.AddLog(this._successQuestLogText, true);
				}
				MobileParty questBanditMobileParty = this._questBanditMobileParty;
				if (questBanditMobileParty != null)
				{
					questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
				}
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06004A7C RID: 19068 RVA: 0x00157DB4 File Offset: 0x00155FB4
			private void FailConsequences(bool banditsWon = false)
			{
				base.QuestGiver.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 10f;
				if (this._questBanditMobileParty != null)
				{
					this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
					if (base.IsTracked(this._questBanditMobileParty))
					{
						base.RemoveTrackedObject(this._questBanditMobileParty);
					}
				}
				if (this._questCaravanMobileParty != null)
				{
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questCaravanMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
				if (this._questBanditMobileParty != null && !banditsWon)
				{
					if (base.IsTracked(this._questBanditMobileParty))
					{
						base.RemoveTrackedObject(this._questBanditMobileParty);
					}
					this._questBanditMobileParty.SetPartyUsedByQuest(false);
					this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
					if (this._questBanditMobileParty.IsActive && this._questBanditMobileParty.IsVisible)
					{
						DestroyPartyAction.Apply(null, this._questBanditMobileParty);
					}
				}
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004A7D RID: 19069 RVA: 0x00157EF4 File Offset: 0x001560F4
			private void FailByPlayerHostileConsequences()
			{
				base.QuestGiver.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = -10;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -80)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 20f;
				base.AddLog(this._caravanDestroyedByPlayerQuestLogText, true);
				MobileParty questBanditMobileParty = this._questBanditMobileParty;
				if (questBanditMobileParty != null)
				{
					questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
				}
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004A7E RID: 19070 RVA: 0x00157F86 File Offset: 0x00156186
			protected override void InitializeQuestOnGameLoad()
			{
				MobileParty questCaravanMobileParty = this._questCaravanMobileParty;
				if (questCaravanMobileParty != null && questCaravanMobileParty.IsCaravan)
				{
					base.CompleteQuestWithCancel(null);
				}
				this.SetDialogs();
			}

			// Token: 0x06004A7F RID: 19071 RVA: 0x00157FAC File Offset: 0x001561AC
			private void ActivateBanditParty()
			{
				Settlement closestHideout = SettlementHelper.FindNearestHideout((Settlement x) => x.IsActive, null);
				Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);
				this._questBanditMobileParty = BanditPartyComponent.CreateBanditParty("escort_caravan_quest_" + base.StringId, clan, closestHideout.Hideout, false);
				PartyTemplateObject partyTemplateObject = Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("kingdom_hero_party_caravan_ambushers") ?? clan.DefaultPartyTemplate;
				this._questBanditMobileParty.InitializeMobilePartyAroundPosition(partyTemplateObject, this._questCaravanMobileParty.TargetSettlement.GatePosition, 1f, 0.5f, -1);
				this._questBanditMobileParty.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
				Campaign.Current.MobilePartyLocator.UpdateLocator(this._questBanditMobileParty);
				this._questBanditMobileParty.ActualClan = clan;
				this._questBanditMobileParty.MemberRoster.Clear();
				for (int i = 0; i < this.BanditPartyTroopCount; i++)
				{
					List<ValueTuple<PartyTemplateStack, float>> list = new List<ValueTuple<PartyTemplateStack, float>>();
					foreach (PartyTemplateStack partyTemplateStack in partyTemplateObject.Stacks)
					{
						list.Add(new ValueTuple<PartyTemplateStack, float>(partyTemplateStack, (float)(64 - partyTemplateStack.Character.Level)));
					}
					PartyTemplateStack partyTemplateStack2 = MBRandom.ChooseWeighted<PartyTemplateStack>(list);
					this._questBanditMobileParty.MemberRoster.AddToCounts(partyTemplateStack2.Character, 1, false, 0, 0, true, -1);
				}
				this._questBanditMobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, this.BanditPartyTroopCount);
				this._questBanditMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse"), this.BanditPartyTroopCount);
				this._questBanditMobileParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				SetPartyAiAction.GetActionForEngagingParty(this._questBanditMobileParty, this._questCaravanMobileParty);
				this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(true);
				base.AddTrackedObject(this._questBanditMobileParty);
			}

			// Token: 0x06004A80 RID: 19072 RVA: 0x001581DC File Offset: 0x001563DC
			protected override void OnFinalize()
			{
				if (this._questCaravanMobileParty != null && this._questCaravanMobileParty.IsActive && this._questCaravanMobileParty.IsCustomParty)
				{
					this._questCaravanMobileParty.PartyComponent = new CaravanPartyComponent(base.QuestGiver.CurrentSettlement, base.QuestGiver, null);
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questCaravanMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
				if (this._questCaravanMobileParty != null)
				{
					base.RemoveTrackedObject(this._questCaravanMobileParty);
				}
				if (this._otherBanditParty != null && this._otherBanditParty.IsActive)
				{
					this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
					this._otherBanditParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
			}

			// Token: 0x06004A81 RID: 19073 RVA: 0x00158298 File Offset: 0x00156498
			protected override void OnTimedOut()
			{
				base.QuestGiver.AddPower(-5f);
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 20f;
				base.AddLog(new TextObject("{=pUrSIed8}You have failed to escort the caravan to its destination.", null), false);
			}

			// Token: 0x0400193C RID: 6460
			private const int BanditPartyAttackRadiusMin = 8;

			// Token: 0x0400193D RID: 6461
			private const int BattleFakeSimulationDuration = 3;

			// Token: 0x0400193E RID: 6462
			private const int QuestBanditPartySpawnDistance = 80;

			// Token: 0x0400193F RID: 6463
			private const string CustomPartyComponentTalkId = "escort_caravan_talk";

			// Token: 0x04001940 RID: 6464
			[SaveableField(2)]
			private readonly int _requiredSettlementNumber;

			// Token: 0x04001941 RID: 6465
			[SaveableField(3)]
			private List<Settlement> _visitedSettlements;

			// Token: 0x04001942 RID: 6466
			[SaveableField(4)]
			private MobileParty _questCaravanMobileParty;

			// Token: 0x04001943 RID: 6467
			[SaveableField(5)]
			private MobileParty _questBanditMobileParty;

			// Token: 0x04001944 RID: 6468
			[SaveableField(7)]
			private readonly float _difficultyMultiplier;

			// Token: 0x04001945 RID: 6469
			[SaveableField(12)]
			private bool _isPlayerNotifiedForDanger;

			// Token: 0x04001946 RID: 6470
			[SaveableField(26)]
			private MobileParty _otherBanditParty;

			// Token: 0x04001947 RID: 6471
			[SaveableField(30)]
			private int _questBanditPartyFollowDuration;

			// Token: 0x04001948 RID: 6472
			[SaveableField(31)]
			private int _otherBanditPartyFollowDuration;

			// Token: 0x04001949 RID: 6473
			[SaveableField(11)]
			private int _daysSpentForEscorting = 1;

			// Token: 0x0400194A RID: 6474
			private int _caravanWaitedInSettlementForHours;

			// Token: 0x0400194B RID: 6475
			[SaveableField(23)]
			private bool _questBanditPartyAlreadyAttacked;

			// Token: 0x0400194C RID: 6476
			private CustomPartyComponent _customPartyComponent;

			// Token: 0x0400194D RID: 6477
			[SaveableField(1)]
			private JournalLog _playerStartsQuestLog;
		}

		// Token: 0x02000617 RID: 1559
		public class EscortMerchantCaravanIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06004A84 RID: 19076 RVA: 0x0015832F File Offset: 0x0015652F
			public EscortMerchantCaravanIssueTypeDefiner() : base(450000)
			{
			}

			// Token: 0x06004A85 RID: 19077 RVA: 0x0015833C File Offset: 0x0015653C
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), 1, null);
				base.AddClassDefinition(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest), 2, null);
			}
		}
	}
}
