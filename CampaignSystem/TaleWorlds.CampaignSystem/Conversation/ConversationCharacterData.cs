using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001E9 RID: 489
	public struct ConversationCharacterData : ISerializableObject
	{
		// Token: 0x06001D8A RID: 7562 RVA: 0x00085FA0 File Offset: 0x000841A0
		public ConversationCharacterData(CharacterObject character, PartyBase party = null, bool noHorse = false, bool noWeapon = false, bool spawnAfterFight = false, bool isCivilianEquipmentRequiredForLeader = false, bool isCivilianEquipmentRequiredForBodyGuardCharacters = false, bool noBodyguards = false)
		{
			this.Character = character;
			this.Party = party;
			this.NoHorse = noHorse;
			this.NoWeapon = noWeapon;
			this.NoBodyguards = noBodyguards;
			this.SpawnedAfterFight = spawnAfterFight;
			this.IsCivilianEquipmentRequiredForLeader = isCivilianEquipmentRequiredForLeader;
			this.IsCivilianEquipmentRequiredForBodyGuardCharacters = isCivilianEquipmentRequiredForBodyGuardCharacters;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x00085FE0 File Offset: 0x000841E0
		void ISerializableObject.DeserializeFrom(IReader reader)
		{
			MBGUID objectId = new MBGUID(reader.ReadUInt());
			this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(objectId);
			int index = reader.ReadInt();
			this.Party = ConversationCharacterData.FindParty(index);
			this.NoHorse = reader.ReadBool();
			this.NoWeapon = reader.ReadBool();
			this.SpawnedAfterFight = reader.ReadBool();
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00086048 File Offset: 0x00084248
		void ISerializableObject.SerializeTo(IWriter writer)
		{
			writer.WriteUInt(this.Character.Id.InternalValue);
			writer.WriteInt((this.Party == null) ? -1 : this.Party.Index);
			writer.WriteBool(this.NoHorse);
			writer.WriteBool(this.NoWeapon);
			writer.WriteBool(this.SpawnedAfterFight);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000860B0 File Offset: 0x000842B0
		private static PartyBase FindParty(int index)
		{
			MobileParty mobileParty = Campaign.Current.CampaignObjectManager.Find<MobileParty>((MobileParty x) => x.Party.Index == index);
			if (mobileParty != null)
			{
				return mobileParty.Party;
			}
			Settlement settlement = Settlement.All.FirstOrDefaultQ((Settlement x) => x.Party.Index == index);
			if (settlement != null)
			{
				return settlement.Party;
			}
			return null;
		}

		// Token: 0x0400091E RID: 2334
		public CharacterObject Character;

		// Token: 0x0400091F RID: 2335
		public PartyBase Party;

		// Token: 0x04000920 RID: 2336
		public bool NoHorse;

		// Token: 0x04000921 RID: 2337
		public bool NoWeapon;

		// Token: 0x04000922 RID: 2338
		public bool NoBodyguards;

		// Token: 0x04000923 RID: 2339
		public bool SpawnedAfterFight;

		// Token: 0x04000924 RID: 2340
		public bool IsCivilianEquipmentRequiredForLeader;

		// Token: 0x04000925 RID: 2341
		public bool IsCivilianEquipmentRequiredForBodyGuardCharacters;
	}
}
