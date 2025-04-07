using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000011 RID: 17
	public sealed class BannerEffect : PropertyObject
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000041C5 File Offset: 0x000023C5
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000041CD File Offset: 0x000023CD
		public BannerEffect.EffectIncrementType IncrementType { get; private set; }

		// Token: 0x060000CE RID: 206 RVA: 0x000041D6 File Offset: 0x000023D6
		public BannerEffect(string stringId) : base(stringId)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000041EC File Offset: 0x000023EC
		public void Initialize(string name, string description, float level1Bonus, float level2Bonus, float level3Bonus, BannerEffect.EffectIncrementType incrementType)
		{
			TextObject description2 = new TextObject(description, null);
			this._levelBonuses[0] = level1Bonus;
			this._levelBonuses[1] = level2Bonus;
			this._levelBonuses[2] = level3Bonus;
			this.IncrementType = incrementType;
			base.Initialize(new TextObject(name, null), description2);
			base.AfterInitialized();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000423C File Offset: 0x0000243C
		public float GetBonusAtLevel(int bannerLevel)
		{
			int num = bannerLevel - 1;
			num = MBMath.ClampIndex(num, 0, this._levelBonuses.Length);
			return this._levelBonuses[num];
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004268 File Offset: 0x00002468
		public string GetBonusStringAtLevel(int bannerLevel)
		{
			float bonusAtLevel = this.GetBonusAtLevel(bannerLevel);
			return string.Format("{0:P2}", bonusAtLevel);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004290 File Offset: 0x00002490
		public TextObject GetDescription(int bannerLevel)
		{
			float bonusAtLevel = this.GetBonusAtLevel(bannerLevel);
			if (bonusAtLevel > 0f)
			{
				TextObject textObject = new TextObject("{=Ffwgecvr}{PLUS_OR_MINUS}{BONUSEFFECT}", null);
				textObject.SetTextVariable("BONUSEFFECT", bonusAtLevel);
				textObject.SetTextVariable("PLUS_OR_MINUS", "{=eTw2aNV5}+");
				return base.Description.SetTextVariable("BONUS_AMOUNT", textObject);
			}
			return base.Description.SetTextVariable("BONUS_AMOUNT", bonusAtLevel);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000042FA File Offset: 0x000024FA
		public override string ToString()
		{
			return base.Name.ToString();
		}

		// Token: 0x040000EC RID: 236
		private readonly float[] _levelBonuses = new float[3];

		// Token: 0x020000D4 RID: 212
		public enum EffectIncrementType
		{
			// Token: 0x0400061E RID: 1566
			Invalid = -1,
			// Token: 0x0400061F RID: 1567
			Add,
			// Token: 0x04000620 RID: 1568
			AddFactor
		}
	}
}
