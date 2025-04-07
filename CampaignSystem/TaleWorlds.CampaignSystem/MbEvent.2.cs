using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003C RID: 60
	public class MbEvent<T> : IMbEvent<T>, IMbEventBase
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x0002204C File Offset: 0x0002024C
		public void AddNonSerializedListener(object owner, Action<T> action)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = new MbEvent<T>.EventHandlerRec<T>(owner, action);
			MbEvent<T>.EventHandlerRec<T> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00022076 File Offset: 0x00020276
		public void Invoke(T t)
		{
			this.InvokeList(this._nonSerializedListenerList, t);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00022085 File Offset: 0x00020285
		private void InvokeList(MbEvent<T>.EventHandlerRec<T> list, T t)
		{
			while (list != null)
			{
				list.Action(t);
				list = list.Next;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000220A0 File Offset: 0x000202A0
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000220B0 File Offset: 0x000202B0
		private void ClearListenerOfList(ref MbEvent<T>.EventHandlerRec<T> list, object o)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec2 = list;
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

		// Token: 0x04000280 RID: 640
		private MbEvent<T>.EventHandlerRec<T> _nonSerializedListenerList;

		// Token: 0x0200048C RID: 1164
		internal class EventHandlerRec<TS>
		{
			// Token: 0x17000D69 RID: 3433
			// (get) Token: 0x060041EC RID: 16876 RVA: 0x0014333D File Offset: 0x0014153D
			// (set) Token: 0x060041ED RID: 16877 RVA: 0x00143345 File Offset: 0x00141545
			internal Action<TS> Action { get; private set; }

			// Token: 0x17000D6A RID: 3434
			// (get) Token: 0x060041EE RID: 16878 RVA: 0x0014334E File Offset: 0x0014154E
			// (set) Token: 0x060041EF RID: 16879 RVA: 0x00143356 File Offset: 0x00141556
			internal object Owner { get; private set; }

			// Token: 0x060041F0 RID: 16880 RVA: 0x0014335F File Offset: 0x0014155F
			public EventHandlerRec(object owner, Action<TS> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013C3 RID: 5059
			public MbEvent<T>.EventHandlerRec<TS> Next;
		}
	}
}
