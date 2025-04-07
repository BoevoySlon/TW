using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000161 RID: 353
	[EncyclopediaModel(new Type[]
	{
		typeof(Concept)
	})]
	public class DefaultEncyclopediaConceptPage : EncyclopediaPage
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x0007D5A2 File Offset: 0x0007B7A2
		public DefaultEncyclopediaConceptPage()
		{
			base.HomePageOrderIndex = 600;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0007D5B5 File Offset: 0x0007B7B5
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			foreach (Concept concept in Concept.All)
			{
				yield return new EncyclopediaListItem(concept, concept.Title.ToString(), concept.Description.ToString(), concept.StringId, base.GetIdentifier(typeof(Concept)), true, null);
			}
			List<Concept>.Enumerator enumerator = default(List<Concept>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0007D5C8 File Offset: 0x0007B7C8
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=uauMia0D} Characters", null), (object c) => Concept.IsGroupMember("Characters", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=cwRkqIt4} Kingdoms", null), (object c) => Concept.IsGroupMember("Kingdoms", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=x6knoNnC} Clans", null), (object c) => Concept.IsGroupMember("Clans", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=GYzkb4iB} Parties", null), (object c) => Concept.IsGroupMember("Parties", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=u6GM5Spa} Armies", null), (object c) => Concept.IsGroupMember("Armies", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=zPYRGJtD} Troops", null), (object c) => Concept.IsGroupMember("Troops", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=3PUkH5Zf} Items", null), (object c) => Concept.IsGroupMember("Items", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=xKVBAL3m} Campaign Issues", null), (object c) => Concept.IsGroupMember("CampaignIssues", (Concept)c)));
			list.Add(new EncyclopediaFilterGroup(list2, new TextObject("{=tBx7XXps}Types", null)));
			return list;
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0007D79F File Offset: 0x0007B99F
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>();
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0007D7A6 File Offset: 0x0007B9A6
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaConceptPage";
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0007D7AD File Offset: 0x0007B9AD
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_concepts", null);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0007D7BA File Offset: 0x0007B9BA
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_concepts_description", null);
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0007D7C7 File Offset: 0x0007B9C7
		public override string GetStringID()
		{
			return "EncyclopediaConcept";
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0007D7D0 File Offset: 0x0007B9D0
		public override bool IsValidEncyclopediaItem(object o)
		{
			Concept concept = o as Concept;
			return concept != null && concept.Title != null && concept.Description != null;
		}
	}
}
