using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000E8 RID: 232
	public class DefaultAgeModel : AgeModel
	{
		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0005ADB5 File Offset: 0x00058FB5
		public override int BecomeInfantAge
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0005ADB8 File Offset: 0x00058FB8
		public override int BecomeChildAge
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0005ADBB File Offset: 0x00058FBB
		public override int BecomeTeenagerAge
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0005ADBF File Offset: 0x00058FBF
		public override int HeroComesOfAge
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0005ADC3 File Offset: 0x00058FC3
		public override int BecomeOldAge
		{
			get
			{
				return 47;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0005ADC7 File Offset: 0x00058FC7
		public override int MaxAge
		{
			get
			{
				return 128;
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0005ADD0 File Offset: 0x00058FD0
		public override void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "")
		{
			if (character.Occupation == Occupation.TavernWench)
			{
				minimumAge = 20;
				maximumAge = 28;
				return;
			}
			if (character.Occupation == Occupation.Townsfolk)
			{
				if (additionalTags == "TavernVisitor")
				{
					minimumAge = 20;
					maximumAge = 60;
					return;
				}
				if (additionalTags == "TavernDrinker")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "SlowTownsman")
				{
					minimumAge = 50;
					maximumAge = 70;
					return;
				}
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "BroomsWoman")
				{
					minimumAge = 30;
					maximumAge = 45;
					return;
				}
				if (additionalTags == "Dancer")
				{
					minimumAge = 20;
					maximumAge = 28;
					return;
				}
				if (additionalTags == "Beggar")
				{
					minimumAge = 60;
					maximumAge = 90;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				if (additionalTags == "Notary" || additionalTags == "Barber")
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else if (character.Occupation == Occupation.Villager)
			{
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else
			{
				if (character.Occupation == Occupation.TavernGameHost)
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.Musician)
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.ArenaMaster)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.ShopWorker)
				{
					minimumAge = 18;
					maximumAge = 50;
					return;
				}
				if (character.Occupation == Occupation.Tavernkeeper)
				{
					minimumAge = 40;
					maximumAge = 80;
					return;
				}
				if (character.Occupation == Occupation.RansomBroker)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.Blacksmith || character.Occupation == Occupation.GoodsTrader || character.Occupation == Occupation.HorseTrader || character.Occupation == Occupation.Armorer || character.Occupation == Occupation.Weaponsmith)
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				if (additionalTags == "AlleyGangMember")
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = this.MaxAge;
				return;
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005B08C File Offset: 0x0005928C
		public override float GetSkillScalingModifierForAge(Hero hero, SkillObject skill, bool isByNaturalGrowth)
		{
			if (!isByNaturalGrowth)
			{
				return 1f;
			}
			float age = hero.Age;
			float result = 0f;
			if (age >= (float)this.BecomeChildAge && age < (float)this.BecomeTeenagerAge)
			{
				result = 0.2f;
			}
			else if (age >= (float)this.BecomeTeenagerAge && age < (float)this.HeroComesOfAge)
			{
				result = 0.5f;
			}
			else if (age >= (float)this.HeroComesOfAge)
			{
				if (skill == DefaultSkills.Riding)
				{
					result = 1f;
				}
				else
				{
					result = 0.8f;
				}
			}
			return result;
		}

		// Token: 0x04000703 RID: 1795
		public const string TavernVisitorTag = "TavernVisitor";

		// Token: 0x04000704 RID: 1796
		public const string TavernDrinkerTag = "TavernDrinker";

		// Token: 0x04000705 RID: 1797
		public const string SlowTownsmanTag = "SlowTownsman";

		// Token: 0x04000706 RID: 1798
		public const string TownsfolkCarryingStuffTag = "TownsfolkCarryingStuff";

		// Token: 0x04000707 RID: 1799
		public const string BroomsWomanTag = "BroomsWoman";

		// Token: 0x04000708 RID: 1800
		public const string DancerTag = "Dancer";

		// Token: 0x04000709 RID: 1801
		public const string BeggarTag = "Beggar";

		// Token: 0x0400070A RID: 1802
		public const string ChildTag = "Child";

		// Token: 0x0400070B RID: 1803
		public const string TeenagerTag = "Teenager";

		// Token: 0x0400070C RID: 1804
		public const string InfantTag = "Infant";

		// Token: 0x0400070D RID: 1805
		public const string NotaryTag = "Notary";

		// Token: 0x0400070E RID: 1806
		public const string BarberTag = "Barber";

		// Token: 0x0400070F RID: 1807
		public const string AlleyGangMemberTag = "AlleyGangMember";
	}
}
