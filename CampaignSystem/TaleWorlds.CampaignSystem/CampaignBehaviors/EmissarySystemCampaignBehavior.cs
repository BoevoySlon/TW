using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200038C RID: 908
	public class EmissarySystemCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060035F2 RID: 13810 RVA: 0x000EFFDD File Offset: 0x000EE1DD
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000EFFF6 File Offset: 0x000EE1F6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000EFFF8 File Offset: 0x000EE1F8
		private void DailyTick()
		{
			EmissaryModel emissaryModel = Campaign.Current.Models.EmissaryModel;
			foreach (Hero hero in Clan.PlayerClan.Heroes)
			{
				if (emissaryModel.IsEmissary(hero))
				{
					float num = (0.1f + 0.75f * ((float)hero.GetSkillValue(DefaultSkills.Charm) / 300f)) / 10f;
					if (MBRandom.RandomFloat <= num)
					{
						bool flag = MBRandom.RandomFloat <= 0.5f;
						if (!flag)
						{
							goto IL_AF;
						}
						if (!hero.CurrentSettlement.HeroesWithoutParty.Any((Hero h) => h.Occupation == Occupation.Lord))
						{
							goto IL_AF;
						}
						bool flag2 = true;
						IL_FA:
						if (!flag2)
						{
							Hero randomElement = hero.CurrentSettlement.Notables.GetRandomElement<Hero>();
							if (randomElement != null)
							{
								ChangeRelationAction.ApplyEmissaryRelation(hero, randomElement, emissaryModel.EmissaryRelationBonusForMainClan, true);
								continue;
							}
							continue;
						}
						else
						{
							Hero randomElementWithPredicate = hero.CurrentSettlement.HeroesWithoutParty.GetRandomElementWithPredicate((Hero n) => !n.IsPrisoner && n.CharacterObject.Occupation == Occupation.Lord && n.Clan != Clan.PlayerClan);
							if (randomElementWithPredicate != null)
							{
								ChangeRelationAction.ApplyEmissaryRelation(hero, randomElementWithPredicate, emissaryModel.EmissaryRelationBonusForMainClan, true);
								continue;
							}
							continue;
						}
						IL_AF:
						if (!flag && hero.CurrentSettlement.Notables.Count == 0)
						{
							flag2 = hero.CurrentSettlement.HeroesWithoutParty.Any((Hero h) => h.Occupation == Occupation.Lord);
							goto IL_FA;
						}
						flag2 = false;
						goto IL_FA;
					}
				}
			}
		}
	}
}
