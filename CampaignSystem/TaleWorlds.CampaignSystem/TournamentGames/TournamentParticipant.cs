using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027D RID: 637
	public class TournamentParticipant
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x000912B8 File Offset: 0x0008F4B8
		// (set) Token: 0x06002214 RID: 8724 RVA: 0x000912C0 File Offset: 0x0008F4C0
		public int Score { get; private set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000912C9 File Offset: 0x0008F4C9
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x000912D1 File Offset: 0x0008F4D1
		public CharacterObject Character { get; private set; }

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000912DA File Offset: 0x0008F4DA
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x000912E2 File Offset: 0x0008F4E2
		public UniqueTroopDescriptor Descriptor { get; private set; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000912EB File Offset: 0x0008F4EB
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x000912F3 File Offset: 0x0008F4F3
		public TournamentTeam Team { get; private set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000912FC File Offset: 0x0008F4FC
		// (set) Token: 0x0600221C RID: 8732 RVA: 0x00091304 File Offset: 0x0008F504
		public Equipment MatchEquipment { get; set; }

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x0009130D File Offset: 0x0008F50D
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x00091315 File Offset: 0x0008F515
		public bool IsAssigned { get; set; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x0009131E File Offset: 0x0008F51E
		public bool IsPlayer
		{
			get
			{
				CharacterObject character = this.Character;
				return character != null && character.IsPlayerCharacter;
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00091331 File Offset: 0x0008F531
		public TournamentParticipant(CharacterObject character, UniqueTroopDescriptor descriptor = default(UniqueTroopDescriptor))
		{
			this.Character = character;
			this.Descriptor = (descriptor.IsValid ? descriptor : new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed));
			this.Team = null;
			this.IsAssigned = false;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x0009136F File Offset: 0x0008F56F
		public void SetTeam(TournamentTeam team)
		{
			this.Team = team;
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00091378 File Offset: 0x0008F578
		public int AddScore(int score)
		{
			this.Score += score;
			return this.Score;
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x0009138E File Offset: 0x0008F58E
		public void ResetScore()
		{
			this.Score = 0;
		}
	}
}
