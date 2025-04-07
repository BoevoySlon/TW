using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027C RID: 636
	public class TournamentMatch
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x00090DC3 File Offset: 0x0008EFC3
		public IEnumerable<TournamentTeam> Teams
		{
			get
			{
				return this._teams.AsEnumerable<TournamentTeam>();
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x00090DD0 File Offset: 0x0008EFD0
		public IEnumerable<TournamentParticipant> Participants
		{
			get
			{
				return this._participants.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002206 RID: 8710 RVA: 0x00090DDD File Offset: 0x0008EFDD
		// (set) Token: 0x06002207 RID: 8711 RVA: 0x00090DE5 File Offset: 0x0008EFE5
		public TournamentMatch.MatchState State { get; private set; }

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x00090DEE File Offset: 0x0008EFEE
		public IEnumerable<TournamentParticipant> Winners
		{
			get
			{
				return this._winners.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00090DFB File Offset: 0x0008EFFB
		public bool IsReady
		{
			get
			{
				return this.State == TournamentMatch.MatchState.Ready;
			}
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00090E08 File Offset: 0x0008F008
		public TournamentMatch(int participantCount, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this._participants = new List<TournamentParticipant>();
			this._participantCount = participantCount;
			this._teams = new TournamentTeam[numberOfTeamsPerMatch];
			this._winners = new List<TournamentParticipant>();
			this._numberOfWinnerParticipants = numberOfWinnerParticipants;
			this.QualificationMode = qualificationMode;
			this._teamSize = participantCount / numberOfTeamsPerMatch;
			int[] array = new int[]
			{
				119,
				118,
				120,
				121
			};
			int num = 0;
			for (int i = 0; i < numberOfTeamsPerMatch; i++)
			{
				this._teams[i] = new TournamentTeam(this._teamSize, BannerManager.GetColor(array[num]), Banner.CreateOneColoredEmptyBanner(array[num]));
				num++;
				num %= 4;
			}
			this.State = TournamentMatch.MatchState.Ready;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00090EAE File Offset: 0x0008F0AE
		public void End()
		{
			this.State = TournamentMatch.MatchState.Finished;
			this._winners = this.GetWinners();
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00090EC4 File Offset: 0x0008F0C4
		public void Start()
		{
			if (this.State != TournamentMatch.MatchState.Started)
			{
				this.State = TournamentMatch.MatchState.Started;
				foreach (TournamentParticipant tournamentParticipant in this.Participants)
				{
					tournamentParticipant.ResetScore();
				}
			}
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00090F20 File Offset: 0x0008F120
		public TournamentParticipant GetParticipant(int uniqueSeed)
		{
			return this._participants.FirstOrDefault((TournamentParticipant p) => p.Descriptor.CompareTo(uniqueSeed) == 0);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00090F51 File Offset: 0x0008F151
		public bool IsParticipantRequired()
		{
			return this._participants.Count < this._participantCount;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00090F68 File Offset: 0x0008F168
		public void AddParticipant(TournamentParticipant participant, bool firstTime)
		{
			this._participants.Add(participant);
			foreach (TournamentTeam tournamentTeam in this.Teams)
			{
				if (tournamentTeam.IsParticipantRequired() && ((participant.Team != null && participant.Team.TeamColor == tournamentTeam.TeamColor) || firstTime))
				{
					tournamentTeam.AddParticipant(participant);
					return;
				}
			}
			foreach (TournamentTeam tournamentTeam2 in this.Teams)
			{
				if (tournamentTeam2.IsParticipantRequired())
				{
					tournamentTeam2.AddParticipant(participant);
					break;
				}
			}
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00091034 File Offset: 0x0008F234
		public bool IsPlayerParticipating()
		{
			return this.Participants.Any((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00091060 File Offset: 0x0008F260
		public bool IsPlayerWinner()
		{
			if (this.IsPlayerParticipating())
			{
				return this.GetWinners().Any((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
			}
			return false;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00091098 File Offset: 0x0008F298
		private List<TournamentParticipant> GetWinners()
		{
			List<TournamentParticipant> list = new List<TournamentParticipant>();
			if (this.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
			{
				List<TournamentParticipant> list2 = (from x in this._participants
				orderby x.Score descending
				select x).Take(this._numberOfWinnerParticipants).ToList<TournamentParticipant>();
				using (List<TournamentParticipant>.Enumerator enumerator = this._participants.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TournamentParticipant tournamentParticipant = enumerator.Current;
						if (list2.Contains(tournamentParticipant))
						{
							tournamentParticipant.IsAssigned = false;
							list.Add(tournamentParticipant);
						}
					}
					return list;
				}
			}
			if (this.QualificationMode == TournamentGame.QualificationMode.TeamScore)
			{
				IOrderedEnumerable<TournamentTeam> orderedEnumerable = from x in this._teams
				orderby x.Score descending
				select x;
				List<TournamentTeam> list3 = orderedEnumerable.Take(this._numberOfWinnerParticipants / this._teamSize).ToList<TournamentTeam>();
				foreach (TournamentTeam tournamentTeam in this._teams)
				{
					if (list3.Contains(tournamentTeam))
					{
						foreach (TournamentParticipant tournamentParticipant2 in tournamentTeam.Participants)
						{
							tournamentParticipant2.IsAssigned = false;
							list.Add(tournamentParticipant2);
						}
					}
				}
				foreach (TournamentTeam tournamentTeam2 in orderedEnumerable)
				{
					int num = this._numberOfWinnerParticipants - list.Count;
					if (tournamentTeam2.Participants.Count<TournamentParticipant>() >= num)
					{
						IOrderedEnumerable<TournamentParticipant> source = from x in tournamentTeam2.Participants
						orderby x.Score descending
						select x;
						list.AddRange(source.Take(num));
						break;
					}
					list.AddRange(tournamentTeam2.Participants);
				}
			}
			return list;
		}

		// Token: 0x04000A77 RID: 2679
		private readonly int _numberOfWinnerParticipants;

		// Token: 0x04000A78 RID: 2680
		public readonly TournamentGame.QualificationMode QualificationMode;

		// Token: 0x04000A79 RID: 2681
		private readonly TournamentTeam[] _teams;

		// Token: 0x04000A7A RID: 2682
		private readonly List<TournamentParticipant> _participants;

		// Token: 0x04000A7C RID: 2684
		private List<TournamentParticipant> _winners;

		// Token: 0x04000A7D RID: 2685
		private readonly int _participantCount;

		// Token: 0x04000A7E RID: 2686
		private int _teamSize;

		// Token: 0x02000596 RID: 1430
		public enum MatchState
		{
			// Token: 0x0400174A RID: 5962
			Ready,
			// Token: 0x0400174B RID: 5963
			Started,
			// Token: 0x0400174C RID: 5964
			Finished
		}
	}
}
