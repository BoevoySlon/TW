﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x02000356 RID: 854
	public class Hideout : SettlementComponent, ISpottable
	{
		// Token: 0x060030C7 RID: 12487 RVA: 0x000CE453 File Offset: 0x000CC653
		internal static void AutoGeneratedStaticCollectObjectsHideout(object o, List<object> collectedObjects)
		{
			((Hideout)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000CE461 File Offset: 0x000CC661
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._nextPossibleAttackTime, collectedObjects);
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000CE47B File Offset: 0x000CC67B
		internal static object AutoGeneratedGetMemberValue_nextPossibleAttackTime(object o)
		{
			return ((Hideout)o)._nextPossibleAttackTime;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000CE48D File Offset: 0x000CC68D
		internal static object AutoGeneratedGetMemberValue_isSpotted(object o)
		{
			return ((Hideout)o)._isSpotted;
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000CE49F File Offset: 0x000CC69F
		public CampaignTime NextPossibleAttackTime
		{
			get
			{
				return this._nextPossibleAttackTime;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000CE4A7 File Offset: 0x000CC6A7
		public static MBReadOnlyList<Hideout> All
		{
			get
			{
				return Campaign.Current.AllHideouts;
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000CE4B3 File Offset: 0x000CC6B3
		public void UpdateNextPossibleAttackTime()
		{
			this._nextPossibleAttackTime = CampaignTime.Now + CampaignTime.Hours(12f);
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x000CE4D0 File Offset: 0x000CC6D0
		public bool IsInfested
		{
			get
			{
				return base.Owner.Settlement.Parties.CountQ((MobileParty x) => x.IsBandit) >= Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			}
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000CE52A File Offset: 0x000CC72A
		public IEnumerable<PartyBase> GetDefenderParties(MapEvent.BattleTypes battleType)
		{
			yield return base.Settlement.Party;
			foreach (MobileParty mobileParty in base.Settlement.Parties)
			{
				if (mobileParty.IsBandit || mobileParty.IsBanditBossParty)
				{
					yield return mobileParty.Party;
				}
			}
			List<MobileParty>.Enumerator enumerator = default(List<MobileParty>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000CE53C File Offset: 0x000CC73C
		public PartyBase GetNextDefenderParty(ref int partyIndex, MapEvent.BattleTypes battleType)
		{
			partyIndex++;
			if (partyIndex == 0)
			{
				return base.Settlement.Party;
			}
			for (int i = partyIndex - 1; i < base.Settlement.Parties.Count; i++)
			{
				MobileParty mobileParty = base.Settlement.Parties[i];
				if (mobileParty.IsBandit || mobileParty.IsBanditBossParty)
				{
					partyIndex = i + 1;
					return mobileParty.Party;
				}
			}
			return null;
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000CE5AC File Offset: 0x000CC7AC
		// (set) Token: 0x060030D2 RID: 12498 RVA: 0x000CE5B4 File Offset: 0x000CC7B4
		public string SceneName { get; private set; }

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x000CE5C0 File Offset: 0x000CC7C0
		public IFaction MapFaction
		{
			get
			{
				foreach (MobileParty mobileParty in base.Settlement.Parties)
				{
					if (mobileParty.IsBandit)
					{
						return mobileParty.ActualClan;
					}
				}
				foreach (Clan clan in Clan.All)
				{
					if (clan.IsBanditFaction)
					{
						return clan;
					}
				}
				return null;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x000CE670 File Offset: 0x000CC870
		// (set) Token: 0x060030D5 RID: 12501 RVA: 0x000CE678 File Offset: 0x000CC878
		public bool IsSpotted
		{
			get
			{
				return this._isSpotted;
			}
			set
			{
				this._isSpotted = value;
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000CE681 File Offset: 0x000CC881
		public void SetScene(string sceneName)
		{
			this.SceneName = sceneName;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000CE68A File Offset: 0x000CC88A
		public Hideout()
		{
			this.IsSpotted = false;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000CE699 File Offset: 0x000CC899
		public override void OnPartyEntered(MobileParty mobileParty)
		{
			base.OnPartyEntered(mobileParty);
			this.UpdateOwnership();
			if (mobileParty.MapFaction.IsBanditFaction)
			{
				mobileParty.BanditPartyComponent.SetHomeHideout(base.Owner.Settlement.Hideout);
			}
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000CE6D0 File Offset: 0x000CC8D0
		public override void OnPartyLeft(MobileParty mobileParty)
		{
			this.UpdateOwnership();
			if (base.Owner.Settlement.Parties.Count == 0)
			{
				this.OnHideoutIsEmpty();
			}
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000CE6F5 File Offset: 0x000CC8F5
		public override void OnRelatedPartyRemoved(MobileParty mobileParty)
		{
			if (base.Owner.Settlement.Parties.Count == 0)
			{
				this.OnHideoutIsEmpty();
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000CE714 File Offset: 0x000CC914
		private void OnHideoutIsEmpty()
		{
			this.IsSpotted = false;
			base.Owner.Settlement.IsVisible = false;
			CampaignEventDispatcher.Instance.OnHideoutDeactivated(base.Settlement);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000CE73E File Offset: 0x000CC93E
		public override void OnInit()
		{
			base.Owner.Settlement.IsVisible = false;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000CE754 File Offset: 0x000CC954
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.BackgroundCropPosition = float.Parse(node.Attributes.get_ItemOf("background_crop_position").Value);
			base.BackgroundMeshName = node.Attributes.get_ItemOf("background_mesh").Value;
			base.WaitMeshName = node.Attributes.get_ItemOf("wait_mesh").Value;
			base.Deserialize(objectManager, node);
			if (node.Attributes.get_ItemOf("scene_name") != null)
			{
				this.SceneName = node.Attributes.get_ItemOf("scene_name").InnerText;
			}
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000CE7EC File Offset: 0x000CC9EC
		private void UpdateOwnership()
		{
			if (base.Owner.MemberRoster.Count == 0 || base.Owner.Settlement.Parties.All((MobileParty x) => x.Party.Owner != base.Owner.Owner))
			{
				base.Owner.Settlement.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000CE843 File Offset: 0x000CCA43
		protected override void OnInventoryUpdated(ItemRosterElement item, int count)
		{
		}

		// Token: 0x04000FE8 RID: 4072
		[SaveableField(200)]
		private CampaignTime _nextPossibleAttackTime;

		// Token: 0x04000FE9 RID: 4073
		[SaveableField(201)]
		private bool _isSpotted;
	}
}
