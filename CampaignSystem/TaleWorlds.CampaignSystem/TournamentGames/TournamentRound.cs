using System;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027E RID: 638
	public class TournamentRound
	{
		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x00091397 File Offset: 0x0008F597
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x0009139F File Offset: 0x0008F59F
		public TournamentMatch[] Matches { get; private set; }

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x000913A8 File Offset: 0x0008F5A8
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x000913B0 File Offset: 0x0008F5B0
		public int CurrentMatchIndex { get; private set; }

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000913B9 File Offset: 0x0008F5B9
		public TournamentMatch CurrentMatch
		{
			get
			{
				if (this.CurrentMatchIndex >= this.Matches.Length)
				{
					return null;
				}
				return this.Matches[this.CurrentMatchIndex];
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000913DC File Offset: 0x0008F5DC
		public TournamentRound(int participantCount, int numberOfMatches, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this.Matches = new TournamentMatch[numberOfMatches];
			this.CurrentMatchIndex = 0;
			int participantCount2 = participantCount / numberOfMatches;
			for (int i = 0; i < numberOfMatches; i++)
			{
				this.Matches[i] = new TournamentMatch(participantCount2, numberOfTeamsPerMatch, numberOfWinnerParticipants / numberOfMatches, qualificationMode);
			}
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00091428 File Offset: 0x0008F628
		public void OnMatchEnded()
		{
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00091448 File Offset: 0x0008F648
		public void EndMatch()
		{
			this.CurrentMatch.End();
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00091470 File Offset: 0x0008F670
		public void AddParticipant(TournamentParticipant participant, bool firstTime = false)
		{
			foreach (TournamentMatch tournamentMatch in this.Matches)
			{
				if (tournamentMatch.IsParticipantRequired())
				{
					tournamentMatch.AddParticipant(participant, firstTime);
					return;
				}
			}
		}
	}
}
