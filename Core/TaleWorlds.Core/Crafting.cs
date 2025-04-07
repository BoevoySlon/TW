using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000041 RID: 65
	public class Crafting
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x00012D7A File Offset: 0x00010F7A
		public Crafting(CraftingTemplate craftingTemplate, BasicCultureObject culture, TextObject name)
		{
			this.CraftedWeaponName = name;
			this.CurrentCraftingTemplate = craftingTemplate;
			this.CurrentCulture = culture;
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00012D97 File Offset: 0x00010F97
		public BasicCultureObject CurrentCulture { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00012D9F File Offset: 0x00010F9F
		public CraftingTemplate CurrentCraftingTemplate { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00012DA7 File Offset: 0x00010FA7
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00012DAF File Offset: 0x00010FAF
		public WeaponDesign CurrentWeaponDesign { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00012DB8 File Offset: 0x00010FB8
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00012DC0 File Offset: 0x00010FC0
		public ItemModifierGroup CurrentItemModifierGroup { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00012DC9 File Offset: 0x00010FC9
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00012DD1 File Offset: 0x00010FD1
		public TextObject CraftedWeaponName { get; private set; }

		// Token: 0x06000550 RID: 1360 RVA: 0x00012DDC File Offset: 0x00010FDC
		public void SetCraftedWeaponName(TextObject name)
		{
			if (!name.Equals(this.CraftedWeaponName.ToString()))
			{
				this.CraftedWeaponName = name;
				this._craftedItemObject.WeaponDesign.SetWeaponName(this.CraftedWeaponName);
				this._craftedItemObject.SetName(this.CraftedWeaponName);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00012E2C File Offset: 0x0001102C
		public void Init()
		{
			this._history = new List<WeaponDesign>();
			this.UsablePiecesList = new List<WeaponDesignElement>[4];
			using (IEnumerator<CraftingPiece> enumerator = ((IEnumerable<CraftingPiece>)this.CurrentCraftingTemplate.Pieces).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CraftingPiece craftingPiece = enumerator.Current;
					if (!this.CurrentCraftingTemplate.BuildOrders.All((PieceData x) => x.PieceType != craftingPiece.PieceType))
					{
						int pieceType = (int)craftingPiece.PieceType;
						if (this.UsablePiecesList[pieceType] == null)
						{
							this.UsablePiecesList[pieceType] = new List<WeaponDesignElement>();
						}
						this.UsablePiecesList[pieceType].Add(WeaponDesignElement.CreateUsablePiece(craftingPiece, 100));
					}
				}
			}
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			for (int i = 0; i < array.Length; i++)
			{
				if (this.UsablePiecesList[i] != null)
				{
					array[i] = this.UsablePiecesList[i].First((WeaponDesignElement p) => !p.CraftingPiece.IsHiddenOnDesigner);
				}
				else
				{
					array[i] = WeaponDesignElement.GetInvalidPieceForType((CraftingPiece.PieceTypes)i);
				}
			}
			this.CurrentWeaponDesign = new WeaponDesign(this.CurrentCraftingTemplate, null, array);
			this._history.Add(this.CurrentWeaponDesign);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00012F7C File Offset: 0x0001117C
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00012F84 File Offset: 0x00011184
		public List<WeaponDesignElement>[] UsablePiecesList { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00012F8D File Offset: 0x0001118D
		public WeaponDesignElement[] SelectedPieces
		{
			get
			{
				return this.CurrentWeaponDesign.UsedPieces;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012F9C File Offset: 0x0001119C
		public WeaponDesignElement GetRandomPieceOfType(CraftingPiece.PieceTypes pieceType, bool randomScale)
		{
			if (!this.CurrentCraftingTemplate.IsPieceTypeUsable(pieceType))
			{
				return WeaponDesignElement.GetInvalidPieceForType(pieceType);
			}
			WeaponDesignElement copy = this.UsablePiecesList[(int)pieceType][MBRandom.RandomInt(this.UsablePiecesList[(int)pieceType].Count)].GetCopy();
			if (randomScale)
			{
				copy.SetScale((int)(90f + MBRandom.RandomFloat * 20f));
			}
			return copy;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00013000 File Offset: 0x00011200
		public void SwitchToCraftedItem(ItemObject item)
		{
			WeaponDesignElement[] usedPieces = item.WeaponDesign.UsedPieces;
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = usedPieces[i].GetCopy();
			}
			this.CurrentWeaponDesign = new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, array);
			this.ReIndex(false);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00013064 File Offset: 0x00011264
		public void Randomize()
		{
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.GetRandomPieceOfType((CraftingPiece.PieceTypes)i, true);
			}
			this.CurrentWeaponDesign = new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, array);
			this.ReIndex(false);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000130BC File Offset: 0x000112BC
		public void SwitchToPiece(WeaponDesignElement piece)
		{
			CraftingPiece.PieceTypes pieceType = piece.CraftingPiece.PieceType;
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			for (int i = 0; i < array.Length; i++)
			{
				if (pieceType == (CraftingPiece.PieceTypes)i)
				{
					array[i] = piece.GetCopy();
					array[i].SetScale(100);
				}
				else
				{
					array[i] = this.CurrentWeaponDesign.UsedPieces[i].GetCopy();
					if (array[i].IsValid)
					{
						array[i].SetScale(this.CurrentWeaponDesign.UsedPieces[i].ScalePercentage);
					}
				}
			}
			this.CurrentWeaponDesign = new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, array);
			this.ReIndex(false);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00013168 File Offset: 0x00011368
		public void ScaleThePiece(CraftingPiece.PieceTypes scalingPieceType, int percentage)
		{
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.SelectedPieces[i].GetCopy();
				if (this.SelectedPieces[i].IsPieceScaled)
				{
					array[i].SetScale(this.SelectedPieces[i].ScalePercentage);
				}
			}
			array[(int)scalingPieceType].SetScale(percentage);
			this.CurrentWeaponDesign = new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, array);
			this.ReIndex(false);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000131F0 File Offset: 0x000113F0
		public void ReIndex(bool enforceReCreation = false)
		{
			if (!TextObject.IsNullOrEmpty(this.CurrentWeaponDesign.WeaponName) && !this.CurrentWeaponDesign.WeaponName.ToString().Equals(this.CraftedWeaponName.ToString()))
			{
				this.CraftedWeaponName = this.CurrentWeaponDesign.WeaponName.CopyTextObject();
			}
			if (enforceReCreation)
			{
				this.CurrentWeaponDesign = new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, this.CurrentWeaponDesign.UsedPieces.ToArray<WeaponDesignElement>());
			}
			this.SetItemObject(null);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013282 File Offset: 0x00011482
		public bool Undo()
		{
			if (this._currentHistoryIndex <= 0)
			{
				return false;
			}
			this._currentHistoryIndex--;
			this.CurrentWeaponDesign = this._history[this._currentHistoryIndex];
			this.ReIndex(false);
			return true;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000132BC File Offset: 0x000114BC
		public bool Redo()
		{
			if (this._currentHistoryIndex + 1 >= this._history.Count)
			{
				return false;
			}
			this._currentHistoryIndex++;
			this.CurrentWeaponDesign = this._history[this._currentHistoryIndex];
			this.ReIndex(false);
			return true;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00013310 File Offset: 0x00011510
		public void UpdateHistory()
		{
			if (this._currentHistoryIndex < this._history.Count - 1)
			{
				this._history.RemoveRange(this._currentHistoryIndex + 1, this._history.Count - 1 - this._currentHistoryIndex);
			}
			WeaponDesignElement[] array = new WeaponDesignElement[this.CurrentWeaponDesign.UsedPieces.Length];
			for (int i = 0; i < this.CurrentWeaponDesign.UsedPieces.Length; i++)
			{
				array[i] = this.CurrentWeaponDesign.UsedPieces[i].GetCopy();
				if (array[i].IsValid)
				{
					array[i].SetScale(this.CurrentWeaponDesign.UsedPieces[i].ScalePercentage);
				}
			}
			this._history.Add(new WeaponDesign(this.CurrentWeaponDesign.Template, this.CurrentWeaponDesign.WeaponName, array));
			this._currentHistoryIndex = this._history.Count - 1;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000133F7 File Offset: 0x000115F7
		public TextObject GetRandomCraftName()
		{
			return new TextObject("{=!}RANDOM_NAME", null);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013404 File Offset: 0x00011604
		public static void GenerateItem(WeaponDesign weaponDesignTemplate, TextObject name, BasicCultureObject culture, ItemModifierGroup itemModifierGroup, ref ItemObject itemObject)
		{
			if (itemObject == null)
			{
				itemObject = new ItemObject();
			}
			WeaponDesignElement[] array = new WeaponDesignElement[weaponDesignTemplate.UsedPieces.Length];
			for (int i = 0; i < weaponDesignTemplate.UsedPieces.Length; i++)
			{
				WeaponDesignElement weaponDesignElement = weaponDesignTemplate.UsedPieces[i];
				array[i] = WeaponDesignElement.CreateUsablePiece(weaponDesignElement.CraftingPiece, weaponDesignElement.ScalePercentage);
			}
			WeaponDesign weaponDesign = new WeaponDesign(weaponDesignTemplate.Template, name, array);
			float weight = MathF.Round(weaponDesign.UsedPieces.Sum((WeaponDesignElement selectedUsablePiece) => selectedUsablePiece.ScaledWeight), 2);
			float appearance = weaponDesign.UsedPieces[3].IsValid ? weaponDesign.UsedPieces[3].CraftingPiece.Appearance : weaponDesign.UsedPieces[0].CraftingPiece.Appearance;
			itemObject.StringId = ((!string.IsNullOrEmpty(itemObject.StringId)) ? itemObject.StringId : weaponDesign.HashedCode);
			ItemObject.InitCraftedItemObject(ref itemObject, name, culture, Crafting.GetItemFlags(weaponDesign), weight, appearance, weaponDesign, weaponDesign.Template.ItemType);
			itemObject = Crafting.CraftedItemGenerationHelper.GenerateCraftedItem(itemObject, weaponDesign, itemModifierGroup);
			if (itemObject != null)
			{
				if (itemObject.IsCraftedByPlayer)
				{
					itemObject.IsReady = true;
				}
				itemObject.DetermineValue();
				itemObject.DetermineItemCategoryForItem();
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013553 File Offset: 0x00011753
		private static ItemFlags GetItemFlags(WeaponDesign weaponDesign)
		{
			return weaponDesign.UsedPieces[0].CraftingPiece.AdditionalItemFlags;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00013567 File Offset: 0x00011767
		private void SetItemObject(ItemObject itemObject = null)
		{
			if (itemObject == null)
			{
				itemObject = new ItemObject();
			}
			Crafting.GenerateItem(this.CurrentWeaponDesign, this.CraftedWeaponName, this.CurrentCulture, this.CurrentItemModifierGroup, ref itemObject);
			this._craftedItemObject = itemObject;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00013599 File Offset: 0x00011799
		public ItemObject GetCurrentCraftedItemObject(bool forceReCreate = false)
		{
			if (forceReCreate)
			{
				this.SetItemObject(null);
			}
			return this._craftedItemObject;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000135AB File Offset: 0x000117AB
		public static IEnumerable<CraftingStatData> GetStatDatasFromTemplate(int usageIndex, ItemObject craftedItemObject, CraftingTemplate template)
		{
			WeaponComponentData weapon = craftedItemObject.GetWeaponWithUsageIndex(usageIndex);
			DamageTypes statDamageType = DamageTypes.Invalid;
			foreach (KeyValuePair<CraftingTemplate.CraftingStatTypes, float> keyValuePair in template.GetStatDatas(usageIndex, weapon.ThrustDamageType, weapon.SwingDamageType))
			{
				TextObject textObject = GameTexts.FindText("str_crafting_stat", keyValuePair.Key.ToString());
				switch (keyValuePair.Key)
				{
				case CraftingTemplate.CraftingStatTypes.ThrustDamage:
					textObject.SetTextVariable("THRUST_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.ThrustDamageType).ToString()));
					statDamageType = weapon.ThrustDamageType;
					break;
				case CraftingTemplate.CraftingStatTypes.SwingDamage:
					textObject.SetTextVariable("SWING_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.SwingDamageType).ToString()));
					statDamageType = weapon.SwingDamageType;
					break;
				case CraftingTemplate.CraftingStatTypes.MissileDamage:
					if (weapon.ThrustDamageType != DamageTypes.Invalid)
					{
						textObject.SetTextVariable("THRUST_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.ThrustDamageType).ToString()));
						statDamageType = weapon.ThrustDamageType;
					}
					else if (weapon.SwingDamageType != DamageTypes.Invalid)
					{
						textObject.SetTextVariable("SWING_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.SwingDamageType).ToString()));
						statDamageType = weapon.SwingDamageType;
					}
					else
					{
						Debug.FailedAssert("Missile damage type is missing.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Crafting.cs", "GetStatDatasFromTemplate", 1170);
					}
					break;
				}
				float value = keyValuePair.Value;
				float num = Crafting.GetValueForCraftingStatForWeaponOfUsageIndex(keyValuePair.Key, craftedItemObject, weapon);
				num = MBMath.ClampFloat(num, 0f, value);
				yield return new CraftingStatData(textObject, num, value, keyValuePair.Key, statDamageType);
			}
			IEnumerator<KeyValuePair<CraftingTemplate.CraftingStatTypes, float>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000135CC File Offset: 0x000117CC
		private static float GetValueForCraftingStatForWeaponOfUsageIndex(CraftingTemplate.CraftingStatTypes craftingStatType, ItemObject item, WeaponComponentData weapon)
		{
			switch (craftingStatType)
			{
			case CraftingTemplate.CraftingStatTypes.Weight:
				return item.Weight;
			case CraftingTemplate.CraftingStatTypes.WeaponReach:
				return (float)weapon.WeaponLength;
			case CraftingTemplate.CraftingStatTypes.ThrustSpeed:
				return (float)weapon.ThrustSpeed;
			case CraftingTemplate.CraftingStatTypes.SwingSpeed:
				return (float)weapon.SwingSpeed;
			case CraftingTemplate.CraftingStatTypes.ThrustDamage:
				return (float)weapon.ThrustDamage;
			case CraftingTemplate.CraftingStatTypes.SwingDamage:
				return (float)weapon.SwingDamage;
			case CraftingTemplate.CraftingStatTypes.Handling:
				return (float)weapon.Handling;
			case CraftingTemplate.CraftingStatTypes.MissileDamage:
				return (float)weapon.MissileDamage;
			case CraftingTemplate.CraftingStatTypes.MissileSpeed:
				return (float)weapon.MissileSpeed;
			case CraftingTemplate.CraftingStatTypes.Accuracy:
				return (float)weapon.Accuracy;
			case CraftingTemplate.CraftingStatTypes.StackAmount:
				return (float)weapon.GetModifiedStackCount(null);
			default:
				throw new ArgumentOutOfRangeException("craftingStatType", craftingStatType, null);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013676 File Offset: 0x00011876
		public IEnumerable<CraftingStatData> GetStatDatas(int usageIndex)
		{
			WeaponComponentData weapon = this._craftedItemObject.GetWeaponWithUsageIndex(usageIndex);
			foreach (KeyValuePair<CraftingTemplate.CraftingStatTypes, float> keyValuePair in this.CurrentCraftingTemplate.GetStatDatas(usageIndex, weapon.ThrustDamageType, weapon.SwingDamageType))
			{
				DamageTypes damageType = DamageTypes.Invalid;
				TextObject textObject = GameTexts.FindText("str_crafting_stat", keyValuePair.Key.ToString());
				switch (keyValuePair.Key)
				{
				case CraftingTemplate.CraftingStatTypes.ThrustDamage:
					textObject.SetTextVariable("THRUST_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.ThrustDamageType).ToString()));
					damageType = weapon.ThrustDamageType;
					break;
				case CraftingTemplate.CraftingStatTypes.SwingDamage:
					textObject.SetTextVariable("SWING_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.SwingDamageType).ToString()));
					damageType = weapon.SwingDamageType;
					break;
				case CraftingTemplate.CraftingStatTypes.MissileDamage:
					if (weapon.ThrustDamageType != DamageTypes.Invalid)
					{
						textObject.SetTextVariable("THRUST_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.ThrustDamageType).ToString()));
						damageType = weapon.ThrustDamageType;
					}
					else if (weapon.SwingDamageType != DamageTypes.Invalid)
					{
						textObject.SetTextVariable("SWING_DAMAGE_TYPE", GameTexts.FindText("str_inventory_dmg_type", ((int)weapon.SwingDamageType).ToString()));
						damageType = weapon.SwingDamageType;
					}
					else
					{
						Debug.FailedAssert("Missile damage type is missing.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Crafting.cs", "GetStatDatas", 1255);
					}
					break;
				}
				float valueForCraftingStatForWeaponOfUsageIndex = Crafting.GetValueForCraftingStatForWeaponOfUsageIndex(keyValuePair.Key, this._craftedItemObject, weapon);
				float value = keyValuePair.Value;
				yield return new CraftingStatData(textObject, valueForCraftingStatForWeaponOfUsageIndex, value, keyValuePair.Key, damageType);
			}
			IEnumerator<KeyValuePair<CraftingTemplate.CraftingStatTypes, float>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013690 File Offset: 0x00011890
		public string GetXmlCodeForCurrentItem(ItemObject item)
		{
			string text = "";
			text = string.Concat(new object[]
			{
				text,
				"<CraftedItem id=\"",
				this.CurrentWeaponDesign.HashedCode,
				"\"\n\t\t\t\t\t\t\t name=\"",
				this.CraftedWeaponName,
				"\"\n\t\t\t\t\t\t\t crafting_template=\"",
				this.CurrentCraftingTemplate.StringId,
				"\">"
			});
			text += "\n";
			text += "<Pieces>";
			text += "\n";
			foreach (WeaponDesignElement weaponDesignElement in this.SelectedPieces)
			{
				if (weaponDesignElement.IsValid)
				{
					string text2 = "";
					if (weaponDesignElement.ScalePercentage != 100)
					{
						int scalePercentage = weaponDesignElement.ScalePercentage;
						text2 = "\n\t\t\t scale_factor=\"" + scalePercentage + "\"";
					}
					text = string.Concat(new object[]
					{
						text,
						"<Piece id=\"",
						weaponDesignElement.CraftingPiece.StringId,
						"\"\n\t\t\t Type=\"",
						weaponDesignElement.CraftingPiece.PieceType,
						"\"",
						text2,
						"/>"
					});
					text += "\n";
				}
			}
			text += "</Pieces>";
			text += "\n";
			text += "<!-- ";
			text = text + "Length: " + item.PrimaryWeapon.WeaponLength;
			text = text + " Weight: " + MathF.Round(item.Weight, 2);
			text += " -->";
			text += "\n";
			return text + "</CraftedItem>";
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001385C File Offset: 0x00011A5C
		public bool TryGetWeaponPropertiesFromXmlCode(string xmlCode, out CraftingTemplate craftingTemplate, out ValueTuple<CraftingPiece, int>[] pieces)
		{
			bool result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlCode);
				pieces = new ValueTuple<CraftingPiece, int>[4];
				XmlNode xmlNode = xmlDocument.SelectSingleNode("CraftedItem");
				string value = xmlNode.Attributes.get_ItemOf("crafting_template").Value;
				craftingTemplate = CraftingTemplate.GetTemplateFromId(value);
				foreach (object obj in xmlNode.SelectSingleNode("Pieces").ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					CraftingPiece.PieceTypes pieceTypes = CraftingPiece.PieceTypes.Invalid;
					string pieceId = null;
					int item = 100;
					foreach (object obj2 in xmlNode2.Attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj2;
						if (xmlAttribute.Name == "Type")
						{
							pieceTypes = (CraftingPiece.PieceTypes)Enum.Parse(typeof(CraftingPiece.PieceTypes), xmlAttribute.Value);
						}
						else if (xmlAttribute.Name == "id")
						{
							pieceId = xmlAttribute.Value;
						}
						else if (xmlAttribute.Name == "scale_factor")
						{
							item = int.Parse(xmlAttribute.Value);
						}
					}
					if (pieceTypes != CraftingPiece.PieceTypes.Invalid && !string.IsNullOrEmpty(pieceId) && craftingTemplate.IsPieceTypeUsable(pieceTypes))
					{
						pieces[(int)pieceTypes] = new ValueTuple<CraftingPiece, int>(CraftingPiece.All.FirstOrDefault((CraftingPiece p) => p.StringId == pieceId), item);
					}
				}
				result = true;
			}
			catch (Exception)
			{
				craftingTemplate = null;
				pieces = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013A54 File Offset: 0x00011C54
		public static ItemObject CreatePreCraftedWeapon(ItemObject itemObject, WeaponDesignElement[] usedPieces, string templateId, TextObject weaponName, ItemModifierGroup itemModifierGroup)
		{
			for (int i = 0; i < usedPieces.Length; i++)
			{
				if (usedPieces[i] == null)
				{
					usedPieces[i] = WeaponDesignElement.GetInvalidPieceForType((CraftingPiece.PieceTypes)i);
				}
			}
			TextObject textObject = (!TextObject.IsNullOrEmpty(weaponName)) ? weaponName : new TextObject("{=Uz1HHeKg}Crafted Random Weapon", null);
			WeaponDesign weaponDesign = new WeaponDesign(CraftingTemplate.GetTemplateFromId(templateId), textObject, usedPieces);
			Crafting crafting = new Crafting(CraftingTemplate.GetTemplateFromId(templateId), null, textObject);
			crafting.CurrentWeaponDesign = weaponDesign;
			crafting.CurrentItemModifierGroup = itemModifierGroup;
			crafting._history = new List<WeaponDesign>
			{
				weaponDesign
			};
			crafting.SetItemObject(itemObject);
			return crafting._craftedItemObject;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013ADC File Offset: 0x00011CDC
		public static ItemObject InitializePreCraftedWeaponOnLoad(ItemObject itemObject, WeaponDesign craftedData, TextObject itemName, BasicCultureObject culture)
		{
			Crafting crafting = new Crafting(craftedData.Template, culture, itemName);
			crafting.CurrentWeaponDesign = craftedData;
			crafting._history = new List<WeaponDesign>
			{
				craftedData
			};
			crafting.SetItemObject(itemObject);
			return crafting._craftedItemObject;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013B10 File Offset: 0x00011D10
		public static ItemObject CreateRandomCraftedItem(BasicCultureObject culture)
		{
			CraftingTemplate randomElement = CraftingTemplate.All.GetRandomElement<CraftingTemplate>();
			TextObject textObject = new TextObject("{=uZhHh7pm}Crafted {CURR_TEMPLATE_NAME}", null);
			textObject.SetTextVariable("CURR_TEMPLATE_NAME", randomElement.TemplateName);
			Crafting crafting = new Crafting(randomElement, culture, textObject);
			crafting.Init();
			crafting.Randomize();
			string hashedCode = crafting._craftedItemObject.WeaponDesign.HashedCode;
			crafting._craftedItemObject.StringId = hashedCode;
			ItemObject itemObject = MBObjectManager.Instance.GetObject<ItemObject>(hashedCode);
			if (itemObject == null)
			{
				itemObject = MBObjectManager.Instance.RegisterObject<ItemObject>(crafting._craftedItemObject);
			}
			return itemObject;
		}

		// Token: 0x04000263 RID: 611
		public const int WeightOfCrudeIron = 1;

		// Token: 0x04000264 RID: 612
		public const int WeightOfIron = 2;

		// Token: 0x04000265 RID: 613
		public const int WeightOfCompositeIron = 3;

		// Token: 0x04000266 RID: 614
		public const int WeightOfSteel = 4;

		// Token: 0x04000267 RID: 615
		public const int WeightOfRefinedSteel = 5;

		// Token: 0x04000268 RID: 616
		public const int WeightOfCalradianSteel = 6;

		// Token: 0x0400026F RID: 623
		private List<WeaponDesign> _history;

		// Token: 0x04000270 RID: 624
		private int _currentHistoryIndex;

		// Token: 0x04000271 RID: 625
		private ItemObject _craftedItemObject;

		// Token: 0x020000EC RID: 236
		public class RefiningFormula
		{
			// Token: 0x06000A0D RID: 2573 RVA: 0x00020B78 File Offset: 0x0001ED78
			public RefiningFormula(CraftingMaterials input1, int input1Count, CraftingMaterials input2, int input2Count, CraftingMaterials output, int outputCount = 1, CraftingMaterials output2 = CraftingMaterials.IronOre, int output2Count = 0)
			{
				this.Output = output;
				this.OutputCount = outputCount;
				this.Output2 = output2;
				this.Output2Count = output2Count;
				this.Input1 = input1;
				this.Input1Count = input1Count;
				this.Input2 = input2;
				this.Input2Count = input2Count;
			}

			// Token: 0x1700034E RID: 846
			// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00020BC8 File Offset: 0x0001EDC8
			public CraftingMaterials Output { get; }

			// Token: 0x1700034F RID: 847
			// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00020BD0 File Offset: 0x0001EDD0
			public int OutputCount { get; }

			// Token: 0x17000350 RID: 848
			// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00020BD8 File Offset: 0x0001EDD8
			public CraftingMaterials Output2 { get; }

			// Token: 0x17000351 RID: 849
			// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00020BE0 File Offset: 0x0001EDE0
			public int Output2Count { get; }

			// Token: 0x17000352 RID: 850
			// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00020BE8 File Offset: 0x0001EDE8
			public CraftingMaterials Input1 { get; }

			// Token: 0x17000353 RID: 851
			// (get) Token: 0x06000A13 RID: 2579 RVA: 0x00020BF0 File Offset: 0x0001EDF0
			public int Input1Count { get; }

			// Token: 0x17000354 RID: 852
			// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00020BF8 File Offset: 0x0001EDF8
			public CraftingMaterials Input2 { get; }

			// Token: 0x17000355 RID: 853
			// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00020C00 File Offset: 0x0001EE00
			public int Input2Count { get; }
		}

		// Token: 0x020000ED RID: 237
		private static class CraftedItemGenerationHelper
		{
			// Token: 0x06000A16 RID: 2582 RVA: 0x00020C08 File Offset: 0x0001EE08
			public static ItemObject GenerateCraftedItem(ItemObject item, WeaponDesign weaponDesign, ItemModifierGroup itemModifierGroup)
			{
				foreach (WeaponDesignElement weaponDesignElement in weaponDesign.UsedPieces)
				{
					if ((weaponDesignElement.IsValid && !weaponDesign.Template.Pieces.Contains(weaponDesignElement.CraftingPiece)) || (weaponDesignElement.CraftingPiece.IsInitialized && !weaponDesignElement.IsValid))
					{
						Debug.Print(weaponDesignElement.CraftingPiece.StringId + " is not a valid valid anymore.", 0, Debug.DebugColor.White, 17592186044416UL);
						return null;
					}
				}
				bool isAlternative = false;
				foreach (WeaponDescription weaponDescription in weaponDesign.Template.WeaponDescriptions)
				{
					int num = 4;
					for (int j = 0; j < weaponDesign.UsedPieces.Length; j++)
					{
						if (!weaponDesign.UsedPieces[j].IsValid)
						{
							num--;
						}
					}
					foreach (CraftingPiece craftingPiece in weaponDescription.AvailablePieces)
					{
						int pieceType = (int)craftingPiece.PieceType;
						if (weaponDesign.UsedPieces[pieceType].CraftingPiece == craftingPiece)
						{
							num--;
						}
						if (num == 0)
						{
							break;
						}
					}
					if (num <= 0)
					{
						WeaponFlags weaponFlags = weaponDescription.WeaponFlags | weaponDesign.WeaponFlags;
						WeaponComponentData weapon;
						Crafting.CraftedItemGenerationHelper.CraftingStats.FillWeapon(item, weaponDescription, weaponFlags, isAlternative, out weapon);
						item.AddWeapon(weapon, itemModifierGroup);
						isAlternative = true;
					}
				}
				return item;
			}

			// Token: 0x02000119 RID: 281
			private struct CraftingStats
			{
				// Token: 0x06000A8D RID: 2701 RVA: 0x00022118 File Offset: 0x00020318
				public static void FillWeapon(ItemObject item, WeaponDescription weaponDescription, WeaponFlags weaponFlags, bool isAlternative, out WeaponComponentData filledWeapon)
				{
					filledWeapon = new WeaponComponentData(item, weaponDescription.WeaponClass, weaponFlags);
					Crafting.CraftedItemGenerationHelper.CraftingStats craftingStats = new Crafting.CraftedItemGenerationHelper.CraftingStats
					{
						_craftedData = item.WeaponDesign,
						_weaponDescription = weaponDescription
					};
					craftingStats.CalculateStats();
					craftingStats.SetWeaponData(filledWeapon, isAlternative);
				}

				// Token: 0x06000A8E RID: 2702 RVA: 0x00022168 File Offset: 0x00020368
				private void CalculateStats()
				{
					WeaponDesign craftedData = this._craftedData;
					this._stoppingTorque = 10f;
					this._armInertia = 2.9f;
					if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.NotUsableWithOneHand))
					{
						this._stoppingTorque *= 1.5f;
						this._armInertia *= 1.4f;
					}
					if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.WideGrip))
					{
						this._stoppingTorque *= 1.5f;
						this._armInertia *= 1.4f;
					}
					this._currentWeaponWeight = 0f;
					this._currentWeaponReach = 0f;
					this._currentWeaponCenterOfMass = 0f;
					this._currentWeaponInertia = 0f;
					this._currentWeaponInertiaAroundShoulder = 0f;
					this._currentWeaponInertiaAroundGrip = 0f;
					this._currentWeaponSwingSpeed = 1f;
					this._currentWeaponThrustSpeed = 1f;
					this._currentWeaponSwingDamage = 0f;
					this._currentWeaponThrustDamage = 0f;
					this._currentWeaponHandling = 1f;
					this._currentWeaponTier = WeaponComponentData.WeaponTiers.Tier1;
					this._currentWeaponWeight = MathF.Round(craftedData.UsedPieces.Sum((WeaponDesignElement selectedUsablePiece) => selectedUsablePiece.ScaledWeight), 2);
					this._currentWeaponReach = MathF.Round(this._craftedData.CraftedWeaponLength, 2);
					this._currentWeaponCenterOfMass = this.CalculateCenterOfMass();
					this._currentWeaponInertia = this.CalculateWeaponInertia();
					this._currentWeaponInertiaAroundShoulder = Crafting.CraftedItemGenerationHelper.CraftingStats.ParallelAxis(this._currentWeaponInertia, this._currentWeaponWeight, 0.5f + this._currentWeaponCenterOfMass);
					this._currentWeaponInertiaAroundGrip = Crafting.CraftedItemGenerationHelper.CraftingStats.ParallelAxis(this._currentWeaponInertia, this._currentWeaponWeight, this._currentWeaponCenterOfMass);
					this._currentWeaponSwingSpeed = this.CalculateSwingSpeed();
					this._currentWeaponThrustSpeed = this.CalculateThrustSpeed();
					this._currentWeaponHandling = (float)this.CalculateAgility();
					this._currentWeaponTier = this.CalculateWeaponTier();
					this._swingDamageFactor = this._craftedData.UsedPieces[0].CraftingPiece.BladeData.SwingDamageFactor;
					this._thrustDamageFactor = this._craftedData.UsedPieces[0].CraftingPiece.BladeData.ThrustDamageFactor;
					if (this._weaponDescription.WeaponClass == WeaponClass.ThrowingAxe || this._weaponDescription.WeaponClass == WeaponClass.ThrowingKnife || this._weaponDescription.WeaponClass == WeaponClass.Javelin)
					{
						this._currentWeaponSwingDamage = 0f;
						this.CalculateMissileDamage(out this._currentWeaponThrustDamage);
					}
					else
					{
						this.CalculateSwingBaseDamage(out this._currentWeaponSwingDamage);
						this.CalculateThrustBaseDamage(out this._currentWeaponThrustDamage, false);
					}
					this._currentWeaponSweetSpot = this.CalculateSweetSpot();
				}

				// Token: 0x06000A8F RID: 2703 RVA: 0x0002240C File Offset: 0x0002060C
				private void SetWeaponData(WeaponComponentData weapon, bool isAlternative)
				{
					BladeData bladeData = this._craftedData.UsedPieces[0].CraftingPiece.BladeData;
					short maxDataValue = 0;
					string passBySoundCode = "";
					int accuracy = 0;
					int missileSpeed = 0;
					MatrixFrame identity = MatrixFrame.Identity;
					short reloadPhaseCount = 1;
					if (this._weaponDescription.WeaponClass == WeaponClass.Javelin || this._weaponDescription.WeaponClass == WeaponClass.ThrowingAxe || this._weaponDescription.WeaponClass == WeaponClass.ThrowingKnife)
					{
						short num = isAlternative ? 1 : bladeData.StackAmount;
						switch (this._weaponDescription.WeaponClass)
						{
						case WeaponClass.ThrowingAxe:
							maxDataValue = num;
							accuracy = 93;
							passBySoundCode = "event:/mission/combat/throwing/passby";
							break;
						case WeaponClass.ThrowingKnife:
							maxDataValue = num;
							accuracy = 95;
							passBySoundCode = "event:/mission/combat/throwing/passby";
							break;
						case WeaponClass.Javelin:
							maxDataValue = num;
							accuracy = 92;
							passBySoundCode = "event:/mission/combat/missile/passby";
							break;
						}
						missileSpeed = MathF.Floor(this.CalculateMissileSpeed());
						Mat3 identity2 = Mat3.Identity;
						switch (this._weaponDescription.WeaponClass)
						{
						case WeaponClass.ThrowingAxe:
						{
							float bladeWidth = this._craftedData.UsedPieces[0].CraftingPiece.BladeData.BladeWidth;
							float num2 = this._craftedData.PiecePivotDistances[0];
							float scaledDistanceToNextPiece = this._craftedData.UsedPieces[0].ScaledDistanceToNextPiece;
							identity2.RotateAboutUp(1.5707964f);
							identity2.RotateAboutSide(-(15f + scaledDistanceToNextPiece * 3f / num2 * 25f) * 0.017453292f);
							identity = new MatrixFrame(identity2, -identity2.u * (num2 + scaledDistanceToNextPiece * 0.6f) - identity2.f * bladeWidth * 0.8f);
							break;
						}
						case WeaponClass.ThrowingKnife:
							identity2.RotateAboutForward(-1.5707964f);
							identity = new MatrixFrame(identity2, Vec3.Side * this._currentWeaponReach);
							break;
						case WeaponClass.Javelin:
							identity2.RotateAboutSide(1.5707964f);
							identity = new MatrixFrame(identity2, -Vec3.Up * this._currentWeaponReach);
							break;
						}
					}
					if (this._weaponDescription.WeaponClass == WeaponClass.Arrow || this._weaponDescription.WeaponClass == WeaponClass.Bolt)
					{
						identity.rotation.RotateAboutSide(1.5707964f);
					}
					Vec3 zero = Vec3.Zero;
					if (this._weaponDescription.WeaponClass == WeaponClass.ThrowingAxe)
					{
						zero = new Vec3(0f, 18f, 0f, -1f);
					}
					else if (this._weaponDescription.WeaponClass == WeaponClass.ThrowingKnife)
					{
						zero = new Vec3(0f, 24f, 0f, -1f);
					}
					weapon.Init(this._weaponDescription.StringId, bladeData.PhysicsMaterial, this.GetItemUsage(), bladeData.ThrustDamageType, bladeData.SwingDamageType, this.GetWeaponHandArmorBonus(), (int)(this._currentWeaponReach * 100f), MathF.Round(this.GetWeaponBalance(), 2), this._currentWeaponInertia, this._currentWeaponCenterOfMass, MathF.Floor(this._currentWeaponHandling), MathF.Round(this._swingDamageFactor, 2), MathF.Round(this._thrustDamageFactor, 2), maxDataValue, passBySoundCode, accuracy, missileSpeed, identity, this.GetAmmoClass(), this._currentWeaponSweetSpot, MathF.Floor(this._currentWeaponSwingSpeed * 4.5454545f), MathF.Round(this._currentWeaponSwingDamage), MathF.Floor(this._currentWeaponThrustSpeed * 11.764706f), MathF.Round(this._currentWeaponThrustDamage), zero, this._currentWeaponTier, reloadPhaseCount);
					Mat3 identity3 = Mat3.Identity;
					Vec3 v = Vec3.Zero;
					if (this._weaponDescription.RotatedInHand)
					{
						identity3.RotateAboutSide(3.1415927f);
					}
					if (this._weaponDescription.UseCenterOfMassAsHandBase)
					{
						v = -Vec3.Up * this._currentWeaponCenterOfMass;
					}
					weapon.SetFrame(new MatrixFrame(identity3, identity3.TransformToParent(v)));
				}

				// Token: 0x06000A90 RID: 2704 RVA: 0x000227D4 File Offset: 0x000209D4
				private float CalculateSweetSpot()
				{
					float num = -1f;
					float result = -1f;
					for (int i = 0; i < 100; i++)
					{
						float num2 = 0.01f * (float)i;
						float num3 = CombatStatCalculator.CalculateStrikeMagnitudeForSwing(this._currentWeaponSwingSpeed, num2, this._currentWeaponWeight, this._currentWeaponReach, this._currentWeaponInertia, this._currentWeaponCenterOfMass, 0f);
						if (num < num3)
						{
							num = num3;
							result = num2;
						}
					}
					return result;
				}

				// Token: 0x06000A91 RID: 2705 RVA: 0x0002283C File Offset: 0x00020A3C
				private float CalculateCenterOfMass()
				{
					float num = 0f;
					float num2 = 0f;
					float num3 = 0f;
					foreach (PieceData pieceData in this._craftedData.Template.BuildOrders)
					{
						CraftingPiece.PieceTypes pieceType = pieceData.PieceType;
						WeaponDesignElement weaponDesignElement = this._craftedData.UsedPieces[(int)pieceType];
						if (weaponDesignElement.IsValid)
						{
							float scaledWeight = weaponDesignElement.ScaledWeight;
							float num4 = 0f;
							if (pieceData.Order < 0)
							{
								num4 -= (num3 + (weaponDesignElement.ScaledLength - weaponDesignElement.ScaledCenterOfMass)) * scaledWeight;
								num3 += weaponDesignElement.ScaledLength;
							}
							else
							{
								num4 += (num2 + weaponDesignElement.ScaledCenterOfMass) * scaledWeight;
								num2 += weaponDesignElement.ScaledLength;
							}
							num += num4;
						}
					}
					return num / this._currentWeaponWeight - (this._craftedData.UsedPieces[2].ScaledDistanceToPreviousPiece - this._craftedData.UsedPieces[2].ScaledPieceOffset);
				}

				// Token: 0x06000A92 RID: 2706 RVA: 0x00022940 File Offset: 0x00020B40
				private float CalculateWeaponInertia()
				{
					float num = -this._currentWeaponCenterOfMass;
					float num2 = 0f;
					foreach (PieceData pieceData in this._craftedData.Template.BuildOrders)
					{
						WeaponDesignElement weaponDesignElement = this._craftedData.UsedPieces[(int)pieceData.PieceType];
						if (weaponDesignElement.IsValid)
						{
							float weightMultiplier = 1f;
							num2 += Crafting.CraftedItemGenerationHelper.CraftingStats.ParallelAxis(weaponDesignElement, num, weightMultiplier);
							num += weaponDesignElement.ScaledLength;
						}
					}
					return num2;
				}

				// Token: 0x06000A93 RID: 2707 RVA: 0x000229C0 File Offset: 0x00020BC0
				private float CalculateSwingSpeed()
				{
					double num = 1.0 * (double)this._currentWeaponInertiaAroundShoulder + 0.9;
					double num2 = 170.0;
					double num3 = 90.0;
					double num4 = 27.0;
					double num5 = 15.0;
					double num6 = 7.0;
					if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.NotUsableWithOneHand))
					{
						if (this._weaponDescription.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip))
						{
							num += 1.5;
							num6 *= 4.0;
							num5 *= 1.7;
							num3 *= 1.3;
							num2 *= 1.15;
						}
						else
						{
							num += 1.0;
							num6 *= 2.4;
							num5 *= 1.3;
							num3 *= 1.35;
							num2 *= 1.15;
						}
					}
					num4 = MathF.Max(1.0, num4 - num);
					num5 = MathF.Max(1.0, num5 - num);
					num6 = MathF.Max(1.0, num6 - num);
					double num7;
					double num8;
					this.SimulateSwingLayer(1.5, 200.0, num4, 2.0 + num, out num7, out num8);
					double num9;
					double num10;
					this.SimulateSwingLayer(1.5, num2, num5, 1.0 + num, out num9, out num10);
					double num11;
					double num12;
					this.SimulateSwingLayer(1.5, num3, num6, 0.5 + num, out num11, out num12);
					double num13 = 0.33 * (num8 + num10 + num12);
					return (float)(20.8 / num13);
				}

				// Token: 0x06000A94 RID: 2708 RVA: 0x00022B94 File Offset: 0x00020D94
				private float CalculateThrustSpeed()
				{
					double num = 1.8 + (double)this._currentWeaponWeight + (double)this._currentWeaponInertiaAroundGrip * 0.2;
					double num2 = 170.0;
					double num3 = 90.0;
					double num4 = 24.0;
					double num5 = 15.0;
					if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.NotUsableWithOneHand) && !this._weaponDescription.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip))
					{
						num += 0.6;
						num5 *= 1.9;
						num4 *= 1.1;
						num3 *= 1.2;
						num2 *= 1.05;
					}
					else if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.WideGrip))
					{
						num += 0.9;
						num5 *= 2.1;
						num4 *= 1.2;
						num3 *= 1.2;
						num2 *= 1.05;
					}
					double num6;
					double num7;
					this.SimulateThrustLayer(0.6, 250.0, 48.0, 4.0 + num, out num6, out num7);
					double num8;
					double num9;
					this.SimulateThrustLayer(0.6, num2, num4, 2.0 + num, out num8, out num9);
					double num10;
					double num11;
					this.SimulateThrustLayer(0.6, num3, num5, 0.5 + num, out num10, out num11);
					double num12 = 0.33 * (num7 + num9 + num11);
					return (float)(3.8500000000000005 / num12);
				}

				// Token: 0x06000A95 RID: 2709 RVA: 0x00022D40 File Offset: 0x00020F40
				private void CalculateSwingBaseDamage(out float damage)
				{
					float num = 0f;
					for (float num2 = 0.93f; num2 > 0.5f; num2 -= 0.05f)
					{
						float num3 = CombatStatCalculator.CalculateBaseBlowMagnitudeForSwing(this._currentWeaponSwingSpeed, this._currentWeaponReach, this._currentWeaponWeight, this._currentWeaponInertia, this._currentWeaponCenterOfMass, num2, 0f);
						if (num3 > num)
						{
							num = num3;
						}
					}
					damage = num * this._swingDamageFactor;
				}

				// Token: 0x06000A96 RID: 2710 RVA: 0x00022DA8 File Offset: 0x00020FA8
				private void CalculateThrustBaseDamage(out float damage, bool isThrown = false)
				{
					float num = CombatStatCalculator.CalculateStrikeMagnitudeForThrust(this._currentWeaponThrustSpeed, this._currentWeaponWeight, 0f, isThrown);
					damage = num * this._thrustDamageFactor;
				}

				// Token: 0x06000A97 RID: 2711 RVA: 0x00022DD8 File Offset: 0x00020FD8
				private void CalculateMissileDamage(out float damage)
				{
					switch (this._weaponDescription.WeaponClass)
					{
					case WeaponClass.ThrowingAxe:
						this.CalculateSwingBaseDamage(out damage);
						damage *= 2f;
						return;
					case WeaponClass.ThrowingKnife:
						this.CalculateThrustBaseDamage(out damage, true);
						damage *= 3.3f;
						return;
					case WeaponClass.Javelin:
						this.CalculateThrustBaseDamage(out damage, true);
						damage *= 9f;
						return;
					default:
						damage = 0f;
						Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Crafting.cs", "CalculateMissileDamage", 508);
						return;
					}
				}

				// Token: 0x06000A98 RID: 2712 RVA: 0x00022E60 File Offset: 0x00021060
				private WeaponComponentData.WeaponTiers CalculateWeaponTier()
				{
					int num = 0;
					int num2 = 0;
					foreach (WeaponDesignElement weaponDesignElement in from ucp in this._craftedData.UsedPieces
					where ucp.IsValid
					select ucp)
					{
						num += weaponDesignElement.CraftingPiece.PieceTier;
						num2++;
					}
					WeaponComponentData.WeaponTiers result;
					if (Enum.TryParse<WeaponComponentData.WeaponTiers>(((int)((float)num / (float)num2)).ToString(), out result))
					{
						return result;
					}
					Debug.FailedAssert("Couldn't calculate weapon tier", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Crafting.cs", "CalculateWeaponTier", 529);
					return WeaponComponentData.WeaponTiers.Tier1;
				}

				// Token: 0x06000A99 RID: 2713 RVA: 0x00022F20 File Offset: 0x00021120
				private string GetItemUsage()
				{
					List<string> list = this._weaponDescription.ItemUsageFeatures.Split(new char[]
					{
						':'
					}).ToList<string>();
					foreach (WeaponDesignElement weaponDesignElement in from ucp in this._craftedData.UsedPieces
					where ucp.IsValid
					select ucp)
					{
						foreach (string text in weaponDesignElement.CraftingPiece.ItemUsageFeaturesToExclude.Split(new char[]
						{
							':'
						}))
						{
							if (!string.IsNullOrEmpty(text))
							{
								list.Remove(text);
							}
						}
					}
					string text2 = "";
					for (int j = 0; j < list.Count; j++)
					{
						text2 += list[j];
						if (j < list.Count - 1)
						{
							text2 += "_";
						}
					}
					return text2;
				}

				// Token: 0x06000A9A RID: 2714 RVA: 0x00023038 File Offset: 0x00021238
				private float CalculateMissileSpeed()
				{
					if (this._weaponDescription.WeaponClass == WeaponClass.ThrowingAxe)
					{
						return this._currentWeaponThrustSpeed * 3.2f;
					}
					if (this._weaponDescription.WeaponClass == WeaponClass.ThrowingKnife)
					{
						return this._currentWeaponThrustSpeed * 3.9f;
					}
					if (this._weaponDescription.WeaponClass == WeaponClass.Javelin)
					{
						return this._currentWeaponThrustSpeed * 3.6f;
					}
					Debug.FailedAssert("Weapon is not a missile.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Crafting.cs", "CalculateMissileSpeed", 580);
					return 10f;
				}

				// Token: 0x06000A9B RID: 2715 RVA: 0x000230B8 File Offset: 0x000212B8
				private int CalculateAgility()
				{
					float num = this._currentWeaponInertiaAroundGrip;
					if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.NotUsableWithOneHand))
					{
						num *= 0.5f;
						num += 0.9f;
					}
					else if (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.WideGrip))
					{
						num *= 0.4f;
						num += 1f;
					}
					else
					{
						num += 0.7f;
					}
					float num2 = MathF.Pow(1f / num, 0.55f);
					num2 *= 1f;
					return MathF.Round(100f * num2);
				}

				// Token: 0x06000A9C RID: 2716 RVA: 0x00023148 File Offset: 0x00021348
				private float GetWeaponBalance()
				{
					return MBMath.ClampFloat((this._currentWeaponSwingSpeed * 4.5454545f - 70f) / 30f, 0f, 1f);
				}

				// Token: 0x06000A9D RID: 2717 RVA: 0x00023171 File Offset: 0x00021371
				private int GetWeaponHandArmorBonus()
				{
					WeaponDesignElement weaponDesignElement = this._craftedData.UsedPieces[1];
					if (weaponDesignElement == null)
					{
						return 0;
					}
					return weaponDesignElement.CraftingPiece.ArmorBonus;
				}

				// Token: 0x06000A9E RID: 2718 RVA: 0x00023190 File Offset: 0x00021390
				private WeaponClass GetAmmoClass()
				{
					if (this._weaponDescription.WeaponClass != WeaponClass.ThrowingKnife && this._weaponDescription.WeaponClass != WeaponClass.ThrowingAxe && this._weaponDescription.WeaponClass != WeaponClass.Javelin)
					{
						return WeaponClass.Undefined;
					}
					return this._weaponDescription.WeaponClass;
				}

				// Token: 0x06000A9F RID: 2719 RVA: 0x000231CC File Offset: 0x000213CC
				private static float ParallelAxis(WeaponDesignElement selectedPiece, float offset, float weightMultiplier)
				{
					float inertia = selectedPiece.CraftingPiece.Inertia;
					float offsetFromCm = offset + selectedPiece.CraftingPiece.CenterOfMass;
					float mass = selectedPiece.ScaledWeight * weightMultiplier;
					return Crafting.CraftedItemGenerationHelper.CraftingStats.ParallelAxis(inertia, mass, offsetFromCm);
				}

				// Token: 0x06000AA0 RID: 2720 RVA: 0x00023202 File Offset: 0x00021402
				private static float ParallelAxis(float inertiaAroundCm, float mass, float offsetFromCm)
				{
					return inertiaAroundCm + mass * offsetFromCm * offsetFromCm;
				}

				// Token: 0x06000AA1 RID: 2721 RVA: 0x0002320C File Offset: 0x0002140C
				private void SimulateSwingLayer(double angleSpan, double usablePower, double maxUsableTorque, double inertia, out double finalSpeed, out double finalTime)
				{
					double num = 0.0;
					double num2 = 0.01;
					double num3 = 0.0;
					double num4 = 3.9 * (double)this._currentWeaponReach * (this._weaponDescription.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon | WeaponFlags.WideGrip) ? 1.0 : 0.3);
					while (num < angleSpan)
					{
						double num5 = usablePower / num2;
						if (num5 > maxUsableTorque)
						{
							num5 = maxUsableTorque;
						}
						num5 -= num2 * num4;
						double num6 = 0.009999999776482582 * num5 / inertia;
						num2 += num6;
						num += num2 * 0.009999999776482582;
						num3 += 0.009999999776482582;
					}
					finalSpeed = num2;
					finalTime = num3;
				}

				// Token: 0x06000AA2 RID: 2722 RVA: 0x000232C8 File Offset: 0x000214C8
				private void SimulateThrustLayer(double distance, double usablePower, double maxUsableForce, double mass, out double finalSpeed, out double finalTime)
				{
					double num = 0.0;
					double num2 = 0.01;
					double num3 = 0.0;
					while (num < distance)
					{
						double num4 = usablePower / num2;
						if (num4 > maxUsableForce)
						{
							num4 = maxUsableForce;
						}
						double num5 = 0.01 * num4 / mass;
						num2 += num5;
						num += num2 * 0.01;
						num3 += 0.01;
					}
					finalSpeed = num2;
					finalTime = num3;
				}

				// Token: 0x04000737 RID: 1847
				private WeaponDesign _craftedData;

				// Token: 0x04000738 RID: 1848
				private WeaponDescription _weaponDescription;

				// Token: 0x04000739 RID: 1849
				private float _stoppingTorque;

				// Token: 0x0400073A RID: 1850
				private float _armInertia;

				// Token: 0x0400073B RID: 1851
				private float _swingDamageFactor;

				// Token: 0x0400073C RID: 1852
				private float _thrustDamageFactor;

				// Token: 0x0400073D RID: 1853
				private float _currentWeaponWeight;

				// Token: 0x0400073E RID: 1854
				private float _currentWeaponReach;

				// Token: 0x0400073F RID: 1855
				private float _currentWeaponSweetSpot;

				// Token: 0x04000740 RID: 1856
				private float _currentWeaponCenterOfMass;

				// Token: 0x04000741 RID: 1857
				private float _currentWeaponInertia;

				// Token: 0x04000742 RID: 1858
				private float _currentWeaponInertiaAroundShoulder;

				// Token: 0x04000743 RID: 1859
				private float _currentWeaponInertiaAroundGrip;

				// Token: 0x04000744 RID: 1860
				private float _currentWeaponSwingSpeed;

				// Token: 0x04000745 RID: 1861
				private float _currentWeaponThrustSpeed;

				// Token: 0x04000746 RID: 1862
				private float _currentWeaponHandling;

				// Token: 0x04000747 RID: 1863
				private float _currentWeaponSwingDamage;

				// Token: 0x04000748 RID: 1864
				private float _currentWeaponThrustDamage;

				// Token: 0x04000749 RID: 1865
				private WeaponComponentData.WeaponTiers _currentWeaponTier;
			}
		}
	}
}
