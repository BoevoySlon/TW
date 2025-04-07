using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x02000350 RID: 848
	public sealed class FeatObject : PropertyObject
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x000CDD3D File Offset: 0x000CBF3D
		public static MBReadOnlyList<FeatObject> All
		{
			get
			{
				return Campaign.Current.AllFeats;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x000CDD49 File Offset: 0x000CBF49
		// (set) Token: 0x0600302F RID: 12335 RVA: 0x000CDD51 File Offset: 0x000CBF51
		public float EffectBonus { get; private set; }

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000CDD5A File Offset: 0x000CBF5A
		// (set) Token: 0x06003031 RID: 12337 RVA: 0x000CDD62 File Offset: 0x000CBF62
		public FeatObject.AdditionType IncrementType { get; private set; }

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x000CDD6B File Offset: 0x000CBF6B
		// (set) Token: 0x06003033 RID: 12339 RVA: 0x000CDD73 File Offset: 0x000CBF73
		public bool IsPositive { get; private set; }

		// Token: 0x06003034 RID: 12340 RVA: 0x000CDD7C File Offset: 0x000CBF7C
		public FeatObject(string stringId) : base(stringId)
		{
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000CDD85 File Offset: 0x000CBF85
		public void Initialize(string name, string description, float effectBonus, bool isPositiveEffect, FeatObject.AdditionType incrementType)
		{
			base.Initialize(new TextObject(name, null), new TextObject(description, null));
			this.EffectBonus = effectBonus;
			this.IncrementType = incrementType;
			this.IsPositive = isPositiveEffect;
			base.AfterInitialized();
		}

		// Token: 0x02000699 RID: 1689
		public enum AdditionType
		{
			// Token: 0x04001B7E RID: 7038
			Add,
			// Token: 0x04001B7F RID: 7039
			AddFactor
		}
	}
}
