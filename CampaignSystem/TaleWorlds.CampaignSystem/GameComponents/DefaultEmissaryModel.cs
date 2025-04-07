using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000103 RID: 259
	public class DefaultEmissaryModel : EmissaryModel
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00066DE4 File Offset: 0x00064FE4
		public override int EmissaryRelationBonusForMainClan
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00066DE8 File Offset: 0x00064FE8
		public override bool IsEmissary(Hero hero)
		{
			return (hero.CompanionOf == Clan.PlayerClan || hero.Clan == Clan.PlayerClan) && hero.PartyBelongedTo == null && hero.CurrentSettlement != null && hero.CurrentSettlement.IsFortification && !hero.IsPrisoner && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
		}
	}
}
