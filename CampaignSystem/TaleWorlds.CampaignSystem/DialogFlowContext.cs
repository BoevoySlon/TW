using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000079 RID: 121
	internal class DialogFlowContext
	{
		// Token: 0x06000F3C RID: 3900 RVA: 0x000482FC File Offset: 0x000464FC
		public DialogFlowContext(string token, bool byPlayer, DialogFlowContext parent)
		{
			this.Token = token;
			this.ByPlayer = byPlayer;
			this.Parent = parent;
		}

		// Token: 0x0400051B RID: 1307
		internal readonly string Token;

		// Token: 0x0400051C RID: 1308
		internal readonly bool ByPlayer;

		// Token: 0x0400051D RID: 1309
		internal readonly DialogFlowContext Parent;
	}
}
