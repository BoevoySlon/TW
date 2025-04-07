using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000399 RID: 921
	public interface ICraftingCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003778 RID: 14200
		IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> CraftingOrders { get; }

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003779 RID: 14201
		IReadOnlyCollection<WeaponDesign> CraftingHistory { get; }

		// Token: 0x0600377A RID: 14202
		void CompleteOrder(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero);

		// Token: 0x0600377B RID: 14203
		ItemModifier GetCurrentItemModifier();

		// Token: 0x0600377C RID: 14204
		void SetCurrentItemModifier(ItemModifier modifier);

		// Token: 0x0600377D RID: 14205
		void SetCraftedWeaponName(ItemObject craftedWeaponItem, TextObject name);

		// Token: 0x0600377E RID: 14206
		void GetOrderResult(CraftingOrder craftingOrder, ItemObject craftedItem, out bool isSucceed, out TextObject orderRemark, out TextObject orderResult, out int finalPrice);

		// Token: 0x0600377F RID: 14207
		int GetCraftingDifficulty(WeaponDesign weaponDesign);

		// Token: 0x06003780 RID: 14208
		bool CanHeroUsePart(Hero hero, CraftingPiece craftingPiece);

		// Token: 0x06003781 RID: 14209
		int GetHeroCraftingStamina(Hero hero);

		// Token: 0x06003782 RID: 14210
		void SetHeroCraftingStamina(Hero hero, int value);

		// Token: 0x06003783 RID: 14211
		int GetMaxHeroCraftingStamina(Hero hero);

		// Token: 0x06003784 RID: 14212
		void DoRefinement(Hero hero, Crafting.RefiningFormula refineFormula);

		// Token: 0x06003785 RID: 14213
		void DoSmelting(Hero currentCraftingHero, EquipmentElement equipmentElement);

		// Token: 0x06003786 RID: 14214
		ItemObject CreateCraftedWeaponInFreeBuildMode(Hero hero, WeaponDesign currentWeaponDesign, ItemModifier weaponModifier = null);

		// Token: 0x06003787 RID: 14215
		ItemObject CreateCraftedWeaponInCraftingOrderMode(Hero crafterHero, CraftingOrder craftingOrder, WeaponDesign weaponDesign);

		// Token: 0x06003788 RID: 14216
		bool IsOpened(CraftingPiece craftingPiece, CraftingTemplate craftingTemplate);

		// Token: 0x06003789 RID: 14217
		void InitializeCraftingElements();

		// Token: 0x0600378A RID: 14218
		CraftingOrder CreateCustomOrderForHero(Hero orderOwner, float orderDifficulty = -1f, WeaponDesign weaponDesign = null, CraftingTemplate craftingTemplate = null);

		// Token: 0x0600378B RID: 14219
		void CancelCustomOrder(Town town, CraftingOrder craftingOrder);
	}
}
