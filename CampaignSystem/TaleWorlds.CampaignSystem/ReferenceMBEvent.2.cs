using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000040 RID: 64
	public class ReferenceMBEvent<T1, T2, T3> : ReferenceIMBEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x000221E0 File Offset: 0x000203E0
		public void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0002220A File Offset: 0x0002040A
		internal void Invoke(T1 t1, T2 t2, ref T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, ref t3);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002221B File Offset: 0x0002041B
		private void InvokeList(ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, ref T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, ref t3);
				list = list.Next;
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00022239 File Offset: 0x00020439
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00022248 File Offset: 0x00020448
		private void ClearListenerOfList(ref ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x04000282 RID: 642
		private ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x0200048E RID: 1166
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000D6D RID: 3437
			// (get) Token: 0x060041F6 RID: 16886 RVA: 0x001433AD File Offset: 0x001415AD
			// (set) Token: 0x060041F7 RID: 16887 RVA: 0x001433B5 File Offset: 0x001415B5
			internal ReferenceAction<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000D6E RID: 3438
			// (get) Token: 0x060041F8 RID: 16888 RVA: 0x001433BE File Offset: 0x001415BE
			// (set) Token: 0x060041F9 RID: 16889 RVA: 0x001433C6 File Offset: 0x001415C6
			internal object Owner { get; private set; }

			// Token: 0x060041FA RID: 16890 RVA: 0x001433CF File Offset: 0x001415CF
			public EventHandlerRec(object owner, ReferenceAction<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013C9 RID: 5065
			public ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
