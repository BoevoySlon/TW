using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000210 RID: 528
	public class NoConflictTag : ConversationTag
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00088C28 File Offset: 0x00086E28
		public override string StringId
		{
			get
			{
				return "NoConflictTag";
			}
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x00088C30 File Offset: 0x00086E30
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = new HostileRelationshipTag().IsApplicableTo(character);
			bool flag2 = new PlayerIsEnemyTag().IsApplicableTo(character);
			return !flag && !flag2;
		}

		// Token: 0x04000996 RID: 2454
		public const string Id = "NoConflictTag";
	}
}
