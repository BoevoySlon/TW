using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EF RID: 239
	public class DefaultBattleRewardModel : BattleRewardModel
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0005CC08 File Offset: 0x0005AE08
		public override float DestroyHideoutBannerLootChance
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0005CC0F File Offset: 0x0005AE0F
		public override float CaptureSettlementBannerLootChance
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0005CC16 File Offset: 0x0005AE16
		public override float DefeatRegularHeroBannerLootChance
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0005CC1D File Offset: 0x0005AE1D
		public override float DefeatClanLeaderBannerLootChance
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0005CC24 File Offset: 0x0005AE24
		public override float DefeatKingdomRulerBannerLootChance
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0005CC2C File Offset: 0x0005AE2C
		public override int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero)
		{
			MapEventSide mapEventSide = mapEvent.AttackerSide.IsMainPartyAmongParties() ? mapEvent.AttackerSide : mapEvent.DefenderSide;
			float playerPartyContributionRate = mapEventSide.GetPlayerPartyContributionRate();
			float num = (mapEvent.StrengthOfSide[(int)PartyBase.MainParty.Side] - PlayerEncounter.Current.PlayerPartyInitialStrength) / (mapEvent.StrengthOfSide[(int)PartyBase.MainParty.OpponentSide] + 1f);
			float num2 = (num < 1f) ? (1f + (1f - num)) : ((num < 3f) ? (0.5f * (3f - num)) : 0f);
			float renownValueAtMapEventEnd = mapEvent.GetRenownValueAtMapEventEnd((mapEventSide == mapEvent.AttackerSide) ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			ExplainedNumber explainedNumber = new ExplainedNumber(0.75f + MathF.Pow(playerPartyContributionRate * 1.3f * (num2 + renownValueAtMapEventEnd), 0.67f), false, null);
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.Camaraderie))
			{
				explainedNumber.AddFactor(DefaultPerks.Charm.Camaraderie.PrimaryBonus, DefaultPerks.Charm.Camaraderie.Name);
			}
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0005CD34 File Offset: 0x0005AF34
		public override ExplainedNumber CalculateRenownGain(PartyBase party, float renownValueOfBattle, float contributionShare)
		{
			ExplainedNumber result = new ExplainedNumber(renownValueOfBattle * contributionShare, true, null);
			if (party.IsMobile)
			{
				if (party.MobileParty.HasPerk(DefaultPerks.Throwing.LongReach, true))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.LongReach, party.MobileParty, false, ref result);
				}
				if (party.MobileParty.HasPerk(DefaultPerks.Charm.PublicSpeaker, false))
				{
					result.AddFactor(DefaultPerks.Charm.PublicSpeaker.PrimaryBonus, DefaultPerks.Charm.PublicSpeaker.Name);
				}
				if (party.LeaderHero != null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Leadership.FamousCommander, party.LeaderHero.CharacterObject, true, ref result);
				}
				if (PartyBaseHelper.HasFeat(party, DefaultCulturalFeats.VlandianRenownMercenaryFeat))
				{
					result.AddFactor(DefaultCulturalFeats.VlandianRenownMercenaryFeat.EffectBonus, GameTexts.FindText("str_culture", null));
				}
			}
			return result;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0005CDF8 File Offset: 0x0005AFF8
		public override ExplainedNumber CalculateInfluenceGain(PartyBase party, float influenceValueOfBattle, float contributionShare)
		{
			ExplainedNumber result = new ExplainedNumber(party.MapFaction.IsKingdomFaction ? (influenceValueOfBattle * contributionShare) : 0f, true, null);
			Hero leaderHero = party.LeaderHero;
			if (leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Charm.Warlord))
			{
				result.AddFactor(DefaultPerks.Charm.Warlord.PrimaryBonus, DefaultPerks.Charm.Warlord.Name);
			}
			return result;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0005CE5C File Offset: 0x0005B05C
		public override ExplainedNumber CalculateMoraleGainVictory(PartyBase party, float renownValueOfBattle, float contributionShare)
		{
			ExplainedNumber result = new ExplainedNumber(0.5f + renownValueOfBattle * contributionShare * 0.5f, true, null);
			if (party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Throwing.LongReach, true))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.LongReach, party.MobileParty, false, ref result);
			}
			if (party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Leadership.CitizenMilitia, true))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.CitizenMilitia, party.MobileParty, false, ref result);
			}
			return result;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0005CEE0 File Offset: 0x0005B0E0
		public override int CalculateGoldLossAfterDefeat(Hero partyLeaderHero)
		{
			float num = (float)partyLeaderHero.Gold * 0.05f;
			if (num > 10000f)
			{
				num = 10000f;
			}
			return (int)num;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0005CF0C File Offset: 0x0005B10C
		public override EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue)
		{
			bool flag = MobileParty.MainParty.HasPerk(DefaultPerks.Engineering.Metallurgy, false);
			Equipment randomElement = character.AllEquipments.GetRandomElement<Equipment>();
			EquipmentElement randomItem = this.GetRandomItem(randomElement, targetValue);
			if (flag && randomItem.ItemModifier != null && randomItem.ItemModifier.PriceMultiplier < 1f && MBRandom.RandomFloat < DefaultPerks.Engineering.Metallurgy.PrimaryBonus)
			{
				randomItem = new EquipmentElement(randomItem.Item, null, null, false);
			}
			return randomItem;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0005CF80 File Offset: 0x0005B180
		private EquipmentElement GetRandomItem(Equipment equipment, float targetValue = 0f)
		{
			int num = 0;
			for (int i = 0; i < 12; i++)
			{
				if (equipment[i].Item != null && !equipment[i].Item.NotMerchandise)
				{
					DefaultBattleRewardModel._indices[num] = i;
					num++;
				}
			}
			for (int j = 0; j < num - 1; j++)
			{
				int num2 = j;
				int value = equipment[DefaultBattleRewardModel._indices[j]].Item.Value;
				for (int k = j + 1; k < num; k++)
				{
					if (equipment[DefaultBattleRewardModel._indices[k]].Item.Value > value)
					{
						num2 = k;
						value = equipment[DefaultBattleRewardModel._indices[k]].Item.Value;
					}
				}
				int num3 = DefaultBattleRewardModel._indices[j];
				DefaultBattleRewardModel._indices[j] = DefaultBattleRewardModel._indices[num2];
				DefaultBattleRewardModel._indices[num2] = num3;
			}
			if (num > 0)
			{
				for (int l = 0; l < num; l++)
				{
					int index = DefaultBattleRewardModel._indices[l];
					EquipmentElement result = equipment[index];
					if (result.Item != null && !equipment[index].Item.NotMerchandise)
					{
						float b = (float)result.Item.Value + 0.1f;
						float num4 = 0.6f * (targetValue / (MathF.Max(targetValue, b) * (float)(num - l)));
						if (MBRandom.RandomFloat < num4)
						{
							ItemComponent itemComponent = result.Item.ItemComponent;
							ItemModifier itemModifier;
							if (itemComponent == null)
							{
								itemModifier = null;
							}
							else
							{
								ItemModifierGroup itemModifierGroup = itemComponent.ItemModifierGroup;
								itemModifier = ((itemModifierGroup != null) ? itemModifierGroup.GetRandomItemModifierLootScoreBased() : null);
							}
							ItemModifier itemModifier2 = itemModifier;
							if (itemModifier2 != null)
							{
								result = new EquipmentElement(result.Item, itemModifier2, null, false);
							}
							return result;
						}
					}
				}
			}
			return default(EquipmentElement);
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0005D148 File Offset: 0x0005B348
		public override float GetPartySavePrisonerAsMemberShareProbability(PartyBase winnerParty, float lootAmount)
		{
			float result = lootAmount;
			if (winnerParty.IsMobile && (winnerParty.MobileParty.IsVillager || winnerParty.MobileParty.IsCaravan || winnerParty.MobileParty.IsMilitia || (winnerParty.MobileParty.IsBandit && winnerParty.MobileParty.CurrentSettlement != null && winnerParty.MobileParty.CurrentSettlement.IsHideout)))
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0005D1B9 File Offset: 0x0005B3B9
		public override float GetExpectedLootedItemValue(CharacterObject character)
		{
			return 6f * (float)(character.Level * character.Level);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0005D1CF File Offset: 0x0005B3CF
		public override float GetAITradePenalty()
		{
			return 0.018181818f;
		}

		// Token: 0x0400071F RID: 1823
		private static readonly int[] _indices = new int[12];
	}
}
