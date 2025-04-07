using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x0200041C RID: 1052
	public class PeaceBarterable : Barterable
	{
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0013B126 File Offset: 0x00139326
		public CampaignTime Duration { get; }

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0013B12E File Offset: 0x0013932E
		public override string StringID
		{
			get
			{
				return "peace_barterable";
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0013B135 File Offset: 0x00139335
		public PeaceBarterable(Hero owner, IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration) : base(owner, null)
		{
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0013B155 File Offset: 0x00139355
		public PeaceBarterable(IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration) : base(peaceOfferingFaction.Leader, null)
		{
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003FDD RID: 16349 RVA: 0x0013B179 File Offset: 0x00139379
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=R0bJS0pn}Make peace with the {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OfferedFaction.InformalName);
				return textObject;
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0013B1A0 File Offset: 0x001393A0
		public override int GetUnitValueForFaction(IFaction factionToEvaluateFor)
		{
			float num = 0f;
			IFaction faction = this.OfferedFaction;
			IFaction faction2 = this.PeaceOfferingFaction;
			if (factionToEvaluateFor.MapFaction == faction)
			{
				IFaction faction3 = faction;
				faction = faction2;
				faction2 = faction3;
			}
			if (faction == null || faction2 == null)
			{
				return 0;
			}
			TextObject textObject;
			num = (float)((int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(faction2, faction, factionToEvaluateFor, out textObject));
			if (factionToEvaluateFor.IsKingdomFaction)
			{
				float num2 = 0f;
				int num3 = 0;
				foreach (Clan clan in ((Kingdom)factionToEvaluateFor).Clans)
				{
					float num4 = (clan.Leader != null) ? ((clan.Leader.Gold < 50000) ? (1f + 0.5f * ((50000f - (float)clan.Leader.Gold) / 50000f)) : ((clan.Leader.Gold > 200000) ? MathF.Max(0.66f, MathF.Pow(200000f / (float)clan.Leader.Gold, 0.4f)) : 1f)) : 1f;
					num2 += num4;
					num3++;
				}
				float num5 = (num2 + 1f) / ((float)num3 + 1f);
				num /= num5;
			}
			return (int)num;
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0013B308 File Offset: 0x00139508
		public override bool IsCompatible(Barterable barterable)
		{
			PeaceBarterable peaceBarterable = barterable as PeaceBarterable;
			return peaceBarterable == null || peaceBarterable.OfferedFaction != base.OriginalOwner.MapFaction;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0013B337 File Offset: 0x00139537
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0013B33A File Offset: 0x0013953A
		public override string GetEncyclopediaLink()
		{
			return base.OriginalOwner.MapFaction.EncyclopediaLink;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0013B34C File Offset: 0x0013954C
		public override void Apply()
		{
			if (this.PeaceOfferingFaction.MapFaction.IsAtWarWith(this.OfferedFaction))
			{
				MakePeaceAction.Apply(this.PeaceOfferingFaction.MapFaction, this.OfferedFaction, 0);
				if (PlayerEncounter.Current != null && Hero.OneToOneConversationHero == base.OriginalOwner)
				{
					PlayerEncounter.LeaveEncounter = true;
					PartyBase originalParty = base.OriginalParty;
					bool flag;
					if (originalParty == null)
					{
						flag = (null != null);
					}
					else
					{
						MobileParty mobileParty = originalParty.MobileParty;
						flag = (((mobileParty != null) ? mobileParty.Ai.AiBehaviorPartyBase : null) != null);
					}
					if (flag)
					{
						LocatableSearchData<MobileParty> locatableSearchData = Campaign.Current.MobilePartyLocator.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position2D, 5f);
						for (MobileParty mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData); mobileParty2 != null; mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData))
						{
							if (!mobileParty2.IsMainParty && mobileParty2.MapFaction == base.OriginalOwner.MapFaction && (mobileParty2.TargetParty == MobileParty.MainParty || mobileParty2.Ai.AiBehaviorPartyBase == PartyBase.MainParty))
							{
								mobileParty2.Ai.SetMoveModeHold();
							}
						}
						if (base.OriginalParty.MobileParty.Army != null && MobileParty.MainParty.Army != base.OriginalParty.MobileParty.Army)
						{
							base.OriginalParty.MobileParty.Army.LeaderParty.Ai.SetMoveModeHold();
						}
					}
				}
			}
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0013B4AD File Offset: 0x001396AD
		internal static void AutoGeneratedStaticCollectObjectsPeaceBarterable(object o, List<object> collectedObjects)
		{
			((PeaceBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0013B4BB File Offset: 0x001396BB
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x040012A6 RID: 4774
		public readonly IFaction PeaceOfferingFaction;

		// Token: 0x040012A7 RID: 4775
		public readonly IFaction OfferedFaction;
	}
}
