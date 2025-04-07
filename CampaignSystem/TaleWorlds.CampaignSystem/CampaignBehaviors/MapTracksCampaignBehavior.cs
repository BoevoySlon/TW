using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003AC RID: 940
	public class MapTracksCampaignBehavior : CampaignBehaviorBase, IMapTracksCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x0010EE41 File Offset: 0x0010D041
		public MBReadOnlyList<Track> DetectedTracks
		{
			get
			{
				return this._detectedTracksCache;
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x0010EE4C File Offset: 0x0010D04C
		public MapTracksCampaignBehavior()
		{
			this._trackPool = new MapTracksCampaignBehavior.TrackPool(2048);
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x0010EEA4 File Offset: 0x0010D0A4
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.GameLoadFinished));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnHourlyTickParty));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x0010EF24 File Offset: 0x0010D124
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._trackDataDictionary.ContainsKey(mobileParty))
			{
				this._trackDataDictionary.Remove(mobileParty);
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x0010EF41 File Offset: 0x0010D141
		private void OnNewGameCreated(CampaignGameStarter gameStarted)
		{
			this._trackDataDictionary = new Dictionary<MobileParty, Vec2>();
			this.AddEventHandler();
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x0010EF54 File Offset: 0x0010D154
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Track>>("_allTracks", ref this._allTracks);
			dataStore.SyncData<Dictionary<MobileParty, Vec2>>("_trackDataDictionary", ref this._trackDataDictionary);
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x0010EF7C File Offset: 0x0010D17C
		private void OnHourlyTickParty(MobileParty mobileParty)
		{
			if (Campaign.Current.Models.MapTrackModel.CanPartyLeaveTrack(mobileParty))
			{
				Vec2 v = Vec2.Zero;
				if (this._trackDataDictionary.ContainsKey(mobileParty))
				{
					v = this._trackDataDictionary[mobileParty];
				}
				if (v.DistanceSquared(mobileParty.Position2D) > 5f && this.IsTrackDropped(mobileParty))
				{
					Vec2 position2D = mobileParty.Position2D;
					Vec2 trackDirection = mobileParty.Position2D - v;
					trackDirection.Normalize();
					this.AddTrack(mobileParty, position2D, trackDirection);
					this._trackDataDictionary[mobileParty] = position2D;
				}
			}
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x0010F010 File Offset: 0x0010D210
		private void OnHourlyTick()
		{
			this.RemoveExpiredTracks();
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x0010F018 File Offset: 0x0010D218
		private void GameLoadFinished()
		{
			this._allTracks.RemoveAll((Track x) => x.IsExpired);
			this._detectedTracksCache = (from x in this._allTracks
			where x.IsDetected
			select x).ToMBList<Track>();
			this.AddEventHandler();
			foreach (Track locatable in this._allTracks)
			{
				this._trackLocator.UpdateLocator(locatable);
			}
			foreach (MobileParty mobileParty in this._trackDataDictionary.Keys.ToList<MobileParty>())
			{
				if (!mobileParty.IsActive)
				{
					this._trackDataDictionary.Remove(mobileParty);
				}
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x0010F134 File Offset: 0x0010D334
		private void AddEventHandler()
		{
			this._quarterHourlyTick = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(0.25f), CampaignTime.Hours(0.1f));
			this._quarterHourlyTick.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.QuarterHourlyTick));
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x0010F16C File Offset: 0x0010D36C
		private void QuarterHourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			if (!PartyBase.MainParty.IsValid)
			{
				return;
			}
			int num = (MobileParty.MainParty.EffectiveScout != null) ? MobileParty.MainParty.EffectiveScout.GetSkillValue(DefaultSkills.Scouting) : 0;
			if (num != 0)
			{
				float maxTrackSpottingDistanceForMainParty = Campaign.Current.Models.MapTrackModel.GetMaxTrackSpottingDistanceForMainParty();
				LocatableSearchData<Track> locatableSearchData = this._trackLocator.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position2D, maxTrackSpottingDistanceForMainParty);
				for (Track track = this._trackLocator.FindNextLocatable(ref locatableSearchData); track != null; track = this._trackLocator.FindNextLocatable(ref locatableSearchData))
				{
					if (!track.IsDetected && this._allTracks.Contains(track) && Campaign.Current.Models.MapTrackModel.GetTrackDetectionDifficultyForMainParty(track, maxTrackSpottingDistanceForMainParty) < (float)num)
					{
						this.TrackDetected(track);
					}
				}
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0010F234 File Offset: 0x0010D434
		private void RemoveExpiredTracks()
		{
			for (int i = this._allTracks.Count - 1; i >= 0; i--)
			{
				Track track = this._allTracks[i];
				if (track.IsExpired)
				{
					this._allTracks.Remove(track);
					if (this._detectedTracksCache.Contains(track))
					{
						this._detectedTracksCache.Remove(track);
						CampaignEventDispatcher.Instance.TrackLost(track);
					}
					this._trackLocator.RemoveLocatable(track);
					this._trackPool.ReleaseTrack(track);
				}
			}
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x0010F2B9 File Offset: 0x0010D4B9
		private void TrackDetected(Track track)
		{
			track.IsDetected = true;
			this._detectedTracksCache.Add(track);
			CampaignEventDispatcher.Instance.TrackDetected(track);
			SkillLevelingManager.OnTrackDetected(track);
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x0010F2E0 File Offset: 0x0010D4E0
		public bool IsTrackDropped(MobileParty mobileParty)
		{
			float skipTrackChance = Campaign.Current.Models.MapTrackModel.GetSkipTrackChance(mobileParty);
			if (MBRandom.RandomFloat < skipTrackChance)
			{
				return false;
			}
			float num = mobileParty.Position2D.DistanceSquared(MobileParty.MainParty.Position2D);
			float num2 = MobileParty.MainParty.Speed * Campaign.Current.Models.MapTrackModel.MaxTrackLife;
			return num2 * num2 > num;
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x0010F350 File Offset: 0x0010D550
		public void AddTrack(MobileParty party, Vec2 trackPosition, Vec2 trackDirection)
		{
			Track track = this._trackPool.RequestTrack(party, trackPosition, trackDirection);
			this._allTracks.Add(track);
			this._trackLocator.UpdateLocator(track);
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x0010F388 File Offset: 0x0010D588
		public void AddMapArrow(TextObject pointerName, Vec2 trackPosition, Vec2 trackDirection, float life)
		{
			Track track = this._trackPool.RequestMapArrow(pointerName, trackPosition, trackDirection, life);
			this._allTracks.Add(track);
			this._trackLocator.UpdateLocator(track);
			this.TrackDetected(track);
		}

		// Token: 0x04001198 RID: 4504
		private const float PartyTrackPositionDelta = 5f;

		// Token: 0x04001199 RID: 4505
		private List<Track> _allTracks = new List<Track>();

		// Token: 0x0400119A RID: 4506
		private MBList<Track> _detectedTracksCache = new MBList<Track>();

		// Token: 0x0400119B RID: 4507
		private Dictionary<MobileParty, Vec2> _trackDataDictionary = new Dictionary<MobileParty, Vec2>();

		// Token: 0x0400119C RID: 4508
		private MBCampaignEvent _quarterHourlyTick;

		// Token: 0x0400119D RID: 4509
		private LocatorGrid<Track> _trackLocator = new LocatorGrid<Track>(5f, 32, 32);

		// Token: 0x0400119E RID: 4510
		private MapTracksCampaignBehavior.TrackPool _trackPool;

		// Token: 0x02000714 RID: 1812
		private class TrackPool
		{
			// Token: 0x170013B3 RID: 5043
			// (get) Token: 0x060058B0 RID: 22704 RVA: 0x00183104 File Offset: 0x00181304
			private int MaxSize { get; }

			// Token: 0x170013B4 RID: 5044
			// (get) Token: 0x060058B1 RID: 22705 RVA: 0x0018310C File Offset: 0x0018130C
			public int Size
			{
				get
				{
					Stack<Track> stack = this._stack;
					if (stack == null)
					{
						return 0;
					}
					return stack.Count;
				}
			}

			// Token: 0x060058B2 RID: 22706 RVA: 0x00183120 File Offset: 0x00181320
			public TrackPool(int size)
			{
				this.MaxSize = size;
				this._stack = new Stack<Track>();
				for (int i = 0; i < size; i++)
				{
					this._stack.Push(new Track());
				}
			}

			// Token: 0x060058B3 RID: 22707 RVA: 0x00183164 File Offset: 0x00181364
			public Track RequestTrack(MobileParty party, Vec2 trackPosition, Vec2 trackDirection)
			{
				Track track = (this._stack.Count > 0) ? this._stack.Pop() : new Track();
				int num = party.Party.NumberOfAllMembers;
				int num2 = party.Party.NumberOfHealthyMembers;
				int num3 = party.Party.NumberOfMenWithHorse;
				int num4 = party.Party.NumberOfMenWithoutHorse;
				int num5 = party.Party.NumberOfPackAnimals;
				int num6 = party.Party.NumberOfPrisoners;
				TextObject partyName = party.Name;
				if (party.Army != null && party.Army.LeaderParty == party)
				{
					partyName = party.ArmyName;
					foreach (MobileParty mobileParty in party.Army.LeaderParty.AttachedParties)
					{
						num += mobileParty.Party.NumberOfAllMembers;
						num2 += mobileParty.Party.NumberOfHealthyMembers;
						num3 += mobileParty.Party.NumberOfMenWithHorse;
						num4 += mobileParty.Party.NumberOfMenWithoutHorse;
						num5 += mobileParty.Party.NumberOfPackAnimals;
						num6 += mobileParty.Party.NumberOfPrisoners;
					}
				}
				track.Position = trackPosition;
				track.Direction = trackDirection.RotationInRadians;
				track.PartyType = Track.GetPartyTypeEnum(party);
				track.PartyName = partyName;
				track.Culture = party.Party.Culture;
				if (track.Culture == null)
				{
					string message = string.Format("Track culture is null for {0}: {1}", party.StringId, party.Name);
					Debug.Print(message, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert(message, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\MapTracksCampaignBehavior.cs", "RequestTrack", 62);
				}
				track.Speed = party.Speed;
				track.Life = (float)Campaign.Current.Models.MapTrackModel.GetTrackLife(party);
				track.IsEnemy = FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, party.MapFaction);
				track.NumberOfAllMembers = num;
				track.NumberOfHealthyMembers = num2;
				track.NumberOfMenWithHorse = num3;
				track.NumberOfMenWithoutHorse = num4;
				track.NumberOfPackAnimals = num5;
				track.NumberOfPrisoners = num6;
				track.IsPointer = false;
				track.IsDetected = false;
				track.CreationTime = CampaignTime.Now;
				return track;
			}

			// Token: 0x060058B4 RID: 22708 RVA: 0x001833B8 File Offset: 0x001815B8
			public Track RequestMapArrow(TextObject pointerName, Vec2 trackPosition, Vec2 trackDirection, float life)
			{
				Track track = (this._stack.Count > 0) ? this._stack.Pop() : new Track();
				track.Position = trackPosition;
				track.Direction = trackDirection.RotationInRadians;
				track.PartyName = pointerName;
				track.Life = life;
				track.IsPointer = true;
				track.IsDetected = true;
				track.CreationTime = CampaignTime.Now;
				return track;
			}

			// Token: 0x060058B5 RID: 22709 RVA: 0x00183421 File Offset: 0x00181621
			public void ReleaseTrack(Track track)
			{
				track.Reset();
				if (this._stack.Count < this.MaxSize)
				{
					this._stack.Push(track);
				}
			}

			// Token: 0x060058B6 RID: 22710 RVA: 0x00183448 File Offset: 0x00181648
			public override string ToString()
			{
				return string.Format("TrackPool: {0}", this.Size);
			}

			// Token: 0x04001DA7 RID: 7591
			private Stack<Track> _stack;
		}
	}
}
