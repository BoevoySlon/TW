﻿using System;
using System.Collections.Generic;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000BA RID: 186
	public class SaveableCoreTypeDefiner : SaveableTypeDefiner
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0001F43C File Offset: 0x0001D63C
		public SaveableCoreTypeDefiner() : base(10000)
		{
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001F44C File Offset: 0x0001D64C
		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(ArmorComponent), 2, null);
			base.AddClassDefinition(typeof(Banner), 3, null);
			base.AddClassDefinition(typeof(BannerData), 4, null);
			base.AddClassDefinition(typeof(BasicCharacterObject), 5, null);
			base.AddClassDefinition(typeof(CharacterAttribute), 6, null);
			base.AddClassDefinition(typeof(CharacterSkills), 8, null);
			base.AddClassDefinition(typeof(WeaponDesign), 9, null);
			base.AddClassDefinition(typeof(CraftingPiece), 10, null);
			base.AddClassDefinition(typeof(CraftingTemplate), 11, null);
			base.AddClassDefinition(typeof(EntitySystem<>), 15, null);
			base.AddClassDefinition(typeof(Equipment), 16, null);
			base.AddClassDefinition(typeof(TradeItemComponent), 18, null);
			base.AddClassDefinition(typeof(GameType), 26, null);
			base.AddClassDefinition(typeof(HorseComponent), 27, null);
			base.AddClassDefinition(typeof(ItemCategory), 28, null);
			base.AddClassDefinition(typeof(ItemComponent), 29, null);
			base.AddClassDefinition(typeof(ItemModifier), 30, null);
			base.AddClassDefinition(typeof(ItemModifierGroup), 31, null);
			base.AddClassDefinition(typeof(ItemObject), 32, null);
			base.AddClassDefinition(typeof(MissionResult), 36, null);
			base.AddClassDefinition(typeof(PropertyObject), 38, null);
			base.AddClassDefinition(typeof(SkillObject), 39, null);
			base.AddClassDefinition(typeof(PropertyOwner<>), 40, null);
			base.AddClassDefinition(typeof(PropertyOwnerF<>), 41, null);
			base.AddClassDefinition(typeof(SiegeEngineType), 42, null);
			base.AddClassDefinition(typeof(WeaponDesignElement), 44, null);
			base.AddClassDefinition(typeof(WeaponComponent), 45, null);
			base.AddClassDefinition(typeof(WeaponComponentData), 46, null);
			base.AddClassDefinition(typeof(InformationData), 50, null);
			base.AddClassDefinition(typeof(MBFastRandom), 52, null);
			base.AddClassDefinition(typeof(BannerComponent), 53, null);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001F6A0 File Offset: 0x0001D8A0
		protected override void DefineStructTypes()
		{
			base.AddStructDefinition(typeof(ItemRosterElement), 1004, null);
			base.AddStructDefinition(typeof(UniqueTroopDescriptor), 1006, null);
			base.AddStructDefinition(typeof(StaticBodyProperties), 1009, null);
			base.AddStructDefinition(typeof(EquipmentElement), 1011, null);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001F708 File Offset: 0x0001D908
		protected override void DefineEnumTypes()
		{
			base.AddEnumDefinition(typeof(BattleSideEnum), 2001, null);
			base.AddEnumDefinition(typeof(Equipment.EquipmentType), 2006, null);
			base.AddEnumDefinition(typeof(WeaponFlags), 2007, null);
			base.AddEnumDefinition(typeof(FormationClass), 2008, null);
			base.AddEnumDefinition(typeof(BattleState), 2009, null);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001F783 File Offset: 0x0001D983
		protected override void DefineInterfaceTypes()
		{
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001F785 File Offset: 0x0001D985
		protected override void DefineRootClassTypes()
		{
			base.AddRootClassDefinition(typeof(Game), 4001, null);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001F79D File Offset: 0x0001D99D
		protected override void DefineGenericClassDefinitions()
		{
			base.ConstructGenericClassDefinition(typeof(Tuple<int, int>));
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001F7AF File Offset: 0x0001D9AF
		protected override void DefineGenericStructDefinitions()
		{
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		protected override void DefineContainerDefinitions()
		{
			base.ConstructContainerDefinition(typeof(ItemRosterElement[]));
			base.ConstructContainerDefinition(typeof(EquipmentElement[]));
			base.ConstructContainerDefinition(typeof(Equipment[]));
			base.ConstructContainerDefinition(typeof(WeaponDesignElement[]));
			base.ConstructContainerDefinition(typeof(List<ItemObject>));
			base.ConstructContainerDefinition(typeof(List<ItemComponent>));
			base.ConstructContainerDefinition(typeof(List<ItemModifier>));
			base.ConstructContainerDefinition(typeof(List<ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(List<CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(List<SkillObject>));
			base.ConstructContainerDefinition(typeof(List<ItemCategory>));
			base.ConstructContainerDefinition(typeof(List<CraftingPiece>));
			base.ConstructContainerDefinition(typeof(List<CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(List<SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(List<PropertyObject>));
			base.ConstructContainerDefinition(typeof(List<UniqueTroopDescriptor>));
			base.ConstructContainerDefinition(typeof(List<Equipment>));
			base.ConstructContainerDefinition(typeof(List<BannerData>));
			base.ConstructContainerDefinition(typeof(List<EquipmentElement>));
			base.ConstructContainerDefinition(typeof(List<WeaponDesign>));
			base.ConstructContainerDefinition(typeof(List<ItemRosterElement>));
			base.ConstructContainerDefinition(typeof(List<InformationData>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemCategory>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CraftingPiece>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, PropertyObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, SkillObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemComponent>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemModifier>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemCategory>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CraftingPiece>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, PropertyObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, SkillObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemComponent>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemModifier>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemCategory, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemCategory, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<SiegeEngineType, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<SkillObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<PropertyObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<PropertyObject, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<CharacterAttribute, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<CraftingTemplate, List<CraftingPiece>>));
			base.ConstructContainerDefinition(typeof(Dictionary<CraftingTemplate, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<long, Dictionary<long, int>>));
			base.ConstructContainerDefinition(typeof(Dictionary<int, Tuple<int, int>>));
			base.ConstructContainerDefinition(typeof(Dictionary<EquipmentElement, int>));
		}
	}
}
