using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x02000420 RID: 1056
	public class PartyAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004007 RID: 16391 RVA: 0x0013BE64 File Offset: 0x0013A064
		// (set) Token: 0x06004008 RID: 16392 RVA: 0x0013BEC5 File Offset: 0x0013A0C5
		public PartyBase Party
		{
			get
			{
				PartyBase party = this._party;
				if (this._troop.IsHero && this._troop.HeroObject.PartyBelongedTo != null && this._troop.HeroObject.PartyBelongedTo.Party != null)
				{
					party = this._troop.HeroObject.PartyBelongedTo.Party;
				}
				return party;
			}
			set
			{
				this._party = value;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004009 RID: 16393 RVA: 0x0013BECE File Offset: 0x0013A0CE
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x0013BED8 File Offset: 0x0013A0D8
		public Banner Banner
		{
			get
			{
				if (this.Party == null)
				{
					if (!this._troop.IsHero)
					{
						return null;
					}
					return this._troop.HeroObject.MapFaction.Banner;
				}
				else
				{
					if (this.Party.LeaderHero == null)
					{
						return this.Party.MapFaction.Banner;
					}
					return this.Party.LeaderHero.ClanBanner;
				}
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600400B RID: 16395 RVA: 0x0013BF40 File Offset: 0x0013A140
		public BasicCharacterObject Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x0013BF48 File Offset: 0x0013A148
		// (set) Token: 0x0600400D RID: 16397 RVA: 0x0013BF50 File Offset: 0x0013A150
		public int Rank { get; private set; }

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x0013BF5C File Offset: 0x0013A15C
		public bool IsUnderPlayersCommand
		{
			get
			{
				PartyBase party = this.Party;
				return (party != null && party == PartyBase.MainParty) || party.Owner == Hero.MainHero || party.MapFaction.Leader == Hero.MainHero;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600400F RID: 16399 RVA: 0x0013BF9C File Offset: 0x0013A19C
		public uint FactionColor
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x0013BFCC File Offset: 0x0013A1CC
		public uint FactionColor2
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color2;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x0013BFFC File Offset: 0x0013A1FC
		public int Seed
		{
			get
			{
				if (this.Party == null)
				{
					return 0;
				}
				return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this._troop, this.Rank);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004012 RID: 16402 RVA: 0x0013C020 File Offset: 0x0013A220
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0013C03C File Offset: 0x0013A23C
		public PartyAgentOrigin(PartyBase partyBase, CharacterObject characterObject, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor), bool alwaysWounded = false)
		{
			this.Party = partyBase;
			this._troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._alwaysWounded = alwaysWounded;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0013C0A0 File Offset: 0x0013A2A0
		public void SetWounded()
		{
			if (this._troop.IsHero)
			{
				this._troop.HeroObject.MakeWounded(null, KillCharacterAction.KillCharacterActionDetail.None);
			}
			if (this.Party != null)
			{
				this.Party.MemberRoster.AddToCounts(this._troop, 0, false, 1, 0, true, -1);
			}
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0013C0F4 File Offset: 0x0013A2F4
		public void SetKilled()
		{
			if (this._alwaysWounded)
			{
				this.SetWounded();
				return;
			}
			if (this._troop.IsHero)
			{
				KillCharacterAction.ApplyByBattle(this._troop.HeroObject, null, true);
				return;
			}
			if (!this._troop.IsHero)
			{
				PartyBase party = this.Party;
				if (party == null)
				{
					return;
				}
				party.MemberRoster.AddToCounts(this._troop, -1, false, 0, 0, true, -1);
			}
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0013C15F File Offset: 0x0013A35F
		public void SetRouted()
		{
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0013C161 File Offset: 0x0013A361
		public void OnAgentRemoved(float agentHealth)
		{
			if (this._troop.IsHero && this._troop.HeroObject.HeroState != Hero.CharacterStates.Dead)
			{
				this._troop.HeroObject.HitPoints = MathF.Max(1, MathF.Round(agentHealth));
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0013C19F File Offset: 0x0013A39F
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x0013C1A1 File Offset: 0x0013A3A1
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040012AF RID: 4783
		private PartyBase _party;

		// Token: 0x040012B0 RID: 4784
		private CharacterObject _troop;

		// Token: 0x040012B2 RID: 4786
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x040012B3 RID: 4787
		private readonly bool _alwaysWounded;
	}
}
