using System;

namespace TaleWorlds.CampaignSystem.Issues.IssueQuestTasks
{
	// Token: 0x02000327 RID: 807
	public class TalkToNpcQuestTask : QuestTaskBase
	{
		// Token: 0x06002E39 RID: 11833 RVA: 0x000C195B File Offset: 0x000BFB5B
		public TalkToNpcQuestTask(Hero hero, Action onSucceededAction, DialogFlow dialogFlow = null) : base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = hero.CharacterObject;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000C1973 File Offset: 0x000BFB73
		public TalkToNpcQuestTask(CharacterObject character, Action onSucceededAction, DialogFlow dialogFlow = null) : base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = character;
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000C1986 File Offset: 0x000BFB86
		public bool IsTaskCharacter()
		{
			return this._character == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000C1995 File Offset: 0x000BFB95
		protected override void OnFinished()
		{
			this._character = null;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000C199E File Offset: 0x000BFB9E
		public override void SetReferences()
		{
		}

		// Token: 0x04000DD7 RID: 3543
		private CharacterObject _character;
	}
}
