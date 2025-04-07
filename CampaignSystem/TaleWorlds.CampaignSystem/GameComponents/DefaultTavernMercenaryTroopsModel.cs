using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000140 RID: 320
	public class DefaultTavernMercenaryTroopsModel : TavernMercenaryTroopsModel
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0007A0F4 File Offset: 0x000782F4
		public override float RegularMercenariesSpawnChance
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
