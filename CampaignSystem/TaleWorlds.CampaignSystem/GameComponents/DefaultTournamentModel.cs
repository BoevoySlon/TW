using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000141 RID: 321
	public class DefaultTournamentModel : TournamentModel
	{
		// Token: 0x06001818 RID: 6168 RVA: 0x0007A103 File Offset: 0x00078303
		public override TournamentGame CreateTournament(Town town)
		{
			return new FightTournamentGame(town);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0007A10C File Offset: 0x0007830C
		public override float GetTournamentStartChance(Town town)
		{
			if (town.Settlement.SiegeEvent != null)
			{
				return 0f;
			}
			if (Math.Abs(town.StringId.GetHashCode() % 3) != CampaignTime.Now.GetWeekOfSeason)
			{
				return 0f;
			}
			return 0.1f * (float)(town.Settlement.Parties.Count((MobileParty x) => x.IsLordParty) + town.Settlement.HeroesWithoutParty.Count((Hero x) => this.SuitableForTournament(x))) - 0.2f;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0007A1AC File Offset: 0x000783AC
		public override int GetNumLeaderboardVictoriesAtGameStart()
		{
			return 500;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0007A1B4 File Offset: 0x000783B4
		public override float GetTournamentEndChance(TournamentGame tournament)
		{
			float elapsedDaysUntilNow = tournament.CreationTime.ElapsedDaysUntilNow;
			return MathF.Max(0f, (elapsedDaysUntilNow - 10f) * 0.05f);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0007A1E7 File Offset: 0x000783E7
		private bool SuitableForTournament(Hero hero)
		{
			return hero.Age >= 18f && MathF.Max(hero.GetSkillValue(DefaultSkills.OneHanded), hero.GetSkillValue(DefaultSkills.TwoHanded)) > 100;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0007A218 File Offset: 0x00078418
		public override float GetTournamentSimulationScore(CharacterObject character)
		{
			return (character.IsHero ? 1f : 0.4f) * (MathF.Max((float)character.GetSkillValue(DefaultSkills.OneHanded), (float)character.GetSkillValue(DefaultSkills.TwoHanded), (float)character.GetSkillValue(DefaultSkills.Polearm)) + (float)character.GetSkillValue(DefaultSkills.Athletics) + (float)character.GetSkillValue(DefaultSkills.Riding)) * 0.01f;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0007A284 File Offset: 0x00078484
		public override int GetRenownReward(Hero winner, Town town)
		{
			float num = 3f;
			if (winner.GetPerkValue(DefaultPerks.OneHanded.Duelist))
			{
				num *= DefaultPerks.OneHanded.Duelist.SecondaryBonus;
			}
			if (winner.GetPerkValue(DefaultPerks.Charm.SelfPromoter))
			{
				num += DefaultPerks.Charm.SelfPromoter.PrimaryBonus;
			}
			return MathF.Round(num);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0007A2D1 File Offset: 0x000784D1
		public override int GetInfluenceReward(Hero winner, Town town)
		{
			return 0;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0007A2D4 File Offset: 0x000784D4
		[return: TupleElementNames(new string[]
		{
			"skill",
			"xp"
		})]
		public override ValueTuple<SkillObject, int> GetSkillXpGainFromTournament(Town town)
		{
			float randomFloat = MBRandom.RandomFloat;
			SkillObject item = (randomFloat < 0.2f) ? DefaultSkills.OneHanded : ((randomFloat < 0.4f) ? DefaultSkills.TwoHanded : ((randomFloat < 0.6f) ? DefaultSkills.Polearm : ((randomFloat < 0.8f) ? DefaultSkills.Riding : DefaultSkills.Athletics)));
			int item2 = 500;
			return new ValueTuple<SkillObject, int>(item, item2);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0007A334 File Offset: 0x00078534
		public override Equipment GetParticipantArmor(CharacterObject participant)
		{
			if (CampaignMission.Current != null && CampaignMission.Current.Mode != MissionMode.Tournament && Settlement.CurrentSettlement != null)
			{
				return (Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_" + Settlement.CurrentSettlement.MapFaction.Culture.StringId) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_empire")).RandomBattleEquipment;
			}
			return participant.RandomBattleEquipment;
		}
	}
}
