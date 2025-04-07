using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000417 RID: 1047
	public class JoinKingdomAsClanBarterable : Barterable
	{
		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x0013A1B6 File Offset: 0x001383B6
		public override string StringID
		{
			get
			{
				return "join_faction_barterable";
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0013A1BD File Offset: 0x001383BD
		public JoinKingdomAsClanBarterable(Hero owner, Kingdom targetKingdom, bool isDefecting = false) : base(owner, null)
		{
			this.TargetKingdom = targetKingdom;
			this.IsDefecting = isDefecting;
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x0013A1D5 File Offset: 0x001383D5
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=8Az4q2wp}Join {FACTION}", null);
				textObject.SetTextVariable("FACTION", this.TargetKingdom.Name);
				return textObject;
			}
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0013A1FC File Offset: 0x001383FC
		public override int GetUnitValueForFaction(IFaction factionForEvaluation)
		{
			float num = -1000000f;
			if (factionForEvaluation == base.OriginalOwner.Clan)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfClanToJoinKingdom(base.OriginalOwner.Clan, this.TargetKingdom);
				if (base.OriginalOwner.Clan.Kingdom != null)
				{
					int valueForFaction = new LeaveKingdomAsClanBarterable(base.OriginalOwner, base.OriginalParty).GetValueForFaction(factionForEvaluation);
					if (!this.TargetKingdom.IsAtWarWith(base.OriginalOwner.Clan.Kingdom))
					{
						float num2 = base.OriginalOwner.Clan.CalculateTotalSettlementValueForFaction(base.OriginalOwner.Clan.Kingdom);
						num -= num2 * ((this.TargetKingdom.Leader == Hero.MainHero) ? 0.5f : 1f);
					}
					num += (float)valueForFaction;
				}
			}
			else if (factionForEvaluation.MapFaction == this.TargetKingdom)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfKingdomToGetClan(this.TargetKingdom, base.OriginalOwner.Clan);
			}
			if (this.TargetKingdom == Clan.PlayerClan.Kingdom && Hero.MainHero.GetPerkValue(DefaultPerks.Trade.SilverTongue))
			{
				num += num * DefaultPerks.Trade.SilverTongue.PrimaryBonus;
			}
			return (int)num;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0013A349 File Offset: 0x00138549
		public override void CheckBarterLink(Barterable linkedBarterable)
		{
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0013A34C File Offset: 0x0013854C
		public override bool IsCompatible(Barterable barterable)
		{
			LeaveKingdomAsClanBarterable leaveKingdomAsClanBarterable = barterable as LeaveKingdomAsClanBarterable;
			return leaveKingdomAsClanBarterable == null || leaveKingdomAsClanBarterable.OriginalOwner.MapFaction != this.TargetKingdom;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0013A37B File Offset: 0x0013857B
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new ImageIdentifier(BannerCode.CreateFrom(this.TargetKingdom.Banner), false);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0013A393 File Offset: 0x00138593
		public override string GetEncyclopediaLink()
		{
			return this.TargetKingdom.EncyclopediaLink;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0013A3A0 File Offset: 0x001385A0
		public override void Apply()
		{
			if (this.TargetKingdom != null && this.TargetKingdom != null && this.TargetKingdom.Leader == Hero.MainHero)
			{
				int valueForFaction = base.GetValueForFaction(base.OriginalOwner.Clan);
				int relation = (valueForFaction < 0) ? (20 - valueForFaction / 20000) : 20;
				ChangeRelationAction.ApplyPlayerRelation(base.OriginalOwner.Clan.Leader, relation, true, true);
				if (base.OriginalOwner.Clan.MapFaction != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(base.OriginalOwner.Clan.Leader, base.OriginalOwner.Clan.MapFaction.Leader, -100, true);
				}
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker)
			{
				PlayerEncounter.Current.SetPlayerSiegeInterruptedByEnemyDefection();
			}
			if (base.OriginalOwner.Clan.Kingdom != null)
			{
				if (base.OriginalOwner.Clan.Kingdom != null && this.TargetKingdom != null && base.OriginalOwner.Clan.Kingdom.IsAtWarWith(this.TargetKingdom))
				{
					ChangeKingdomAction.ApplyByLeaveWithRebellionAgainstKingdom(base.OriginalOwner.Clan, true);
				}
				else
				{
					ChangeKingdomAction.ApplyByLeaveKingdom(base.OriginalOwner.Clan, true);
				}
			}
			if (this.IsDefecting)
			{
				ChangeKingdomAction.ApplyByJoinToKingdomByDefection(base.OriginalOwner.Clan, this.TargetKingdom, true);
				return;
			}
			ChangeKingdomAction.ApplyByJoinToKingdom(base.OriginalOwner.Clan, this.TargetKingdom, true);
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0013A521 File Offset: 0x00138721
		internal static void AutoGeneratedStaticCollectObjectsJoinKingdomAsClanBarterable(object o, List<object> collectedObjects)
		{
			((JoinKingdomAsClanBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0013A52F File Offset: 0x0013872F
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0400129C RID: 4764
		public readonly Kingdom TargetKingdom;

		// Token: 0x0400129D RID: 4765
		public readonly bool IsDefecting;
	}
}
