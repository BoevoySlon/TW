using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000042 RID: 66
	public struct CraftingStatData
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00013B9D File Offset: 0x00011D9D
		public bool IsValid
		{
			get
			{
				return this.MaxValue >= 0f;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013BAF File Offset: 0x00011DAF
		public CraftingStatData(TextObject descriptionText, float curValue, float maxValue, CraftingTemplate.CraftingStatTypes type, DamageTypes damageType = DamageTypes.Invalid)
		{
			this.DescriptionText = descriptionText;
			this.CurValue = curValue;
			this.MaxValue = maxValue;
			this.Type = type;
			this.DamageType = damageType;
		}

		// Token: 0x04000272 RID: 626
		public readonly TextObject DescriptionText;

		// Token: 0x04000273 RID: 627
		public readonly float CurValue;

		// Token: 0x04000274 RID: 628
		public readonly float MaxValue;

		// Token: 0x04000275 RID: 629
		public readonly CraftingTemplate.CraftingStatTypes Type;

		// Token: 0x04000276 RID: 630
		public readonly DamageTypes DamageType;
	}
}
