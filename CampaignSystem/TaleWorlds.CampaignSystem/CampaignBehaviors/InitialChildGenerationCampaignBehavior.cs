using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003A0 RID: 928
	public class InitialChildGenerationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600379F RID: 14239 RVA: 0x000FB214 File Offset: 0x000F9414
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000FB230 File Offset: 0x000F9430
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int index)
		{
			if (index == 0)
			{
				using (List<Clan>.Enumerator enumerator = Clan.All.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						if (!clan.IsBanditFaction && !clan.IsMinorFaction && !clan.IsEliminated && clan != Clan.PlayerClan)
						{
							List<Hero> list = new List<Hero>();
							MBList<Hero> mblist = new MBList<Hero>();
							MBList<Hero> mblist2 = new MBList<Hero>();
							foreach (Hero hero in clan.Lords)
							{
								if (hero.IsAlive)
								{
									if (hero.IsChild)
									{
										list.Add(hero);
									}
									else if (hero.IsFemale)
									{
										mblist.Add(hero);
									}
									else
									{
										mblist2.Add(hero);
									}
								}
							}
							int num = MathF.Ceiling((float)(mblist2.Count + mblist.Count) / 2f) - list.Count;
							float num2 = 0.49f;
							if (mblist2.Count == 0)
							{
								num2 = -1f;
							}
							Func<Clan, bool> <>9__0;
							for (int i = 0; i < num; i++)
							{
								bool isFemale = MBRandom.RandomFloat <= num2;
								Hero hero2 = isFemale ? mblist.GetRandomElement<Hero>() : mblist2.GetRandomElement<Hero>();
								if (hero2 == null)
								{
									IEnumerable<Clan> nonBanditFactions = Clan.NonBanditFactions;
									Func<Clan, bool> predicate;
									if ((predicate = <>9__0) == null)
									{
										predicate = (<>9__0 = ((Clan t) => t != clan && t.Culture == clan.Culture));
									}
									MBList<Clan> e = nonBanditFactions.Where(predicate).ToMBList<Clan>();
									Func<Hero, bool> <>9__1;
									for (int j = 0; j < 10; j++)
									{
										IEnumerable<Hero> lords = e.GetRandomElement<Clan>().Lords;
										Func<Hero, bool> predicate2;
										if ((predicate2 = <>9__1) == null)
										{
											predicate2 = (<>9__1 = ((Hero t) => t.IsAlive && t.IsFemale == isFemale));
										}
										hero2 = lords.Where(predicate2).ToMBList<Hero>().GetRandomElement<Hero>();
										if (hero2 != null)
										{
											break;
										}
									}
								}
								if (hero2 != null)
								{
									int age = MBRandom.RandomInt(2, 18);
									Hero hero3 = HeroCreator.CreateSpecialHero(hero2.CharacterObject, clan.HomeSettlement, clan, null, age);
									hero3.UpdateHomeSettlement();
									hero3.HeroDeveloper.InitializeHeroDeveloper(true, null);
									MBEquipmentRoster randomElementInefficiently = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForInitialChildrenGeneration(hero3).GetRandomElementInefficiently<MBEquipmentRoster>();
									if (randomElementInefficiently != null)
									{
										Equipment randomCivilianEquipment = randomElementInefficiently.GetRandomCivilianEquipment();
										EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, randomCivilianEquipment);
										Equipment equipment = new Equipment(false);
										equipment.FillFrom(randomCivilianEquipment, false);
										EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, equipment);
									}
								}
								if (num2 <= 0f)
								{
									num2 = 0.49f;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000FB534 File Offset: 0x000F9734
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04001177 RID: 4471
		private const float FemaleChildrenChance = 0.49f;
	}
}
