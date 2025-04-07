using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x0200041B RID: 1051
	public class NoAttackBarterable : Barterable
	{
		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x0013AFA0 File Offset: 0x001391A0
		public override string StringID
		{
			get
			{
				return "no_attack_barterable";
			}
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0013AFA7 File Offset: 0x001391A7
		public NoAttackBarterable(Hero originalOwner, Hero otherHero, PartyBase ownerParty, PartyBase otherParty, CampaignTime duration) : base(originalOwner, ownerParty)
		{
			this._otherFaction = otherParty.MapFaction;
			this._duration = duration;
			this._otherHero = otherHero;
			this._otherParty = otherParty;
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x0013AFD8 File Offset: 0x001391D8
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=Y3lGJT8H}{PARTY} Won't attack {FACTION} for {DURATION} {?DURATION>1}days{?}day{\\?}.", null);
				textObject.SetTextVariable("PARTY", base.OriginalParty.Name);
				textObject.SetTextVariable("FACTION", this._otherFaction.Name);
				textObject.SetTextVariable("DURATION", this._duration.ToDays.ToString());
				return textObject;
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0013B040 File Offset: 0x00139240
		public override void Apply()
		{
			if (base.OriginalParty == MobileParty.MainParty.Party)
			{
				if (this._otherFaction.NotAttackableByPlayerUntilTime.IsPast)
				{
					this._otherFaction.NotAttackableByPlayerUntilTime = CampaignTime.Now;
				}
				this._otherFaction.NotAttackableByPlayerUntilTime += this._duration;
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0013B0A0 File Offset: 0x001392A0
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int result = 0;
			float militaryValueOfParty = Campaign.Current.Models.ValuationModel.GetMilitaryValueOfParty(base.OriginalParty.MobileParty);
			if (faction.MapFaction == this._otherFaction.MapFaction && faction.MapFaction.IsAtWarWith(base.OriginalParty.MapFaction))
			{
				result = (int)(militaryValueOfParty * 0.1f);
			}
			else if (faction.MapFaction == base.OriginalParty.MapFaction)
			{
				result = -(int)(militaryValueOfParty * 0.1f);
			}
			return result;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0013B123 File Offset: 0x00139323
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x040012A1 RID: 4769
		private readonly IFaction _otherFaction;

		// Token: 0x040012A2 RID: 4770
		private readonly CampaignTime _duration;

		// Token: 0x040012A3 RID: 4771
		private readonly Hero _otherHero;

		// Token: 0x040012A4 RID: 4772
		private readonly PartyBase _otherParty;
	}
}
