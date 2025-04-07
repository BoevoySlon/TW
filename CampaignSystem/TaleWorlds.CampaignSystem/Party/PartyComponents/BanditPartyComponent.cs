﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Party.PartyComponents
{
	// Token: 0x020002B0 RID: 688
	public class BanditPartyComponent : WarPartyComponent
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x000AAD7C File Offset: 0x000A8F7C
		internal static void AutoGeneratedStaticCollectObjectsBanditPartyComponent(object o, List<object> collectedObjects)
		{
			((BanditPartyComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000AAD8A File Offset: 0x000A8F8A
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._relatedSettlement);
			collectedObjects.Add(this.Hideout);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000AADAB File Offset: 0x000A8FAB
		internal static object AutoGeneratedGetMemberValueHideout(object o)
		{
			return ((BanditPartyComponent)o).Hideout;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000AADB8 File Offset: 0x000A8FB8
		internal static object AutoGeneratedGetMemberValueIsBossParty(object o)
		{
			return ((BanditPartyComponent)o).IsBossParty;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000AADCA File Offset: 0x000A8FCA
		internal static object AutoGeneratedGetMemberValue_relatedSettlement(object o)
		{
			return ((BanditPartyComponent)o)._relatedSettlement;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000AADD8 File Offset: 0x000A8FD8
		public static MobileParty CreateBanditParty(string stringId, Clan clan, Hideout hideout, bool isBossParty)
		{
			return MobileParty.CreateParty(stringId, new BanditPartyComponent(hideout, isBossParty), delegate(MobileParty mobileParty)
			{
				mobileParty.ActualClan = clan;
			});
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000AAE0C File Offset: 0x000A900C
		public static MobileParty CreateLooterParty(string stringId, Clan clan, Settlement relatedSettlement, bool isBossParty)
		{
			return MobileParty.CreateParty(stringId, new BanditPartyComponent(relatedSettlement), delegate(MobileParty mobileParty)
			{
				mobileParty.ActualClan = clan;
			});
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000AAE3E File Offset: 0x000A903E
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x000AAE46 File Offset: 0x000A9046
		[SaveableProperty(1)]
		public Hideout Hideout { get; private set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000AAE4F File Offset: 0x000A904F
		// (set) Token: 0x060027F9 RID: 10233 RVA: 0x000AAE57 File Offset: 0x000A9057
		[SaveableProperty(2)]
		public bool IsBossParty { get; private set; }

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x000AAE60 File Offset: 0x000A9060
		public override Settlement HomeSettlement
		{
			get
			{
				if (this.Hideout == null)
				{
					return this._relatedSettlement;
				}
				return this.Hideout.Settlement;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x000AAE7C File Offset: 0x000A907C
		public override Hero PartyOwner
		{
			get
			{
				Clan actualClan = base.MobileParty.ActualClan;
				if (actualClan == null)
				{
					return null;
				}
				return actualClan.Leader;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x000AAE94 File Offset: 0x000A9094
		public override TextObject Name
		{
			get
			{
				TextObject textObject;
				if (!Game.Current.IsDevelopmentMode)
				{
					if ((textObject = this._cachedName) == null)
					{
						textObject = (this._cachedName = ((this.Hideout != null) ? this.Hideout.MapFaction.Name : base.MobileParty.MapFaction.Name));
					}
				}
				else
				{
					textObject = new TextObject(base.MobileParty.StringId, null);
				}
				TextObject textObject2 = textObject;
				textObject2.SetTextVariable("IS_BANDIT", 1);
				return textObject2;
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000AAF0A File Offset: 0x000A910A
		protected internal BanditPartyComponent(Hideout hideout, bool isBossParty)
		{
			this.Hideout = hideout;
			this.IsBossParty = isBossParty;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000AAF20 File Offset: 0x000A9120
		protected internal BanditPartyComponent(Settlement relatedSettlement)
		{
			this._relatedSettlement = relatedSettlement;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000AAF2F File Offset: 0x000A912F
		public void SetHomeHideout(Hideout hideout)
		{
			this.Hideout = hideout;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000AAF38 File Offset: 0x000A9138
		public override void ClearCachedName()
		{
			this._cachedName = null;
		}

		// Token: 0x04000C2C RID: 3116
		[CachedData]
		private TextObject _cachedName;

		// Token: 0x04000C2E RID: 3118
		[SaveableField(3)]
		private readonly Settlement _relatedSettlement;
	}
}
