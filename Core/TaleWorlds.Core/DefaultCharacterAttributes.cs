using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x0200004B RID: 75
	public class DefaultCharacterAttributes
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001459C File Offset: 0x0001279C
		private static DefaultCharacterAttributes Instance
		{
			get
			{
				return Game.Current.DefaultCharacterAttributes;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x000145A8 File Offset: 0x000127A8
		public static CharacterAttribute Vigor
		{
			get
			{
				return DefaultCharacterAttributes.Instance._vigor;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x000145B4 File Offset: 0x000127B4
		public static CharacterAttribute Control
		{
			get
			{
				return DefaultCharacterAttributes.Instance._control;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000145C0 File Offset: 0x000127C0
		public static CharacterAttribute Endurance
		{
			get
			{
				return DefaultCharacterAttributes.Instance._endurance;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x000145CC File Offset: 0x000127CC
		public static CharacterAttribute Cunning
		{
			get
			{
				return DefaultCharacterAttributes.Instance._cunning;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000145D8 File Offset: 0x000127D8
		public static CharacterAttribute Social
		{
			get
			{
				return DefaultCharacterAttributes.Instance._social;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x000145E4 File Offset: 0x000127E4
		public static CharacterAttribute Intelligence
		{
			get
			{
				return DefaultCharacterAttributes.Instance._intelligence;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000145F0 File Offset: 0x000127F0
		private CharacterAttribute Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<CharacterAttribute>(new CharacterAttribute(stringId));
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014607 File Offset: 0x00012807
		internal DefaultCharacterAttributes()
		{
			this.RegisterAll();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014618 File Offset: 0x00012818
		private void RegisterAll()
		{
			this._vigor = this.Create("vigor");
			this._control = this.Create("control");
			this._endurance = this.Create("endurance");
			this._cunning = this.Create("cunning");
			this._social = this.Create("social");
			this._intelligence = this.Create("intelligence");
			this.InitializeAll();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00014694 File Offset: 0x00012894
		private void InitializeAll()
		{
			this._vigor.Initialize(new TextObject("{=YWkdD7Ki}Vigor", null), new TextObject("{=jJ9sLOLb}Vigor represents the ability to move with speed and force. It's important for melee combat.", null), new TextObject("{=Ve8xoa3i}VIG", null));
			this._control.Initialize(new TextObject("{=controlskill}Control", null), new TextObject("{=vx0OCvaj}Control represents the ability to use strength without sacrificing precision. It's necessary for using ranged weapons.", null), new TextObject("{=HuXafdmR}CTR", null));
			this._endurance.Initialize(new TextObject("{=kvOavzcs}Endurance", null), new TextObject("{=K8rCOQUZ}Endurance is the ability to perform taxing physical activity for a long time.", null), new TextObject("{=d2ApwXJr}END", null));
			this._cunning.Initialize(new TextObject("{=JZM1mQvb}Cunning", null), new TextObject("{=YO5LUfiO}Cunning is the ability to predict what other people will do, and to outwit their plans.", null), new TextObject("{=tH6Ooj0P}CNG", null));
			this._social.Initialize(new TextObject("{=socialskill}Social", null), new TextObject("{=XMDTt96y}Social is the ability to understand people's motivations and to sway them.", null), new TextObject("{=PHoxdReD}SOC", null));
			this._intelligence.Initialize(new TextObject("{=sOrJoxiC}Intelligence", null), new TextObject("{=TeUtEGV0}Intelligence represents aptitude for reading and theoretical learning.", null), new TextObject("{=Bn7IsMpu}INT", null));
		}

		// Token: 0x040002B3 RID: 691
		private CharacterAttribute _control;

		// Token: 0x040002B4 RID: 692
		private CharacterAttribute _vigor;

		// Token: 0x040002B5 RID: 693
		private CharacterAttribute _endurance;

		// Token: 0x040002B6 RID: 694
		private CharacterAttribute _cunning;

		// Token: 0x040002B7 RID: 695
		private CharacterAttribute _social;

		// Token: 0x040002B8 RID: 696
		private CharacterAttribute _intelligence;
	}
}
