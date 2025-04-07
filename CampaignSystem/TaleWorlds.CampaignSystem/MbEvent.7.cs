using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004A RID: 74
	public class MbEvent<T1, T2, T3, T4, T5, T6> : IMbEvent<T1, T2, T3, T4, T5, T6>, IMbEventBase
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x000225E8 File Offset: 0x000207E8
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5, T6> action)
		{
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6>(owner, action);
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00022612 File Offset: 0x00020812
		internal void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5, t6);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00022629 File Offset: 0x00020829
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5, t6);
				list = list.Next;
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002264D File Offset: 0x0002084D
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002265C File Offset: 0x0002085C
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> eventHandlerRec2 = list;
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

		// Token: 0x04000287 RID: 647
		private MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<T1, T2, T3, T4, T5, T6> _nonSerializedListenerList;

		// Token: 0x02000493 RID: 1171
		internal class EventHandlerRec<TA, TB, TC, TD, TE, TF>
		{
			// Token: 0x17000D77 RID: 3447
			// (get) Token: 0x0600420F RID: 16911 RVA: 0x001434C5 File Offset: 0x001416C5
			// (set) Token: 0x06004210 RID: 16912 RVA: 0x001434CD File Offset: 0x001416CD
			internal Action<TA, TB, TC, TD, TE, TF> Action { get; private set; }

			// Token: 0x17000D78 RID: 3448
			// (get) Token: 0x06004211 RID: 16913 RVA: 0x001434D6 File Offset: 0x001416D6
			// (set) Token: 0x06004212 RID: 16914 RVA: 0x001434DE File Offset: 0x001416DE
			internal object Owner { get; private set; }

			// Token: 0x06004213 RID: 16915 RVA: 0x001434E7 File Offset: 0x001416E7
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE, TF> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013D8 RID: 5080
			public MbEvent<T1, T2, T3, T4, T5, T6>.EventHandlerRec<TA, TB, TC, TD, TE, TF> Next;
		}
	}
}
