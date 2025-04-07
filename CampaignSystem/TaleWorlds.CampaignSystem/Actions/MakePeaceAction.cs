using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200044E RID: 1102
	public static class MakePeaceAction
	{
		// Token: 0x060040F4 RID: 16628 RVA: 0x001409EC File Offset: 0x0013EBEC
		private static void ApplyInternal(IFaction faction1, IFaction faction2, int dailyTributeFrom1To2, MakePeaceAction.MakePeaceDetail detail = MakePeaceAction.MakePeaceDetail.Default)
		{
			StanceLink stanceWith = faction1.GetStanceWith(faction2);
			stanceWith.StanceType = StanceType.Neutral;
			stanceWith.SetDailyTributePaid(faction1, dailyTributeFrom1To2);
			if (faction1 == Hero.MainHero.MapFaction || faction2 == Hero.MainHero.MapFaction)
			{
				IFaction dirtySide = (faction1 == Hero.MainHero.MapFaction) ? faction2 : faction1;
				IEnumerable<Settlement> all = Settlement.All;
				Func<Settlement, bool> predicate;
				Func<Settlement, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((Settlement party) => party.IsVisible && party.MapFaction == dirtySide));
				}
				foreach (Settlement settlement in all.Where(predicate))
				{
					settlement.Party.SetVisualAsDirty();
				}
				IEnumerable<MobileParty> all2 = MobileParty.All;
				Func<MobileParty, bool> predicate2;
				Func<MobileParty, bool> <>9__1;
				if ((predicate2 = <>9__1) == null)
				{
					predicate2 = (<>9__1 = ((MobileParty party) => party.IsVisible && party.MapFaction == dirtySide));
				}
				foreach (MobileParty mobileParty in all2.Where(predicate2))
				{
					mobileParty.Party.SetVisualAsDirty();
				}
			}
			CampaignEventDispatcher.Instance.OnMakePeace(faction1, faction2, detail);
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00140B28 File Offset: 0x0013ED28
		public static void ApplyPardonPlayer(IFaction faction)
		{
			MakePeaceAction.ApplyInternal(faction, Hero.MainHero.MapFaction, 0, MakePeaceAction.MakePeaceDetail.Default);
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00140B3C File Offset: 0x0013ED3C
		public static void Apply(IFaction faction1, IFaction faction2, int dailyTributeFrom1To2 = 0)
		{
			MakePeaceAction.ApplyInternal(faction1, faction2, dailyTributeFrom1To2, MakePeaceAction.MakePeaceDetail.Default);
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00140B47 File Offset: 0x0013ED47
		public static void ApplyByKingdomDecision(IFaction faction1, IFaction faction2, int dailyTributeFrom1To2 = 0)
		{
			MakePeaceAction.ApplyInternal(faction1, faction2, dailyTributeFrom1To2, MakePeaceAction.MakePeaceDetail.ByKingdomDecision);
		}

		// Token: 0x040012CC RID: 4812
		private const float DefaultValueForBeingLimitedAfterPeace = 100000f;

		// Token: 0x02000777 RID: 1911
		public enum MakePeaceDetail
		{
			// Token: 0x04001F3E RID: 7998
			Default,
			// Token: 0x04001F3F RID: 7999
			ByKingdomDecision
		}
	}
}
