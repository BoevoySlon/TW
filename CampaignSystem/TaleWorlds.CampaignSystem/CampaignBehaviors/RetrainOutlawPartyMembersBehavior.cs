using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D0 RID: 976
	public class RetrainOutlawPartyMembersBehavior : CampaignBehaviorBase, IRetrainOutlawPartyMembersCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x06003C0E RID: 15374 RVA: 0x00120488 File Offset: 0x0011E688
		private int GetRetrainedNumberInternal(CharacterObject character)
		{
			int result;
			if (!this._retrainTable.TryGetValue(character, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x001204A8 File Offset: 0x0011E6A8
		private int SetRetrainedNumberInternal(CharacterObject character, int numberRetrained)
		{
			this._retrainTable[character] = numberRetrained;
			return numberRetrained;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x001204C5 File Offset: 0x0011E6C5
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x001204E0 File Offset: 0x0011E6E0
		private void DailyTick()
		{
			if (MBRandom.RandomFloat > 0.5f)
			{
				int num = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
				bool flag = false;
				int num2 = 0;
				while (num2 < MobileParty.MainParty.MemberRoster.Count && !flag)
				{
					int index = (num2 + num) % MobileParty.MainParty.MemberRoster.Count;
					CharacterObject characterAtIndex = MobileParty.MainParty.MemberRoster.GetCharacterAtIndex(index);
					if (characterAtIndex.Occupation == Occupation.Bandit)
					{
						int elementNumber = MobileParty.MainParty.MemberRoster.GetElementNumber(index);
						int num3 = this.GetRetrainedNumberInternal(characterAtIndex);
						if (num3 < elementNumber && !flag)
						{
							num3++;
							this.SetRetrainedNumberInternal(characterAtIndex, num3);
						}
						else if (num3 > elementNumber)
						{
							this.SetRetrainedNumberInternal(characterAtIndex, elementNumber);
						}
					}
					num2++;
				}
			}
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x001205AD File Offset: 0x0011E7AD
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<CharacterObject, int>>("_retrainTable", ref this._retrainTable);
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x001205C4 File Offset: 0x0011E7C4
		public int GetRetrainedNumber(CharacterObject character)
		{
			if (character.Occupation == Occupation.Bandit)
			{
				int retrainedNumberInternal = this.GetRetrainedNumberInternal(character);
				int troopCount = MobileParty.MainParty.MemberRoster.GetTroopCount(character);
				return MathF.Min(retrainedNumberInternal, troopCount);
			}
			return 0;
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x001205FB File Offset: 0x0011E7FB
		public void SetRetrainedNumber(CharacterObject character, int number)
		{
			this.SetRetrainedNumberInternal(character, number);
		}

		// Token: 0x040011F7 RID: 4599
		private Dictionary<CharacterObject, int> _retrainTable = new Dictionary<CharacterObject, int>();
	}
}
