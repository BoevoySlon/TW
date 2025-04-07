using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000060 RID: 96
	public static class FormationClassExtensions
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x000173A0 File Offset: 0x000155A0
		public static string GetName(this FormationClass formationClass)
		{
			switch (formationClass)
			{
			case FormationClass.Infantry:
				return "Infantry";
			case FormationClass.Ranged:
				return "Ranged";
			case FormationClass.Cavalry:
				return "Cavalry";
			case FormationClass.HorseArcher:
				return "HorseArcher";
			case FormationClass.NumberOfDefaultFormations:
				return "Skirmisher";
			case FormationClass.HeavyInfantry:
				return "HeavyInfantry";
			case FormationClass.LightCavalry:
				return "LightCavalry";
			case FormationClass.HeavyCavalry:
				return "HeavyCavalry";
			case FormationClass.NumberOfRegularFormations:
				return "General";
			case FormationClass.Bodyguard:
				return "Bodyguard";
			default:
				return "Unset";
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00017420 File Offset: 0x00015620
		public static TextObject GetLocalizedName(this FormationClass formationClass)
		{
			string id = "str_troop_group_name";
			int num = (int)formationClass;
			return GameTexts.FindText(id, num.ToString());
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00017440 File Offset: 0x00015640
		public static TroopUsageFlags GetTroopUsageFlags(this FormationClass troopClass)
		{
			switch (troopClass)
			{
			case FormationClass.Ranged:
				return TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			case FormationClass.Cavalry:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
			case FormationClass.HorseArcher:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			}
			return TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00017474 File Offset: 0x00015674
		public static TroopType GetTroopTypeForRegularFormation(this FormationClass formationClass)
		{
			TroopType result = TroopType.Invalid;
			switch (formationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.HeavyInfantry:
				result = TroopType.Infantry;
				break;
			case FormationClass.Ranged:
			case FormationClass.NumberOfDefaultFormations:
				result = TroopType.Ranged;
				break;
			case FormationClass.Cavalry:
			case FormationClass.HorseArcher:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				result = TroopType.Cavalry;
				break;
			default:
				Debug.FailedAssert(string.Format("Undefined formation class {0} for TroopType!", formationClass), "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\FormationClass.cs", "GetTroopTypeForRegularFormation", 311);
				break;
			}
			return result;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000174DC File Offset: 0x000156DC
		public static bool IsDefaultFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfDefaultFormations;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000174F8 File Offset: 0x000156F8
		public static bool IsRegularFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfRegularFormations;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00017511 File Offset: 0x00015711
		public static FormationClass FallbackClass(this FormationClass formationClass)
		{
			if (formationClass == FormationClass.Ranged || formationClass == FormationClass.NumberOfDefaultFormations)
			{
				return FormationClass.Ranged;
			}
			if (formationClass == FormationClass.Cavalry || formationClass == FormationClass.HeavyCavalry)
			{
				return FormationClass.Cavalry;
			}
			if (formationClass == FormationClass.HorseArcher || formationClass == FormationClass.LightCavalry)
			{
				return FormationClass.HorseArcher;
			}
			return FormationClass.Infantry;
		}

		// Token: 0x0400037A RID: 890
		public const TroopUsageFlags DefaultInfantryTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x0400037B RID: 891
		public const TroopUsageFlags DefaultRangedTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x0400037C RID: 892
		public const TroopUsageFlags DefaultCavalryTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x0400037D RID: 893
		public const TroopUsageFlags DefaultHorseArcherTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x0400037E RID: 894
		public static FormationClass[] FormationClassValues = (FormationClass[])Enum.GetValues(typeof(FormationClass));
	}
}
