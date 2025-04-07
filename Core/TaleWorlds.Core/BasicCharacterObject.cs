using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000016 RID: 22
	public class BasicCharacterObject : MBObjectBase
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004F66 File Offset: 0x00003166
		public virtual TextObject Name
		{
			get
			{
				return this._basicName;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004F6E File Offset: 0x0000316E
		private void SetName(TextObject name)
		{
			this._basicName = name;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004F77 File Offset: 0x00003177
		public override TextObject GetName()
		{
			return this.Name;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004F7F File Offset: 0x0000317F
		public override string ToString()
		{
			return this.Name.ToString();
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004F8C File Offset: 0x0000318C
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004F94 File Offset: 0x00003194
		public virtual MBBodyProperty BodyPropertyRange { get; protected set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004F9D File Offset: 0x0000319D
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004FA5 File Offset: 0x000031A5
		public int DefaultFormationGroup { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004FAE File Offset: 0x000031AE
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004FB6 File Offset: 0x000031B6
		public FormationClass DefaultFormationClass { get; protected set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004FBF File Offset: 0x000031BF
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004FC7 File Offset: 0x000031C7
		public float KnockbackResistance { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00004FD0 File Offset: 0x000031D0
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004FD8 File Offset: 0x000031D8
		public float KnockdownResistance { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004FE1 File Offset: 0x000031E1
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00004FE9 File Offset: 0x000031E9
		public float DismountResistance { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004FF2 File Offset: 0x000031F2
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00004FFA File Offset: 0x000031FA
		public FormationPositionPreference FormationPositionPreference { get; protected set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005003 File Offset: 0x00003203
		public bool IsInfantry
		{
			get
			{
				return !this.IsRanged && !this.IsMounted;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00005018 File Offset: 0x00003218
		public virtual bool IsMounted
		{
			get
			{
				return this._isMounted;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005020 File Offset: 0x00003220
		public virtual bool IsRanged
		{
			get
			{
				return this._isRanged;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005028 File Offset: 0x00003228
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005030 File Offset: 0x00003230
		public int Race { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005039 File Offset: 0x00003239
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00005041 File Offset: 0x00003241
		public virtual bool IsFemale { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000504A File Offset: 0x0000324A
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005052 File Offset: 0x00003252
		public bool FaceMeshCache { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000505B File Offset: 0x0000325B
		public virtual MBReadOnlyList<Equipment> AllEquipments
		{
			get
			{
				if (this._equipmentRoster == null)
				{
					return new MBList<Equipment>
					{
						MBEquipmentRoster.EmptyEquipment
					};
				}
				return this._equipmentRoster.AllEquipments;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005081 File Offset: 0x00003281
		public virtual Equipment Equipment
		{
			get
			{
				if (this._equipmentRoster == null)
				{
					return MBEquipmentRoster.EmptyEquipment;
				}
				return this._equipmentRoster.DefaultEquipment;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000509C File Offset: 0x0000329C
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000050A4 File Offset: 0x000032A4
		public bool IsObsolete { get; private set; }

		// Token: 0x0600011B RID: 283 RVA: 0x000050AD File Offset: 0x000032AD
		private bool HasCivilianEquipment()
		{
			return this.AllEquipments.Any((Equipment eq) => eq.IsCivilian);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000050D9 File Offset: 0x000032D9
		public void InitializeEquipmentsOnLoad(BasicCharacterObject character)
		{
			this._equipmentRoster = character._equipmentRoster;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000050E8 File Offset: 0x000032E8
		public Equipment GetFirstEquipment(bool civilianSet)
		{
			if (!civilianSet)
			{
				return this.Equipment;
			}
			if (!this.HasCivilianEquipment())
			{
				return this.Equipment;
			}
			return this.AllEquipments.FirstOrDefault((Equipment eq) => eq.IsCivilian);
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005138 File Offset: 0x00003338
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005140 File Offset: 0x00003340
		public virtual int Level { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005149 File Offset: 0x00003349
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005151 File Offset: 0x00003351
		public BasicCultureObject Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000515A File Offset: 0x0000335A
		public virtual bool IsPlayerCharacter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000515D File Offset: 0x0000335D
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00005165 File Offset: 0x00003365
		public virtual float Age
		{
			get
			{
				return this._age;
			}
			set
			{
				this._age = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000516E File Offset: 0x0000336E
		public virtual int HitPoints
		{
			get
			{
				return this.MaxHitPoints();
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005176 File Offset: 0x00003376
		public virtual BodyProperties GetBodyPropertiesMin(bool returnBaseValue = false)
		{
			return this.BodyPropertyRange.BodyPropertyMin;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005184 File Offset: 0x00003384
		protected void FillFrom(BasicCharacterObject character)
		{
			this._culture = character._culture;
			this.DefaultFormationClass = character.DefaultFormationClass;
			this.DefaultFormationGroup = character.DefaultFormationGroup;
			this.BodyPropertyRange = character.BodyPropertyRange;
			this.FormationPositionPreference = character.FormationPositionPreference;
			this.IsFemale = character.IsFemale;
			this.Race = character.Race;
			this.Level = character.Level;
			this._basicName = character._basicName;
			this._age = character._age;
			this.KnockbackResistance = character.KnockbackResistance;
			this.KnockdownResistance = character.KnockdownResistance;
			this.DismountResistance = character.DismountResistance;
			this.DefaultCharacterSkills = character.DefaultCharacterSkills;
			this.HairTags = character.HairTags;
			this.BeardTags = character.BeardTags;
			this.InitializeEquipmentsOnLoad(character);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005258 File Offset: 0x00003458
		public virtual BodyProperties GetBodyPropertiesMax()
		{
			return this.BodyPropertyRange.BodyPropertyMax;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005268 File Offset: 0x00003468
		public virtual BodyProperties GetBodyProperties(Equipment equipment, int seed = -1)
		{
			BodyProperties bodyPropertiesMin = this.GetBodyPropertiesMin(false);
			BodyProperties bodyPropertiesMax = this.GetBodyPropertiesMax();
			return FaceGen.GetRandomBodyProperties(this.Race, this.IsFemale, bodyPropertiesMin, bodyPropertiesMax, (int)((equipment != null) ? equipment.HairCoverType : ArmorComponent.HairCoverTypes.None), seed, this.HairTags, this.BeardTags, this.TattooTags);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000052B6 File Offset: 0x000034B6
		public virtual void UpdatePlayerCharacterBodyProperties(BodyProperties properties, int race, bool isFemale)
		{
			this.BodyPropertyRange.Init(properties, properties);
			this.Race = race;
			this.IsFemale = isFemale;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000052D3 File Offset: 0x000034D3
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000052DB File Offset: 0x000034DB
		public float FaceDirtAmount { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000052E4 File Offset: 0x000034E4
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000052EC File Offset: 0x000034EC
		public virtual string HairTags { get; set; } = "";

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000052F5 File Offset: 0x000034F5
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000052FD File Offset: 0x000034FD
		public virtual string BeardTags { get; set; } = "";

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005306 File Offset: 0x00003506
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000530E File Offset: 0x0000350E
		public virtual string TattooTags { get; set; } = "";

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005317 File Offset: 0x00003517
		public virtual bool IsHero
		{
			get
			{
				return this._isBasicHero;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000531F File Offset: 0x0000351F
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00005327 File Offset: 0x00003527
		public bool IsSoldier { get; private set; }

		// Token: 0x06000136 RID: 310 RVA: 0x00005330 File Offset: 0x00003530
		public BasicCharacterObject()
		{
			this.DefaultFormationClass = FormationClass.Infantry;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005360 File Offset: 0x00003560
		public int GetDefaultFaceSeed(int rank)
		{
			int num = base.StringId.GetDeterministicHashCode() * 6791 + rank * 197;
			return ((num >= 0) ? num : (-num)) % 2000;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005396 File Offset: 0x00003596
		public float GetStepSize()
		{
			return Math.Min(0.8f + 0.2f * (float)this.GetSkillValue(DefaultSkills.Athletics) * 0.00333333f, 1f);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000053C0 File Offset: 0x000035C0
		public bool HasMount()
		{
			return this.Equipment[10].Item != null;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000053E5 File Offset: 0x000035E5
		public virtual int MaxHitPoints()
		{
			return FaceGen.GetBaseMonsterFromRace(this.Race).HitPoints;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000053F8 File Offset: 0x000035F8
		public virtual float GetPower()
		{
			int num = this.Level + 10;
			return 0.2f + (float)(num * num) * 0.0025f;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000541F File Offset: 0x0000361F
		public virtual float GetBattlePower()
		{
			return 1f;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005426 File Offset: 0x00003626
		public virtual float GetMoraleResistance()
		{
			return 1f;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000542D File Offset: 0x0000362D
		public virtual int GetMountKeySeed()
		{
			return MBRandom.RandomInt();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005434 File Offset: 0x00003634
		public virtual int GetBattleTier()
		{
			if (this.IsHero)
			{
				return 7;
			}
			return MathF.Min(MathF.Max(MathF.Ceiling(((float)this.Level - 5f) / 5f), 0), 7);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005464 File Offset: 0x00003664
		public virtual int GetSkillValue(SkillObject skill)
		{
			return this.DefaultCharacterSkills.Skills.GetPropertyValue(skill);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005478 File Offset: 0x00003678
		protected void InitializeHeroBasicCharacterOnAfterLoad(BasicCharacterObject originCharacter)
		{
			this.IsSoldier = originCharacter.IsSoldier;
			this._isBasicHero = originCharacter._isBasicHero;
			this.DefaultCharacterSkills = originCharacter.DefaultCharacterSkills;
			this.HairTags = originCharacter.HairTags;
			this.BeardTags = originCharacter.BeardTags;
			this.TattooTags = originCharacter.TattooTags;
			this.BodyPropertyRange = originCharacter.BodyPropertyRange;
			this.IsFemale = originCharacter.IsFemale;
			this.Race = originCharacter.Race;
			this.Culture = originCharacter.Culture;
			this.DefaultFormationGroup = originCharacter.DefaultFormationGroup;
			this.DefaultFormationClass = originCharacter.DefaultFormationClass;
			this.FormationPositionPreference = originCharacter.FormationPositionPreference;
			this._equipmentRoster = originCharacter._equipmentRoster;
			this.KnockbackResistance = originCharacter.KnockbackResistance;
			this.KnockdownResistance = originCharacter.KnockdownResistance;
			this.DismountResistance = originCharacter.DismountResistance;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005554 File Offset: 0x00003754
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf("name");
			if (xmlAttribute != null)
			{
				this.SetName(new TextObject(xmlAttribute.Value, null));
			}
			this.HairTags = "";
			this.BeardTags = "";
			this.TattooTags = "";
			this.Race = 0;
			XmlAttribute xmlAttribute2 = node.Attributes.get_ItemOf("race");
			if (xmlAttribute2 != null)
			{
				this.Race = FaceGen.GetRaceOrDefault(xmlAttribute2.Value);
			}
			XmlNode xmlNode = node.Attributes.get_ItemOf("occupation");
			if (xmlNode != null)
			{
				this.IsSoldier = (xmlNode.InnerText.IndexOf("soldier", StringComparison.OrdinalIgnoreCase) >= 0);
			}
			this._isBasicHero = XmlHelper.ReadBool(node, "is_hero");
			this.FaceMeshCache = XmlHelper.ReadBool(node, "face_mesh_cache");
			this.IsObsolete = XmlHelper.ReadBool(node, "is_obsolete");
			MBCharacterSkills mbcharacterSkills = objectManager.ReadObjectReferenceFromXml("skill_template", typeof(MBCharacterSkills), node) as MBCharacterSkills;
			if (mbcharacterSkills != null)
			{
				this.DefaultCharacterSkills = mbcharacterSkills;
			}
			else
			{
				this.DefaultCharacterSkills = MBObjectManager.Instance.CreateObject<MBCharacterSkills>(base.StringId);
			}
			BodyProperties bodyPropertyMin = default(BodyProperties);
			BodyProperties bodyProperties = default(BodyProperties);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.Name == "Skills" || xmlNode2.Name == "skills")
				{
					if (mbcharacterSkills == null)
					{
						this.DefaultCharacterSkills.Init(objectManager, xmlNode2);
					}
				}
				else if (xmlNode2.Name == "Equipments" || xmlNode2.Name == "equipments")
				{
					List<XmlNode> list = new List<XmlNode>();
					foreach (object obj2 in xmlNode2.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj2;
						if (xmlNode3.Name == "equipment")
						{
							list.Add(xmlNode3);
						}
					}
					foreach (object obj3 in xmlNode2.ChildNodes)
					{
						XmlNode xmlNode4 = (XmlNode)obj3;
						if (xmlNode4.Name == "EquipmentRoster" || xmlNode4.Name == "equipmentRoster")
						{
							if (this._equipmentRoster == null)
							{
								this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
							}
							this._equipmentRoster.Init(objectManager, xmlNode4);
						}
						else if (xmlNode4.Name == "EquipmentSet" || xmlNode4.Name == "equipmentSet")
						{
							string innerText = xmlNode4.Attributes.get_ItemOf("id").InnerText;
							bool isCivilian = xmlNode4.Attributes.get_ItemOf("civilian") != null && bool.Parse(xmlNode4.Attributes.get_ItemOf("civilian").InnerText);
							if (this._equipmentRoster == null)
							{
								this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
							}
							this._equipmentRoster.AddEquipmentRoster(MBObjectManager.Instance.GetObject<MBEquipmentRoster>(innerText), isCivilian);
						}
					}
					if (list.Count > 0)
					{
						this._equipmentRoster.AddOverridenEquipments(objectManager, list);
					}
				}
				else
				{
					if (xmlNode2.Name == "face")
					{
						using (IEnumerator enumerator2 = xmlNode2.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj4 = enumerator2.Current;
								XmlNode xmlNode5 = (XmlNode)obj4;
								if (xmlNode5.Name == "hair_tags")
								{
									using (IEnumerator enumerator3 = xmlNode5.ChildNodes.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											object obj5 = enumerator3.Current;
											XmlNode xmlNode6 = (XmlNode)obj5;
											this.HairTags = this.HairTags + xmlNode6.Attributes.get_ItemOf("name").Value + ",";
										}
										continue;
									}
								}
								if (xmlNode5.Name == "beard_tags")
								{
									using (IEnumerator enumerator3 = xmlNode5.ChildNodes.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											object obj6 = enumerator3.Current;
											XmlNode xmlNode7 = (XmlNode)obj6;
											this.BeardTags = this.BeardTags + xmlNode7.Attributes.get_ItemOf("name").Value + ",";
										}
										continue;
									}
								}
								if (xmlNode5.Name == "tattoo_tags")
								{
									using (IEnumerator enumerator3 = xmlNode5.ChildNodes.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											object obj7 = enumerator3.Current;
											XmlNode xmlNode8 = (XmlNode)obj7;
											this.TattooTags = this.TattooTags + xmlNode8.Attributes.get_ItemOf("name").Value + ",";
										}
										continue;
									}
								}
								if (xmlNode5.Name == "BodyProperties")
								{
									if (!BodyProperties.FromXmlNode(xmlNode5, out bodyPropertyMin))
									{
										Debug.FailedAssert("cannot read body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 428);
									}
								}
								else if (xmlNode5.Name == "BodyPropertiesMax")
								{
									if (!BodyProperties.FromXmlNode(xmlNode5, out bodyProperties))
									{
										bodyPropertyMin = bodyProperties;
										Debug.FailedAssert("cannot read max body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 437);
									}
								}
								else if (xmlNode5.Name == "face_key_template")
								{
									MBBodyProperty bodyPropertyRange = objectManager.ReadObjectReferenceFromXml<MBBodyProperty>("value", xmlNode5);
									this.BodyPropertyRange = bodyPropertyRange;
								}
							}
							continue;
						}
					}
					if (xmlNode2.Name == "Resistances" || xmlNode2.Name == "resistances")
					{
						this.KnockbackResistance = XmlHelper.ReadFloat(xmlNode2, "knockback", 25f) * 0.01f;
						this.KnockbackResistance = MBMath.ClampFloat(this.KnockbackResistance, 0f, 1f);
						this.KnockdownResistance = XmlHelper.ReadFloat(xmlNode2, "knockdown", 50f) * 0.01f;
						this.KnockdownResistance = MBMath.ClampFloat(this.KnockdownResistance, 0f, 1f);
						this.DismountResistance = XmlHelper.ReadFloat(xmlNode2, "dismount", 50f) * 0.01f;
						this.DismountResistance = MBMath.ClampFloat(this.DismountResistance, 0f, 1f);
					}
				}
			}
			if (this.BodyPropertyRange == null)
			{
				this.BodyPropertyRange = MBObjectManager.Instance.RegisterPresumedObject<MBBodyProperty>(new MBBodyProperty(base.StringId));
				this.BodyPropertyRange.Init(bodyPropertyMin, bodyProperties);
			}
			this.IsFemale = false;
			this.DefaultFormationGroup = 0;
			XmlNode xmlNode9 = node.Attributes.get_ItemOf("is_female");
			if (xmlNode9 != null)
			{
				this.IsFemale = Convert.ToBoolean(xmlNode9.InnerText);
			}
			this.Culture = objectManager.ReadObjectReferenceFromXml<BasicCultureObject>("culture", node);
			XmlNode xmlNode10 = node.Attributes.get_ItemOf("age");
			this.Age = ((xmlNode10 == null) ? MathF.Max(20f, this.BodyPropertyRange.BodyPropertyMax.Age) : ((float)Convert.ToInt32(xmlNode10.InnerText)));
			XmlNode xmlNode11 = node.Attributes.get_ItemOf("level");
			this.Level = ((xmlNode11 != null) ? Convert.ToInt32(xmlNode11.InnerText) : 1);
			XmlNode xmlNode12 = node.Attributes.get_ItemOf("default_group");
			if (xmlNode12 != null)
			{
				this.DefaultFormationGroup = this.FetchDefaultFormationGroup(xmlNode12.InnerText);
			}
			this.DefaultFormationClass = (FormationClass)this.DefaultFormationGroup;
			this._isRanged = this.DefaultFormationClass.IsRanged();
			this._isMounted = this.DefaultFormationClass.IsMounted();
			XmlNode xmlNode13 = node.Attributes.get_ItemOf("formation_position_preference");
			this.FormationPositionPreference = ((xmlNode13 != null) ? ((FormationPositionPreference)Enum.Parse(typeof(FormationPositionPreference), xmlNode13.InnerText)) : FormationPositionPreference.Middle);
			XmlNode xmlNode14 = node.Attributes.get_ItemOf("default_equipment_set");
			if (xmlNode14 != null)
			{
				this._equipmentRoster.InitializeDefaultEquipment(xmlNode14.Value);
			}
			MBEquipmentRoster equipmentRoster = this._equipmentRoster;
			if (equipmentRoster == null)
			{
				return;
			}
			equipmentRoster.OrderEquipments();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005EB4 File Offset: 0x000040B4
		protected int FetchDefaultFormationGroup(string innerText)
		{
			FormationClass result;
			if (Enum.TryParse<FormationClass>(innerText, true, out result))
			{
				return (int)result;
			}
			return -1;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005ECF File Offset: 0x000040CF
		public virtual FormationClass GetFormationClass()
		{
			return this.DefaultFormationClass;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005ED7 File Offset: 0x000040D7
		internal static void AutoGeneratedStaticCollectObjectsBasicCharacterObject(object o, List<object> collectedObjects)
		{
			((BasicCharacterObject)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005EE5 File Offset: 0x000040E5
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0400010C RID: 268
		public const float DefaultKnockbackResistance = 25f;

		// Token: 0x0400010D RID: 269
		public const float DefaultKnockdownResistance = 50f;

		// Token: 0x0400010E RID: 270
		public const float DefaultDismountResistance = 50f;

		// Token: 0x0400010F RID: 271
		protected TextObject _basicName;

		// Token: 0x04000113 RID: 275
		private bool _isMounted;

		// Token: 0x04000114 RID: 276
		private bool _isRanged;

		// Token: 0x0400011C RID: 284
		private MBEquipmentRoster _equipmentRoster;

		// Token: 0x0400011F RID: 287
		private BasicCultureObject _culture;

		// Token: 0x04000120 RID: 288
		[CachedData]
		private float _age;

		// Token: 0x04000125 RID: 293
		[CachedData]
		private bool _isBasicHero;

		// Token: 0x04000127 RID: 295
		protected MBCharacterSkills DefaultCharacterSkills;
	}
}
