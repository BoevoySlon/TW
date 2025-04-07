using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001D4 RID: 468
	public class CharacterCreation
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0007F0BF File Offset: 0x0007D2BF
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x0007F0C7 File Offset: 0x0007D2C7
		public bool IsPlayerAlone { get; set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x0007F0D0 File Offset: 0x0007D2D0
		// (set) Token: 0x06001C13 RID: 7187 RVA: 0x0007F0D8 File Offset: 0x0007D2D8
		public bool HasSecondaryCharacter { get; set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0007F0E1 File Offset: 0x0007D2E1
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x0007F0E9 File Offset: 0x0007D2E9
		public string PrefabId { get; private set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0007F0F2 File Offset: 0x0007D2F2
		// (set) Token: 0x06001C17 RID: 7191 RVA: 0x0007F0FA File Offset: 0x0007D2FA
		public sbyte PrefabBoneUsage { get; private set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0007F103 File Offset: 0x0007D303
		public MBReadOnlyList<FaceGenChar> FaceGenChars
		{
			get
			{
				return this._faceGenChars;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x0007F10B File Offset: 0x0007D30B
		// (set) Token: 0x06001C1A RID: 7194 RVA: 0x0007F113 File Offset: 0x0007D313
		public FaceGenMount FaceGenMount { get; private set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x0007F11C File Offset: 0x0007D31C
		// (set) Token: 0x06001C1C RID: 7196 RVA: 0x0007F124 File Offset: 0x0007D324
		public bool CharsEquipmentNeedsRefresh { get; private set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x0007F12D File Offset: 0x0007D32D
		// (set) Token: 0x06001C1E RID: 7198 RVA: 0x0007F135 File Offset: 0x0007D335
		public bool CharsNeedsRefresh { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0007F13E File Offset: 0x0007D33E
		// (set) Token: 0x06001C20 RID: 7200 RVA: 0x0007F146 File Offset: 0x0007D346
		public bool MountsNeedsRefresh { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x0007F14F File Offset: 0x0007D34F
		// (set) Token: 0x06001C22 RID: 7202 RVA: 0x0007F157 File Offset: 0x0007D357
		public string Name { get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x0007F160 File Offset: 0x0007D360
		public int CharacterCreationMenuCount
		{
			get
			{
				return this.CharacterCreationMenus.Count;
			}
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0007F170 File Offset: 0x0007D370
		public void ChangeFaceGenChars(List<FaceGenChar> newChars)
		{
			this._faceGenChars.Clear();
			foreach (FaceGenChar item in newChars)
			{
				this._faceGenChars.Add(item);
			}
			this.CharsNeedsRefresh = true;
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0007F1D8 File Offset: 0x0007D3D8
		public void SetFaceGenMount(FaceGenMount newMount)
		{
			this.FaceGenMount = null;
			if (newMount != null)
			{
				this.FaceGenMount = newMount;
			}
			this.MountsNeedsRefresh = true;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0007F1F2 File Offset: 0x0007D3F2
		public void ClearFaceGenMounts()
		{
			this.FaceGenMount = null;
			this.MountsNeedsRefresh = true;
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0007F202 File Offset: 0x0007D402
		public void ClearFaceGenChars()
		{
			this._faceGenChars.Clear();
			this.CharsNeedsRefresh = true;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0007F216 File Offset: 0x0007D416
		public void ClearFaceGenPrefab()
		{
			this.PrefabId = "";
			this.PrefabBoneUsage = 0;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0007F22C File Offset: 0x0007D42C
		public void ChangeCharactersEquipment(List<Equipment> equipmentList)
		{
			for (int i = 0; i < equipmentList.Count; i++)
			{
				this._faceGenChars[i].Equipment.FillFrom(equipmentList[i], true);
			}
			this.CharsEquipmentNeedsRefresh = true;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0007F270 File Offset: 0x0007D470
		public void ClearCharactersEquipment()
		{
			for (int i = 0; i < this._faceGenChars.Count; i++)
			{
				this._faceGenChars[i].Equipment.FillFrom(new Equipment(), true);
			}
			this.CharsEquipmentNeedsRefresh = true;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0007F2B6 File Offset: 0x0007D4B6
		public void ChangeCharacterPrefab(string id, sbyte boneUsage)
		{
			this.PrefabId = id;
			this.PrefabBoneUsage = boneUsage;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0007F2C8 File Offset: 0x0007D4C8
		public void ChangeCharsAnimation(List<string> actionList)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				this._faceGenChars[i].ActionName = actionList[i];
			}
			this.CharsEquipmentNeedsRefresh = true;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0007F305 File Offset: 0x0007D505
		public void ChangeMountsAnimation(string action)
		{
			this.FaceGenMount.ActionName = action;
			this.MountsNeedsRefresh = true;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0007F31A File Offset: 0x0007D51A
		public CharacterCreation()
		{
			this._faceGenChars = new MBList<FaceGenChar>();
			this.CharacterCreationMenus = new List<CharacterCreationMenu>();
			this.CharsEquipmentNeedsRefresh = false;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0007F33F File Offset: 0x0007D53F
		public void AddNewMenu(CharacterCreationMenu menu)
		{
			this.CharacterCreationMenus.Add(menu);
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0007F34D File Offset: 0x0007D54D
		public CharacterCreationMenu GetCurrentMenu(int index)
		{
			if (index >= 0 && index < this.CharacterCreationMenus.Count)
			{
				return this.CharacterCreationMenus[index];
			}
			return null;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0007F370 File Offset: 0x0007D570
		public IEnumerable<CharacterCreationOption> GetCurrentMenuOptions(int index)
		{
			List<CharacterCreationOption> list = new List<CharacterCreationOption>();
			CharacterCreationMenu currentMenu = this.GetCurrentMenu(index);
			if (currentMenu != null)
			{
				foreach (CharacterCreationCategory characterCreationCategory in currentMenu.CharacterCreationCategories)
				{
					CharacterCreationOnCondition categoryCondition = characterCreationCategory.CategoryCondition;
					if (categoryCondition == null || categoryCondition())
					{
						foreach (CharacterCreationOption characterCreationOption in characterCreationCategory.CharacterCreationOptions)
						{
							if (characterCreationOption.OnCondition == null || characterCreationOption.OnCondition())
							{
								list.Add(characterCreationOption);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0007F444 File Offset: 0x0007D644
		public void ResetMenuOptions()
		{
			for (int i = 0; i < this.CharacterCreationMenus.Count; i++)
			{
				this.CharacterCreationMenus[i].SelectedOptions.Clear();
			}
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0007F47D File Offset: 0x0007D67D
		public void OnInit(int stage)
		{
			if (this.CharacterCreationMenus[stage].OnInit != null)
			{
				this.CharacterCreationMenus[stage].OnInit(this);
			}
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x0007F4A9 File Offset: 0x0007D6A9
		public TextObject GetCurrentMenuText(int stage)
		{
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			return this.CharacterCreationMenus[stage].Text;
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0007F4CE File Offset: 0x0007D6CE
		public TextObject GetCurrentMenuTitle(int stage)
		{
			return this.CharacterCreationMenus[stage].Title;
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0007F4E4 File Offset: 0x0007D6E4
		public void RunConsequence(CharacterCreationOption option, int stage, bool fromInit)
		{
			if (this.CharacterCreationMenus[stage].MenuType == CharacterCreationMenu.MenuTypes.MultipleChoice)
			{
				this.CharacterCreationMenus[stage].SelectedOptions.Clear();
			}
			if (!fromInit && this.CharacterCreationMenus[stage].SelectedOptions.Contains(option.Id))
			{
				this.CharacterCreationMenus[stage].SelectedOptions.Remove(option.Id);
				return;
			}
			this.CharacterCreationMenus[stage].SelectedOptions.Add(option.Id);
			if (option.OnSelect != null)
			{
				option.OnSelect(this);
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0007F589 File Offset: 0x0007D789
		public IEnumerable<int> GetSelectedOptions(int stage)
		{
			return this.CharacterCreationMenus[stage].SelectedOptions;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0007F59C File Offset: 0x0007D79C
		public void ApplyFinalEffects()
		{
			Clan.PlayerClan.Renown = 0f;
			CharacterCreationContentBase.Instance.ApplyCulture(this);
			foreach (CharacterCreationMenu characterCreationMenu in this.CharacterCreationMenus)
			{
				characterCreationMenu.ApplyFinalEffect(this);
			}
			Campaign.Current.PlayerTraitDeveloper.UpdateTraitXPAccordingToTraitLevels();
		}

		// Token: 0x040008D4 RID: 2260
		private readonly MBList<FaceGenChar> _faceGenChars;

		// Token: 0x040008D8 RID: 2264
		public readonly List<CharacterCreationMenu> CharacterCreationMenus;
	}
}
