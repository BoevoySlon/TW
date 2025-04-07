using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A1 RID: 161
	public class TroopUpgradeTracker
	{
		// Token: 0x060011A5 RID: 4517 RVA: 0x00051B78 File Offset: 0x0004FD78
		public void AddTrackedTroop(PartyBase party, CharacterObject character)
		{
			if (character.IsHero)
			{
				int count = Skills.All.Count;
				int[] array = new int[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = character.GetSkillValue(Skills.All[i]);
				}
				this._heroSkills[character.HeroObject] = array;
				return;
			}
			int num = party.MemberRoster.FindIndexOfTroop(character);
			if (num >= 0)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(num);
				int value = this.CalculateReadyToUpgradeSafe(ref elementCopyAtIndex, party);
				this._upgradedRegulars[new Tuple<PartyBase, CharacterObject>(party, character)] = value;
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00051C10 File Offset: 0x0004FE10
		public IEnumerable<SkillObject> CheckSkillUpgrades(Hero hero)
		{
			if (!this._heroSkills.IsEmpty<KeyValuePair<Hero, int[]>>())
			{
				int[] oldSkillLevels = this._heroSkills[hero];
				int num;
				for (int i = 0; i < Skills.All.Count; i = num)
				{
					SkillObject skill = Skills.All[i];
					int newSkillLevel = hero.CharacterObject.GetSkillValue(skill);
					while (newSkillLevel > oldSkillLevels[i])
					{
						oldSkillLevels[i]++;
						yield return skill;
					}
					skill = null;
					num = i + 1;
				}
				oldSkillLevels = null;
			}
			yield break;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00051C28 File Offset: 0x0004FE28
		public int CheckUpgradedCount(PartyBase party, CharacterObject character)
		{
			int result = 0;
			if (!character.IsHero)
			{
				int num = party.MemberRoster.FindIndexOfTroop(character);
				int num4;
				if (num >= 0)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(num);
					int num2 = this.CalculateReadyToUpgradeSafe(ref elementCopyAtIndex, party);
					int num3;
					if (this._upgradedRegulars.TryGetValue(new Tuple<PartyBase, CharacterObject>(party, character), out num3) && num2 > num3)
					{
						num3 = MathF.Min(elementCopyAtIndex.Number, num3);
						result = num2 - num3;
						this._upgradedRegulars[new Tuple<PartyBase, CharacterObject>(party, character)] = num2;
					}
				}
				else if (this._upgradedRegulars.TryGetValue(new Tuple<PartyBase, CharacterObject>(party, character), out num4) && num4 > 0)
				{
					result = -num4;
				}
			}
			return result;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00051CD0 File Offset: 0x0004FED0
		private int CalculateReadyToUpgradeSafe(ref TroopRosterElement el, PartyBase owner)
		{
			int b = 0;
			CharacterObject character = el.Character;
			if (!character.IsHero && character.UpgradeTargets.Length != 0)
			{
				int num = 0;
				for (int i = 0; i < character.UpgradeTargets.Length; i++)
				{
					int upgradeXpCost = character.GetUpgradeXpCost(owner, i);
					if (num < upgradeXpCost)
					{
						num = upgradeXpCost;
					}
				}
				if (num > 0)
				{
					b = (el.Xp + el.DeltaXp) / num;
				}
			}
			return MathF.Max(MathF.Min(el.Number, b), 0);
		}

		// Token: 0x040005FC RID: 1532
		private Dictionary<Tuple<PartyBase, CharacterObject>, int> _upgradedRegulars = new Dictionary<Tuple<PartyBase, CharacterObject>, int>();

		// Token: 0x040005FD RID: 1533
		private Dictionary<Hero, int[]> _heroSkills = new Dictionary<Hero, int[]>();
	}
}
