using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000425 RID: 1061
	public static class AdoptHeroAction
	{
		// Token: 0x06004044 RID: 16452 RVA: 0x0013C7D7 File Offset: 0x0013A9D7
		private static void ApplyInternal(Hero adoptedHero)
		{
			if (Hero.MainHero.IsFemale)
			{
				adoptedHero.Mother = Hero.MainHero;
			}
			else
			{
				adoptedHero.Father = Hero.MainHero;
			}
			adoptedHero.Clan = Clan.PlayerClan;
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0013C808 File Offset: 0x0013AA08
		public static void Apply(Hero adoptedHero)
		{
			AdoptHeroAction.ApplyInternal(adoptedHero);
		}
	}
}
