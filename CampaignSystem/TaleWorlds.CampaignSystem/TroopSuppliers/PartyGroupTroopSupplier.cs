using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TroopSuppliers
{
	// Token: 0x020000A4 RID: 164
	public class PartyGroupTroopSupplier : IMissionTroopSupplier
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x00051D71 File Offset: 0x0004FF71
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00051D79 File Offset: 0x0004FF79
		internal MapEventSide PartyGroup { get; private set; }

		// Token: 0x060011B0 RID: 4528 RVA: 0x00051D84 File Offset: 0x0004FF84
		public PartyGroupTroopSupplier(MapEvent mapEvent, BattleSideEnum side, FlattenedTroopRoster priorTroops = null, Func<UniqueTroopDescriptor, MapEventParty, bool> customAllocationConditions = null)
		{
			this._customAllocationConditions = customAllocationConditions;
			this.PartyGroup = mapEvent.GetMapEventSide(side);
			this._initialTroopCount = this.PartyGroup.TroopCount;
			this.PartyGroup.MakeReadyForMission(priorTroops);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00051DD0 File Offset: 0x0004FFD0
		public IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate)
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.AllocateTroops(ref list, numberToAllocate, this._customAllocationConditions);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			this._numAllocated += list.Count;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PartyGroupAgentOrigin(this, list[i], i);
			}
			if (array.Length < numberToAllocate)
			{
				this._anyTroopRemainsToBeSupplied = false;
			}
			return array;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00051E40 File Offset: 0x00050040
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			List<UniqueTroopDescriptor> list = null;
			this.PartyGroup.GetAllTroops(ref list);
			PartyGroupAgentOrigin[] array = new PartyGroupAgentOrigin[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PartyGroupAgentOrigin(this, list[i], i);
			}
			return array;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00051E88 File Offset: 0x00050088
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this.PartyGroup.LeaderParty.General;
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00051E9A File Offset: 0x0005009A
		public int NumRemovedTroops
		{
			get
			{
				return this._numWounded + this._numKilled + this._numRouted;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00051EB0 File Offset: 0x000500B0
		public int NumTroopsNotSupplied
		{
			get
			{
				return this._initialTroopCount - this._numAllocated;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00051EBF File Offset: 0x000500BF
		public bool AnyTroopRemainsToBeSupplied
		{
			get
			{
				return this._anyTroopRemainsToBeSupplied;
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00051EC8 File Offset: 0x000500C8
		public int GetNumberOfPlayerControllableTroops()
		{
			int num = 0;
			foreach (MapEventParty mapEventParty in this.PartyGroup.Parties)
			{
				PartyBase party = mapEventParty.Party;
				if (PartyGroupAgentOrigin.IsPartyUnderPlayerCommand(party) || (party.Side == PartyBase.MainParty.Side && this.PartyGroup.MapEvent.IsPlayerSergeant()))
				{
					num += party.NumberOfHealthyMembers;
				}
			}
			return num;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00051F58 File Offset: 0x00050158
		public void OnTroopWounded(UniqueTroopDescriptor troopDescriptor)
		{
			this._numWounded++;
			this.PartyGroup.OnTroopWounded(troopDescriptor);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00051F74 File Offset: 0x00050174
		public void OnTroopKilled(UniqueTroopDescriptor troopDescriptor)
		{
			this._numKilled++;
			this.PartyGroup.OnTroopKilled(troopDescriptor);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00051F90 File Offset: 0x00050190
		public void OnTroopRouted(UniqueTroopDescriptor troopDescriptor)
		{
			this._numRouted++;
			this.PartyGroup.OnTroopRouted(troopDescriptor);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00051FAC File Offset: 0x000501AC
		internal CharacterObject GetTroop(UniqueTroopDescriptor troopDescriptor)
		{
			return this.PartyGroup.GetAllocatedTroop(troopDescriptor) ?? this.PartyGroup.GetReadyTroop(troopDescriptor);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00051FCC File Offset: 0x000501CC
		public PartyBase GetParty(UniqueTroopDescriptor troopDescriptor)
		{
			PartyBase partyBase = this.PartyGroup.GetAllocatedTroopParty(troopDescriptor);
			if (partyBase == null)
			{
				partyBase = this.PartyGroup.GetReadyTroopParty(troopDescriptor);
			}
			return partyBase;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00051FF7 File Offset: 0x000501F7
		public void OnTroopScoreHit(UniqueTroopDescriptor descriptor, BasicCharacterObject attackedCharacter, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			this.PartyGroup.OnTroopScoreHit(descriptor, (CharacterObject)attackedCharacter, damage, isFatal, isTeamKill, attackerWeapon, false);
		}

		// Token: 0x04000600 RID: 1536
		private readonly int _initialTroopCount;

		// Token: 0x04000601 RID: 1537
		private int _numAllocated;

		// Token: 0x04000602 RID: 1538
		private int _numWounded;

		// Token: 0x04000603 RID: 1539
		private int _numKilled;

		// Token: 0x04000604 RID: 1540
		private int _numRouted;

		// Token: 0x04000605 RID: 1541
		private Func<UniqueTroopDescriptor, MapEventParty, bool> _customAllocationConditions;

		// Token: 0x04000606 RID: 1542
		private bool _anyTroopRemainsToBeSupplied = true;
	}
}
