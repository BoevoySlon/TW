using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012D RID: 301
	public class DefaultRaidModel : RaidModel
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00071E9C File Offset: 0x0007009C
		private MBReadOnlyList<ValueTuple<ItemObject, float>> CommonLootItemSpawnChances
		{
			get
			{
				if (this._commonLootItems == null)
				{
					List<ValueTuple<ItemObject, float>> list = new List<ValueTuple<ItemObject, float>>
					{
						new ValueTuple<ItemObject, float>(DefaultItems.Hides, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.HardWood, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.Tools, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.Grain, 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("linen"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("sheep"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("mule"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("pottery"), 1f)
					};
					for (int i = list.Count - 1; i >= 0; i--)
					{
						ItemObject item = list[i].Item1;
						float item2 = 100f / ((float)item.Value + 1f);
						list[i] = new ValueTuple<ItemObject, float>(item, item2);
					}
					this._commonLootItems = new MBReadOnlyList<ValueTuple<ItemObject, float>>(list);
				}
				return this._commonLootItems;
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00071FF4 File Offset: 0x000701F4
		public override float CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints)
		{
			float num = (MathF.Sqrt((float)attackerSide.TroopCount) + 5f) / 900f * (float)CampaignTime.DeltaTime.ToHours;
			float num2 = 0f;
			foreach (MapEventParty mapEventParty in attackerSide.Parties)
			{
				MobileParty mobileParty = mapEventParty.Party.MobileParty;
				if (((mobileParty != null) ? mobileParty.LeaderHero : null) != null && mapEventParty.Party.MobileParty.LeaderHero.GetPerkValue(DefaultPerks.Roguery.NoRestForTheWicked))
				{
					num2 += DefaultPerks.Roguery.NoRestForTheWicked.SecondaryBonus;
				}
			}
			return num + num * num2;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x000720B8 File Offset: 0x000702B8
		public override MBReadOnlyList<ValueTuple<ItemObject, float>> GetCommonLootItemScores()
		{
			return this.CommonLootItemSpawnChances;
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x000720C0 File Offset: 0x000702C0
		public override int GoldRewardForEachLostHearth
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x04000816 RID: 2070
		private MBReadOnlyList<ValueTuple<ItemObject, float>> _commonLootItems;
	}
}
