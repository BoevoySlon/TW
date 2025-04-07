using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027F RID: 639
	public class TournamentTeam
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000914A7 File Offset: 0x0008F6A7
		// (set) Token: 0x0600222E RID: 8750 RVA: 0x000914AF File Offset: 0x0008F6AF
		public int TeamSize { get; private set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x000914B8 File Offset: 0x0008F6B8
		// (set) Token: 0x06002230 RID: 8752 RVA: 0x000914C0 File Offset: 0x0008F6C0
		public uint TeamColor { get; private set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x000914C9 File Offset: 0x0008F6C9
		// (set) Token: 0x06002232 RID: 8754 RVA: 0x000914D1 File Offset: 0x0008F6D1
		public Banner TeamBanner { get; private set; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x000914DA File Offset: 0x0008F6DA
		// (set) Token: 0x06002234 RID: 8756 RVA: 0x000914E2 File Offset: 0x0008F6E2
		public bool IsPlayerTeam { get; private set; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000914EB File Offset: 0x0008F6EB
		public IEnumerable<TournamentParticipant> Participants
		{
			get
			{
				return this._participants.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000914F8 File Offset: 0x0008F6F8
		public int Score
		{
			get
			{
				int num = 0;
				foreach (TournamentParticipant tournamentParticipant in this._participants)
				{
					num += tournamentParticipant.Score;
				}
				return num;
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00091550 File Offset: 0x0008F750
		public TournamentTeam(int teamSize, uint teamColor, Banner teamBanner)
		{
			this.TeamColor = teamColor;
			this.TeamBanner = teamBanner;
			this.TeamSize = teamSize;
			this._participants = new List<TournamentParticipant>();
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00091578 File Offset: 0x0008F778
		public bool IsParticipantRequired()
		{
			return this._participants.Count < this.TeamSize;
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x0009158D File Offset: 0x0008F78D
		public void AddParticipant(TournamentParticipant participant)
		{
			participant.IsAssigned = true;
			this._participants.Add(participant);
			participant.SetTeam(this);
			if (participant.IsPlayer)
			{
				this.IsPlayerTeam = true;
			}
		}

		// Token: 0x04000A87 RID: 2695
		private List<TournamentParticipant> _participants;
	}
}
