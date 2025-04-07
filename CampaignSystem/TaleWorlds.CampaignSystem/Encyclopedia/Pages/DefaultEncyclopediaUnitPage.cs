using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000165 RID: 357
	[EncyclopediaModel(new Type[]
	{
		typeof(CharacterObject)
	})]
	public class DefaultEncyclopediaUnitPage : EncyclopediaPage
	{
		// Token: 0x060018E7 RID: 6375 RVA: 0x0007E1D4 File Offset: 0x0007C3D4
		public DefaultEncyclopediaUnitPage()
		{
			base.HomePageOrderIndex = 300;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0007E1E7 File Offset: 0x0007C3E7
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (List<CharacterObject>.Enumerator enumerator = CharacterObject.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterObject character = enumerator.Current;
					if (this.IsValidEncyclopediaItem(character))
					{
						yield return new EncyclopediaListItem(character, character.Name.ToString(), "", character.StringId, base.GetIdentifier(typeof(CharacterObject)), true, delegate()
						{
							InformationManager.ShowTooltip(typeof(CharacterObject), new object[]
							{
								character
							});
						});
					}
				}
			}
			List<CharacterObject>.Enumerator enumerator = default(List<CharacterObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0007E1F8 File Offset: 0x0007C3F8
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=1Bm1Wk1v}Infantry", null), (object s) => ((CharacterObject)s).IsInfantry));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=bIiBytSB}Archers", null), (object s) => ((CharacterObject)s).IsRanged && !((CharacterObject)s).IsMounted));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=YVGtcLHF}Cavalry", null), (object s) => ((CharacterObject)s).IsMounted && !((CharacterObject)s).IsRanged));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=I1CMeL9R}Mounted Archers", null), (object s) => ((CharacterObject)s).IsRanged && ((CharacterObject)s).IsMounted));
			List<EncyclopediaFilterItem> filters = list2;
			list.Add(new EncyclopediaFilterGroup(filters, new TextObject("{=zMMqgxb1}Type", null)));
			List<EncyclopediaFilterItem> list3 = new List<EncyclopediaFilterItem>();
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Soldier"), (object s) => ((CharacterObject)s).Occupation == Occupation.Soldier));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Mercenary"), (object s) => ((CharacterObject)s).Occupation == Occupation.Mercenary));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Bandit"), (object s) => ((CharacterObject)s).Occupation == Occupation.Bandit));
			List<EncyclopediaFilterItem> filters2 = list3;
			list.Add(new EncyclopediaFilterGroup(filters2, new TextObject("{=GZxFIeiJ}Occupation", null)));
			List<EncyclopediaFilterItem> list4 = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
			orderby !x.IsMainCulture descending
			select x).ThenBy((CultureObject f) => f.Name.ToString()).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.StringId != "neutral_culture")
					{
						list4.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((CharacterObject)c).Culture == culture));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list4, GameTexts.FindText("str_culture", null)));
			return list;
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0007E4C4 File Offset: 0x0007C6C4
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=cc1d7mkq}Tier", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_level_tag", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer())
			};
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0007E501 File Offset: 0x0007C701
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaUnitPage";
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0007E508 File Offset: 0x0007C708
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_troops", null);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0007E515 File Offset: 0x0007C715
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_unit_description", null);
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0007E522 File Offset: 0x0007C722
		public override string GetStringID()
		{
			return "EncyclopediaUnit";
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0007E52C File Offset: 0x0007C72C
		public override bool IsValidEncyclopediaItem(object o)
		{
			CharacterObject characterObject = o as CharacterObject;
			return characterObject != null && !characterObject.IsTemplate && characterObject != null && !characterObject.HiddenInEncylopedia && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Bandit || characterObject.Occupation == Occupation.Gangster || characterObject.Occupation == Occupation.CaravanGuard || (characterObject.Occupation == Occupation.Villager && characterObject.UpgradeTargets.Length != 0));
		}

		// Token: 0x0200054A RID: 1354
		private class EncyclopediaListUnitTierComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x06004508 RID: 17672 RVA: 0x001498FF File Offset: 0x00147AFF
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer._comparison);
			}

			// Token: 0x06004509 RID: 17673 RVA: 0x00149910 File Offset: 0x00147B10
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = (item.Object as CharacterObject)) != null)
				{
					return characterObject.Tier.ToString();
				}
				Debug.FailedAssert("Unable to get the tier of a non-character object.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 138);
				return "";
			}

			// Token: 0x04001657 RID: 5719
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Tier.CompareTo(c2.Tier);
		}

		// Token: 0x0200054B RID: 1355
		private class EncyclopediaListUnitLevelComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x0600450C RID: 17676 RVA: 0x00149978 File Offset: 0x00147B78
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer._comparison);
			}

			// Token: 0x0600450D RID: 17677 RVA: 0x00149988 File Offset: 0x00147B88
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = (item.Object as CharacterObject)) != null)
				{
					return characterObject.Level.ToString();
				}
				Debug.FailedAssert("Unable to get the level of a non-character object.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 159);
				return "";
			}

			// Token: 0x04001658 RID: 5720
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Level.CompareTo(c2.Level);
		}

		// Token: 0x0200054C RID: 1356
		public abstract class EncyclopediaListUnitComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06004510 RID: 17680 RVA: 0x001499F0 File Offset: 0x00147BF0
			public int CompareUnits(EncyclopediaListItem x, EncyclopediaListItem y, Func<CharacterObject, CharacterObject, int> comparison)
			{
				CharacterObject arg;
				CharacterObject arg2;
				if ((arg = (x.Object as CharacterObject)) == null || (arg2 = (y.Object as CharacterObject)) == null)
				{
					Debug.FailedAssert("Both objects should be character objects.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "CompareUnits", 174);
					return 0;
				}
				int num = comparison(arg, arg2) * (base.IsAscending ? 1 : -1);
				if (num == 0)
				{
					return base.ResolveEquality(x, y);
				}
				return num;
			}
		}
	}
}
