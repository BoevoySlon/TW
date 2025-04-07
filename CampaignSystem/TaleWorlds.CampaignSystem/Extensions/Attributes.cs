using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200014E RID: 334
	public static class Attributes
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0007C63E File Offset: 0x0007A83E
		public static MBReadOnlyList<CharacterAttribute> All
		{
			get
			{
				return Campaign.Current.AllCharacterAttributes;
			}
		}
	}
}
