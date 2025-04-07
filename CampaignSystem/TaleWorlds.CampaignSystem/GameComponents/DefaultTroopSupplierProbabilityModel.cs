using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000144 RID: 324
	public class DefaultTroopSupplierProbabilityModel : TroopSupplierProbabilityModel
	{
		// Token: 0x06001831 RID: 6193 RVA: 0x0007ADE4 File Offset: 0x00078FE4
		public override void EnqueueTroopSpawnProbabilitiesAccordingToUnitSpawnPrioritization(MapEventParty battleParty, FlattenedTroopRoster priorityTroops, bool includePlayer, int sizeOfSide, bool forcePriorityTroops, List<ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>> priorityList)
		{
			UnitSpawnPrioritizations unitSpawnPrioritizations = UnitSpawnPrioritizations.HighLevel;
			MapEvent battle = PlayerEncounter.Battle;
			bool flag = battle != null && battle.IsSiegeAmbush;
			if (battleParty.Party == PartyBase.MainParty && !flag)
			{
				unitSpawnPrioritizations = Game.Current.UnitSpawnPrioritization;
			}
			if (unitSpawnPrioritizations != UnitSpawnPrioritizations.Default && !forcePriorityTroops)
			{
				StackArray.StackArray8Int stackArray8Int = default(StackArray.StackArray8Int);
				int num = 0;
				foreach (FlattenedTroopRosterElement troopRoster in battleParty.Troops)
				{
					if (this.CanTroopJoinBattle(troopRoster, includePlayer))
					{
						int num2 = (int)troopRoster.Troop.DefaultFormationClass;
						int num3 = stackArray8Int[num2];
						stackArray8Int[num2] = num3 + 1;
						num++;
					}
				}
				StackArray.StackArray8Int stackArray8Int2 = default(StackArray.StackArray8Int);
				float num4 = 1000f;
				using (IEnumerator<FlattenedTroopRosterElement> enumerator = battleParty.Troops.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement = enumerator.Current;
						if (this.CanTroopJoinBattle(flattenedTroopRosterElement, includePlayer))
						{
							CharacterObject troop = flattenedTroopRosterElement.Troop;
							FormationClass formationClass = troop.GetFormationClass();
							float num6;
							if (priorityTroops != null && this.IsPriorityTroop(flattenedTroopRosterElement, priorityTroops))
							{
								float num5 = num4;
								num4 = num5 - 1f;
								num6 = num5;
							}
							else
							{
								float num7 = (float)stackArray8Int[(int)formationClass] / (float)((unitSpawnPrioritizations == UnitSpawnPrioritizations.Homogeneous) ? (stackArray8Int2[(int)formationClass] + 1) : num);
								float num8;
								if (!troop.IsHero)
								{
									num8 = num7;
								}
								else
								{
									num4 = (num8 = num4) - 1f;
								}
								num6 = num8;
								if (!troop.IsHero && (unitSpawnPrioritizations == UnitSpawnPrioritizations.HighLevel || unitSpawnPrioritizations == UnitSpawnPrioritizations.LowLevel))
								{
									num6 += (float)troop.Level;
									if (unitSpawnPrioritizations == UnitSpawnPrioritizations.LowLevel)
									{
										num6 *= -1f;
									}
								}
							}
							int num3 = (int)formationClass;
							int num2 = stackArray8Int[num3];
							stackArray8Int[num3] = num2 - 1;
							num2 = (int)formationClass;
							num3 = stackArray8Int2[num2];
							stackArray8Int2[num2] = num3 + 1;
							priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(flattenedTroopRosterElement, battleParty, num6));
						}
					}
					return;
				}
			}
			int numberOfHealthyMembers = battleParty.Party.NumberOfHealthyMembers;
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement2 in battleParty.Troops)
			{
				if (this.CanTroopJoinBattle(flattenedTroopRosterElement2, includePlayer))
				{
					float num9 = 1f;
					if (flattenedTroopRosterElement2.Troop.IsHero)
					{
						num9 *= 150f;
						if (priorityTroops != null)
						{
							UniqueTroopDescriptor descriptor = priorityTroops.FindIndexOfCharacter(flattenedTroopRosterElement2.Troop);
							if (descriptor.IsValid)
							{
								num9 *= 100f;
								priorityTroops.Remove(descriptor);
							}
						}
						if (flattenedTroopRosterElement2.Troop.HeroObject.IsHumanPlayerCharacter)
						{
							num9 *= 10f;
						}
						priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(flattenedTroopRosterElement2, battleParty, num9));
					}
					else
					{
						int num10 = 0;
						int num11 = 0;
						for (int i = 0; i < battleParty.Party.MemberRoster.Count; i++)
						{
							TroopRosterElement elementCopyAtIndex = battleParty.Party.MemberRoster.GetElementCopyAtIndex(i);
							if (!elementCopyAtIndex.Character.IsHero)
							{
								if (elementCopyAtIndex.Character == flattenedTroopRosterElement2.Troop)
								{
									num10 = i - num11;
									break;
								}
							}
							else
							{
								num11++;
							}
						}
						int num12 = (int)(100f / MathF.Pow(1.2f, (float)num10));
						if (num12 < 10)
						{
							num12 = 10;
						}
						int num13 = numberOfHealthyMembers / sizeOfSide * 100;
						if (num13 < 10)
						{
							num13 = 10;
						}
						int num14 = 0;
						if (priorityTroops != null)
						{
							UniqueTroopDescriptor descriptor2 = priorityTroops.FindIndexOfCharacter(flattenedTroopRosterElement2.Troop);
							if (descriptor2.IsValid)
							{
								num14 = 20000;
								priorityTroops.Remove(descriptor2);
							}
						}
						num9 = (float)(num14 + MBRandom.RandomInt((int)((float)num12 * 0.5f + (float)num13 * 0.5f)));
						priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(flattenedTroopRosterElement2, battleParty, num9));
					}
				}
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0007B1E0 File Offset: 0x000793E0
		private bool IsPriorityTroop(FlattenedTroopRosterElement troop, FlattenedTroopRoster priorityTroops)
		{
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in priorityTroops)
			{
				if (flattenedTroopRosterElement.Troop == troop.Troop)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0007B238 File Offset: 0x00079438
		private bool CanTroopJoinBattle(FlattenedTroopRosterElement troopRoster, bool includePlayer)
		{
			return !troopRoster.IsWounded && !troopRoster.IsRouted && !troopRoster.IsKilled && (includePlayer || !troopRoster.Troop.IsPlayerCharacter);
		}
	}
}
