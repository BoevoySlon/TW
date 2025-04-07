using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000021 RID: 33
	public class CharacterCode
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006BB1 File Offset: 0x00004DB1
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.Code);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006BBE File Offset: 0x00004DBE
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00006BC6 File Offset: 0x00004DC6
		public string EquipmentCode { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006BCF File Offset: 0x00004DCF
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00006BD7 File Offset: 0x00004DD7
		public string Code { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00006BE0 File Offset: 0x00004DE0
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00006BE8 File Offset: 0x00004DE8
		public bool IsFemale { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006BF1 File Offset: 0x00004DF1
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00006BF9 File Offset: 0x00004DF9
		public bool IsHero { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006C02 File Offset: 0x00004E02
		public float FaceDirtAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00006C09 File Offset: 0x00004E09
		public Banner Banner
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006C0C File Offset: 0x00004E0C
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00006C14 File Offset: 0x00004E14
		public FormationClass FormationClass { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006C1D File Offset: 0x00004E1D
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00006C25 File Offset: 0x00004E25
		public uint Color1 { get; set; } = Color.White.ToUnsignedInteger();

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006C2E File Offset: 0x00004E2E
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006C36 File Offset: 0x00004E36
		public uint Color2 { get; set; } = Color.White.ToUnsignedInteger();

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006C3F File Offset: 0x00004E3F
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006C47 File Offset: 0x00004E47
		public int Race { get; private set; }

		// Token: 0x060001AC RID: 428 RVA: 0x00006C50 File Offset: 0x00004E50
		public Equipment CalculateEquipment()
		{
			if (this.EquipmentCode == null)
			{
				return null;
			}
			return Equipment.CreateFromEquipmentCode(this.EquipmentCode);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006C68 File Offset: 0x00004E68
		private CharacterCode()
		{
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public static CharacterCode CreateFrom(BasicCharacterObject character)
		{
			CharacterCode characterCode = new CharacterCode();
			Equipment equipment = character.Equipment;
			string text = (equipment != null) ? equipment.CalculateEquipmentCode() : null;
			characterCode.EquipmentCode = text;
			characterCode.BodyProperties = character.GetBodyProperties(character.Equipment, -1);
			characterCode.IsFemale = character.IsFemale;
			characterCode.IsHero = character.IsHero;
			characterCode.FormationClass = character.DefaultFormationClass;
			characterCode.Race = character.Race;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateFrom");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(text);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)characterCode.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			characterCode.Code = mbstringBuilder.ToStringAndRelease();
			return characterCode;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006E64 File Offset: 0x00005064
		public static CharacterCode CreateFrom(string equipmentCode, BodyProperties bodyProperties, bool isFemale, bool isHero, uint color1, uint color2, FormationClass formationClass, int race)
		{
			CharacterCode characterCode = new CharacterCode();
			characterCode.EquipmentCode = equipmentCode;
			characterCode.BodyProperties = bodyProperties;
			characterCode.IsFemale = isFemale;
			characterCode.IsHero = isHero;
			characterCode.Color1 = color1;
			characterCode.Color2 = color2;
			characterCode.FormationClass = formationClass;
			characterCode.Race = race;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateFrom");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(equipmentCode);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)characterCode.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			characterCode.Code = mbstringBuilder.ToStringAndRelease();
			return characterCode;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007000 File Offset: 0x00005200
		public string CreateNewCodeString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateNewCodeString");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.EquipmentCode);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)this.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007156 File Offset: 0x00005356
		public static CharacterCode CreateEmpty()
		{
			return new CharacterCode();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007160 File Offset: 0x00005360
		public static CharacterCode CreateFrom(string code)
		{
			CharacterCode characterCode = new CharacterCode();
			int num = 0;
			int num2;
			for (num2 = code.IndexOf("@---@", StringComparison.InvariantCulture); num2 == num; num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture))
			{
				num = num2 + 5;
			}
			string equipmentCode = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string keyValue = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string value = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string value2 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string value3 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string value4 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string value5 = code.Substring(num, num2 - num);
			num = num2 + 5;
			num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			string value6 = (num2 >= 0) ? code.Substring(num, num2 - num) : code.Substring(num);
			characterCode.EquipmentCode = equipmentCode;
			BodyProperties bodyProperties;
			if (BodyProperties.FromString(keyValue, out bodyProperties))
			{
				characterCode.BodyProperties = bodyProperties;
			}
			else
			{
				Debug.FailedAssert("Cannot read the character code body property", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\CharacterCode.cs", "CreateFrom", 235);
			}
			characterCode.IsFemale = (Convert.ToInt32(value) == 1);
			characterCode.IsHero = (Convert.ToInt32(value2) == 1);
			characterCode.FormationClass = (FormationClass)Convert.ToInt32(value3);
			characterCode.Color1 = Convert.ToUInt32(value4);
			characterCode.Color2 = Convert.ToUInt32(value5);
			characterCode.Race = Convert.ToInt32(value6);
			characterCode.Code = code;
			return characterCode;
		}

		// Token: 0x0400015D RID: 349
		public const string SpecialCodeSeparator = "@---@";

		// Token: 0x0400015E RID: 350
		public const int SpecialCodeSeparatorLength = 5;

		// Token: 0x04000161 RID: 353
		public BodyProperties BodyProperties;
	}
}
