using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003E RID: 62
	public class ReferenceMBEvent<T1, T2> : ReferenceIMBEvent<T1, T2>, IMbEventBase
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x00022114 File Offset: 0x00020314
		public void AddNonSerializedListener(object owner, ReferenceAction<T1, T2> action)
		{
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = new ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2>(owner, action);
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002213E File Offset: 0x0002033E
		internal void Invoke(T1 t1, ref T2 t2)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, ref t2);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002214E File Offset: 0x0002034E
		private void InvokeList(ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> list, T1 t1, ref T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, ref t2);
				list = list.Next;
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002216A File Offset: 0x0002036A
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002217C File Offset: 0x0002037C
		private void ClearListenerOfList(ref ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> list, object o)
		{
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec2 = list;
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

		// Token: 0x04000281 RID: 641
		private ReferenceMBEvent<T1, T2>.EventHandlerRec<T1, T2> _nonSerializedListenerList;

		// Token: 0x0200048D RID: 1165
		internal class EventHandlerRec<TS, TQ>
		{
			// Token: 0x17000D6B RID: 3435
			// (get) Token: 0x060041F1 RID: 16881 RVA: 0x00143375 File Offset: 0x00141575
			// (set) Token: 0x060041F2 RID: 16882 RVA: 0x0014337D File Offset: 0x0014157D
			internal ReferenceAction<TS, TQ> Action { get; private set; }

			// Token: 0x17000D6C RID: 3436
			// (get) Token: 0x060041F3 RID: 16883 RVA: 0x00143386 File Offset: 0x00141586
			// (set) Token: 0x060041F4 RID: 16884 RVA: 0x0014338E File Offset: 0x0014158E
			internal object Owner { get; private set; }

			// Token: 0x060041F5 RID: 16885 RVA: 0x00143397 File Offset: 0x00141597
			public EventHandlerRec(object owner, ReferenceAction<TS, TQ> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013C6 RID: 5062
			public ReferenceMBEvent<T1, T2>.EventHandlerRec<TS, TQ> Next;
		}
	}
}
