using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004C RID: 76
	public class MbEvent<T1, T2, T3, T4, T5, T6, T7> : IMbEvent<T1, T2, T3, T4, T5, T6, T7>, IMbEventBase
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x000226C0 File Offset: 0x000208C0
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7>(owner, action);
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000226EC File Offset: 0x000208EC
		internal void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5, t6, t7);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00022710 File Offset: 0x00020910
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5, t6, t7);
				list = list.Next;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00022736 File Offset: 0x00020936
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00022748 File Offset: 0x00020948
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> eventHandlerRec2 = list;
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

		// Token: 0x04000288 RID: 648
		private MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<T1, T2, T3, T4, T5, T6, T7> _nonSerializedListenerList;

		// Token: 0x02000494 RID: 1172
		internal class EventHandlerRec<TA, TB, TC, TD, TE, TF, TG>
		{
			// Token: 0x17000D79 RID: 3449
			// (get) Token: 0x06004214 RID: 16916 RVA: 0x001434FD File Offset: 0x001416FD
			// (set) Token: 0x06004215 RID: 16917 RVA: 0x00143505 File Offset: 0x00141705
			internal Action<TA, TB, TC, TD, TE, TF, TG> Action { get; private set; }

			// Token: 0x17000D7A RID: 3450
			// (get) Token: 0x06004216 RID: 16918 RVA: 0x0014350E File Offset: 0x0014170E
			// (set) Token: 0x06004217 RID: 16919 RVA: 0x00143516 File Offset: 0x00141716
			internal object Owner { get; private set; }

			// Token: 0x06004218 RID: 16920 RVA: 0x0014351F File Offset: 0x0014171F
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE, TF, TG> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013DB RID: 5083
			public MbEvent<T1, T2, T3, T4, T5, T6, T7>.EventHandlerRec<TA, TB, TC, TD, TE, TF, TG> Next;
		}
	}
}
