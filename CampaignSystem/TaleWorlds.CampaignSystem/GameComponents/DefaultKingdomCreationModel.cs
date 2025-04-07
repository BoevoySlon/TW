using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000110 RID: 272
	public class DefaultKingdomCreationModel : KingdomCreationModel
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x00068EAC File Offset: 0x000670AC
		public override int MinimumClanTierToCreateKingdom
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x00068EAF File Offset: 0x000670AF
		public override int MinimumNumberOfSettlementsOwnedToCreateKingdom
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00068EB2 File Offset: 0x000670B2
		public override int MinimumTroopCountToCreateKingdom
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00068EB6 File Offset: 0x000670B6
		public override int MaximumNumberOfInitialPolicies
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00068EBC File Offset: 0x000670BC
		public override bool IsPlayerKingdomCreationPossible(out List<TextObject> explanations)
		{
			bool result = true;
			explanations = new List<TextObject>();
			if (Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				result = false;
				TextObject item = new TextObject("{=w5b79MmE}Player clan should be independent.", null);
				explanations.Add(item);
			}
			if (Clan.PlayerClan.Tier < this.MinimumClanTierToCreateKingdom)
			{
				result = false;
				TextObject textObject = new TextObject("{=j0UDi2AN}Clan tier should be at least {TIER}.", null);
				textObject.SetTextVariable("TIER", this.MinimumClanTierToCreateKingdom);
				explanations.Add(textObject);
			}
			if (Clan.PlayerClan.Settlements.Count((Settlement t) => t.IsTown || t.IsCastle) < this.MinimumNumberOfSettlementsOwnedToCreateKingdom)
			{
				result = false;
				TextObject textObject2 = new TextObject("{=YsGSgaba}Number of towns or castles you own should be at least {SETTLEMENT_COUNT}.", null);
				textObject2.SetTextVariable("SETTLEMENT_COUNT", this.MinimumNumberOfSettlementsOwnedToCreateKingdom);
				explanations.Add(textObject2);
			}
			if (Clan.PlayerClan.Fiefs.Sum(delegate(Town t)
			{
				MobileParty garrisonParty = t.GarrisonParty;
				int? num;
				if (garrisonParty == null)
				{
					num = null;
				}
				else
				{
					TroopRoster memberRoster = garrisonParty.MemberRoster;
					num = ((memberRoster != null) ? new int?(memberRoster.TotalHealthyCount) : null);
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 0;
				}
				return num2.GetValueOrDefault();
			}) + Clan.PlayerClan.WarPartyComponents.Sum((WarPartyComponent t) => t.MobileParty.MemberRoster.TotalHealthyCount) < this.MinimumTroopCountToCreateKingdom)
			{
				result = false;
				TextObject textObject3 = new TextObject("{=K2txLdOS}You should have at least {TROOP_COUNT} men ready to fight.", null);
				textObject3.SetTextVariable("TROOP_COUNT", this.MinimumTroopCountToCreateKingdom);
				explanations.Add(textObject3);
			}
			return result;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00069024 File Offset: 0x00067224
		public override bool IsPlayerKingdomAbdicationPossible(out List<TextObject> explanations)
		{
			explanations = new List<TextObject>();
			object obj = Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.RulingClan == Clan.PlayerClan;
			bool flag = MobileParty.MainParty.MapEvent != null || MobileParty.MainParty.SiegeEvent != null;
			object obj2 = obj;
			bool flag2 = obj2 != null && !Clan.PlayerClan.Kingdom.UnresolvedDecisions.IsEmpty<KingdomDecision>();
			if (obj2 == null)
			{
				explanations.Add(new TextObject("{=s1ERZ4ZR}You must be the king", null));
			}
			if (flag)
			{
				explanations.Add(new TextObject("{=uaMmmhRV}You must conclude your current encounter", null));
			}
			if (flag2)
			{
				explanations.Add(new TextObject("{=etKrpcHe}You must resolve pending decisions", null));
			}
			return obj2 != null && !flag && !flag2;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x000690E2 File Offset: 0x000672E2
		public override IEnumerable<CultureObject> GetAvailablePlayerKingdomCultures()
		{
			List<CultureObject> list = new List<CultureObject>();
			list.Add(Clan.PlayerClan.Culture);
			foreach (Settlement settlement in from t in Clan.PlayerClan.Settlements
			where t.IsTown || t.IsCastle
			select t)
			{
				if (!list.Contains(settlement.Culture))
				{
					list.Add(settlement.Culture);
				}
			}
			foreach (CultureObject cultureObject in list)
			{
				yield return cultureObject;
			}
			List<CultureObject>.Enumerator enumerator2 = default(List<CultureObject>.Enumerator);
			yield break;
			yield break;
		}
	}
}
