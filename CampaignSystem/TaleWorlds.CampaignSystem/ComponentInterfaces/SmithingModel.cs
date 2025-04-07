using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000178 RID: 376
	public abstract class SmithingModel : GameModel
	{
		// Token: 0x06001971 RID: 6513
		public abstract int GetCraftingPartDifficulty(CraftingPiece craftingPiece);

		// Token: 0x06001972 RID: 6514
		public abstract int CalculateWeaponDesignDifficulty(WeaponDesign weaponDesign);

		// Token: 0x06001973 RID: 6515
		public abstract ItemModifier GetCraftedWeaponModifier(WeaponDesign weaponDesign, Hero weaponsmith);

		// Token: 0x06001974 RID: 6516
		public abstract IEnumerable<Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith);

		// Token: 0x06001975 RID: 6517
		public abstract ItemObject GetCraftingMaterialItem(CraftingMaterials craftingMaterial);

		// Token: 0x06001976 RID: 6518
		public abstract int[] GetSmeltingOutputForItem(ItemObject item);

		// Token: 0x06001977 RID: 6519
		public abstract int GetSkillXpForRefining(ref Crafting.RefiningFormula refineFormula);

		// Token: 0x06001978 RID: 6520
		public abstract int GetSkillXpForSmelting(ItemObject item);

		// Token: 0x06001979 RID: 6521
		public abstract int GetSkillXpForSmithingInFreeBuildMode(ItemObject item);

		// Token: 0x0600197A RID: 6522
		public abstract int GetSkillXpForSmithingInCraftingOrderMode(ItemObject item);

		// Token: 0x0600197B RID: 6523
		public abstract int[] GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign);

		// Token: 0x0600197C RID: 6524
		public abstract int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero);

		// Token: 0x0600197D RID: 6525
		public abstract int GetEnergyCostForSmithing(ItemObject item, Hero hero);

		// Token: 0x0600197E RID: 6526
		public abstract int GetEnergyCostForSmelting(ItemObject item, Hero hero);

		// Token: 0x0600197F RID: 6527
		public abstract float ResearchPointsNeedForNewPart(int totalPartCount, int openedPartCount);

		// Token: 0x06001980 RID: 6528
		public abstract int GetPartResearchGainForSmeltingItem(ItemObject item, Hero hero);

		// Token: 0x06001981 RID: 6529
		public abstract int GetPartResearchGainForSmithingItem(ItemObject item, Hero hero, bool isFreeBuildMode);
	}
}
