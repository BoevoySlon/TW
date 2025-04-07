using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000130 RID: 304
	public class DefaultSettlementAccessModel : SettlementAccessModel
	{
		// Token: 0x0600171B RID: 5915 RVA: 0x000722DC File Offset: 0x000704DC
		public override void CanMainHeroEnterSettlement(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			if (settlement.IsFortification && Hero.MainHero.MapFaction == settlement.MapFaction && (settlement.Town.GarrisonParty == null || settlement.Town.GarrisonParty.Party.NumberOfAllMembers == 0))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (settlement.IsTown)
			{
				this.CanMainHeroEnterTown(settlement, out accessDetails);
				return;
			}
			if (settlement.IsCastle)
			{
				this.CanMainHeroEnterCastle(settlement, out accessDetails);
				return;
			}
			if (settlement.IsVillage)
			{
				this.CanMainHeroEnterVillage(settlement, out accessDetails);
				return;
			}
			Debug.FailedAssert("Invalid type of settlement", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroEnterSettlement", 41);
			accessDetails = new SettlementAccessModel.AccessDetails
			{
				AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
				AccessMethod = SettlementAccessModel.AccessMethod.Direct
			};
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x000723AF File Offset: 0x000705AF
		public override void CanMainHeroEnterDungeon(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			this.CanMainHeroEnterKeepInternal(settlement, out accessDetails);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000723C0 File Offset: 0x000705C0
		public override void CanMainHeroEnterLordsHall(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			this.CanMainHeroEnterKeepInternal(settlement, out accessDetails);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000723D4 File Offset: 0x000705D4
		private void CanMainHeroEnterKeepInternal(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			Hero mainHero = Hero.MainHero;
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
			}
			else if (FactionManager.IsAlliedWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				if (Campaign.Current.IsMainHeroDisguised)
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.Disguised
					};
				}
				else if (Campaign.Current.Models.CrimeModel.DoesPlayerHaveAnyCrimeRating(settlement.MapFaction))
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Bribe,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating
					};
				}
				else if (mainHero.Clan.Tier < 3)
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Bribe,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.ClanTier
					};
				}
				else
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
						AccessMethod = SettlementAccessModel.AccessMethod.Direct
					};
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
					LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.Disguised
				};
			}
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe || accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise) && settlement.LocationComplex.GetListOfCharactersInLocation("lordshall").IsEmpty<LocationCharacter>() && settlement.LocationComplex.GetListOfCharactersInLocation("prison").IsEmpty<LocationCharacter>())
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
				accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.LocationEmpty;
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000725D8 File Offset: 0x000707D8
		public override bool CanMainHeroAccessLocation(Settlement settlement, string locationId, out bool disableOption, out TextObject disabledText)
		{
			disabledText = TextObject.Empty;
			disableOption = false;
			bool result = true;
			if (locationId == "center")
			{
				result = this.CanMainHeroWalkAroundTownCenter(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "arena")
			{
				result = this.CanMainHeroGoToArena(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "tavern")
			{
				result = this.CanMainHeroGoToTavern(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "lordshall")
			{
				SettlementAccessModel.AccessDetails accessDetails;
				this.CanMainHeroEnterLordsHall(settlement, out accessDetails);
				if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					result = (Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(settlement) == 0);
				}
				else
				{
					result = (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess);
				}
			}
			else if (locationId == "prison")
			{
				SettlementAccessModel.AccessDetails accessDetails2;
				this.CanMainHeroEnterDungeon(settlement, out accessDetails2);
				if (accessDetails2.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails2.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					result = (Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(settlement) == 0);
				}
				else
				{
					result = (accessDetails2.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess);
				}
			}
			else if (locationId == "house_1" || locationId == "house_2" || locationId == "house_3")
			{
				Location locationWithId = settlement.LocationComplex.GetLocationWithId(locationId);
				result = (locationWithId.IsReserved && (locationWithId.SpecialItems.Count > 0 || locationWithId.GetCharacterList().Any<LocationCharacter>()));
			}
			else
			{
				Debug.FailedAssert("invalid location which is not supported by DefaultSettlementAccessModel", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroAccessLocation", 199);
			}
			return result;
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00072768 File Offset: 0x00070968
		public override bool IsRequestMeetingOptionAvailable(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			bool result = true;
			disableOption = false;
			disabledText = TextObject.Empty;
			SettlementAccessModel.AccessDetails accessDetails;
			this.CanMainHeroEnterSettlement(settlement, out accessDetails);
			if (settlement.OwnerClan == Clan.PlayerClan)
			{
				result = false;
			}
			else if (FactionManager.IsAlliedWithFaction(settlement.MapFaction, Clan.PlayerClan.MapFaction) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				result = TownHelpers.IsThereAnyoneToMeetInTown(settlement);
			}
			else if (settlement.IsTown && FactionManager.IsNeutralWithFaction(Hero.MainHero.MapFaction, settlement.MapFaction) && Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingMild(settlement.MapFaction))
			{
				result = false;
			}
			else if (Clan.PlayerClan.Tier < 3)
			{
				disableOption = true;
				disabledText = new TextObject("{=bdzZUVxf}Your clan tier is not high enough to request a meeting.", null);
				result = true;
			}
			else if (TownHelpers.IsThereAnyoneToMeetInTown(settlement))
			{
				result = true;
			}
			else
			{
				disableOption = true;
				disabledText = new TextObject("{=196tGVIm}There are no nobles to meet.", null);
			}
			return result;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00072844 File Offset: 0x00070A44
		public override bool CanMainHeroDoSettlementAction(Settlement settlement, SettlementAccessModel.SettlementAction settlementAction, out bool disableOption, out TextObject disabledText)
		{
			switch (settlementAction)
			{
			case SettlementAccessModel.SettlementAction.RecruitTroops:
				return this.CanMainHeroRecruitTroops(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.Craft:
				return this.CanMainHeroCraft(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WalkAroundTheArena:
				return this.CanMainHeroEnterArena(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.JoinTournament:
				return this.CanMainHeroJoinTournament(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WatchTournament:
				return this.CanMainHeroWatchTournament(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.Trade:
				return this.CanMainHeroTrade(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WaitInSettlement:
				return this.CanMainHeroWaitInSettlement(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.ManageTown:
				return this.CanMainHeroManageTown(settlement, out disableOption, out disabledText);
			default:
				Debug.FailedAssert("Invalid Settlement Action", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroDoSettlementAction", 268);
				disableOption = false;
				disabledText = TextObject.Empty;
				return true;
			}
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000728F8 File Offset: 0x00070AF8
		private bool CanMainHeroGoToArena(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disabledText = new TextObject("{=brzz79Je}You cannot enter arena while in disguise.", null);
				disableOption = true;
				return false;
			}
			if (Campaign.Current.IsDay)
			{
				disabledText = TextObject.Empty;
				disableOption = false;
				return true;
			}
			disabledText = new TextObject("{=wsbkjJhz}Arena is closed at night.", null);
			disableOption = true;
			return false;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0007294C File Offset: 0x00070B4C
		private bool CanMainHeroGoToTavern(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = TextObject.Empty;
			disableOption = false;
			return true;
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00072959 File Offset: 0x00070B59
		private bool CanMainHeroEnterArena(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = TextObject.Empty;
			return true;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00072968 File Offset: 0x00070B68
		private void CanMainHeroEnterVillage(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = new SettlementAccessModel.AccessDetails
			{
				AccessLevel = SettlementAccessModel.AccessLevel.NoAccess,
				AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.None,
				PreliminaryActionObligation = SettlementAccessModel.PreliminaryActionObligation.None,
				PreliminaryActionType = SettlementAccessModel.PreliminaryActionType.None
			};
			MobileParty partyBelongedTo = mainHero.PartyBelongedTo;
			if (partyBelongedTo != null && (partyBelongedTo.Army == null || partyBelongedTo.Army.LeaderParty == partyBelongedTo))
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.FullAccess;
				accessDetails.AccessMethod = SettlementAccessModel.AccessMethod.Direct;
			}
			if (settlement.Village.VillageState == Village.VillageStates.Looted)
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
				accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.VillageIsLooted;
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x000729F2 File Offset: 0x00070BF2
		private bool CanMainHeroManageTown(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = TextObject.Empty;
			disableOption = false;
			return settlement.IsTown && settlement.OwnerClan.Leader == Hero.MainHero;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00072A1C File Offset: 0x00070C1C
		private void CanMainHeroEnterCastle(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = default(SettlementAccessModel.AccessDetails);
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (FactionManager.IsAlliedWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest
				};
				if (!settlement.Town.IsOwnerUnassigned && settlement.OwnerClan.Leader.GetRelationWithPlayer() < -4f && Hero.MainHero.MapFaction.Leader != Hero.MainHero)
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.RelationshipWithOwner;
					return;
				}
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest
				};
				if (Campaign.Current.Models.CrimeModel.DoesPlayerHaveAnyCrimeRating(settlement.MapFaction))
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating;
					return;
				}
				if (settlement.OwnerClan.Leader.GetRelationWithPlayer() < 0f)
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.RelationshipWithOwner;
					return;
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.NoAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.HostileFaction
				};
			}
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00072BA8 File Offset: 0x00070DA8
		private void CanMainHeroEnterTown(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = default(SettlementAccessModel.AccessDetails);
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (FactionManager.IsAlliedWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(settlement.MapFaction) || Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(settlement.MapFaction))
				{
					accessDetails.PreliminaryActionType = SettlementAccessModel.PreliminaryActionType.FaceCharges;
					accessDetails.PreliminaryActionObligation = SettlementAccessModel.PreliminaryActionObligation.Optional;
					return;
				}
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(settlement.MapFaction) || Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(settlement.MapFaction))
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess;
					accessDetails.AccessMethod = SettlementAccessModel.AccessMethod.None;
					accessDetails.LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating;
					return;
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
					LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.HostileFaction
				};
			}
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00072D2D File Offset: 0x00070F2D
		private bool CanMainHeroWalkAroundTownCenter(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = TextObject.Empty;
			disableOption = false;
			return settlement.IsTown || settlement.IsCastle;
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00072D49 File Offset: 0x00070F49
		private bool CanMainHeroRecruitTroops(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = TextObject.Empty;
			disableOption = false;
			return true;
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00072D56 File Offset: 0x00070F56
		private bool CanMainHeroCraft(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = TextObject.Empty;
			return Campaign.Current.IsCraftingEnabled;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00072D6C File Offset: 0x00070F6C
		private bool CanMainHeroJoinTournament(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			bool flag = settlement.Town.HasTournament && Campaign.Current.IsDay;
			disableOption = false;
			disabledText = TextObject.Empty;
			if (!flag)
			{
				return false;
			}
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disableOption = true;
				disabledText = new TextObject("{=mu6Xl4RS}You cannot enter the tournament while disguised.", null);
				return false;
			}
			if (Hero.MainHero.IsWounded)
			{
				disableOption = true;
				disabledText = new TextObject("{=68rmPu7Z}Your health is too low to fight.", null);
				return false;
			}
			return true;
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00072DDE File Offset: 0x00070FDE
		private bool CanMainHeroWatchTournament(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = TextObject.Empty;
			return settlement.Town.HasTournament && Campaign.Current.IsDay;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00072E03 File Offset: 0x00071003
		private bool CanMainHeroTrade(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disableOption = true;
				disabledText = new TextObject("{=shU7OlQT}You cannot trade while in disguise.", null);
				return false;
			}
			disableOption = false;
			disabledText = TextObject.Empty;
			return true;
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00072E30 File Offset: 0x00071030
		private bool CanMainHeroWaitInSettlement(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = TextObject.Empty;
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disableOption = true;
				disabledText = new TextObject("{=dN5Qc9vN}You cannot wait in town while disguised.", null);
				return false;
			}
			if (settlement.IsVillage && settlement.Party.MapEvent != null)
			{
				disableOption = true;
				disabledText = new TextObject("{=dN5Qc7vN}You cannot wait in village while it is being raided.", null);
				return false;
			}
			return MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
		}
	}
}
