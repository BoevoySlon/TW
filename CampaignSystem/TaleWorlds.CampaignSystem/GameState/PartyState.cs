using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200033E RID: 830
	public class PartyState : PlayerGameState
	{
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06002F0A RID: 12042 RVA: 0x000C25EE File Offset: 0x000C07EE
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x000C25F1 File Offset: 0x000C07F1
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x000C25F9 File Offset: 0x000C07F9
		public PartyScreenLogic PartyScreenLogic { get; private set; }

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002F0D RID: 12045 RVA: 0x000C2602 File Offset: 0x000C0802
		// (set) Token: 0x06002F0E RID: 12046 RVA: 0x000C260A File Offset: 0x000C080A
		public IPartyScreenLogicHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000C2613 File Offset: 0x000C0813
		public void InitializeLogic(PartyScreenLogic partyScreenLogic)
		{
			this.PartyScreenLogic = partyScreenLogic;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000C261C File Offset: 0x000C081C
		public void RequestUserInput(string text, Action accept, Action cancel)
		{
			if (this.Handler != null)
			{
				this.Handler.RequestUserInput(text, accept, cancel);
			}
		}

		// Token: 0x04000DFF RID: 3583
		private IPartyScreenLogicHandler _handler;
	}
}
