using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000017 RID: 23
	public class BasicCultureObject : MBObjectBase
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00005EEE File Offset: 0x000040EE
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00005EF6 File Offset: 0x000040F6
		public TextObject Name { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00005EFF File Offset: 0x000040FF
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00005F07 File Offset: 0x00004107
		public bool IsMainCulture { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00005F10 File Offset: 0x00004110
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00005F18 File Offset: 0x00004118
		public bool IsBandit { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00005F21 File Offset: 0x00004121
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00005F29 File Offset: 0x00004129
		public bool CanHaveSettlement { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00005F32 File Offset: 0x00004132
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00005F3A File Offset: 0x0000413A
		public uint Color { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00005F43 File Offset: 0x00004143
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00005F4B File Offset: 0x0000414B
		public uint Color2 { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00005F54 File Offset: 0x00004154
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00005F5C File Offset: 0x0000415C
		public uint ClothAlternativeColor { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00005F65 File Offset: 0x00004165
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00005F6D File Offset: 0x0000416D
		public uint ClothAlternativeColor2 { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00005F76 File Offset: 0x00004176
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00005F7E File Offset: 0x0000417E
		public uint BackgroundColor1 { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00005F87 File Offset: 0x00004187
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00005F8F File Offset: 0x0000418F
		public uint ForegroundColor1 { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00005F98 File Offset: 0x00004198
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005FA0 File Offset: 0x000041A0
		public uint BackgroundColor2 { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005FA9 File Offset: 0x000041A9
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005FB1 File Offset: 0x000041B1
		public uint ForegroundColor2 { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005FBA File Offset: 0x000041BA
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005FC2 File Offset: 0x000041C2
		public string EncounterBackgroundMesh { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005FCB File Offset: 0x000041CB
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00005FD3 File Offset: 0x000041D3
		public string BannerKey { get; set; }

		// Token: 0x06000163 RID: 355 RVA: 0x00005FDC File Offset: 0x000041DC
		public override string ToString()
		{
			return this.Name.ToString();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005FEC File Offset: 0x000041EC
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.Name = new TextObject(node.Attributes.get_ItemOf("name").Value, null);
			this.Color = ((node.Attributes.get_ItemOf("color") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("color").Value, 16));
			this.Color2 = ((node.Attributes.get_ItemOf("color2") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("color2").Value, 16));
			this.ClothAlternativeColor = ((node.Attributes.get_ItemOf("cloth_alternative_color1") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("cloth_alternative_color1").Value, 16));
			this.ClothAlternativeColor2 = ((node.Attributes.get_ItemOf("cloth_alternative_color2") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("cloth_alternative_color2").Value, 16));
			this.BackgroundColor1 = ((node.Attributes.get_ItemOf("banner_background_color1") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("banner_background_color1").Value, 16));
			this.ForegroundColor1 = ((node.Attributes.get_ItemOf("banner_foreground_color1") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("banner_foreground_color1").Value, 16));
			this.BackgroundColor2 = ((node.Attributes.get_ItemOf("banner_background_color2") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("banner_background_color2").Value, 16));
			this.ForegroundColor2 = ((node.Attributes.get_ItemOf("banner_foreground_color2") == null) ? uint.MaxValue : Convert.ToUInt32(node.Attributes.get_ItemOf("banner_foreground_color2").Value, 16));
			this.IsMainCulture = (node.Attributes.get_ItemOf("is_main_culture") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("is_main_culture").Value));
			this.EncounterBackgroundMesh = ((node.Attributes.get_ItemOf("encounter_background_mesh") == null) ? null : node.Attributes.get_ItemOf("encounter_background_mesh").Value);
			this.BannerKey = ((node.Attributes.get_ItemOf("faction_banner_key") == null) ? null : node.Attributes.get_ItemOf("faction_banner_key").Value);
			this.IsBandit = false;
			this.IsBandit = (node.Attributes.get_ItemOf("is_bandit") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("is_bandit").Value));
			this.CanHaveSettlement = false;
			this.CanHaveSettlement = (node.Attributes.get_ItemOf("can_have_settlement") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("can_have_settlement").Value));
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000062E8 File Offset: 0x000044E8
		public CultureCode GetCultureCode()
		{
			CultureCode result;
			if (Enum.TryParse<CultureCode>(base.StringId, true, out result))
			{
				return result;
			}
			Debug.FailedAssert("Could not get CultureCode from stringId: " + base.StringId, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCultureObject.cs", "GetCultureCode", 83);
			return CultureCode.Invalid;
		}
	}
}
