using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000C0 RID: 192
	public class BattleTypeEnumResolver : IEnumResolver
	{
		// Token: 0x06001278 RID: 4728 RVA: 0x00055714 File Offset: 0x00053914
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("EndCaptivityDetail data is null or empty", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\BattleTypeEnumResolver.cs", "ResolveObject", 16);
				return MapEvent.BattleTypes.None.ToString();
			}
			if (originalObject.Equals("AlleyFight"))
			{
				return MapEvent.BattleTypes.None.ToString();
			}
			return originalObject;
		}
	}
}
