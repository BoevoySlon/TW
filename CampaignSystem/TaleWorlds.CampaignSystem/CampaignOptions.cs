using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000053 RID: 83
	public class CampaignOptions
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00023BC7 File Offset: 0x00021DC7
		private static CampaignOptions _current
		{
			get
			{
				Campaign campaign = Campaign.Current;
				if (campaign == null)
				{
					return null;
				}
				return campaign.Options;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00023BD9 File Offset: 0x00021DD9
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x00023BEB File Offset: 0x00021DEB
		public static bool IsLifeDeathCycleDisabled
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isLifeDeathCycleDisabled;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isLifeDeathCycleDisabled = value;
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00023BFF File Offset: 0x00021DFF
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x00023C11 File Offset: 0x00021E11
		public static bool AutoAllocateClanMemberPerks
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._autoAllocateClanMemberPerks;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._autoAllocateClanMemberPerks = value;
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x00023C25 File Offset: 0x00021E25
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00023C37 File Offset: 0x00021E37
		public static bool IsIronmanMode
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isIronmanMode;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isIronmanMode = value;
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00023C4B File Offset: 0x00021E4B
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00023C5D File Offset: 0x00021E5D
		public static CampaignOptions.Difficulty PlayerTroopsReceivedDamage
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerTroopsReceivedDamage;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerTroopsReceivedDamage = value;
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00023C71 File Offset: 0x00021E71
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00023C83 File Offset: 0x00021E83
		public static CampaignOptions.Difficulty PlayerReceivedDamage
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerReceivedDamage;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerReceivedDamage = value;
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00023C97 File Offset: 0x00021E97
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00023CA9 File Offset: 0x00021EA9
		public static CampaignOptions.Difficulty RecruitmentDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._recruitmentDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._recruitmentDifficulty = value;
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00023CBD File Offset: 0x00021EBD
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00023CCF File Offset: 0x00021ECF
		public static CampaignOptions.Difficulty PlayerMapMovementSpeed
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerMapMovementSpeed;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerMapMovementSpeed = value;
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00023CE3 File Offset: 0x00021EE3
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00023CF5 File Offset: 0x00021EF5
		public static CampaignOptions.Difficulty CombatAIDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._combatAIDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._combatAIDifficulty = value;
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00023D09 File Offset: 0x00021F09
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00023D1B File Offset: 0x00021F1B
		public static CampaignOptions.Difficulty PersuasionSuccessChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._persuasionSuccessChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._persuasionSuccessChance = value;
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00023D2F File Offset: 0x00021F2F
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00023D41 File Offset: 0x00021F41
		public static CampaignOptions.Difficulty ClanMemberDeathChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._clanMemberDeathChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._clanMemberDeathChance = value;
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00023D55 File Offset: 0x00021F55
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x00023D67 File Offset: 0x00021F67
		public static CampaignOptions.Difficulty BattleDeath
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._battleDeath;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._battleDeath = value;
				}
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00023D7C File Offset: 0x00021F7C
		public CampaignOptions()
		{
			this._playerTroopsReceivedDamage = CampaignOptions.Difficulty.VeryEasy;
			this._playerReceivedDamage = CampaignOptions.Difficulty.VeryEasy;
			this._recruitmentDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._playerMapMovementSpeed = CampaignOptions.Difficulty.VeryEasy;
			this._combatAIDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._persuasionSuccessChance = CampaignOptions.Difficulty.VeryEasy;
			this._clanMemberDeathChance = CampaignOptions.Difficulty.VeryEasy;
			this._battleDeath = CampaignOptions.Difficulty.VeryEasy;
			this._isLifeDeathCycleDisabled = false;
			this._autoAllocateClanMemberPerks = false;
			this._isIronmanMode = false;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00023DDC File Offset: 0x00021FDC
		internal static void AutoGeneratedStaticCollectObjectsCampaignOptions(object o, List<object> collectedObjects)
		{
			((CampaignOptions)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00023DEA File Offset: 0x00021FEA
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00023DEC File Offset: 0x00021FEC
		internal static object AutoGeneratedGetMemberValue_autoAllocateClanMemberPerks(object o)
		{
			return ((CampaignOptions)o)._autoAllocateClanMemberPerks;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00023DFE File Offset: 0x00021FFE
		internal static object AutoGeneratedGetMemberValue_playerTroopsReceivedDamage(object o)
		{
			return ((CampaignOptions)o)._playerTroopsReceivedDamage;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00023E10 File Offset: 0x00022010
		internal static object AutoGeneratedGetMemberValue_playerReceivedDamage(object o)
		{
			return ((CampaignOptions)o)._playerReceivedDamage;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00023E22 File Offset: 0x00022022
		internal static object AutoGeneratedGetMemberValue_recruitmentDifficulty(object o)
		{
			return ((CampaignOptions)o)._recruitmentDifficulty;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00023E34 File Offset: 0x00022034
		internal static object AutoGeneratedGetMemberValue_playerMapMovementSpeed(object o)
		{
			return ((CampaignOptions)o)._playerMapMovementSpeed;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00023E46 File Offset: 0x00022046
		internal static object AutoGeneratedGetMemberValue_combatAIDifficulty(object o)
		{
			return ((CampaignOptions)o)._combatAIDifficulty;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00023E58 File Offset: 0x00022058
		internal static object AutoGeneratedGetMemberValue_isLifeDeathCycleDisabled(object o)
		{
			return ((CampaignOptions)o)._isLifeDeathCycleDisabled;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00023E6A File Offset: 0x0002206A
		internal static object AutoGeneratedGetMemberValue_persuasionSuccessChance(object o)
		{
			return ((CampaignOptions)o)._persuasionSuccessChance;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00023E7C File Offset: 0x0002207C
		internal static object AutoGeneratedGetMemberValue_clanMemberDeathChance(object o)
		{
			return ((CampaignOptions)o)._clanMemberDeathChance;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00023E8E File Offset: 0x0002208E
		internal static object AutoGeneratedGetMemberValue_isIronmanMode(object o)
		{
			return ((CampaignOptions)o)._isIronmanMode;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00023EA0 File Offset: 0x000220A0
		internal static object AutoGeneratedGetMemberValue_battleDeath(object o)
		{
			return ((CampaignOptions)o)._battleDeath;
		}

		// Token: 0x040002A6 RID: 678
		[SaveableField(4)]
		private bool _autoAllocateClanMemberPerks;

		// Token: 0x040002A7 RID: 679
		[SaveableField(5)]
		private CampaignOptions.Difficulty _playerTroopsReceivedDamage;

		// Token: 0x040002A8 RID: 680
		[SaveableField(7)]
		private CampaignOptions.Difficulty _playerReceivedDamage;

		// Token: 0x040002A9 RID: 681
		[SaveableField(8)]
		private CampaignOptions.Difficulty _recruitmentDifficulty;

		// Token: 0x040002AA RID: 682
		[SaveableField(9)]
		private CampaignOptions.Difficulty _playerMapMovementSpeed;

		// Token: 0x040002AB RID: 683
		[SaveableField(11)]
		private CampaignOptions.Difficulty _combatAIDifficulty;

		// Token: 0x040002AC RID: 684
		[SaveableField(12)]
		private bool _isLifeDeathCycleDisabled;

		// Token: 0x040002AD RID: 685
		[SaveableField(13)]
		private CampaignOptions.Difficulty _persuasionSuccessChance;

		// Token: 0x040002AE RID: 686
		[SaveableField(14)]
		private CampaignOptions.Difficulty _clanMemberDeathChance;

		// Token: 0x040002AF RID: 687
		[SaveableField(15)]
		private bool _isIronmanMode;

		// Token: 0x040002B0 RID: 688
		[SaveableField(17)]
		private CampaignOptions.Difficulty _battleDeath;

		// Token: 0x0200049C RID: 1180
		public enum Difficulty : short
		{
			// Token: 0x040013F1 RID: 5105
			VeryEasy,
			// Token: 0x040013F2 RID: 5106
			Easy,
			// Token: 0x040013F3 RID: 5107
			Realistic
		}
	}
}
