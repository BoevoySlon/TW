using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x020003F8 RID: 1016
	public class DiplomaticBartersBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EBB RID: 16059 RVA: 0x00133D13 File Offset: 0x00131F13
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00133D2C File Offset: 0x00131F2C
		private void DailyTickClan(Clan clan)
		{
			bool flag = false;
			using (List<WarPartyComponent>.Enumerator enumerator = clan.WarPartyComponents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.MobileParty.MapEvent != null)
					{
						flag = true;
						break;
					}
				}
			}
			MBList<Clan> e = Clan.NonBanditFactions.ToMBList<Clan>();
			if (clan == Clan.PlayerClan || clan.TotalStrength <= 0f || clan.IsEliminated)
			{
				return;
			}
			if (clan.IsBanditFaction || clan.IsRebelClan)
			{
				return;
			}
			if (clan.Kingdom == null && MBRandom.RandomFloat < 0.5f)
			{
				Clan randomElement = e.GetRandomElement<Clan>();
				if (randomElement.Kingdom == null && randomElement != Clan.PlayerClan && clan.IsAtWarWith(randomElement) && !clan.IsMinorFaction && !randomElement.IsMinorFaction)
				{
					this.ConsiderPeace(clan, randomElement);
					return;
				}
			}
			else if (MBRandom.RandomFloat < 0.2f && !clan.IsUnderMercenaryService && clan.Kingdom != null && !clan.IsClanTypeMercenary)
			{
				if (MBRandom.RandomFloat < 0.1f)
				{
					Clan randomElement2 = e.GetRandomElement<Clan>();
					int num = 0;
					while (randomElement2.Kingdom == null || clan.Kingdom == randomElement2.Kingdom || randomElement2.IsEliminated)
					{
						randomElement2 = e.GetRandomElement<Clan>();
						num++;
						if (num >= 20)
						{
							break;
						}
					}
					if (randomElement2.Kingdom != null && clan.Kingdom != randomElement2.Kingdom && !clan.GetStanceWith(randomElement2.Kingdom).IsAtConstantWar && !flag && randomElement2.MapFaction.IsKingdomFaction && !randomElement2.IsEliminated && randomElement2 != Clan.PlayerClan && randomElement2.MapFaction.Leader != Hero.MainHero)
					{
						if (clan.WarPartyComponents.All((WarPartyComponent x) => x.MobileParty.MapEvent == null))
						{
							this.ConsiderDefection(clan, randomElement2.MapFaction as Kingdom);
							return;
						}
					}
				}
			}
			else if (MBRandom.RandomFloat < ((clan.MapFaction.Leader == Hero.MainHero) ? 0.2f : 0.4f))
			{
				Kingdom kingdom = Kingdom.All[MBRandom.RandomInt(Kingdom.All.Count)];
				int num2 = 0;
				using (List<Kingdom>.Enumerator enumerator2 = Kingdom.All.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Culture == clan.Culture)
						{
							num2 += 10;
						}
						else
						{
							num2++;
						}
					}
				}
				int num3 = (int)(MBRandom.RandomFloat * (float)num2);
				foreach (Kingdom kingdom2 in Kingdom.All)
				{
					if (kingdom2.Culture == clan.Culture)
					{
						num3 -= 10;
					}
					else
					{
						num3--;
					}
					if (num3 < 0)
					{
						kingdom = kingdom2;
						break;
					}
				}
				if (kingdom.Leader != Hero.MainHero && !kingdom.IsEliminated && (clan.Kingdom == null || clan.IsUnderMercenaryService) && clan.MapFaction != kingdom && !clan.MapFaction.IsAtWarWith(kingdom) && !clan.GetStanceWith(kingdom).IsAtConstantWar)
				{
					if (clan.WarPartyComponents.All((WarPartyComponent x) => x.MobileParty.MapEvent == null))
					{
						bool flag2 = true;
						if (!clan.IsMinorFaction)
						{
							foreach (Kingdom kingdom3 in Kingdom.All)
							{
								if (kingdom3 != kingdom && clan.IsAtWarWith(kingdom3) && !kingdom3.IsAtWarWith(kingdom) && kingdom.TotalStrength <= 10f * kingdom3.TotalStrength)
								{
									flag2 = false;
									break;
								}
							}
						}
						if (flag2)
						{
							if (clan.IsMinorFaction)
							{
								this.ConsiderClanJoinAsMercenary(clan, kingdom);
								return;
							}
							this.ConsiderClanJoin(clan, kingdom);
							return;
						}
					}
				}
			}
			else if (MBRandom.RandomFloat < 0.4f)
			{
				if (clan.Kingdom != null && !flag && clan.Kingdom.RulingClan != clan && clan != Clan.PlayerClan)
				{
					if (clan.WarPartyComponents.All((WarPartyComponent x) => x.MobileParty.MapEvent == null))
					{
						if (clan.IsMinorFaction)
						{
							this.ConsiderClanLeaveAsMercenary(clan);
							return;
						}
						this.ConsiderClanLeaveKingdom(clan);
						return;
					}
				}
			}
			else if (MBRandom.RandomFloat < 0.7f)
			{
				Clan randomElement3 = e.GetRandomElement<Clan>();
				IFaction mapFaction = randomElement3.MapFaction;
				if (!clan.IsMinorFaction && (!mapFaction.IsMinorFaction || mapFaction == Clan.PlayerClan) && clan.Kingdom == null && randomElement3 != clan && !mapFaction.IsEliminated && mapFaction.WarPartyComponents.Count > 0 && clan.WarPartyComponents.Count > 0 && !clan.IsAtWarWith(mapFaction) && clan != Clan.PlayerClan)
				{
					this.ConsiderWar(clan, mapFaction);
				}
			}
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x001342A0 File Offset: 0x001324A0
		private void ConsiderClanLeaveKingdom(Clan clan)
		{
			LeaveKingdomAsClanBarterable leaveKingdomAsClanBarterable = new LeaveKingdomAsClanBarterable(clan.Leader, null);
			if (leaveKingdomAsClanBarterable.GetValueForFaction(clan) > 0)
			{
				leaveKingdomAsClanBarterable.Apply();
			}
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x001342CC File Offset: 0x001324CC
		private void ConsiderClanLeaveAsMercenary(Clan clan)
		{
			LeaveKingdomAsClanBarterable leaveKingdomAsClanBarterable = new LeaveKingdomAsClanBarterable(clan.Leader, null);
			if (leaveKingdomAsClanBarterable.GetValueForFaction(clan) > 500)
			{
				leaveKingdomAsClanBarterable.Apply();
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x001342FC File Offset: 0x001324FC
		private void ConsiderClanJoin(Clan clan, Kingdom kingdom)
		{
			JoinKingdomAsClanBarterable joinKingdomAsClanBarterable = new JoinKingdomAsClanBarterable(clan.Leader, kingdom, false);
			if (joinKingdomAsClanBarterable.GetValueForFaction(clan) + joinKingdomAsClanBarterable.GetValueForFaction(kingdom) > 0)
			{
				Campaign.Current.BarterManager.ExecuteAiBarter(clan, kingdom, clan.Leader, kingdom.Leader, joinKingdomAsClanBarterable);
			}
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x00134348 File Offset: 0x00132548
		private void ConsiderClanJoinAsMercenary(Clan clan, Kingdom kingdom)
		{
			MercenaryJoinKingdomBarterable mercenaryJoinKingdomBarterable = new MercenaryJoinKingdomBarterable(clan.Leader, null, kingdom);
			if (mercenaryJoinKingdomBarterable.GetValueForFaction(clan) + mercenaryJoinKingdomBarterable.GetValueForFaction(kingdom) > 0)
			{
				Campaign.Current.BarterManager.ExecuteAiBarter(clan, kingdom, clan.Leader, kingdom.Leader, mercenaryJoinKingdomBarterable);
			}
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x00134394 File Offset: 0x00132594
		private void ConsiderDefection(Clan clan1, Kingdom kingdom)
		{
			JoinKingdomAsClanBarterable joinKingdomAsClanBarterable = new JoinKingdomAsClanBarterable(clan1.Leader, kingdom, false);
			int valueForFaction = joinKingdomAsClanBarterable.GetValueForFaction(clan1);
			int valueForFaction2 = joinKingdomAsClanBarterable.GetValueForFaction(kingdom);
			int num = valueForFaction + valueForFaction2;
			int num2 = 0;
			if (valueForFaction < 0)
			{
				num2 = -valueForFaction;
			}
			if (num > 0 && (float)num2 <= (float)kingdom.Leader.Gold * 0.5f)
			{
				clan1.Leader.GetRelation(clan1.MapFaction.Leader);
				clan1.Leader.GetRelation(kingdom.Leader);
				Campaign.Current.BarterManager.ExecuteAiBarter(clan1, kingdom, clan1.Leader, kingdom.Leader, joinKingdomAsClanBarterable);
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0013442C File Offset: 0x0013262C
		private void ConsiderPeace(Clan clan1, Clan clan2)
		{
			PeaceBarterable peaceBarterable = new PeaceBarterable(clan1.Leader, clan1.MapFaction, clan2.MapFaction, CampaignTime.Years(1f));
			if (peaceBarterable.GetValueForFaction(clan1) + peaceBarterable.GetValueForFaction(clan2) > 0)
			{
				Campaign.Current.BarterManager.ExecuteAiBarter(clan1, clan2, clan1.Leader, clan2.Leader, peaceBarterable);
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0013448C File Offset: 0x0013268C
		private void ConsiderWar(Clan clan, IFaction otherMapFaction)
		{
			DeclareWarBarterable declareWarBarterable = new DeclareWarBarterable(clan, otherMapFaction);
			if (declareWarBarterable.GetValueForFaction(clan) > 1000)
			{
				declareWarBarterable.Apply();
			}
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x001344B5 File Offset: 0x001326B5
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
