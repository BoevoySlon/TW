using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D1 RID: 209
	public struct TransferCommand
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00057886 File Offset: 0x00055A86
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x0005788E File Offset: 0x00055A8E
		public TransferCommand.CommandType TypeOfCommand { get; private set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x00057897 File Offset: 0x00055A97
		public Equipment CharacterEquipment
		{
			get
			{
				if (!this.IsCivilianEquipment)
				{
					CharacterObject character = this.Character;
					if (character == null)
					{
						return null;
					}
					return character.FirstBattleEquipment;
				}
				else
				{
					CharacterObject character2 = this.Character;
					if (character2 == null)
					{
						return null;
					}
					return character2.FirstCivilianEquipment;
				}
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x000578C4 File Offset: 0x00055AC4
		// (set) Token: 0x06001348 RID: 4936 RVA: 0x000578CC File Offset: 0x00055ACC
		public InventoryLogic.InventorySide FromSide { get; private set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x000578D5 File Offset: 0x00055AD5
		// (set) Token: 0x0600134A RID: 4938 RVA: 0x000578DD File Offset: 0x00055ADD
		public InventoryLogic.InventorySide ToSide { get; private set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x000578E6 File Offset: 0x00055AE6
		// (set) Token: 0x0600134C RID: 4940 RVA: 0x000578EE File Offset: 0x00055AEE
		public EquipmentIndex FromEquipmentIndex { get; private set; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x000578F7 File Offset: 0x00055AF7
		// (set) Token: 0x0600134E RID: 4942 RVA: 0x000578FF File Offset: 0x00055AFF
		public EquipmentIndex ToEquipmentIndex { get; private set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x00057908 File Offset: 0x00055B08
		// (set) Token: 0x06001350 RID: 4944 RVA: 0x00057910 File Offset: 0x00055B10
		public int Amount { get; private set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00057919 File Offset: 0x00055B19
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x00057921 File Offset: 0x00055B21
		public ItemRosterElement ElementToTransfer { get; private set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0005792A File Offset: 0x00055B2A
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x00057932 File Offset: 0x00055B32
		public CharacterObject Character { get; private set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x0005793B File Offset: 0x00055B3B
		// (set) Token: 0x06001356 RID: 4950 RVA: 0x00057943 File Offset: 0x00055B43
		public bool IsCivilianEquipment { get; private set; }

		// Token: 0x06001357 RID: 4951 RVA: 0x0005794C File Offset: 0x00055B4C
		public static TransferCommand Transfer(int amount, InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide, ItemRosterElement elementToTransfer, EquipmentIndex fromEquipmentIndex, EquipmentIndex toEquipmentIndex, CharacterObject character, bool civilianEquipment)
		{
			return new TransferCommand
			{
				TypeOfCommand = TransferCommand.CommandType.Transfer,
				FromSide = fromSide,
				ToSide = toSide,
				ElementToTransfer = elementToTransfer,
				FromEquipmentIndex = fromEquipmentIndex,
				ToEquipmentIndex = toEquipmentIndex,
				Character = character,
				Amount = amount,
				IsCivilianEquipment = civilianEquipment
			};
		}

		// Token: 0x020004E7 RID: 1255
		public enum CommandType
		{
			// Token: 0x04001511 RID: 5393
			Transfer
		}
	}
}
