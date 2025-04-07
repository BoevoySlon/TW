using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000051 RID: 81
	public class DefaultSkills
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00016253 File Offset: 0x00014453
		private static DefaultSkills Instance
		{
			get
			{
				return Game.Current.DefaultSkills;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001625F File Offset: 0x0001445F
		public static SkillObject OneHanded
		{
			get
			{
				return DefaultSkills.Instance._skillOneHanded;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001626B File Offset: 0x0001446B
		public static SkillObject TwoHanded
		{
			get
			{
				return DefaultSkills.Instance._skillTwoHanded;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00016277 File Offset: 0x00014477
		public static SkillObject Polearm
		{
			get
			{
				return DefaultSkills.Instance._skillPolearm;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00016283 File Offset: 0x00014483
		public static SkillObject Bow
		{
			get
			{
				return DefaultSkills.Instance._skillBow;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001628F File Offset: 0x0001448F
		public static SkillObject Crossbow
		{
			get
			{
				return DefaultSkills.Instance._skillCrossbow;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001629B File Offset: 0x0001449B
		public static SkillObject Throwing
		{
			get
			{
				return DefaultSkills.Instance._skillThrowing;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000162A7 File Offset: 0x000144A7
		public static SkillObject Riding
		{
			get
			{
				return DefaultSkills.Instance._skillRiding;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x000162B3 File Offset: 0x000144B3
		public static SkillObject Athletics
		{
			get
			{
				return DefaultSkills.Instance._skillAthletics;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x000162BF File Offset: 0x000144BF
		public static SkillObject Crafting
		{
			get
			{
				return DefaultSkills.Instance._skillCrafting;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x000162CB File Offset: 0x000144CB
		public static SkillObject Tactics
		{
			get
			{
				return DefaultSkills.Instance._skillTactics;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000162D7 File Offset: 0x000144D7
		public static SkillObject Scouting
		{
			get
			{
				return DefaultSkills.Instance._skillScouting;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x000162E3 File Offset: 0x000144E3
		public static SkillObject Roguery
		{
			get
			{
				return DefaultSkills.Instance._skillRoguery;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000162EF File Offset: 0x000144EF
		public static SkillObject Charm
		{
			get
			{
				return DefaultSkills.Instance._skillCharm;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000162FB File Offset: 0x000144FB
		public static SkillObject Leadership
		{
			get
			{
				return DefaultSkills.Instance._skillLeadership;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00016307 File Offset: 0x00014507
		public static SkillObject Trade
		{
			get
			{
				return DefaultSkills.Instance._skillTrade;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00016313 File Offset: 0x00014513
		public static SkillObject Steward
		{
			get
			{
				return DefaultSkills.Instance._skillSteward;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001631F File Offset: 0x0001451F
		public static SkillObject Medicine
		{
			get
			{
				return DefaultSkills.Instance._skillMedicine;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001632B File Offset: 0x0001452B
		public static SkillObject Engineering
		{
			get
			{
				return DefaultSkills.Instance._skillEngineering;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00016337 File Offset: 0x00014537
		private SkillObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<SkillObject>(new SkillObject(stringId));
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00016350 File Offset: 0x00014550
		private void InitializeAll()
		{
			this._skillOneHanded.Initialize(new TextObject("{=PiHpR4QL}One Handed", null), new TextObject("{=yEkSSqIm}Mastery of fighting with one-handed weapons either with a shield or without.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
			this._skillTwoHanded.Initialize(new TextObject("{=t78atYqH}Two Handed", null), new TextObject("{=eoLbkhsY}Mastery of fighting with two-handed weapons of average length such as bigger axes and swords.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
			this._skillPolearm.Initialize(new TextObject("{=haax8kMa}Polearm", null), new TextObject("{=iKmXX7i3}Mastery of the spear, lance, staff and other polearms, both one-handed and two-handed.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Vigor);
			this._skillBow.Initialize(new TextObject("{=5rj7xQE4}Bow", null), new TextObject("{=FLf5J3su}Familarity with bows and physical conditioning to shoot with them effectively.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
			this._skillCrossbow.Initialize(new TextObject("{=TTWL7RLe}Crossbow", null), new TextObject("{=haV3nLYA}Knowledge of operating and maintaining crossbows.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
			this._skillThrowing.Initialize(new TextObject("{=2wclahIJ}Throwing", null), new TextObject("{=NwTpATW5}Mastery of throwing projectiles accurately and with power.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Control);
			this._skillRiding.Initialize(new TextObject("{=p9i3zRm9}Riding", null), new TextObject("{=H9Zamrao}The ability to control a horse, to keep your balance when it moves suddenly or unexpectedly, as well as general knowledge of horses, including their care and breeding.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
			this._skillAthletics.Initialize(new TextObject("{=skZS2UlW}Athletics", null), new TextObject("{=bVD9j0wI}Physical fitness, speed and balance.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
			this._skillCrafting.Initialize(new TextObject("{=smithingskill}Smithing", null), new TextObject("{=xWbkjccP}The knowledge of how to forge metal, match handle to blade, turn poles, sew scales, and other skills useful in the assembly of weapons and armor", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Endurance);
			this._skillScouting.Initialize(new TextObject("{=LJ6Krlbr}Scouting", null), new TextObject("{=kmBxaJZd}Knowledge of how to scan the wilderness for life. You can follow tracks, spot movement in the undergrowth, and spot an enemy across the valley from a flash of light on spearpoints or a dustcloud.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Cunning);
			this._skillTactics.Initialize(new TextObject("{=m8o51fc7}Tactics", null), new TextObject("{=FQOFDrAu}Your judgment of how troops will perform in contact. This allows you to make a good prediction of when an unorthodox tactic will work, and when it won't.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Cunning);
			this._skillRoguery.Initialize(new TextObject("{=V0ZMJ0PX}Roguery", null), new TextObject("{=81YLbLok}Experience with the darker side of human life. You can tell when a guard wants a bribe, you know how to intimidate someone, and have a good sense of what you can and can't get away with.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Cunning);
			this._skillCharm.Initialize(new TextObject("{=EGeY1gfs}Charm", null), new TextObject("{=VajIVjkc}The ability to make a person like and trust you. You can make a good guess at people's motivations and the kinds of arguments to which they'll respond.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Social);
			this._skillLeadership.Initialize(new TextObject("{=HsLfmEmb}Leadership", null), new TextObject("{=97EmbcHQ}The ability to inspire. You can fill individuals with confidence and stir up enthusiasm and courage in larger groups.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(DefaultCharacterAttributes.Social);
			this._skillTrade.Initialize(new TextObject("{=GmcgoiGy}Trade", null), new TextObject("{=lsJMCkZy}Familiarity with the most common goods in the marketplace and their prices, as well as the ability to spot defective goods or tell if you've been shortchanged in quantity", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Social);
			this._skillSteward.Initialize(new TextObject("{=stewardskill}Steward", null), new TextObject("{=2K0iVRkW}Ability to organize a group and manage logistics. This helps you to run an estate or administer a town, and can increase the size of a party that you lead or in which you serve as quartermaster.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
			this._skillMedicine.Initialize(new TextObject("{=JKH59XNp}Medicine", null), new TextObject("{=igg5sEh3}Knowledge of how to staunch bleeding, to set broken bones, to remove embedded weapons and clean wounds to prevent infection, and to apply poultices to relieve pain and soothe inflammation.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
			this._skillEngineering.Initialize(new TextObject("{=engineeringskill}Engineering", null), new TextObject("{=hbaMnpVR}Knowledge of how to make things that can withstand powerful forces without collapsing. Useful for building both structures and the devices that knock them down.", null), SkillObject.SkillTypeEnum.Party).SetAttribute(DefaultCharacterAttributes.Intelligence);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00016675 File Offset: 0x00014875
		public DefaultSkills()
		{
			this.RegisterAll();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00016684 File Offset: 0x00014884
		private void RegisterAll()
		{
			this._skillOneHanded = this.Create("OneHanded");
			this._skillTwoHanded = this.Create("TwoHanded");
			this._skillPolearm = this.Create("Polearm");
			this._skillBow = this.Create("Bow");
			this._skillCrossbow = this.Create("Crossbow");
			this._skillThrowing = this.Create("Throwing");
			this._skillRiding = this.Create("Riding");
			this._skillAthletics = this.Create("Athletics");
			this._skillCrafting = this.Create("Crafting");
			this._skillTactics = this.Create("Tactics");
			this._skillScouting = this.Create("Scouting");
			this._skillRoguery = this.Create("Roguery");
			this._skillCharm = this.Create("Charm");
			this._skillTrade = this.Create("Trade");
			this._skillSteward = this.Create("Steward");
			this._skillLeadership = this.Create("Leadership");
			this._skillMedicine = this.Create("Medicine");
			this._skillEngineering = this.Create("Engineering");
			this.InitializeAll();
		}

		// Token: 0x04000307 RID: 775
		private SkillObject _skillEngineering;

		// Token: 0x04000308 RID: 776
		private SkillObject _skillMedicine;

		// Token: 0x04000309 RID: 777
		private SkillObject _skillLeadership;

		// Token: 0x0400030A RID: 778
		private SkillObject _skillSteward;

		// Token: 0x0400030B RID: 779
		private SkillObject _skillTrade;

		// Token: 0x0400030C RID: 780
		private SkillObject _skillCharm;

		// Token: 0x0400030D RID: 781
		private SkillObject _skillRoguery;

		// Token: 0x0400030E RID: 782
		private SkillObject _skillScouting;

		// Token: 0x0400030F RID: 783
		private SkillObject _skillTactics;

		// Token: 0x04000310 RID: 784
		private SkillObject _skillCrafting;

		// Token: 0x04000311 RID: 785
		private SkillObject _skillAthletics;

		// Token: 0x04000312 RID: 786
		private SkillObject _skillRiding;

		// Token: 0x04000313 RID: 787
		private SkillObject _skillThrowing;

		// Token: 0x04000314 RID: 788
		private SkillObject _skillCrossbow;

		// Token: 0x04000315 RID: 789
		private SkillObject _skillBow;

		// Token: 0x04000316 RID: 790
		private SkillObject _skillPolearm;

		// Token: 0x04000317 RID: 791
		private SkillObject _skillTwoHanded;

		// Token: 0x04000318 RID: 792
		private SkillObject _skillOneHanded;
	}
}
