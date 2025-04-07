using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000115 RID: 277
	public class DefaultMapVisibilityModel : MapVisibilityModel
	{
		// Token: 0x0600162E RID: 5678 RVA: 0x0006A514 File Offset: 0x00068714
		public override ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false)
		{
			float baseNumber = Campaign.Current.IsNight ? 6f : 12f;
			ExplainedNumber result = new ExplainedNumber(baseNumber, includeDescriptions, null);
			TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
			SkillHelper.AddSkillBonusForParty(DefaultSkills.Scouting, DefaultSkillEffects.TrackingSpottingDistance, party, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.EagleEye, party, false, ref result);
			Hero effectiveScout = party.EffectiveScout;
			if (effectiveScout != null)
			{
				if ((faceTerrainType == TerrainType.Plain || faceTerrainType == TerrainType.Steppe) && effectiveScout.GetPerkValue(DefaultPerks.Scouting.WaterDiviner))
				{
					result.AddFactor(DefaultPerks.Scouting.WaterDiviner.PrimaryBonus, DefaultPerks.Scouting.WaterDiviner.Name);
				}
				else if (faceTerrainType == TerrainType.Forest && PartyBaseHelper.HasFeat(party.Party, DefaultCulturalFeats.BattanianForestSpeedFeat))
				{
					result.AddFactor(0.15f, GameTexts.FindText("str_culture", null));
				}
				if (Campaign.Current.IsNight && effectiveScout.GetPerkValue(DefaultPerks.Scouting.NightRunner))
				{
					result.AddFactor(DefaultPerks.Scouting.NightRunner.SecondaryBonus, DefaultPerks.Scouting.NightRunner.Name);
				}
				else if (effectiveScout.GetPerkValue(DefaultPerks.Scouting.DayTraveler))
				{
					result.AddFactor(DefaultPerks.Scouting.DayTraveler.SecondaryBonus, DefaultPerks.Scouting.DayTraveler.Name);
				}
				if (!party.IsMoving && party.StationaryStartTime.ElapsedHoursUntilNow >= 1f && effectiveScout.GetPerkValue(DefaultPerks.Scouting.VantagePoint))
				{
					result.AddFactor(DefaultPerks.Scouting.VantagePoint.PrimaryBonus, DefaultPerks.Scouting.VantagePoint.Name);
				}
				if (effectiveScout.GetPerkValue(DefaultPerks.Scouting.MountedScouts))
				{
					float num = 0f;
					for (int i = 0; i < party.MemberRoster.Count; i++)
					{
						if (party.MemberRoster.GetCharacterAtIndex(i).DefaultFormationClass.Equals(FormationClass.Cavalry))
						{
							num += (float)party.MemberRoster.GetElementNumber(i);
						}
					}
					if (num / (float)party.MemberRoster.TotalManCount >= 0.5f)
					{
						result.AddFactor(DefaultPerks.Scouting.MountedScouts.PrimaryBonus, DefaultPerks.Scouting.MountedScouts.Name);
					}
				}
			}
			return result;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0006A72A File Offset: 0x0006892A
		public override float GetPartyRelativeInspectionRange(IMapPoint party)
		{
			return 0.5f;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0006A734 File Offset: 0x00068934
		public override float GetPartySpottingDifficulty(MobileParty spottingParty, MobileParty party)
		{
			float num = 1f;
			if (party != null && spottingParty != null && Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace) == TerrainType.Forest)
			{
				float num2 = 0.3f;
				if (spottingParty.HasPerk(DefaultPerks.Scouting.KeenSight, false))
				{
					num2 += num2 * DefaultPerks.Scouting.KeenSight.PrimaryBonus;
				}
				num += num2;
			}
			return (1f / MathF.Pow((float)(party.Party.NumberOfAllMembers + party.Party.NumberOfPrisoners + 2) * 0.2f, 0.6f) + 0.94f) * num;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0006A7C8 File Offset: 0x000689C8
		public override float GetHideoutSpottingDistance()
		{
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Scouting.RumourNetwork, true))
			{
				return MobileParty.MainParty.SeeingRange * 1.2f * (1f + DefaultPerks.Scouting.RumourNetwork.SecondaryBonus);
			}
			return MobileParty.MainParty.SeeingRange * 1.2f;
		}

		// Token: 0x0400079C RID: 1948
		private const float PartySpottingDifficultyInForests = 0.3f;
	}
}
