using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200032A RID: 810
	public class BannerEditorState : GameState
	{
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x000C1D72 File Offset: 0x000BFF72
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000C1D75 File Offset: 0x000BFF75
		// (set) Token: 0x06002E66 RID: 11878 RVA: 0x000C1D7D File Offset: 0x000BFF7D
		public IBannerEditorStateHandler Handler
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

		// Token: 0x06002E67 RID: 11879 RVA: 0x000C1D86 File Offset: 0x000BFF86
		public Clan GetClan()
		{
			return Clan.PlayerClan;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000C1D8D File Offset: 0x000BFF8D
		public CharacterObject GetCharacter()
		{
			return CharacterObject.PlayerCharacter;
		}

		// Token: 0x04000DDF RID: 3551
		private IBannerEditorStateHandler _handler;
	}
}
