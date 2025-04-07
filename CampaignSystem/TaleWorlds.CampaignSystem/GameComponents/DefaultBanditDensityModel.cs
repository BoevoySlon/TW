using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EB RID: 235
	public class DefaultBanditDensityModel : BanditDensityModel
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0005C699 File Offset: 0x0005A899
		public override int NumberOfMaximumLooterParties
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0005C6A0 File Offset: 0x0005A8A0
		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0005C6A3 File Offset: 0x0005A8A3
		public override int NumberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0005C6A6 File Offset: 0x0005A8A6
		public override int NumberOfMaximumBanditPartiesAroundEachHideout
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0005C6A9 File Offset: 0x0005A8A9
		public override int NumberOfMaximumHideoutsAtEachBanditFaction
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0005C6AD File Offset: 0x0005A8AD
		public override int NumberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0005C6B0 File Offset: 0x0005A8B0
		public override int NumberOfMinimumBanditTroopsInHideoutMission
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0005C6B4 File Offset: 0x0005A8B4
		public override int NumberOfMaximumTroopCountForFirstFightInHideout
		{
			get
			{
				return MathF.Floor(6f * (2f + Campaign.Current.PlayerProgress));
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0005C6D1 File Offset: 0x0005A8D1
		public override int NumberOfMaximumTroopCountForBossFightInHideout
		{
			get
			{
				return MathF.Floor(1f + 5f * (1f + Campaign.Current.PlayerProgress));
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0005C6F4 File Offset: 0x0005A8F4
		public override float SpawnPercentageForFirstFightInHideoutMission
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0005C6FC File Offset: 0x0005A8FC
		public override int GetPlayerMaximumTroopCountForHideoutMission(MobileParty party)
		{
			float num = 10f;
			if (party.HasPerk(DefaultPerks.Tactics.SmallUnitTactics, false))
			{
				num += DefaultPerks.Tactics.SmallUnitTactics.PrimaryBonus;
			}
			return MathF.Round(num);
		}
	}
}
