using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000044 RID: 68
	public class MbEvent<T1, T2, T3> : IMbEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x00022378 File Offset: 0x00020578
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3> action)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000223A2 File Offset: 0x000205A2
		internal void Invoke(T1 t1, T2 t2, T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000223B3 File Offset: 0x000205B3
		private void InvokeList(MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3);
				list = list.Next;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000223D1 File Offset: 0x000205D1
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000223E0 File Offset: 0x000205E0
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x04000284 RID: 644
		private MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x02000490 RID: 1168
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000D71 RID: 3441
			// (get) Token: 0x06004200 RID: 16896 RVA: 0x0014341D File Offset: 0x0014161D
			// (set) Token: 0x06004201 RID: 16897 RVA: 0x00143425 File Offset: 0x00141625
			internal Action<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000D72 RID: 3442
			// (get) Token: 0x06004202 RID: 16898 RVA: 0x0014342E File Offset: 0x0014162E
			// (set) Token: 0x06004203 RID: 16899 RVA: 0x00143436 File Offset: 0x00141636
			internal object Owner { get; private set; }

			// Token: 0x06004204 RID: 16900 RVA: 0x0014343F File Offset: 0x0014163F
			public EventHandlerRec(object owner, Action<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013CF RID: 5071
			public MbEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
