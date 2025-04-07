using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000046 RID: 70
	public class MbEvent<T1, T2, T3, T4> : IMbEvent<T1, T2, T3, T4>, IMbEventBase
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00022444 File Offset: 0x00020644
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4> action)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = new MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4>(owner, action);
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002246E File Offset: 0x0002066E
		internal void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00022481 File Offset: 0x00020681
		private void InvokeList(MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4);
				list = list.Next;
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000224A1 File Offset: 0x000206A1
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000224B0 File Offset: 0x000206B0
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, object o)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		// Token: 0x04000285 RID: 645
		private MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> _nonSerializedListenerList;

		// Token: 0x02000491 RID: 1169
		internal class EventHandlerRec<TA, TB, TC, TD>
		{
			// Token: 0x17000D73 RID: 3443
			// (get) Token: 0x06004205 RID: 16901 RVA: 0x00143455 File Offset: 0x00141655
			// (set) Token: 0x06004206 RID: 16902 RVA: 0x0014345D File Offset: 0x0014165D
			internal Action<TA, TB, TC, TD> Action { get; private set; }

			// Token: 0x17000D74 RID: 3444
			// (get) Token: 0x06004207 RID: 16903 RVA: 0x00143466 File Offset: 0x00141666
			// (set) Token: 0x06004208 RID: 16904 RVA: 0x0014346E File Offset: 0x0014166E
			internal object Owner { get; private set; }

			// Token: 0x06004209 RID: 16905 RVA: 0x00143477 File Offset: 0x00141677
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013D2 RID: 5074
			public MbEvent<T1, T2, T3, T4>.EventHandlerRec<TA, TB, TC, TD> Next;
		}
	}
}
