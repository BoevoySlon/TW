using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D0 RID: 208
	public class TransferCommandResult
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001333 RID: 4915 RVA: 0x0005779D File Offset: 0x0005599D
		// (set) Token: 0x06001334 RID: 4916 RVA: 0x000577A5 File Offset: 0x000559A5
		public CharacterObject TransferCharacter { get; private set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x000577AE File Offset: 0x000559AE
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x000577B6 File Offset: 0x000559B6
		public bool IsCivilianEquipment { get; private set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x000577BF File Offset: 0x000559BF
		public Equipment TransferEquipment
		{
			get
			{
				if (!this.IsCivilianEquipment)
				{
					CharacterObject transferCharacter = this.TransferCharacter;
					if (transferCharacter == null)
					{
						return null;
					}
					return transferCharacter.FirstBattleEquipment;
				}
				else
				{
					CharacterObject transferCharacter2 = this.TransferCharacter;
					if (transferCharacter2 == null)
					{
						return null;
					}
					return transferCharacter2.FirstCivilianEquipment;
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x000577EC File Offset: 0x000559EC
		// (set) Token: 0x06001339 RID: 4921 RVA: 0x000577F4 File Offset: 0x000559F4
		public InventoryLogic.InventorySide ResultSide { get; private set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x000577FD File Offset: 0x000559FD
		// (set) Token: 0x0600133B RID: 4923 RVA: 0x00057805 File Offset: 0x00055A05
		public ItemRosterElement EffectedItemRosterElement { get; private set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0005780E File Offset: 0x00055A0E
		// (set) Token: 0x0600133D RID: 4925 RVA: 0x00057816 File Offset: 0x00055A16
		public int EffectedNumber { get; private set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x0005781F File Offset: 0x00055A1F
		// (set) Token: 0x0600133F RID: 4927 RVA: 0x00057827 File Offset: 0x00055A27
		public int FinalNumber { get; private set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00057830 File Offset: 0x00055A30
		// (set) Token: 0x06001341 RID: 4929 RVA: 0x00057838 File Offset: 0x00055A38
		public EquipmentIndex EffectedEquipmentIndex { get; private set; }

		// Token: 0x06001342 RID: 4930 RVA: 0x00057841 File Offset: 0x00055A41
		public TransferCommandResult()
		{
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00057849 File Offset: 0x00055A49
		public TransferCommandResult(InventoryLogic.InventorySide resultSide, ItemRosterElement effectedItemRosterElement, int effectedNumber, int finalNumber, EquipmentIndex effectedEquipmentIndex, CharacterObject transferCharacter, bool isCivilianEquipment)
		{
			this.ResultSide = resultSide;
			this.EffectedItemRosterElement = effectedItemRosterElement;
			this.EffectedNumber = effectedNumber;
			this.FinalNumber = finalNumber;
			this.EffectedEquipmentIndex = effectedEquipmentIndex;
			this.TransferCharacter = transferCharacter;
			this.IsCivilianEquipment = isCivilianEquipment;
		}
	}
}
