using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000033 RID: 51
	public class MBCampaignEvent
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0001B426 File Offset: 0x00019626
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0001B42E File Offset: 0x0001962E
		public CampaignTime TriggerPeriod { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0001B437 File Offset: 0x00019637
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0001B43F File Offset: 0x0001963F
		public CampaignTime InitialWait { get; private set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0001B448 File Offset: 0x00019648
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0001B450 File Offset: 0x00019650
		public bool isEventDeleted { get; set; }

		// Token: 0x06000368 RID: 872 RVA: 0x0001B459 File Offset: 0x00019659
		public MBCampaignEvent(string eventName)
		{
			this.description = eventName;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001B473 File Offset: 0x00019673
		public MBCampaignEvent(CampaignTime triggerPeriod, CampaignTime initialWait)
		{
			this.TriggerPeriod = triggerPeriod;
			this.InitialWait = initialWait;
			this.NextTriggerTime = CampaignTime.Now + this.InitialWait;
			this.isEventDeleted = false;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001B4B1 File Offset: 0x000196B1
		public void AddHandler(MBCampaignEvent.CampaignEventDelegate gameEventDelegate)
		{
			this.handlers.Add(gameEventDelegate);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001B4C0 File Offset: 0x000196C0
		public void RunHandlers(params object[] delegateParams)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				this.handlers[i](this, delegateParams);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0001B4F8 File Offset: 0x000196F8
		public void Unregister(object instance)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				if (this.handlers[i].Target == instance)
				{
					this.handlers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001B540 File Offset: 0x00019740
		public void CheckUpdate()
		{
			while (this.NextTriggerTime.IsPast && !this.isEventDeleted)
			{
				this.RunHandlers(new object[]
				{
					CampaignTime.Now
				});
				this.NextTriggerTime += this.TriggerPeriod;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0001B594 File Offset: 0x00019794
		public void DeletePeriodicEvent()
		{
			this.isEventDeleted = true;
		}

		// Token: 0x04000177 RID: 375
		public string description;

		// Token: 0x04000178 RID: 376
		protected List<MBCampaignEvent.CampaignEventDelegate> handlers = new List<MBCampaignEvent.CampaignEventDelegate>();

		// Token: 0x04000179 RID: 377
		[CachedData]
		protected CampaignTime NextTriggerTime;

		// Token: 0x02000489 RID: 1161
		// (Invoke) Token: 0x060041CA RID: 16842
		public delegate void CampaignEventDelegate(MBCampaignEvent campaignEvent, params object[] delegateParams);
	}
}
