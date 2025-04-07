using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Election
{
	// Token: 0x02000276 RID: 630
	public class KingdomElection
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x0008E2E2 File Offset: 0x0008C4E2
		public MBReadOnlyList<DecisionOutcome> PossibleOutcomes
		{
			get
			{
				return this._possibleOutcomes;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002175 RID: 8565 RVA: 0x0008E2EA File Offset: 0x0008C4EA
		// (set) Token: 0x06002176 RID: 8566 RVA: 0x0008E2F2 File Offset: 0x0008C4F2
		[SaveableProperty(7)]
		public bool IsCancelled { get; private set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x0008E2FB File Offset: 0x0008C4FB
		public bool IsPlayerSupporter
		{
			get
			{
				return this.PlayerAsSupporter != null;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x0008E306 File Offset: 0x0008C506
		private Supporter PlayerAsSupporter
		{
			get
			{
				return this._supporters.FirstOrDefault((Supporter x) => x.IsPlayer);
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x0008E332 File Offset: 0x0008C532
		public bool IsPlayerChooser
		{
			get
			{
				return this._chooser.Leader.IsHumanPlayerCharacter;
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0008E344 File Offset: 0x0008C544
		public KingdomElection(KingdomDecision decision)
		{
			this._decision = decision;
			this.Setup();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x0008E35C File Offset: 0x0008C55C
		private void Setup()
		{
			MBList<DecisionOutcome> initialCandidates = this._decision.DetermineInitialCandidates().ToMBList<DecisionOutcome>();
			this._possibleOutcomes = this._decision.NarrowDownCandidates(initialCandidates, 3);
			this._supporters = this._decision.DetermineSupporters().ToList<Supporter>();
			this._chooser = this._decision.DetermineChooser();
			this._decision.DetermineSponsors(this._possibleOutcomes);
			this._hasPlayerVoted = false;
			this.IsCancelled = false;
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				decisionOutcome.InitialSupport = this.DetermineInitialSupport(decisionOutcome);
			}
			float num = this._possibleOutcomes.Sum((DecisionOutcome x) => x.InitialSupport);
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				decisionOutcome2.Likelihood = ((num == 0f) ? 0f : (decisionOutcome2.InitialSupport / num));
			}
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x0008E4A8 File Offset: 0x0008C6A8
		public void StartElection()
		{
			this.Setup();
			this.DetermineSupport(this._possibleOutcomes, false);
			this._decision.DetermineSponsors(this._possibleOutcomes);
			this.UpdateSupport(this._possibleOutcomes);
			if (this._decision.ShouldBeCancelled())
			{
				Debug.Print("SELIM_DEBUG - " + this._decision.GetSupportTitle() + " has been cancelled", 0, Debug.DebugColor.White, 17592186044416UL);
				this.IsCancelled = true;
				return;
			}
			if (!this.IsPlayerSupporter || this._ignorePlayerSupport)
			{
				this.ReadyToAiChoose();
				return;
			}
			if (this._decision.IsSingleClanDecision())
			{
				this._chosenOutcome = this._possibleOutcomes.FirstOrDefault((DecisionOutcome t) => t.SponsorClan != null && t.SponsorClan == Clan.PlayerClan);
				Supporter supporter = new Supporter(Clan.PlayerClan);
				supporter.SupportWeight = Supporter.SupportWeights.FullyPush;
				this._chosenOutcome.AddSupport(supporter);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x0008E598 File Offset: 0x0008C798
		private float DetermineInitialSupport(DecisionOutcome possibleOutcome)
		{
			float num = 0f;
			foreach (Supporter supporter in this._supporters)
			{
				if (!supporter.IsPlayer)
				{
					num += MathF.Clamp(this._decision.DetermineSupport(supporter.Clan, possibleOutcome), 0f, 100f);
				}
			}
			return num;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x0008E618 File Offset: 0x0008C818
		public void StartElectionWithoutPlayer()
		{
			this._ignorePlayerSupport = true;
			this.StartElection();
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x0008E627 File Offset: 0x0008C827
		public float GetLikelihoodForOutcome(int outcomeNo)
		{
			if (outcomeNo >= 0 && outcomeNo < this._possibleOutcomes.Count)
			{
				return this._possibleOutcomes[outcomeNo].Likelihood;
			}
			return 0f;
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x0008E654 File Offset: 0x0008C854
		public float GetLikelihoodForSponsor(Clan sponsor)
		{
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				if (decisionOutcome.SponsorClan == sponsor)
				{
					return decisionOutcome.Likelihood;
				}
			}
			Debug.FailedAssert("This clan is not a sponsor of any of the outcomes.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Election\\KingdomDecisionMaker.cs", "GetLikelihoodForSponsor", 151);
			return -1f;
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x0008E6D4 File Offset: 0x0008C8D4
		private void DetermineSupport(MBReadOnlyList<DecisionOutcome> possibleOutcomes, bool calculateRelationshipEffect)
		{
			foreach (Supporter supporter in this._supporters)
			{
				if (!supporter.IsPlayer)
				{
					Supporter.SupportWeights supportWeight = Supporter.SupportWeights.StayNeutral;
					DecisionOutcome decisionOutcome = this._decision.DetermineSupportOption(supporter, possibleOutcomes, out supportWeight, calculateRelationshipEffect);
					if (decisionOutcome != null)
					{
						supporter.SupportWeight = supportWeight;
						decisionOutcome.AddSupport(supporter);
					}
				}
			}
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x0008E74C File Offset: 0x0008C94C
		private void UpdateSupport(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				foreach (Supporter supporter in new List<Supporter>(decisionOutcome.SupporterList))
				{
					decisionOutcome.ResetSupport(supporter);
				}
			}
			this.DetermineSupport(possibleOutcomes, true);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x0008E7E8 File Offset: 0x0008C9E8
		private void ReadyToAiChoose()
		{
			this._chosenOutcome = this.GetAiChoice(this._possibleOutcomes);
			if (this._decision.OnShowDecision())
			{
				this.ApplyChosenOutcome();
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0008E810 File Offset: 0x0008CA10
		private void ApplyChosenOutcome()
		{
			this._decision.ApplyChosenOutcome(this._chosenOutcome);
			this._decision.SupportStatusOfFinalDecision = this.GetSupportStatusOfDecisionOutcome(this._chosenOutcome);
			this.HandleInfluenceCosts();
			this.ApplySecondaryEffects(this._possibleOutcomes, this._chosenOutcome);
			for (int i = 0; i < this._possibleOutcomes.Count; i++)
			{
				if (this._possibleOutcomes[i].SponsorClan != null)
				{
					foreach (Supporter supporter in this._possibleOutcomes[i].SupporterList)
					{
						if (supporter.Clan.Leader != this._possibleOutcomes[i].SponsorClan.Leader && supporter.Clan == Clan.PlayerClan)
						{
							int num = this.GetRelationChangeWithSponsor(supporter.Clan.Leader, supporter.SupportWeight, false);
							if (num != 0)
							{
								num *= ((this._possibleOutcomes.Count > 2) ? 2 : 1);
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(supporter.Clan.Leader, this._possibleOutcomes[i].SponsorClan.Leader, num, true);
							}
						}
					}
					for (int j = 0; j < this._possibleOutcomes.Count; j++)
					{
						if (i != j)
						{
							foreach (Supporter supporter2 in this._possibleOutcomes[j].SupporterList)
							{
								if (supporter2.Clan.Leader != this._possibleOutcomes[i].SponsorClan.Leader && supporter2.Clan == Clan.PlayerClan)
								{
									int relationChangeWithSponsor = this.GetRelationChangeWithSponsor(supporter2.Clan.Leader, supporter2.SupportWeight, true);
									if (relationChangeWithSponsor != 0)
									{
										ChangeRelationAction.ApplyRelationChangeBetweenHeroes(supporter2.Clan.Leader, this._possibleOutcomes[i].SponsorClan.Leader, relationChangeWithSponsor, true);
									}
								}
							}
						}
					}
				}
			}
			this._decision.Kingdom.RemoveDecision(this._decision);
			this._decision.Kingdom.OnKingdomDecisionConcluded();
			CampaignEventDispatcher.Instance.OnKingdomDecisionConcluded(this._decision, this._chosenOutcome, this.IsPlayerChooser || this._hasPlayerVoted);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0008EAA8 File Offset: 0x0008CCA8
		public int GetRelationChangeWithSponsor(Hero opposerOrSupporter, Supporter.SupportWeights supportWeight, bool isOpposingSides)
		{
			int num = 0;
			Clan clan = opposerOrSupporter.Clan;
			if (supportWeight == Supporter.SupportWeights.FullyPush)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.FullyPush) / 20f);
			}
			else if (supportWeight == Supporter.SupportWeights.StronglyFavor)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.StronglyFavor) / 20f);
			}
			else if (supportWeight == Supporter.SupportWeights.SlightlyFavor)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.SlightlyFavor) / 20f);
			}
			int num2 = isOpposingSides ? (num * -1) : (num * 2);
			if (isOpposingSides && opposerOrSupporter.Culture.HasFeat(DefaultCulturalFeats.SturgianDecisionPenaltyFeat))
			{
				num2 += (int)((float)num2 * DefaultCulturalFeats.SturgianDecisionPenaltyFeat.EffectBonus);
			}
			return num2;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0008EB44 File Offset: 0x0008CD44
		private void HandleInfluenceCosts()
		{
			DecisionOutcome decisionOutcome = this._possibleOutcomes[0];
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				if (decisionOutcome2.TotalSupportPoints > decisionOutcome.TotalSupportPoints)
				{
					decisionOutcome = decisionOutcome2;
				}
				for (int i = 0; i < decisionOutcome2.SupporterList.Count; i++)
				{
					Clan clan = decisionOutcome2.SupporterList[i].Clan;
					int num = this._decision.GetInfluenceCost(decisionOutcome2, clan, decisionOutcome2.SupporterList[i].SupportWeight);
					if (this._supporters.Count == 1)
					{
						num = 0;
					}
					if (this._chosenOutcome != decisionOutcome2)
					{
						num /= 2;
					}
					if (decisionOutcome2 == this._chosenOutcome || !clan.Leader.GetPerkValue(DefaultPerks.Charm.GoodNatured))
					{
						ChangeClanInfluenceAction.Apply(clan, (float)(-(float)num));
					}
				}
			}
			if (this._chosenOutcome != decisionOutcome)
			{
				int influenceRequiredToOverrideKingdomDecision = Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(decisionOutcome, this._chosenOutcome, this._decision);
				ChangeClanInfluenceAction.Apply(this._chooser, (float)(-(float)influenceRequiredToOverrideKingdomDecision));
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0008EC88 File Offset: 0x0008CE88
		private void ApplySecondaryEffects(MBReadOnlyList<DecisionOutcome> possibleOutcomes, DecisionOutcome chosenOutcome)
		{
			this._decision.ApplySecondaryEffects(possibleOutcomes, chosenOutcome);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0008EC97 File Offset: 0x0008CE97
		private int GetInfluenceRequiredToOverrideDecision(DecisionOutcome popularOutcome, DecisionOutcome overridingOutcome)
		{
			return Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(popularOutcome, overridingOutcome, this._decision);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0008ECB8 File Offset: 0x0008CEB8
		private DecisionOutcome GetAiChoice(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			this.DetermineOfficialSupport();
			DecisionOutcome decisionOutcome = possibleOutcomes.MaxBy((DecisionOutcome t) => t.TotalSupportPoints);
			DecisionOutcome result = decisionOutcome;
			if (this._decision.IsKingsVoteAllowed)
			{
				DecisionOutcome decisionOutcome2 = possibleOutcomes.MaxBy((DecisionOutcome t) => this._decision.DetermineSupport(this._chooser, t));
				float num = this._decision.DetermineSupport(this._chooser, decisionOutcome2);
				float num2 = this._decision.DetermineSupport(this._chooser, decisionOutcome);
				float num3 = num - num2;
				num3 = MathF.Min(num3, this._chooser.Influence);
				if (num3 > 10f)
				{
					float num4 = 300f + (float)this.GetInfluenceRequiredToOverrideDecision(decisionOutcome, decisionOutcome2);
					if (num3 > num4)
					{
						float num5 = num4 / num3;
						if (MBRandom.RandomFloat > num5)
						{
							result = decisionOutcome2;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0008ED88 File Offset: 0x0008CF88
		public TextObject GetChosenOutcomeText()
		{
			return this._decision.GetChosenOutcomeText(this._chosenOutcome, this._decision.SupportStatusOfFinalDecision, false);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0008EDA8 File Offset: 0x0008CFA8
		private KingdomDecision.SupportStatus GetSupportStatusOfDecisionOutcome(DecisionOutcome chosenOutcome)
		{
			KingdomDecision.SupportStatus result = KingdomDecision.SupportStatus.Equal;
			float num = chosenOutcome.WinChance * 100f;
			int num2 = 50;
			if (num > (float)(num2 + 5))
			{
				result = KingdomDecision.SupportStatus.Majority;
			}
			else if (num < (float)(num2 - 5))
			{
				result = KingdomDecision.SupportStatus.Minority;
			}
			return result;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0008EDDC File Offset: 0x0008CFDC
		public void DetermineOfficialSupport()
		{
			new List<Tuple<DecisionOutcome, float>>();
			float num = 0.001f;
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				float num2 = 0f;
				foreach (Supporter supporter in decisionOutcome.SupporterList)
				{
					num2 += (float)MathF.Max(0, supporter.SupportWeight - Supporter.SupportWeights.StayNeutral);
				}
				decisionOutcome.TotalSupportPoints = num2;
				num += decisionOutcome.TotalSupportPoints;
			}
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				decisionOutcome2.TotalSupportPoints /= num;
			}
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0008EEE4 File Offset: 0x0008D0E4
		public int GetInfluenceCostOfOutcome(DecisionOutcome outcome, Clan supporter, Supporter.SupportWeights weight)
		{
			return this._decision.GetInfluenceCostOfSupport(supporter, weight);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0008EEF3 File Offset: 0x0008D0F3
		public TextObject GetSecondaryEffects()
		{
			return this._decision.GetSecondaryEffects();
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0008EF00 File Offset: 0x0008D100
		public void OnPlayerSupport(DecisionOutcome decisionOutcome, Supporter.SupportWeights supportWeight)
		{
			if (!this.IsPlayerChooser)
			{
				foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
				{
					decisionOutcome2.ResetSupport(this.PlayerAsSupporter);
				}
				this._hasPlayerVoted = true;
				if (decisionOutcome != null)
				{
					this.PlayerAsSupporter.SupportWeight = supportWeight;
					decisionOutcome.AddSupport(this.PlayerAsSupporter);
					return;
				}
			}
			else
			{
				this._chosenOutcome = decisionOutcome;
			}
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0008EF88 File Offset: 0x0008D188
		public void ApplySelection()
		{
			if (!this.IsCancelled)
			{
				if (this._chooser != Clan.PlayerClan)
				{
					this.ReadyToAiChoose();
					return;
				}
				this.ApplyChosenOutcome();
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0008EFAC File Offset: 0x0008D1AC
		public MBList<DecisionOutcome> GetSortedDecisionOutcomes()
		{
			return this._decision.SortDecisionOutcomes(this._possibleOutcomes);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0008EFBF File Offset: 0x0008D1BF
		public TextObject GetGeneralTitle()
		{
			return this._decision.GetGeneralTitle();
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0008EFCC File Offset: 0x0008D1CC
		public TextObject GetTitle()
		{
			if (this.IsPlayerChooser)
			{
				return this._decision.GetChooseTitle();
			}
			return this._decision.GetSupportTitle();
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0008EFED File Offset: 0x0008D1ED
		public TextObject GetDescription()
		{
			if (this.IsPlayerChooser)
			{
				return this._decision.GetChooseDescription();
			}
			return this._decision.GetSupportDescription();
		}

		// Token: 0x04000A5B RID: 2651
		[SaveableField(0)]
		private readonly KingdomDecision _decision;

		// Token: 0x04000A5C RID: 2652
		private MBList<DecisionOutcome> _possibleOutcomes;

		// Token: 0x04000A5D RID: 2653
		[SaveableField(2)]
		private List<Supporter> _supporters;

		// Token: 0x04000A5E RID: 2654
		[SaveableField(3)]
		private Clan _chooser;

		// Token: 0x04000A5F RID: 2655
		[SaveableField(4)]
		private DecisionOutcome _chosenOutcome;

		// Token: 0x04000A60 RID: 2656
		[SaveableField(5)]
		private bool _ignorePlayerSupport;

		// Token: 0x04000A61 RID: 2657
		[SaveableField(6)]
		private bool _hasPlayerVoted;
	}
}
