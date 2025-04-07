using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C9 RID: 201
	public static class WeaponComponentDataExtensions
	{
		// Token: 0x060009D8 RID: 2520 RVA: 0x000205DF File Offset: 0x0001E7DF
		public static int GetModifiedThrustDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.ThrustDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.ThrustDamage);
			}
			return componentData.ThrustDamage;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00020600 File Offset: 0x0001E800
		public static int GetModifiedSwingDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.SwingDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.SwingDamage);
			}
			return componentData.SwingDamage;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00020621 File Offset: 0x0001E821
		public static int GetModifiedMissileDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MissileDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.MissileDamage);
			}
			return componentData.MissileDamage;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00020642 File Offset: 0x0001E842
		public static int GetModifiedThrustSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.ThrustSpeed > 0)
			{
				return itemModifier.ModifySpeed(componentData.ThrustSpeed);
			}
			return componentData.ThrustSpeed;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00020663 File Offset: 0x0001E863
		public static int GetModifiedSwingSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.SwingSpeed > 0)
			{
				return itemModifier.ModifySpeed(componentData.SwingSpeed);
			}
			return componentData.SwingSpeed;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00020684 File Offset: 0x0001E884
		public static int GetModifiedMissileSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MissileSpeed > 0)
			{
				return itemModifier.ModifyMissileSpeed(componentData.MissileSpeed);
			}
			return componentData.MissileSpeed;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x000206A5 File Offset: 0x0001E8A5
		public static int GetModifiedHandling(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.Handling > 0)
			{
				return itemModifier.ModifySpeed(componentData.Handling);
			}
			return componentData.Handling;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x000206C6 File Offset: 0x0001E8C6
		public static short GetModifiedStackCount(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MaxDataValue > 0)
			{
				return itemModifier.ModifyStackCount(componentData.MaxDataValue);
			}
			return componentData.MaxDataValue;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000206E7 File Offset: 0x0001E8E7
		public static short GetModifiedMaximumHitPoints(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MaxDataValue > 0)
			{
				return itemModifier.ModifyHitPoints(componentData.MaxDataValue);
			}
			return componentData.MaxDataValue;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00020708 File Offset: 0x0001E908
		public static int GetModifiedArmor(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.BodyArmor > 0)
			{
				return itemModifier.ModifyArmor(componentData.BodyArmor);
			}
			return componentData.BodyArmor;
		}
	}
}
