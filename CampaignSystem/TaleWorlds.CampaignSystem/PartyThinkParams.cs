using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000088 RID: 136
	public class PartyThinkParams
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0004BF92 File Offset: 0x0004A192
		public MBReadOnlyList<ValueTuple<AIBehaviorTuple, float>> AIBehaviorScores
		{
			get
			{
				return this._aiBehaviorScores;
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0004BF9A File Offset: 0x0004A19A
		public PartyThinkParams(MobileParty mobileParty)
		{
			this._aiBehaviorScores = new MBList<ValueTuple<AIBehaviorTuple, float>>(16);
			this.MobilePartyOf = mobileParty;
			this.WillGatherAnArmy = false;
			this.DoNotChangeBehavior = false;
			this.CurrentObjectiveValue = 0f;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0004BFD0 File Offset: 0x0004A1D0
		public void Reset(MobileParty mobileParty)
		{
			this._aiBehaviorScores.Clear();
			this.MobilePartyOf = mobileParty;
			this.WillGatherAnArmy = false;
			this.DoNotChangeBehavior = false;
			this.CurrentObjectiveValue = 0f;
			this.StrengthOfLordsWithoutArmy = 0f;
			this.StrengthOfLordsWithArmy = 0f;
			this.StrengthOfLordsAtSameClanWithoutArmy = 0f;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0004C02C File Offset: 0x0004A22C
		public void Initialization()
		{
			this.StrengthOfLordsWithoutArmy = 0f;
			this.StrengthOfLordsWithArmy = 0f;
			this.StrengthOfLordsAtSameClanWithoutArmy = 0f;
			foreach (Hero hero in this.MobilePartyOf.MapFaction.Heroes)
			{
				if (hero.PartyBelongedTo != null)
				{
					MobileParty partyBelongedTo = hero.PartyBelongedTo;
					if (partyBelongedTo.Army != null)
					{
						this.StrengthOfLordsWithArmy += partyBelongedTo.Party.TotalStrength;
					}
					else
					{
						this.StrengthOfLordsWithoutArmy += partyBelongedTo.Party.TotalStrength;
						Clan clan = hero.Clan;
						Hero leaderHero = this.MobilePartyOf.LeaderHero;
						if (clan == ((leaderHero != null) ? leaderHero.Clan : null))
						{
							this.StrengthOfLordsAtSameClanWithoutArmy += partyBelongedTo.Party.TotalStrength;
						}
					}
				}
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0004C12C File Offset: 0x0004A32C
		public bool TryGetBehaviorScore(in AIBehaviorTuple aiBehaviorTuple, out float score)
		{
			foreach (ValueTuple<AIBehaviorTuple, float> valueTuple in this._aiBehaviorScores)
			{
				AIBehaviorTuple item = valueTuple.Item1;
				if (item.Equals(aiBehaviorTuple))
				{
					score = valueTuple.Item2;
					return true;
				}
			}
			score = 0f;
			return false;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004C1A4 File Offset: 0x0004A3A4
		public void SetBehaviorScore(in AIBehaviorTuple aiBehaviorTuple, float score)
		{
			for (int i = 0; i < this._aiBehaviorScores.Count; i++)
			{
				if (this._aiBehaviorScores[i].Item1.Equals(aiBehaviorTuple))
				{
					this._aiBehaviorScores[i] = new ValueTuple<AIBehaviorTuple, float>(this._aiBehaviorScores[i].Item1, score);
					return;
				}
			}
			Debug.FailedAssert("AIBehaviorScore not found.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\ICampaignBehaviorManager.cs", "SetBehaviorScore", 152);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0004C225 File Offset: 0x0004A425
		public void AddBehaviorScore(in ValueTuple<AIBehaviorTuple, float> value)
		{
			this._aiBehaviorScores.Add(value);
		}

		// Token: 0x040005CA RID: 1482
		public MobileParty MobilePartyOf;

		// Token: 0x040005CB RID: 1483
		private readonly MBList<ValueTuple<AIBehaviorTuple, float>> _aiBehaviorScores;

		// Token: 0x040005CC RID: 1484
		public float CurrentObjectiveValue;

		// Token: 0x040005CD RID: 1485
		public bool WillGatherAnArmy;

		// Token: 0x040005CE RID: 1486
		public bool DoNotChangeBehavior;

		// Token: 0x040005CF RID: 1487
		public float StrengthOfLordsWithoutArmy;

		// Token: 0x040005D0 RID: 1488
		public float StrengthOfLordsWithArmy;

		// Token: 0x040005D1 RID: 1489
		public float StrengthOfLordsAtSameClanWithoutArmy;
	}
}
