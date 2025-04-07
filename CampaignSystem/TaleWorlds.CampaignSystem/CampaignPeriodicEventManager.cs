using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000037 RID: 55
	public class CampaignPeriodicEventManager
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0002146C File Offset: 0x0001F66C
		private double DeltaHours
		{
			get
			{
				return CampaignTime.DeltaTime.ToHours;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00021488 File Offset: 0x0001F688
		private double DeltaDays
		{
			get
			{
				return CampaignTime.DeltaTime.ToDays;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000214A4 File Offset: 0x0001F6A4
		internal CampaignPeriodicEventManager()
		{
			this._mobilePartyHourlyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._mobilePartyDailyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._hourlyTickMobilePartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._hourlyTickSettlementTicker = new CampaignPeriodicEventManager.PeriodicTicker<Settlement>();
			this._dailyTickSettlementTicker = new CampaignPeriodicEventManager.PeriodicTicker<Settlement>();
			this._hourlyTickClanTicker = new CampaignPeriodicEventManager.PeriodicTicker<Clan>();
			this._dailyTickPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._dailyTickTownTicker = new CampaignPeriodicEventManager.PeriodicTicker<Town>();
			this._dailyTickHeroTicker = new CampaignPeriodicEventManager.PeriodicTicker<Hero>();
			this._dailyTickClanTicker = new CampaignPeriodicEventManager.PeriodicTicker<Clan>();
			this._quarterDailyPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._caravanMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._garrisonMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._militiaMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._villagerMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._customMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._banditMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._lordMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00021594 File Offset: 0x0001F794
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData)
		{
			if (this._caravanMobilePartyPartialHourlyAiEventTicker == null)
			{
				this._caravanMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._garrisonMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._militiaMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._villagerMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._customMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._banditMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._lordMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._quarterDailyPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002160C File Offset: 0x0001F80C
		internal void InitializeTickers()
		{
			MBList<Settlement> list = this.ShuffleSettlements();
			this._mobilePartyHourlyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				x.HourlyTick();
			}, false);
			this._mobilePartyDailyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				x.DailyTick();
			}, false);
			this._hourlyTickMobilePartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.HourlyTickParty(x);
			}, false);
			this._hourlyTickSettlementTicker.Initialize(list, delegate(Settlement x)
			{
				CampaignEventDispatcher.Instance.HourlyTickSettlement(x);
			}, false);
			this._dailyTickSettlementTicker.Initialize(list, delegate(Settlement x)
			{
				CampaignEventDispatcher.Instance.DailyTickSettlement(x);
			}, false);
			this._hourlyTickClanTicker.Initialize(Clan.All, delegate(Clan x)
			{
				CampaignEventDispatcher.Instance.HourlyTickClan(x);
			}, false);
			this._dailyTickPartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.DailyTickParty(x);
			}, false);
			this._dailyTickTownTicker.Initialize(Town.AllTowns, delegate(Town x)
			{
				CampaignEventDispatcher.Instance.DailyTickTown(x);
			}, false);
			this._dailyTickHeroTicker.Initialize(Hero.AllAliveHeroes, delegate(Hero x)
			{
				CampaignEventDispatcher.Instance.DailyTickHero(x);
			}, false);
			this._dailyTickClanTicker.Initialize(Clan.All, delegate(Clan x)
			{
				CampaignEventDispatcher.Instance.DailyTickClan(x);
			}, false);
			bool doParallel = false;
			this._caravanMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllCaravanParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._garrisonMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllGarrisonParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._militiaMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllMilitiaParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._villagerMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllVillagerParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._customMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllCustomParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._banditMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllBanditParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._lordMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllLordParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker.Initialize(MobileParty.AllPartiesWithoutPartyComponent, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, doParallel);
			this._quarterDailyPartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.QuarterDailyPartyTick(x);
			}, false);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000219AC File Offset: 0x0001FBAC
		private MBList<Settlement> ShuffleSettlements()
		{
			Stack<Settlement> stack = new Stack<Settlement>();
			Stack<Settlement> stack2 = new Stack<Settlement>();
			Stack<Settlement> stack3 = new Stack<Settlement>();
			Stack<Settlement> stack4 = new Stack<Settlement>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage)
				{
					stack.Push(settlement);
				}
				else if (settlement.IsCastle)
				{
					stack2.Push(settlement);
				}
				else if (settlement.IsTown)
				{
					stack3.Push(settlement);
				}
				else
				{
					stack4.Push(settlement);
				}
			}
			float num = (float)Settlement.All.Count;
			float num2 = (float)stack.Count / num;
			float num3 = (float)stack2.Count / num;
			float num4 = (float)stack3.Count / num;
			float num5 = (float)stack4.Count / num;
			float num6 = num2;
			float num7 = num3;
			float num8 = num4;
			float num9 = num5;
			MBList<Settlement> mblist = new MBList<Settlement>();
			while (mblist.Count != Settlement.All.Count)
			{
				num6 += num2;
				if (num6 >= 1f && !stack.IsEmpty<Settlement>())
				{
					mblist.Add(stack.Pop());
					num6 -= 1f;
				}
				num7 += num3;
				if (num7 >= 1f && !stack2.IsEmpty<Settlement>())
				{
					mblist.Add(stack2.Pop());
					num7 -= 1f;
				}
				num8 += num4;
				if (num8 >= 1f && !stack3.IsEmpty<Settlement>())
				{
					mblist.Add(stack3.Pop());
					num8 -= 1f;
				}
				num9 += num5;
				if (num9 >= 1f && !stack4.IsEmpty<Settlement>())
				{
					mblist.Add(stack4.Pop());
					num9 -= 1f;
				}
			}
			return mblist;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00021B80 File Offset: 0x0001FD80
		internal void TickPeriodicEvents()
		{
			this.PeriodicHourlyTick();
			this.PeriodicDailyTick();
			this.PeriodicQuarterDailyTick();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00021B94 File Offset: 0x0001FD94
		private void PeriodicQuarterDailyTick()
		{
			double deltaDays = this.DeltaDays;
			this._quarterDailyPartyTicker.PeriodicTickSome(deltaDays * 4.0);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00021BBE File Offset: 0x0001FDBE
		internal void MobilePartyHourlyTick()
		{
			this._mobilePartyHourlyTicker.PeriodicTickSome(this.DeltaHours);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		internal void TickPartialHourlyAi()
		{
			this._caravanMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._garrisonMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._militiaMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._villagerMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._customMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._banditMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._lordMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00021CBC File Offset: 0x0001FEBC
		private void PeriodicHourlyTick()
		{
			double deltaHours = this.DeltaHours;
			this._hourlyTickMobilePartyTicker.PeriodicTickSome(deltaHours);
			this._hourlyTickSettlementTicker.PeriodicTickSome(deltaHours);
			this._hourlyTickClanTicker.PeriodicTickSome(deltaHours);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00021CF4 File Offset: 0x0001FEF4
		private void PeriodicDailyTick()
		{
			double deltaDays = this.DeltaDays;
			this._dailyTickPartyTicker.PeriodicTickSome(deltaDays);
			this._mobilePartyDailyTicker.PeriodicTickSome(deltaDays);
			this._dailyTickTownTicker.PeriodicTickSome(deltaDays);
			this._dailyTickSettlementTicker.PeriodicTickSome(deltaDays);
			this._dailyTickHeroTicker.PeriodicTickSome(deltaDays);
			this._dailyTickClanTicker.PeriodicTickSome(deltaDays);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00021D50 File Offset: 0x0001FF50
		public static MBCampaignEvent CreatePeriodicEvent(CampaignTime triggerPeriod, CampaignTime initialWait)
		{
			MBCampaignEvent mbcampaignEvent = new MBCampaignEvent(triggerPeriod, initialWait);
			Campaign.Current.CustomPeriodicCampaignEvents.Add(mbcampaignEvent);
			return mbcampaignEvent;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00021D78 File Offset: 0x0001FF78
		private void DeleteMarkedPeriodicEvents()
		{
			List<MBCampaignEvent> customPeriodicCampaignEvents = Campaign.Current.CustomPeriodicCampaignEvents;
			for (int i = customPeriodicCampaignEvents.Count - 1; i >= 0; i--)
			{
				if (customPeriodicCampaignEvents[i].isEventDeleted)
				{
					customPeriodicCampaignEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00021DB8 File Offset: 0x0001FFB8
		internal void OnTick(float dt)
		{
			this.SignalPeriodicEvents();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00021DC0 File Offset: 0x0001FFC0
		private void SignalPeriodicEvents()
		{
			if ((this._lastGameTime + CampaignPeriodicEventManager.MinimumPeriodicEventInterval).IsPast)
			{
				this._lastGameTime = CampaignTime.Now;
				List<MBCampaignEvent> customPeriodicCampaignEvents = Campaign.Current.CustomPeriodicCampaignEvents;
				for (int i = customPeriodicCampaignEvents.Count - 1; i >= 0; i--)
				{
					customPeriodicCampaignEvents[i].CheckUpdate();
				}
				this.DeleteMarkedPeriodicEvents();
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				if (mapState == null)
				{
					return;
				}
				mapState.OnSignalPeriodicEvents();
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00021E40 File Offset: 0x00020040
		internal static void AutoGeneratedStaticCollectObjectsCampaignPeriodicEventManager(object o, List<object> collectedObjects)
		{
			((CampaignPeriodicEventManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00021E50 File Offset: 0x00020050
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._mobilePartyHourlyTicker);
			collectedObjects.Add(this._mobilePartyDailyTicker);
			collectedObjects.Add(this._dailyTickPartyTicker);
			collectedObjects.Add(this._hourlyTickMobilePartyTicker);
			collectedObjects.Add(this._hourlyTickSettlementTicker);
			collectedObjects.Add(this._hourlyTickClanTicker);
			collectedObjects.Add(this._dailyTickTownTicker);
			collectedObjects.Add(this._dailyTickSettlementTicker);
			collectedObjects.Add(this._dailyTickHeroTicker);
			collectedObjects.Add(this._dailyTickClanTicker);
			collectedObjects.Add(this._quarterDailyPartyTicker);
			collectedObjects.Add(this._caravanMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._garrisonMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._militiaMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._villagerMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._customMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._banditMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._lordMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00021F41 File Offset: 0x00020141
		internal static object AutoGeneratedGetMemberValue_mobilePartyHourlyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._mobilePartyHourlyTicker;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00021F4E File Offset: 0x0002014E
		internal static object AutoGeneratedGetMemberValue_mobilePartyDailyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._mobilePartyDailyTicker;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00021F5B File Offset: 0x0002015B
		internal static object AutoGeneratedGetMemberValue_dailyTickPartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickPartyTicker;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00021F68 File Offset: 0x00020168
		internal static object AutoGeneratedGetMemberValue_hourlyTickMobilePartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickMobilePartyTicker;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00021F75 File Offset: 0x00020175
		internal static object AutoGeneratedGetMemberValue_hourlyTickSettlementTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickSettlementTicker;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00021F82 File Offset: 0x00020182
		internal static object AutoGeneratedGetMemberValue_hourlyTickClanTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickClanTicker;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00021F8F File Offset: 0x0002018F
		internal static object AutoGeneratedGetMemberValue_dailyTickTownTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickTownTicker;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00021F9C File Offset: 0x0002019C
		internal static object AutoGeneratedGetMemberValue_dailyTickSettlementTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickSettlementTicker;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00021FA9 File Offset: 0x000201A9
		internal static object AutoGeneratedGetMemberValue_dailyTickHeroTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickHeroTicker;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00021FB6 File Offset: 0x000201B6
		internal static object AutoGeneratedGetMemberValue_dailyTickClanTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickClanTicker;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00021FC3 File Offset: 0x000201C3
		internal static object AutoGeneratedGetMemberValue_quarterDailyPartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._quarterDailyPartyTicker;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00021FD0 File Offset: 0x000201D0
		internal static object AutoGeneratedGetMemberValue_caravanMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._caravanMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00021FDD File Offset: 0x000201DD
		internal static object AutoGeneratedGetMemberValue_garrisonMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._garrisonMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00021FEA File Offset: 0x000201EA
		internal static object AutoGeneratedGetMemberValue_militiaMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._militiaMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00021FF7 File Offset: 0x000201F7
		internal static object AutoGeneratedGetMemberValue_villagerMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._villagerMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00022004 File Offset: 0x00020204
		internal static object AutoGeneratedGetMemberValue_customMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._customMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00022011 File Offset: 0x00020211
		internal static object AutoGeneratedGetMemberValue_banditMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._banditMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002201E File Offset: 0x0002021E
		internal static object AutoGeneratedGetMemberValue_lordMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._lordMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002202B File Offset: 0x0002022B
		internal static object AutoGeneratedGetMemberValue_partiesWithoutPartyComponentsPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._partiesWithoutPartyComponentsPartialHourlyAiEventTicker;
		}

		// Token: 0x0400026B RID: 619
		[SaveableField(120)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _mobilePartyHourlyTicker;

		// Token: 0x0400026C RID: 620
		[SaveableField(130)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _mobilePartyDailyTicker;

		// Token: 0x0400026D RID: 621
		[SaveableField(140)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _dailyTickPartyTicker;

		// Token: 0x0400026E RID: 622
		[SaveableField(150)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _hourlyTickMobilePartyTicker;

		// Token: 0x0400026F RID: 623
		[SaveableField(160)]
		private CampaignPeriodicEventManager.PeriodicTicker<Settlement> _hourlyTickSettlementTicker;

		// Token: 0x04000270 RID: 624
		[SaveableField(170)]
		private CampaignPeriodicEventManager.PeriodicTicker<Clan> _hourlyTickClanTicker;

		// Token: 0x04000271 RID: 625
		[SaveableField(180)]
		private CampaignPeriodicEventManager.PeriodicTicker<Town> _dailyTickTownTicker;

		// Token: 0x04000272 RID: 626
		[SaveableField(190)]
		private CampaignPeriodicEventManager.PeriodicTicker<Settlement> _dailyTickSettlementTicker;

		// Token: 0x04000273 RID: 627
		[SaveableField(200)]
		private CampaignPeriodicEventManager.PeriodicTicker<Hero> _dailyTickHeroTicker;

		// Token: 0x04000274 RID: 628
		[SaveableField(210)]
		private CampaignPeriodicEventManager.PeriodicTicker<Clan> _dailyTickClanTicker;

		// Token: 0x04000275 RID: 629
		[SaveableField(320)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _quarterDailyPartyTicker;

		// Token: 0x04000276 RID: 630
		[SaveableField(230)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _caravanMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x04000277 RID: 631
		[SaveableField(250)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _garrisonMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x04000278 RID: 632
		[SaveableField(260)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _militiaMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x04000279 RID: 633
		[SaveableField(270)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _villagerMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x0400027A RID: 634
		[SaveableField(280)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _customMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x0400027B RID: 635
		[SaveableField(290)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _banditMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x0400027C RID: 636
		[SaveableField(300)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _lordMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x0400027D RID: 637
		[SaveableField(310)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _partiesWithoutPartyComponentsPartialHourlyAiEventTicker;

		// Token: 0x0400027E RID: 638
		private static readonly CampaignTime MinimumPeriodicEventInterval = CampaignTime.Hours(0.05f);

		// Token: 0x0400027F RID: 639
		private CampaignTime _lastGameTime = CampaignTime.Zero;

		// Token: 0x0200048A RID: 1162
		internal class PeriodicTicker<T>
		{
			// Token: 0x17000D67 RID: 3431
			// (get) Token: 0x060041CD RID: 16845 RVA: 0x00142FF7 File Offset: 0x001411F7
			// (set) Token: 0x060041CE RID: 16846 RVA: 0x00142FFF File Offset: 0x001411FF
			[SaveableProperty(1)]
			private double TickDebt { get; set; }

			// Token: 0x17000D68 RID: 3432
			// (get) Token: 0x060041CF RID: 16847 RVA: 0x00143008 File Offset: 0x00141208
			// (set) Token: 0x060041D0 RID: 16848 RVA: 0x00143010 File Offset: 0x00141210
			[SaveableProperty(2)]
			private int Index { get; set; }

			// Token: 0x060041D1 RID: 16849 RVA: 0x00143019 File Offset: 0x00141219
			internal PeriodicTicker()
			{
				this.TickDebt = 0.0;
				this.Index = -1;
			}

			// Token: 0x060041D2 RID: 16850 RVA: 0x00143042 File Offset: 0x00141242
			internal void Initialize(MBReadOnlyList<T> list, Action<T> action, bool doParallel)
			{
				this._list = list;
				this._action = action;
				this._doParallel = doParallel;
			}

			// Token: 0x060041D3 RID: 16851 RVA: 0x0014305C File Offset: 0x0014125C
			internal void PeriodicTickSome(double timeUnitsElapsed)
			{
				if (this._list.Count == 0)
				{
					this.TickDebt = 0.0;
					return;
				}
				this.TickDebt += timeUnitsElapsed * (double)this._list.Count;
				while (this.TickDebt > 1.0)
				{
					this.Index++;
					if (this.Index >= this._list.Count)
					{
						this.Index = 0;
					}
					if (this._doParallel)
					{
						this._currentFrameToTickListFlattened.Add(this._list[this.Index]);
					}
					else
					{
						this._action(this._list[this.Index]);
					}
					this.TickDebt -= 1.0;
				}
				if (this._doParallel && this._currentFrameToTickListFlattened.Count > 0)
				{
					TWParallel.For(0, this._currentFrameToTickListFlattened.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int i = startInclusive; i < endExclusive; i++)
						{
							this._action(this._currentFrameToTickListFlattened[i]);
						}
					}, 1);
					this._currentFrameToTickListFlattened.Clear();
				}
			}

			// Token: 0x060041D4 RID: 16852 RVA: 0x0014317C File Offset: 0x0014137C
			public override string ToString()
			{
				object[] array = new object[7];
				array[0] = "PeriodicTicker  @";
				int num = 1;
				object obj;
				if (this.Index != -1)
				{
					T t = this._list[this.Index];
					obj = t.ToString();
				}
				else
				{
					obj = "null";
				}
				array[num] = obj;
				array[2] = "\t\t(";
				array[3] = this.Index;
				array[4] = " / ";
				array[5] = this._list.Count;
				array[6] = ")";
				return string.Concat(array);
			}

			// Token: 0x060041D5 RID: 16853 RVA: 0x00143207 File Offset: 0x00141407
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x040013A7 RID: 5031
			private readonly List<T> _currentFrameToTickListFlattened = new List<T>();

			// Token: 0x040013AA RID: 5034
			private bool _doParallel;

			// Token: 0x040013AB RID: 5035
			private MBReadOnlyList<T> _list;

			// Token: 0x040013AC RID: 5036
			private Action<T> _action;
		}
	}
}
