using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000128 RID: 296
	public class DefaultPlayerProgressionModel : PlayerProgressionModel
	{
		// Token: 0x060016F8 RID: 5880 RVA: 0x00071634 File Offset: 0x0006F834
		public override float GetPlayerProgress()
		{
			return MBMath.ClampFloat((float)Clan.PlayerClan.Fiefs.Count * 0.1f + Clan.PlayerClan.TotalStrength * 0.0008f + Clan.PlayerClan.Renown * 1.5E-05f + (float)Clan.PlayerClan.Lords.Count * 0.002f + (float)Clan.PlayerClan.Companions.Count * 0.01f + (float)Clan.PlayerClan.SupporterNotables.Count * 0.001f + (float)Hero.MainHero.OwnedCaravans.Count * 0.01f + (float)PartyBase.MainParty.NumberOfAllMembers * 0.002f + (float)CharacterObject.PlayerCharacter.Level * 0.002f, 0f, 1f);
		}
	}
}
