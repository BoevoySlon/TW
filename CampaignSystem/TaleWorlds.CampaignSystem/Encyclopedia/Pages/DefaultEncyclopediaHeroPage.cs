using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000163 RID: 355
	[EncyclopediaModel(new Type[]
	{
		typeof(Hero)
	})]
	public class DefaultEncyclopediaHeroPage : EncyclopediaPage
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x0007D9B3 File Offset: 0x0007BBB3
		public DefaultEncyclopediaHeroPage()
		{
			base.HomePageOrderIndex = 200;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0007D9C6 File Offset: 0x0007BBC6
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			int comingOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
			TextObject heroName = new TextObject("{=TauRjAud}{NAME} of the {FACTION}", null);
			string name = string.Empty;
			using (List<Hero>.Enumerator enumerator = Hero.AllAliveHeroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hero hero = enumerator.Current;
					if (this.IsValidEncyclopediaItem(hero) && !hero.IsNotable && hero.Age >= (float)comingOfAge)
					{
						if (hero.Clan != null)
						{
							heroName.SetTextVariable("NAME", hero.FirstName ?? hero.Name);
							TextObject textObject = heroName;
							string tag = "FACTION";
							Clan clan = hero.Clan;
							textObject.SetTextVariable(tag, ((clan != null) ? clan.Name : null) ?? TextObject.Empty);
							name = heroName.ToString();
						}
						else
						{
							name = hero.Name.ToString();
						}
						yield return new EncyclopediaListItem(hero, name, "", hero.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero), delegate()
						{
							InformationManager.ShowTooltip(typeof(Hero), new object[]
							{
								hero,
								false
							});
						});
					}
				}
			}
			List<Hero>.Enumerator enumerator = default(List<Hero>.Enumerator);
			using (List<Hero>.Enumerator enumerator = Hero.DeadOrDisabledHeroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hero hero = enumerator.Current;
					if (this.IsValidEncyclopediaItem(hero) && !hero.IsNotable && hero.Age >= (float)comingOfAge)
					{
						if (hero.Clan != null)
						{
							heroName.SetTextVariable("NAME", hero.FirstName ?? hero.Name);
							TextObject textObject2 = heroName;
							string tag2 = "FACTION";
							Clan clan2 = hero.Clan;
							textObject2.SetTextVariable(tag2, ((clan2 != null) ? clan2.Name : null) ?? TextObject.Empty);
							yield return new EncyclopediaListItem(hero, heroName.ToString(), "", hero.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero), delegate()
							{
								InformationManager.ShowTooltip(typeof(Hero), new object[]
								{
									hero,
									false
								});
							});
						}
						else
						{
							yield return new EncyclopediaListItem(hero, hero.Name.ToString(), "", hero.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero), delegate()
							{
								InformationManager.ShowTooltip(typeof(Hero), new object[]
								{
									hero,
									false
								});
							});
						}
					}
				}
			}
			enumerator = default(List<Hero>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0007D9D8 File Offset: 0x0007BBD8
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=5xi0t1dD}Met Before", null), (object h) => ((Hero)h).HasMet));
			list.Add(new EncyclopediaFilterGroup(list2, new TextObject("{=BlidMNGT}Relation", null)));
			List<EncyclopediaFilterItem> list3 = new List<EncyclopediaFilterItem>();
			list3.Add(new EncyclopediaFilterItem(new TextObject("{=oAb4NqO5}Male", null), (object h) => !((Hero)h).IsFemale));
			list3.Add(new EncyclopediaFilterItem(new TextObject("{=2YUUGQvG}Female", null), (object h) => ((Hero)h).IsFemale));
			list.Add(new EncyclopediaFilterGroup(list3, new TextObject("{=fGFMqlGz}Gender", null)));
			List<EncyclopediaFilterItem> list4 = new List<EncyclopediaFilterItem>();
			list4.Add(new EncyclopediaFilterItem(new TextObject("{=uvjOVy5P}Dead", null), (object h) => !((Hero)h).IsAlive));
			list4.Add(new EncyclopediaFilterItem(new TextObject("{=3TmLIou4}Alive", null), (object h) => ((Hero)h).IsAlive));
			list.Add(new EncyclopediaFilterGroup(list4, new TextObject("{=DXczLzml}Status", null)));
			List<EncyclopediaFilterItem> list5 = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
			orderby !x.IsMainCulture descending
			select x).ThenBy((CultureObject f) => f.Name.ToString()).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.StringId != "neutral_culture" && !culture.IsBandit)
					{
						list5.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((Hero)c).Culture == culture));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list5, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> list6 = new List<EncyclopediaFilterItem>();
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=b9ty57rJ}Faction Leader", null), (object h) => ((Hero)h).IsKingdomLeader || ((Hero)h).IsClanLeader));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=4vleNtxb}Lord/Lady", null), (object h) => ((Hero)h).IsLord));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=vmMqs3Ck}Noble", null), delegate(object h)
			{
				Clan clan = ((Hero)h).Clan;
				return clan != null && clan.IsNoble;
			}));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=FLa5OuyK}Wanderer", null), (object h) => ((Hero)h).IsWanderer));
			list.Add(new EncyclopediaFilterGroup(list6, new TextObject("{=GZxFIeiJ}Occupation", null)));
			List<EncyclopediaFilterItem> list7 = new List<EncyclopediaFilterItem>();
			list7.Add(new EncyclopediaFilterItem(new TextObject("{=qIAgh9VL}Not Married", null), (object h) => ((Hero)h).Spouse == null));
			list7.Add(new EncyclopediaFilterItem(new TextObject("{=xeawD38S}Married", null), (object h) => ((Hero)h).Spouse != null));
			list.Add(new EncyclopediaFilterGroup(list7, new TextObject("{=PMio7set}Marital Status", null)));
			return list;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0007DDDC File Offset: 0x0007BFDC
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=jaaQijQs}Age", null), new DefaultEncyclopediaHeroPage.EncyclopediaListHeroAgeComparer()),
				new EncyclopediaSortController(new TextObject("{=BlidMNGT}Relation", null), new DefaultEncyclopediaHeroPage.EncyclopediaListHeroRelationComparer())
			};
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0007DE19 File Offset: 0x0007C019
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaHeroPage";
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0007DE20 File Offset: 0x0007C020
		public override string GetStringID()
		{
			return "EncyclopediaHero";
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0007DE27 File Offset: 0x0007C027
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_heroes", null);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0007DE34 File Offset: 0x0007C034
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_hero_description", null);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0007DE41 File Offset: 0x0007C041
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return Campaign.Current.CampaignObjectManager.Find<Hero>(stringID);
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0007DE54 File Offset: 0x0007C054
		public override bool IsValidEncyclopediaItem(object o)
		{
			Hero hero = o as Hero;
			if (hero != null && !hero.IsTemplate && hero.IsReady)
			{
				IFaction mapFaction = hero.MapFaction;
				if (mapFaction == null || !mapFaction.IsBanditFaction)
				{
					return !hero.CharacterObject.HiddenInEncylopedia;
				}
			}
			return false;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0007DEA2 File Offset: 0x0007C0A2
		private static bool CanPlayerSeeValuesOf(Hero hero)
		{
			return Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero);
		}

		// Token: 0x02000537 RID: 1335
		private class EncyclopediaListHeroAgeComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
		{
			// Token: 0x060044AC RID: 17580 RVA: 0x00148686 File Offset: 0x00146886
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareHeroes(x, y, DefaultEncyclopediaHeroPage.EncyclopediaListHeroAgeComparer._comparison);
			}

			// Token: 0x060044AD RID: 17581 RVA: 0x00148698 File Offset: 0x00146898
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Hero hero;
				if ((hero = (item.Object as Hero)) == null)
				{
					Debug.FailedAssert("Unable to get the age of a non-hero object.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "GetComparedValueText", 179);
					return "";
				}
				if (!DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero))
				{
					return this._missingValue.ToString();
				}
				return ((int)hero.Age).ToString();
			}

			// Token: 0x04001630 RID: 5680
			private static Func<Hero, Hero, int> _comparison = (Hero h1, Hero h2) => h1.Age.CompareTo(h2.Age);
		}

		// Token: 0x02000538 RID: 1336
		private class EncyclopediaListHeroRelationComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
		{
			// Token: 0x060044B0 RID: 17584 RVA: 0x00148715 File Offset: 0x00146915
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareHeroes(x, y, DefaultEncyclopediaHeroPage.EncyclopediaListHeroRelationComparer._comparison);
			}

			// Token: 0x060044B1 RID: 17585 RVA: 0x00148724 File Offset: 0x00146924
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Hero hero;
				if ((hero = (item.Object as Hero)) == null)
				{
					Debug.FailedAssert("Unable to get the relation between a non-hero object and the player.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "GetComparedValueText", 209);
					return "";
				}
				if (!DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero))
				{
					return this._missingValue.ToString();
				}
				int num = (int)hero.GetRelationWithPlayer();
				MBTextManager.SetTextVariable("NUMBER", num);
				if (num <= 0)
				{
					return num.ToString();
				}
				return GameTexts.FindText("str_plus_with_number", null).ToString();
			}

			// Token: 0x04001631 RID: 5681
			private static Func<Hero, Hero, int> _comparison = (Hero h1, Hero h2) => h1.GetRelationWithPlayer().CompareTo(h2.GetRelationWithPlayer());
		}

		// Token: 0x02000539 RID: 1337
		public abstract class EncyclopediaListHeroComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x060044B4 RID: 17588 RVA: 0x001487C4 File Offset: 0x001469C4
			protected bool CompareVisibility(Hero h1, Hero h2, out int comparisonResult)
			{
				bool flag = DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(h1);
				bool flag2 = DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(h2);
				if (!flag && !flag2)
				{
					comparisonResult = 0;
					return true;
				}
				if (!flag)
				{
					comparisonResult = (base.IsAscending ? 1 : -1);
					return true;
				}
				if (!flag2)
				{
					comparisonResult = (base.IsAscending ? -1 : 1);
					return true;
				}
				comparisonResult = 0;
				return false;
			}

			// Token: 0x060044B5 RID: 17589 RVA: 0x00148814 File Offset: 0x00146A14
			protected int CompareHeroes(EncyclopediaListItem x, EncyclopediaListItem y, Func<Hero, Hero, int> comparison)
			{
				Hero hero;
				Hero hero2;
				if ((hero = (x.Object as Hero)) == null || (hero2 = (y.Object as Hero)) == null)
				{
					Debug.FailedAssert("Both objects should be heroes.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "CompareHeroes", 258);
					return 0;
				}
				int num;
				if (this.CompareVisibility(hero, hero2, out num))
				{
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num * (base.IsAscending ? 1 : -1);
				}
				else
				{
					int num2 = comparison(hero, hero2) * (base.IsAscending ? 1 : -1);
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2;
				}
			}

			// Token: 0x02000793 RID: 1939
			// (Invoke) Token: 0x06005A47 RID: 23111
			protected delegate bool HeroVisibilityComparerDelegate(Hero h1, Hero h2, out int comparisonResult);
		}
	}
}
