using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003BF RID: 959
	public class PerkActivationHandlerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A7D RID: 14973 RVA: 0x00114041 File Offset: 0x00112241
		public override void RegisterEvents()
		{
			CampaignEvents.PerkOpenedEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkOpened));
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x0011405C File Offset: 0x0011225C
		private void OnPerkOpened(Hero hero, PerkObject perk)
		{
			if (hero != null)
			{
				if (perk == DefaultPerks.OneHanded.Trainer || perk == DefaultPerks.OneHanded.UnwaveringDefense || perk == DefaultPerks.TwoHanded.ThickHides || perk == DefaultPerks.Athletics.WellBuilt || perk == DefaultPerks.Medicine.PreventiveMedicine)
				{
					hero.HitPoints += (int)perk.PrimaryBonus;
				}
				else if (perk == DefaultPerks.Crafting.VigorousSmith)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, 1, false);
				}
				else if (perk == DefaultPerks.Crafting.StrongSmith)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, 1, false);
				}
				else if (perk == DefaultPerks.Crafting.EnduringSmith)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, 1, false);
				}
				else if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
				{
					hero.HeroDeveloper.AddFocus(DefaultSkills.OneHanded, 1, false);
					hero.HeroDeveloper.AddFocus(DefaultSkills.TwoHanded, 1, false);
				}
				else if (perk == DefaultPerks.Athletics.Durable)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, 1, false);
				}
				else if (perk == DefaultPerks.Athletics.Steady)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, 1, false);
				}
				else if (perk == DefaultPerks.Athletics.Strong)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, 1, false);
				}
				if (hero == Hero.MainHero && (perk == DefaultPerks.OneHanded.Prestige || perk == DefaultPerks.TwoHanded.Hope || perk == DefaultPerks.Athletics.ImposingStature || perk == DefaultPerks.Bow.MerryMen || perk == DefaultPerks.Tactics.HordeLeader || perk == DefaultPerks.Scouting.MountedScouts || perk == DefaultPerks.Leadership.Authority || perk == DefaultPerks.Leadership.LeaderOfMasses || perk == DefaultPerks.Leadership.UltimateLeader))
				{
					PartyBase.MainParty.MemberRoster.UpdateVersion();
				}
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x001141EC File Offset: 0x001123EC
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
