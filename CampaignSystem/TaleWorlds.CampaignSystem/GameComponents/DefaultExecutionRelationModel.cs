using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000107 RID: 263
	public class DefaultExecutionRelationModel : ExecutionRelationModel
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00067D04 File Offset: 0x00065F04
		public override int HeroKillingHeroClanRelationPenalty
		{
			get
			{
				return -40;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x00067D08 File Offset: 0x00065F08
		public override int HeroKillingHeroFriendRelationPenalty
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00067D0C File Offset: 0x00065F0C
		public override int PlayerExecutingHeroFactionRelationPenaltyDishonorable
		{
			get
			{
				return -5;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x00067D10 File Offset: 0x00065F10
		public override int PlayerExecutingHeroClanRelationPenaltyDishonorable
		{
			get
			{
				return -30;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00067D14 File Offset: 0x00065F14
		public override int PlayerExecutingHeroFriendRelationPenaltyDishonorable
		{
			get
			{
				return -15;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x00067D18 File Offset: 0x00065F18
		public override int PlayerExecutingHeroHonorPenalty
		{
			get
			{
				return -1000;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00067D1F File Offset: 0x00065F1F
		public override int PlayerExecutingHeroFactionRelationPenalty
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x00067D23 File Offset: 0x00065F23
		public override int PlayerExecutingHeroHonorableNobleRelationPenalty
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x00067D27 File Offset: 0x00065F27
		public override int PlayerExecutingHeroClanRelationPenalty
		{
			get
			{
				return -60;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x00067D2B File Offset: 0x00065F2B
		public override int PlayerExecutingHeroFriendRelationPenalty
		{
			get
			{
				return -30;
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00067D30 File Offset: 0x00065F30
		public override int GetRelationChangeForExecutingHero(Hero victim, Hero hero, out bool showQuickNotification)
		{
			int result = 0;
			showQuickNotification = false;
			if (victim.GetTraitLevel(DefaultTraits.Honor) < 0)
			{
				if (!hero.IsHumanPlayerCharacter && hero != victim && hero.Clan != null && hero.Clan.Leader == hero)
				{
					if (hero.Clan == victim.Clan)
					{
						result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroClanRelationPenaltyDishonorable;
						showQuickNotification = true;
					}
					else if (victim.IsFriend(hero))
					{
						result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroFriendRelationPenaltyDishonorable;
						showQuickNotification = true;
					}
					else if (hero.MapFaction == victim.MapFaction && hero.CharacterObject.Occupation == Occupation.Lord)
					{
						result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroFactionRelationPenaltyDishonorable;
						showQuickNotification = true;
					}
				}
			}
			else if (!hero.IsHumanPlayerCharacter && hero != victim && hero.Clan != null && hero.Clan.Leader == hero)
			{
				if (hero.Clan == victim.Clan)
				{
					result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroClanRelationPenalty;
					showQuickNotification = true;
				}
				else if (victim.IsFriend(hero))
				{
					result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroFriendRelationPenalty;
					showQuickNotification = true;
				}
				else if (hero.MapFaction == victim.MapFaction && hero.CharacterObject.Occupation == Occupation.Lord)
				{
					result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroFactionRelationPenalty;
					showQuickNotification = false;
				}
				else if (hero.GetTraitLevel(DefaultTraits.Honor) > 0 && !victim.Clan.IsRebelClan)
				{
					result = Campaign.Current.Models.ExecutionRelationModel.PlayerExecutingHeroHonorableNobleRelationPenalty;
					showQuickNotification = true;
				}
			}
			return result;
		}
	}
}
