using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003BB RID: 955
	public class PartyHealCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A48 RID: 14920 RVA: 0x00112D3C File Offset: 0x00110F3C
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanHourlyTick));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.OnWeeklyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnQuarterDailyPartyTick.AddNonSerializedListener(this, new Action<MobileParty>(this.OnQuarterDailyPartyTick));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00112DEA File Offset: 0x00110FEA
		private void OnQuarterDailyPartyTick(MobileParty mobileParty)
		{
			if (!mobileParty.IsMainParty)
			{
				this.TryHealOrWoundParty(mobileParty, false);
			}
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00112DFC File Offset: 0x00110FFC
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._overflowedHealingForRegulars.ContainsKey(mobileParty.Party))
			{
				this._overflowedHealingForRegulars.Remove(mobileParty.Party);
				if (this._overflowedHealingForHeroes.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForHeroes.Remove(mobileParty.Party);
				}
			}
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00112E54 File Offset: 0x00111054
		private void OnWeeklyTick()
		{
			List<PartyBase> list = new List<PartyBase>();
			foreach (KeyValuePair<PartyBase, float> keyValuePair in this._overflowedHealingForRegulars)
			{
				PartyBase key = keyValuePair.Key;
				if (!key.IsActive && !key.IsValid)
				{
					list.Add(key);
				}
			}
			foreach (PartyBase key2 in list)
			{
				this._overflowedHealingForRegulars.Remove(key2);
				if (this._overflowedHealingForHeroes.ContainsKey(key2))
				{
					this._overflowedHealingForHeroes.Remove(key2);
				}
			}
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00112F28 File Offset: 0x00111128
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsPlayerMapEvent)
			{
				this.OnBattleEndCheckPerkEffects(mapEvent);
			}
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x00112F39 File Offset: 0x00111139
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			this.OnBattleEndCheckPerkEffects(mapEvent);
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00112F44 File Offset: 0x00111144
		private void OnBattleEndCheckPerkEffects(MapEvent mapEvent)
		{
			if (mapEvent.HasWinner)
			{
				foreach (PartyBase partyBase in mapEvent.InvolvedParties)
				{
					if (partyBase.MemberRoster.TotalHeroes > 0)
					{
						foreach (TroopRosterElement troopRosterElement in partyBase.MemberRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero)
							{
								Hero heroObject = troopRosterElement.Character.HeroObject;
								int battleEndHealingAmount = Campaign.Current.Models.PartyHealingModel.GetBattleEndHealingAmount(partyBase.MobileParty, heroObject);
								if (battleEndHealingAmount > 0)
								{
									heroObject.Heal(battleEndHealingAmount, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00113030 File Offset: 0x00111230
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForRegulars", ref this._overflowedHealingForRegulars);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForHeroes", ref this._overflowedHealingForHeroes);
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00113058 File Offset: 0x00111258
		private void OnClanHourlyTick(Clan clan)
		{
			foreach (Hero hero in clan.Heroes)
			{
				if (hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null)
				{
					int a = MBRandom.RoundRandomized(0.5f);
					if (hero.HitPoints < hero.MaxHitPoints)
					{
						int num = MathF.Min(a, hero.MaxHitPoints - hero.HitPoints);
						hero.HitPoints += num;
					}
				}
			}
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x001130F0 File Offset: 0x001112F0
		private void OnHourlyTick()
		{
			this.TryHealOrWoundParty(MobileParty.MainParty, true);
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00113100 File Offset: 0x00111300
		private void TryHealOrWoundParty(MobileParty mobileParty, bool isCheckingForPlayerRelatedParty)
		{
			if (mobileParty.IsActive && mobileParty.MapEvent == null)
			{
				float num;
				if (!this._overflowedHealingForHeroes.TryGetValue(mobileParty.Party, out num))
				{
					this._overflowedHealingForHeroes.Add(mobileParty.Party, 0f);
				}
				float num2;
				if (!this._overflowedHealingForRegulars.TryGetValue(mobileParty.Party, out num2))
				{
					this._overflowedHealingForRegulars.Add(mobileParty.Party, 0f);
				}
				float num3 = isCheckingForPlayerRelatedParty ? (mobileParty.HealingRateForHeroes / 24f) : mobileParty.HealingRateForHeroes;
				float num4 = isCheckingForPlayerRelatedParty ? (mobileParty.HealingRateForRegulars / 24f) : mobileParty.HealingRateForRegulars;
				num += num3;
				num2 += num4;
				if (num >= 1f)
				{
					PartyHealCampaignBehavior.HealHeroes(mobileParty, ref num);
				}
				else if (num <= -1f)
				{
					PartyHealCampaignBehavior.ReduceHpHeroes(mobileParty, ref num);
				}
				if (num2 >= 1f)
				{
					PartyHealCampaignBehavior.HealRegulars(mobileParty, ref num2);
				}
				else if (num2 <= -1f)
				{
					PartyHealCampaignBehavior.ReduceHpRegulars(mobileParty, ref num2);
				}
				this._overflowedHealingForHeroes[mobileParty.Party] = num;
				this._overflowedHealingForRegulars[mobileParty.Party] = num2;
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x0011321C File Offset: 0x0011141C
		private static void HealHeroes(MobileParty mobileParty, ref float heroesHealingValue)
		{
			int num = MathF.Floor(heroesHealingValue);
			heroesHealingValue -= (float)num;
			TroopRoster memberRoster = mobileParty.MemberRoster;
			if (memberRoster.TotalHeroes > 0)
			{
				for (int i = 0; i < memberRoster.Count; i++)
				{
					Hero heroObject = memberRoster.GetCharacterAtIndex(i).HeroObject;
					if (heroObject != null && !heroObject.IsHealthFull())
					{
						heroObject.Heal(num, true);
					}
				}
			}
			TroopRoster prisonRoster = mobileParty.PrisonRoster;
			if (prisonRoster.TotalHeroes > 0)
			{
				for (int j = 0; j < prisonRoster.Count; j++)
				{
					Hero heroObject2 = prisonRoster.GetCharacterAtIndex(j).HeroObject;
					if (heroObject2 != null && !heroObject2.IsHealthFull())
					{
						heroObject2.Heal(1, false);
					}
				}
			}
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x001132C8 File Offset: 0x001114C8
		private static void ReduceHpHeroes(MobileParty mobileParty, ref float heroesHealingValue)
		{
			int a = MathF.Ceiling(heroesHealingValue);
			heroesHealingValue = -(-heroesHealingValue % 1f);
			for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
			{
				Hero heroObject = mobileParty.MemberRoster.GetCharacterAtIndex(i).HeroObject;
				if (heroObject != null && heroObject.HitPoints > 0)
				{
					int num = MathF.Min(a, heroObject.HitPoints);
					heroObject.HitPoints += num;
				}
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00113338 File Offset: 0x00111538
		private static void HealRegulars(MobileParty mobileParty, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = mobileParty.MemberRoster;
			if (memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(regularsHealingValue);
			regularsHealingValue -= (float)num;
			int num2 = 0;
			float num3 = 0f;
			int num4 = MBRandom.RandomInt(memberRoster.Count);
			int num5 = 0;
			while (num5 < memberRoster.Count && num > 0)
			{
				int index = (num4 + num5) % memberRoster.Count;
				CharacterObject characterAtIndex = memberRoster.GetCharacterAtIndex(index);
				if (characterAtIndex.IsRegular)
				{
					int num6 = MathF.Min(num, memberRoster.GetElementWoundedNumber(index));
					if (num6 > 0)
					{
						memberRoster.AddToCountsAtIndex(index, 0, -num6, 0, true);
						num -= num6;
						num2 += num6;
						num3 += (float)(characterAtIndex.Tier * num6);
					}
				}
				num5++;
			}
			if (num2 > 0)
			{
				SkillLevelingManager.OnRegularTroopHealedWhileWaiting(mobileParty, num2, num3 / (float)num2);
			}
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00113408 File Offset: 0x00111608
		private static void ReduceHpRegulars(MobileParty mobileParty, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = mobileParty.MemberRoster;
			if (memberRoster.TotalRegulars - memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(-regularsHealingValue);
			regularsHealingValue = -(-regularsHealingValue % 1f);
			int num2 = MBRandom.RandomInt(memberRoster.Count);
			int num3 = 0;
			while (num3 < memberRoster.Count && num > 0)
			{
				int index = (num2 + num3) % memberRoster.Count;
				if (memberRoster.GetCharacterAtIndex(index).IsRegular)
				{
					int num4 = MathF.Min(memberRoster.GetElementNumber(index) - memberRoster.GetElementWoundedNumber(index), num);
					if (num4 > 0)
					{
						memberRoster.AddToCountsAtIndex(index, 0, num4, 0, true);
						num -= num4;
					}
				}
				num3++;
			}
		}

		// Token: 0x040011B4 RID: 4532
		private Dictionary<PartyBase, float> _overflowedHealingForRegulars = new Dictionary<PartyBase, float>();

		// Token: 0x040011B5 RID: 4533
		private Dictionary<PartyBase, float> _overflowedHealingForHeroes = new Dictionary<PartyBase, float>();
	}
}
