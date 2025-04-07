using System;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000154 RID: 340
	public static class MetaDataExtensions
	{
		// Token: 0x0600186E RID: 6254 RVA: 0x0007C708 File Offset: 0x0007A908
		public static string GetUniqueGameId(this MetaData metaData)
		{
			string result;
			if (metaData == null || !metaData.TryGetValue("UniqueGameId", out result))
			{
				return "";
			}
			return result;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0007C730 File Offset: 0x0007A930
		public static int GetMainHeroLevel(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainHeroLevel", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0007C758 File Offset: 0x0007A958
		public static float GetMainPartyFood(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainPartyFood", out s))
			{
				return 0f;
			}
			return float.Parse(s);
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0007C784 File Offset: 0x0007A984
		public static int GetMainHeroGold(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainHeroGold", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0007C7AC File Offset: 0x0007A9AC
		public static float GetClanInfluence(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("ClanInfluence", out s))
			{
				return 0f;
			}
			return float.Parse(s);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0007C7D8 File Offset: 0x0007A9D8
		public static int GetClanFiefs(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("ClanFiefs", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0007C800 File Offset: 0x0007AA00
		public static int GetMainPartyHealthyMemberCount(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainPartyHealthyMemberCount", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0007C828 File Offset: 0x0007AA28
		public static int GetMainPartyPrisonerMemberCount(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainPartyPrisonerMemberCount", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0007C850 File Offset: 0x0007AA50
		public static int GetMainPartyWoundedMemberCount(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("MainPartyWoundedMemberCount", out s))
			{
				return 0;
			}
			return int.Parse(s);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0007C878 File Offset: 0x0007AA78
		public static string GetClanBannerCode(this MetaData metaData)
		{
			string result;
			if (metaData == null || !metaData.TryGetValue("ClanBannerCode", out result))
			{
				return "";
			}
			return result;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0007C8A0 File Offset: 0x0007AAA0
		public static string GetCharacterName(this MetaData metaData)
		{
			string result;
			if (metaData == null || !metaData.TryGetValue("CharacterName", out result))
			{
				return "";
			}
			return result;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
		public static string GetCharacterVisualCode(this MetaData metaData)
		{
			string result;
			if (metaData == null || !metaData.TryGetValue("MainHeroVisual", out result))
			{
				return "";
			}
			return result;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0007C8F0 File Offset: 0x0007AAF0
		public static double GetDayLong(this MetaData metaData)
		{
			string s;
			if (metaData == null || !metaData.TryGetValue("DayLong", out s))
			{
				return 0.0;
			}
			return double.Parse(s);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0007C920 File Offset: 0x0007AB20
		public static bool GetIronmanMode(this MetaData metaData)
		{
			string s;
			int num;
			return metaData != null && metaData.TryGetValue("IronmanMode", out s) && int.TryParse(s, out num) && num == 1;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0007C950 File Offset: 0x0007AB50
		public static int GetPlayerHealthPercentage(this MetaData metaData)
		{
			string s;
			int result;
			if (metaData == null || !metaData.TryGetValue("HealthPercentage", out s) || !int.TryParse(s, out result))
			{
				return 100;
			}
			return result;
		}
	}
}
