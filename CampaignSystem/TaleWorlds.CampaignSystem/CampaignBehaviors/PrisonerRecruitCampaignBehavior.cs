using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003CA RID: 970
	public class PrisonerRecruitCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x0011B964 File Offset: 0x00119B64
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0011B97D File Offset: 0x00119B7D
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<CharacterObject, float>>("PrisonerTalkRecords", ref this.PrisonerTalkRecords);
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x0011B991 File Offset: 0x00119B91
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x0011B99C File Offset: 0x00119B9C
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("conversation_prisoner_chat_start", "start", "prisoner_recruit_start_player", "{=k7ebznzr}Yes?", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_chat_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "prisoner_recruit_start_response", "{=ksZXyDJG}Don't do anything stupid, like trying to run away. I will be watching you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("conversation_prisoner_chat_response", "prisoner_recruit_start_response", "close_window", "{=Oe1bTJp6}No, I swear I won't.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("conversation_prisoner_recruit_start_1", "start", "prisoner_recruit_start", "{=!}I'm going to take a chance on you, to give you a chance to walk free, if you like.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("conversation_prisoner_recruit_start", "prisoner_recruit_start", "prisoner_recruit", "{=!}Are you willing to join us? To fight alongside us?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_1", "prisoner_recruit", "close_window", "{=!}Aye. I would do that.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_2", "prisoner_recruit", "close_window", "{=!}No. I'm no traitor.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_no_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_3", "prisoner_recruit", "close_window", "{=!}You heard me the first time. You know where to stick your offer.", null, null, 100, null);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0011BAD8 File Offset: 0x00119CD8
		private bool conversation_prisoner_chat_start_on_condition()
		{
			bool flag = (CharacterObject.OneToOneConversationCharacter.IsHero && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsActive)) || (CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsMainParty);
			return MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter) && flag;
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x0011BB6C File Offset: 0x00119D6C
		private bool conversation_prisoner_recruit_start_on_condition()
		{
			bool flag = !CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsMainParty;
			bool flag2 = MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter);
			if (flag2 && !this.PrisonerTalkRecords.ContainsKey(CharacterObject.OneToOneConversationCharacter))
			{
				this.PrisonerTalkRecords.Add(CharacterObject.OneToOneConversationCharacter, -1f);
			}
			return flag2 && flag;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0011BC08 File Offset: 0x00119E08
		public bool conversation_prisoner_recruit_on_condition()
		{
			bool flag = false;
			float num;
			if (this.PrisonerTalkRecords.TryGetValue(CharacterObject.OneToOneConversationCharacter, out num) && (num < 0f || Campaign.CurrentTime - num >= 5f))
			{
				flag = (MBRandom.RandomInt(MBMath.ClampInt(150 - CharacterObject.PlayerCharacter.GetSkillValue(DefaultSkills.Steward), 1, 150)) < 30);
				if (flag)
				{
					this.PrisonerTalkRecords.Remove(CharacterObject.OneToOneConversationCharacter);
					int num2 = MobileParty.MainParty.PrisonRoster.FindIndexOfTroop(CharacterObject.OneToOneConversationCharacter);
					if (num2 != -1)
					{
						TroopRosterElement elementCopyAtIndex = MobileParty.MainParty.PrisonRoster.GetElementCopyAtIndex(num2);
						MobileParty.MainParty.PrisonRoster.AddToCounts(elementCopyAtIndex.Character, -elementCopyAtIndex.Number, false, 0, 0, true, -1);
						MobileParty.MainParty.MemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, 0, 0, true, -1);
					}
				}
			}
			return flag;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x0011BCF8 File Offset: 0x00119EF8
		public bool conversation_prisoner_recruit_no_on_condition()
		{
			bool result = false;
			float num;
			if (this.PrisonerTalkRecords.TryGetValue(CharacterObject.OneToOneConversationCharacter, out num) && num < 0f)
			{
				this.PrisonerTalkRecords[CharacterObject.OneToOneConversationCharacter] = Campaign.CurrentTime;
				result = true;
			}
			return result;
		}

		// Token: 0x040011D6 RID: 4566
		public Dictionary<CharacterObject, float> PrisonerTalkRecords = new Dictionary<CharacterObject, float>();
	}
}
