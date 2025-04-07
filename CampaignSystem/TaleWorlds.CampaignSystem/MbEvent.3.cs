using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000042 RID: 66
	public class MbEvent<T1, T2> : IMbEvent<T1, T2>, IMbEventBase
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x000222AC File Offset: 0x000204AC
		public void AddNonSerializedListener(object owner, Action<T1, T2> action)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = new MbEvent<T1, T2>.EventHandlerRec<T1, T2>(owner, action);
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x000222D6 File Offset: 0x000204D6
		internal void Invoke(T1 t1, T2 t2)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000222E6 File Offset: 0x000204E6
		private void InvokeList(MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, T1 t1, T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, t2);
				list = list.Next;
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00022302 File Offset: 0x00020502
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00022314 File Offset: 0x00020514
		private void ClearListenerOfList(ref MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, object o)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec2 = list;
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

		// Token: 0x04000283 RID: 643
		private MbEvent<T1, T2>.EventHandlerRec<T1, T2> _nonSerializedListenerList;

		// Token: 0x0200048F RID: 1167
		internal class EventHandlerRec<TS, TQ>
		{
			// Token: 0x17000D6F RID: 3439
			// (get) Token: 0x060041FB RID: 16891 RVA: 0x001433E5 File Offset: 0x001415E5
			// (set) Token: 0x060041FC RID: 16892 RVA: 0x001433ED File Offset: 0x001415ED
			internal Action<TS, TQ> Action { get; private set; }

			// Token: 0x17000D70 RID: 3440
			// (get) Token: 0x060041FD RID: 16893 RVA: 0x001433F6 File Offset: 0x001415F6
			// (set) Token: 0x060041FE RID: 16894 RVA: 0x001433FE File Offset: 0x001415FE
			internal object Owner { get; private set; }

			// Token: 0x060041FF RID: 16895 RVA: 0x00143407 File Offset: 0x00141607
			public EventHandlerRec(object owner, Action<TS, TQ> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013CC RID: 5068
			public MbEvent<T1, T2>.EventHandlerRec<TS, TQ> Next;
		}
	}
}
