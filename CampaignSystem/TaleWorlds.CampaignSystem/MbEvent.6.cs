using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000048 RID: 72
	public class MbEvent<T1, T2, T3, T4, T5> : IMbEvent<T1, T2, T3, T4, T5>, IMbEventBase
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x00022514 File Offset: 0x00020714
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5> action)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5>(owner, action);
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002253E File Offset: 0x0002073E
		internal void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00022553 File Offset: 0x00020753
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5);
				list = list.Next;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00022575 File Offset: 0x00020775
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00022584 File Offset: 0x00020784
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec2 = list;
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

		// Token: 0x04000286 RID: 646
		private MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> _nonSerializedListenerList;

		// Token: 0x02000492 RID: 1170
		internal class EventHandlerRec<TA, TB, TC, TD, TE>
		{
			// Token: 0x17000D75 RID: 3445
			// (get) Token: 0x0600420A RID: 16906 RVA: 0x0014348D File Offset: 0x0014168D
			// (set) Token: 0x0600420B RID: 16907 RVA: 0x00143495 File Offset: 0x00141695
			internal Action<TA, TB, TC, TD, TE> Action { get; private set; }

			// Token: 0x17000D76 RID: 3446
			// (get) Token: 0x0600420C RID: 16908 RVA: 0x0014349E File Offset: 0x0014169E
			// (set) Token: 0x0600420D RID: 16909 RVA: 0x001434A6 File Offset: 0x001416A6
			internal object Owner { get; private set; }

			// Token: 0x0600420E RID: 16910 RVA: 0x001434AF File Offset: 0x001416AF
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013D5 RID: 5077
			public MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<TA, TB, TC, TD, TE> Next;
		}
	}
}
