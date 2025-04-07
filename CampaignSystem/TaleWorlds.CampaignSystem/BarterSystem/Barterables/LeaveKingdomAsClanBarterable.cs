﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000418 RID: 1048
	public class LeaveKingdomAsClanBarterable : Barterable
	{
		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003FB0 RID: 16304 RVA: 0x0013A538 File Offset: 0x00138738
		public override string StringID
		{
			get
			{
				return "leave_faction_barterable";
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x0013A53F File Offset: 0x0013873F
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=x5POJVWw}Stop serving {FACTION}", null);
				textObject.SetTextVariable("FACTION", base.OriginalOwner.MapFaction.Name);
				return textObject;
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0013A568 File Offset: 0x00138768
		public LeaveKingdomAsClanBarterable(Hero owner, PartyBase ownerParty) : base(owner, ownerParty)
		{
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0013A574 File Offset: 0x00138774
		public override int GetUnitValueForFaction(IFaction faction)
		{
			Hero leader = base.OriginalOwner.Clan.Leader;
			IFaction mapFaction = base.OriginalOwner.MapFaction;
			if (faction != base.OriginalOwner.Clan)
			{
				float num;
				if (faction == base.OriginalOwner.MapFaction)
				{
					if (base.OriginalOwner.Clan.IsUnderMercenaryService)
					{
						num = Campaign.Current.Models.DiplomacyModel.GetScoreOfMercenaryToLeaveKingdom(base.OriginalOwner.Clan, base.OriginalOwner.Clan.Kingdom);
					}
					else
					{
						num = Campaign.Current.Models.DiplomacyModel.GetScoreOfClanToLeaveKingdom(base.OriginalOwner.Clan, base.OriginalOwner.Clan.Kingdom);
					}
					num *= (float)((faction == base.OriginalOwner.Clan || faction == base.OriginalOwner.Clan.Kingdom) ? -1 : 1);
				}
				else
				{
					float num2 = 0.5f;
					float num3 = 0.01f;
					float num4 = -0.5f;
					float clanStrength = Campaign.Current.Models.DiplomacyModel.GetClanStrength(base.OriginalOwner.Clan);
					if (faction.IsClan && FactionManager.IsAtWarAgainstFaction(faction, base.OriginalOwner.Clan.Kingdom))
					{
						num = clanStrength * num2;
					}
					else if (FactionManager.IsAlliedWithFaction(faction, base.OriginalOwner.Clan.Kingdom))
					{
						num = clanStrength * num4;
					}
					else
					{
						num = clanStrength * num3;
					}
				}
				return (int)num;
			}
			if (base.OriginalOwner.Clan.IsMinorFaction)
			{
				return (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfMercenaryToLeaveKingdom(base.OriginalOwner.Clan, base.OriginalOwner.Clan.Kingdom);
			}
			return (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfClanToLeaveKingdom(base.OriginalOwner.Clan, base.OriginalOwner.Clan.Kingdom);
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0013A756 File Offset: 0x00138956
		public override void CheckBarterLink(Barterable linkedBarterable)
		{
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0013A758 File Offset: 0x00138958
		public override bool IsCompatible(Barterable barterable)
		{
			JoinKingdomAsClanBarterable joinKingdomAsClanBarterable = barterable as JoinKingdomAsClanBarterable;
			return joinKingdomAsClanBarterable == null || joinKingdomAsClanBarterable.OriginalOwner != base.OriginalOwner || joinKingdomAsClanBarterable.TargetKingdom != base.OriginalOwner.MapFaction;
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0013A795 File Offset: 0x00138995
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new ImageIdentifier(BannerCode.CreateFrom(base.OriginalOwner.Clan.Banner), false);
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0013A7B2 File Offset: 0x001389B2
		public override string GetEncyclopediaLink()
		{
			return base.OriginalOwner.MapFaction.EncyclopediaLink;
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0013A7C4 File Offset: 0x001389C4
		public override void Apply()
		{
			if (base.OriginalOwner.Clan.IsUnderMercenaryService)
			{
				ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(base.OriginalOwner.Clan, true);
				return;
			}
			ChangeKingdomAction.ApplyByLeaveKingdom(base.OriginalOwner.Clan, true);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0013A7FB File Offset: 0x001389FB
		internal static void AutoGeneratedStaticCollectObjectsLeaveKingdomAsClanBarterable(object o, List<object> collectedObjects)
		{
			((LeaveKingdomAsClanBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0013A809 File Offset: 0x00138A09
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}
	}
}
