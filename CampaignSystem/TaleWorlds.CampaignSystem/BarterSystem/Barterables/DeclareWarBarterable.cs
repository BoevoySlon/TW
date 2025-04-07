using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000413 RID: 1043
	public class DeclareWarBarterable : Barterable
	{
		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003F74 RID: 16244 RVA: 0x00139BA6 File Offset: 0x00137DA6
		public override string StringID
		{
			get
			{
				return "declare_war_barterable";
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06003F75 RID: 16245 RVA: 0x00139BAD File Offset: 0x00137DAD
		// (set) Token: 0x06003F76 RID: 16246 RVA: 0x00139BB5 File Offset: 0x00137DB5
		public IFaction DeclaringFaction { get; private set; }

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06003F77 RID: 16247 RVA: 0x00139BBE File Offset: 0x00137DBE
		// (set) Token: 0x06003F78 RID: 16248 RVA: 0x00139BC6 File Offset: 0x00137DC6
		public IFaction OtherFaction { get; private set; }

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06003F79 RID: 16249 RVA: 0x00139BCF File Offset: 0x00137DCF
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=GZwNgIon}Declare war against {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OtherFaction.Name);
				return textObject;
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00139BF3 File Offset: 0x00137DF3
		public DeclareWarBarterable(IFaction declaringFaction, IFaction otherFaction) : base(declaringFaction.Leader, null)
		{
			this.DeclaringFaction = declaringFaction;
			this.OtherFaction = otherFaction;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00139C10 File Offset: 0x00137E10
		public override void Apply()
		{
			DeclareWarAction.ApplyByDefault(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction);
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x00139C30 File Offset: 0x00137E30
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int result = 0;
			Clan evaluatingFaction = (faction is Clan) ? ((Clan)faction) : ((Kingdom)faction).RulingClan;
			if (faction.MapFaction == base.OriginalOwner.MapFaction)
			{
				TextObject textObject;
				result = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction, evaluatingFaction, out textObject);
			}
			else if (faction.MapFaction == this.OtherFaction.MapFaction)
			{
				TextObject textObject;
				result = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(this.OtherFaction.MapFaction, base.OriginalOwner.MapFaction, evaluatingFaction, out textObject);
			}
			return result;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x00139CE2 File Offset: 0x00137EE2
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}
	}
}
