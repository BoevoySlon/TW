using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000ED RID: 237
	public class DefaultBarterModel : BarterModel
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0005C844 File Offset: 0x0005AA44
		public override int BarterCooldownWithHeroInDays
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0005C847 File Offset: 0x0005AA47
		private int MaximumOverpayRelationBonus
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0005C84A File Offset: 0x0005AA4A
		public override float MaximumPercentageOfNpcGoldToSpendAtBarter
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0005C854 File Offset: 0x0005AA54
		public override int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount)
		{
			int num = (int)hero.GetRelationWithPlayer();
			float num2 = MathF.Clamp((float)(num + this.MaximumOverpayRelationBonus), -100f, 100f);
			float num3 = 0f;
			int num4 = num;
			while ((float)num4 < num2)
			{
				int num5 = 1000 + 100 * (num4 * num4);
				if (overpayAmount >= (float)num5)
				{
					overpayAmount -= (float)num5;
					num3 += 1f;
					num4++;
				}
				else
				{
					if (MBRandom.RandomFloat <= overpayAmount / (float)num5)
					{
						num3 += 1f;
						break;
					}
					break;
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.Tribute))
			{
				num3 *= 1f + DefaultPerks.Charm.Tribute.PrimaryBonus;
			}
			return MathF.Ceiling(num3);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0005C8F4 File Offset: 0x0005AAF4
		public override ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty)
		{
			ExplainedNumber result;
			if (faction == ((otherHero != null) ? otherHero.Clan : null) || faction == ((otherHero != null) ? otherHero.MapFaction : null) || faction == ((otherParty != null) ? otherParty.MapFaction : null))
			{
				result = new ExplainedNumber(0.4f, false, null);
				if (otherHero != null && itemBarterable.OriginalOwner != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture = otherHero.Culture;
					Hero originalOwner = itemBarterable.OriginalOwner;
					if (culture == ((originalOwner != null) ? originalOwner.Culture : null))
					{
						if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							result.AddFactor(-DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						result.AddFactor(-DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						result.AddFactor(-DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			else
			{
				Hero originalOwner2 = itemBarterable.OriginalOwner;
				if (faction != ((originalOwner2 != null) ? originalOwner2.Clan : null))
				{
					Hero originalOwner3 = itemBarterable.OriginalOwner;
					if (faction != ((originalOwner3 != null) ? originalOwner3.MapFaction : null))
					{
						PartyBase originalParty = itemBarterable.OriginalParty;
						if (faction != ((originalParty != null) ? originalParty.MapFaction : null))
						{
							result = new ExplainedNumber(0f, false, null);
							return result;
						}
					}
				}
				if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsAnimal || itemBarterable.ItemRosterElement.EquipmentElement.Item.IsMountable)
				{
					result = new ExplainedNumber(-8.4f, false, null);
				}
				else if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsFood)
				{
					result = new ExplainedNumber(-12.6f, false, null);
				}
				else
				{
					result = new ExplainedNumber(-2.1f, false, null);
				}
				if (otherHero != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture2 = otherHero.Culture;
					Hero originalOwner4 = itemBarterable.OriginalOwner;
					if (culture2 == ((originalOwner4 != null) ? originalOwner4.Culture : null))
					{
						if (otherHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							result.AddFactor(DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (otherHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						result.AddFactor(DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (otherHero.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						result.AddFactor(DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			return result;
		}
	}
}
