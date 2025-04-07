using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010D RID: 269
	public class DefaultInventoryCapacityModel : InventoryCapacityModel
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x0006877D File Offset: 0x0006697D
		public override int GetItemAverageWeight()
		{
			return 10;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00068784 File Offset: 0x00066984
		public override ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool includeDescriptions = false, int additionalTroops = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			PartyBase party = mobileParty.Party;
			int num = party.NumberOfMounts;
			int num2 = party.NumberOfHealthyMembers;
			int num3 = party.NumberOfPackAnimals;
			if (includeFollowers)
			{
				foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
				{
					num += party.NumberOfMounts;
					num2 += party.NumberOfHealthyMembers;
					num3 += party.NumberOfPackAnimals;
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.ArenicosHorses, false))
			{
				num2 += MathF.Round((float)num2 * DefaultPerks.Steward.ArenicosHorses.PrimaryBonus);
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.ForcedLabor, false))
			{
				num2 += party.PrisonRoster.TotalHealthyCount;
			}
			result.Add(10f, DefaultInventoryCapacityModel._textBase, null);
			result.Add((float)num2 * 2f * 10f, DefaultInventoryCapacityModel._textTroops, null);
			result.Add((float)num * 2f * 10f, DefaultInventoryCapacityModel._textSpareMounts, null);
			ExplainedNumber explainedNumber = new ExplainedNumber((float)num3 * 10f * 10f, false, null);
			if (mobileParty.HasPerk(DefaultPerks.Scouting.BeastWhisperer, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Scouting.BeastWhisperer.SecondaryBonus, DefaultPerks.Scouting.BeastWhisperer.Name);
			}
			if (mobileParty.HasPerk(DefaultPerks.Riding.DeeperSacks, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Riding.DeeperSacks.PrimaryBonus, DefaultPerks.Riding.DeeperSacks.Name);
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.ArenicosMules, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Steward.ArenicosMules.PrimaryBonus, DefaultPerks.Steward.ArenicosMules.Name);
			}
			result.Add(explainedNumber.ResultNumber, DefaultInventoryCapacityModel._textPackAnimals, null);
			if (mobileParty.HasPerk(DefaultPerks.Trade.CaravanMaster, false))
			{
				result.AddFactor(DefaultPerks.Trade.CaravanMaster.PrimaryBonus, DefaultPerks.Trade.CaravanMaster.Name);
			}
			result.LimitMin(10f);
			return result;
		}

		// Token: 0x04000788 RID: 1928
		private const int _itemAverageWeight = 10;

		// Token: 0x04000789 RID: 1929
		private const float TroopsFactor = 2f;

		// Token: 0x0400078A RID: 1930
		private const float SpareMountsFactor = 2f;

		// Token: 0x0400078B RID: 1931
		private const float PackAnimalsFactor = 10f;

		// Token: 0x0400078C RID: 1932
		private static readonly TextObject _textTroops = new TextObject("{=5k4dxUEJ}Troops", null);

		// Token: 0x0400078D RID: 1933
		private static readonly TextObject _textHorses = new TextObject("{=1B8ZDOLs}Horses", null);

		// Token: 0x0400078E RID: 1934
		private static readonly TextObject _textBase = new TextObject("{=basevalue}Base", null);

		// Token: 0x0400078F RID: 1935
		private static readonly TextObject _textSpareMounts = new TextObject("{=rCiKbsyW}Spare Mounts", null);

		// Token: 0x04000790 RID: 1936
		private static readonly TextObject _textPackAnimals = new TextObject("{=dI1AOyqh}Pack Animals", null);
	}
}
