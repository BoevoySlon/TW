using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009E RID: 158
	public class SaveHandler
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00051465 File Offset: 0x0004F665
		// (set) Token: 0x0600117C RID: 4476 RVA: 0x0005146D File Offset: 0x0004F66D
		public IMainHeroVisualSupplier MainHeroVisualSupplier { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00051476 File Offset: 0x0004F676
		public bool IsSaving
		{
			get
			{
				return !this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>();
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00051486 File Offset: 0x0004F686
		public string IronmanModSaveName
		{
			get
			{
				return "Ironman" + Campaign.Current.UniqueGameId;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x0005149C File Offset: 0x0004F69C
		private bool _isAutoSaveEnabled
		{
			get
			{
				return this.AutoSaveInterval > -1;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000514A7 File Offset: 0x0004F6A7
		private double _autoSavePriorityTimeLimit
		{
			get
			{
				return (double)this.AutoSaveInterval * 0.75;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x000514BA File Offset: 0x0004F6BA
		public int AutoSaveInterval
		{
			get
			{
				ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
				if (sandBoxSaveManager == null)
				{
					return 15;
				}
				return sandBoxSaveManager.GetAutoSaveInterval();
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000514D7 File Offset: 0x0004F6D7
		public void QuickSaveCurrentGame()
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.QuickSave, null);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000514E1 File Offset: 0x0004F6E1
		public void SaveAs(string saveName)
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.SaveAs, saveName);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000514EC File Offset: 0x0004F6EC
		private void TryAutoSave(bool isPriority)
		{
			MapState mapState;
			if (this._isAutoSaveEnabled && (mapState = (GameStateManager.Current.ActiveState as MapState)) != null && !mapState.MapConversationActive)
			{
				double totalMinutes = (DateTime.Now - this._lastAutoSaveTime).TotalMinutes;
				double num = isPriority ? this._autoSavePriorityTimeLimit : ((double)this.AutoSaveInterval);
				if (totalMinutes > num)
				{
					this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.AutoSave, null);
				}
			}
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00051552 File Offset: 0x0004F752
		public void CampaignTick()
		{
			if (Campaign.Current.TimeControlMode != CampaignTimeControlMode.Stop)
			{
				this.TryAutoSave(false);
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00051568 File Offset: 0x0004F768
		internal void SaveTick()
		{
			if (!this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>())
			{
				switch (this._saveStep)
				{
				case SaveHandler.SaveSteps.PreSave:
					this._saveStep++;
					this.OnSaveStarted();
					return;
				case SaveHandler.SaveSteps.Saving:
				{
					this._saveStep++;
					CampaignEventDispatcher.Instance.OnBeforeSave();
					if (CampaignOptions.IsIronmanMode)
					{
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), this.IronmanModSaveName, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					}
					SaveHandler.SaveArgs saveArgs = this.SaveArgsQueue.Peek();
					switch (saveArgs.Mode)
					{
					case SaveHandler.SaveArgs.SaveMode.SaveAs:
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), saveArgs.Name, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.QuickSave:
						MBSaveLoad.QuickSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.AutoSave:
						MBSaveLoad.AutoSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					default:
						return;
					}
					break;
				}
				case SaveHandler.SaveSteps.AwaitingCompletion:
					return;
				}
				this._saveStep++;
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0005167B File Offset: 0x0004F87B
		private void OnSaveCompleted(ValueTuple<SaveResult, string> result)
		{
			this._saveStep = SaveHandler.SaveSteps.PreSave;
			if (this.SaveArgsQueue.Dequeue().Mode == SaveHandler.SaveArgs.SaveMode.AutoSave)
			{
				this._lastAutoSaveTime = DateTime.Now;
			}
			this.OnSaveEnded(result.Item1 == SaveResult.Success, result.Item2);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000516B7 File Offset: 0x0004F8B7
		public void SignalAutoSave()
		{
			this.TryAutoSave(true);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000516C0 File Offset: 0x0004F8C0
		private void OnSaveStarted()
		{
			Campaign.Current.WaitAsyncTasks();
			CampaignEventDispatcher.Instance.OnSaveStarted();
			MBInformationManager.HideInformations();
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x000516DC File Offset: 0x0004F8DC
		private void OnSaveEnded(bool isSaveSuccessful, string newSaveGameName)
		{
			ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
			if (sandBoxSaveManager != null)
			{
				sandBoxSaveManager.OnSaveOver(isSaveSuccessful, newSaveGameName);
			}
			CampaignEventDispatcher.Instance.OnSaveOver(isSaveSuccessful, newSaveGameName);
			if (!isSaveSuccessful)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=u9PPxTNL}Save Error!", null), 0, null, "");
			}
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0005172B File Offset: 0x0004F92B
		private void SetSaveArgs(SaveHandler.SaveArgs.SaveMode saveType, string saveName = null)
		{
			this.SaveArgsQueue.Enqueue(new SaveHandler.SaveArgs(saveType, saveName));
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00051740 File Offset: 0x0004F940
		public CampaignSaveMetaDataArgs GetSaveMetaData()
		{
			string[] moduleName = SandBoxManager.Instance.ModuleManager.ModuleNames.ToArray<string>();
			KeyValuePair<string, string>[] array = new KeyValuePair<string, string>[15];
			array[0] = new KeyValuePair<string, string>("UniqueGameId", Campaign.Current.UniqueGameId ?? "");
			array[1] = new KeyValuePair<string, string>("MainHeroLevel", Hero.MainHero.Level.ToString(SaveHandler._invariantCulture));
			array[2] = new KeyValuePair<string, string>("MainPartyFood", Campaign.Current.MainParty.Food.ToString(SaveHandler._invariantCulture));
			array[3] = new KeyValuePair<string, string>("MainHeroGold", Hero.MainHero.Gold.ToString(SaveHandler._invariantCulture));
			array[4] = new KeyValuePair<string, string>("ClanInfluence", Clan.PlayerClan.Influence.ToString(SaveHandler._invariantCulture));
			array[5] = new KeyValuePair<string, string>("ClanFiefs", Clan.PlayerClan.Settlements.Count.ToString(SaveHandler._invariantCulture));
			array[6] = new KeyValuePair<string, string>("MainPartyHealthyMemberCount", Campaign.Current.MainParty.MemberRoster.TotalHealthyCount.ToString(SaveHandler._invariantCulture));
			array[7] = new KeyValuePair<string, string>("MainPartyPrisonerMemberCount", Campaign.Current.MainParty.PrisonRoster.TotalManCount.ToString(SaveHandler._invariantCulture));
			array[8] = new KeyValuePair<string, string>("MainPartyWoundedMemberCount", Campaign.Current.MainParty.MemberRoster.TotalWounded.ToString(SaveHandler._invariantCulture));
			int num = 9;
			string key = "CharacterName";
			TextObject name = Hero.MainHero.Name;
			array[num] = new KeyValuePair<string, string>(key, (name != null) ? name.ToString() : null);
			array[10] = new KeyValuePair<string, string>("DayLong", Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow.ToString(SaveHandler._invariantCulture));
			array[11] = new KeyValuePair<string, string>("ClanBannerCode", Clan.PlayerClan.Banner.Serialize());
			int num2 = 12;
			string key2 = "MainHeroVisual";
			IMainHeroVisualSupplier mainHeroVisualSupplier = this.MainHeroVisualSupplier;
			array[num2] = new KeyValuePair<string, string>(key2, ((mainHeroVisualSupplier != null) ? mainHeroVisualSupplier.GetMainHeroVisualCode() : null) ?? string.Empty);
			array[13] = new KeyValuePair<string, string>("IronmanMode", (CampaignOptions.IsIronmanMode ? 1 : 0).ToString());
			array[14] = new KeyValuePair<string, string>("HealthPercentage", MBMath.ClampInt(Hero.MainHero.HitPoints * 100 / Hero.MainHero.MaxHitPoints, 1, 100).ToString());
			return new CampaignSaveMetaDataArgs(moduleName, array);
		}

		// Token: 0x040005EB RID: 1515
		private SaveHandler.SaveSteps _saveStep;

		// Token: 0x040005EC RID: 1516
		private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

		// Token: 0x040005EE RID: 1518
		private Queue<SaveHandler.SaveArgs> SaveArgsQueue = new Queue<SaveHandler.SaveArgs>();

		// Token: 0x040005EF RID: 1519
		private DateTime _lastAutoSaveTime = DateTime.Now;

		// Token: 0x020004C9 RID: 1225
		private readonly struct SaveArgs
		{
			// Token: 0x06004309 RID: 17161 RVA: 0x001456E5 File Offset: 0x001438E5
			public SaveArgs(SaveHandler.SaveArgs.SaveMode mode, string name)
			{
				this.Mode = mode;
				this.Name = name;
			}

			// Token: 0x0400149E RID: 5278
			public readonly SaveHandler.SaveArgs.SaveMode Mode;

			// Token: 0x0400149F RID: 5279
			public readonly string Name;

			// Token: 0x02000788 RID: 1928
			public enum SaveMode
			{
				// Token: 0x04001F6C RID: 8044
				SaveAs,
				// Token: 0x04001F6D RID: 8045
				QuickSave,
				// Token: 0x04001F6E RID: 8046
				AutoSave
			}
		}

		// Token: 0x020004CA RID: 1226
		private enum SaveSteps
		{
			// Token: 0x040014A1 RID: 5281
			PreSave,
			// Token: 0x040014A2 RID: 5282
			Saving = 2,
			// Token: 0x040014A3 RID: 5283
			AwaitingCompletion
		}
	}
}
