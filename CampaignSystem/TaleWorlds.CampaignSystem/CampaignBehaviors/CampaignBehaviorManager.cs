﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200037B RID: 891
	public class CampaignBehaviorManager : ICampaignBehaviorManager
	{
		// Token: 0x06003472 RID: 13426 RVA: 0x000DCCEA File Offset: 0x000DAEEA
		public CampaignBehaviorManager(IEnumerable<CampaignBehaviorBase> inputComponents)
		{
			this.SetBehaviors(inputComponents);
			this._campaignBehaviorDataStore = new CampaignBehaviorDataStore();
			CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, new Action(this.OnBeforeSave));
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000DCD1B File Offset: 0x000DAF1B
		public void InitializeCampaignBehaviors(IEnumerable<CampaignBehaviorBase> inputComponents)
		{
			this.SetBehaviors(inputComponents);
			CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, new Action(this.OnBeforeSave));
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000DCD3B File Offset: 0x000DAF3B
		private void SetBehaviors(IEnumerable<CampaignBehaviorBase> inputComponents)
		{
			this._campaignBehaviors = inputComponents.ToList<CampaignBehaviorBase>();
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000DCD4C File Offset: 0x000DAF4C
		public void RegisterEvents()
		{
			foreach (CampaignBehaviorBase campaignBehaviorBase in this._campaignBehaviors)
			{
				campaignBehaviorBase.RegisterEvents();
			}
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000DCD9C File Offset: 0x000DAF9C
		private void OnBeforeSave()
		{
			this._campaignBehaviorDataStore.ClearBehaviorData();
			foreach (CampaignBehaviorBase campaignBehavior in this._campaignBehaviors)
			{
				this._campaignBehaviorDataStore.SaveBehaviorData(campaignBehavior);
			}
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x000DCE00 File Offset: 0x000DB000
		public void LoadBehaviorData()
		{
			foreach (CampaignBehaviorBase campaignBehavior in this._campaignBehaviors)
			{
				this._campaignBehaviorDataStore.LoadBehaviorData(campaignBehavior);
			}
			this._campaignBehaviorDataStore.ClearBehaviorData();
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000DCE64 File Offset: 0x000DB064
		public T GetBehavior<T>()
		{
			return this._campaignBehaviors.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000DCE76 File Offset: 0x000DB076
		public IEnumerable<T> GetBehaviors<T>()
		{
			return this._campaignBehaviors.OfType<T>();
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000DCE83 File Offset: 0x000DB083
		public void AddBehavior(CampaignBehaviorBase campaignBehavior)
		{
			this._campaignBehaviors.Add(campaignBehavior);
			campaignBehavior.RegisterEvents();
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000DCE98 File Offset: 0x000DB098
		public void RemoveBehavior<T>() where T : CampaignBehaviorBase
		{
			for (int i = this._campaignBehaviors.Count - 1; i >= 0; i--)
			{
				T t;
				if ((t = (this._campaignBehaviors[i] as T)) != null)
				{
					this._campaignBehaviors.Remove(t);
					CampaignEventDispatcher.Instance.RemoveListeners(t);
					return;
				}
			}
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000DCEFF File Offset: 0x000DB0FF
		public void ClearBehaviors()
		{
			this._campaignBehaviors.Clear();
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000DCF0C File Offset: 0x000DB10C
		internal static void AutoGeneratedStaticCollectObjectsCampaignBehaviorManager(object o, List<object> collectedObjects)
		{
			((CampaignBehaviorManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000DCF1A File Offset: 0x000DB11A
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._campaignBehaviorDataStore);
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000DCF28 File Offset: 0x000DB128
		internal static object AutoGeneratedGetMemberValue_campaignBehaviorDataStore(object o)
		{
			return ((CampaignBehaviorManager)o)._campaignBehaviorDataStore;
		}

		// Token: 0x040010FE RID: 4350
		private List<CampaignBehaviorBase> _campaignBehaviors;

		// Token: 0x040010FF RID: 4351
		[SaveableField(1)]
		private readonly CampaignBehaviorDataStore _campaignBehaviorDataStore;
	}
}
