using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000032 RID: 50
	public class MbEvent : IMbEvent
	{
		// Token: 0x0600035C RID: 860 RVA: 0x0001B360 File Offset: 0x00019560
		public void AddNonSerializedListener(object owner, Action action)
		{
			MbEvent.EventHandlerRec eventHandlerRec = new MbEvent.EventHandlerRec(owner, action);
			MbEvent.EventHandlerRec nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0001B38A File Offset: 0x0001958A
		public void Invoke()
		{
			this.InvokeList(this._nonSerializedListenerList);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001B398 File Offset: 0x00019598
		private void InvokeList(MbEvent.EventHandlerRec list)
		{
			while (list != null)
			{
				list.Action();
				list = list.Next;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001B3B2 File Offset: 0x000195B2
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001B3C4 File Offset: 0x000195C4
		private void ClearListenerOfList(ref MbEvent.EventHandlerRec list, object o)
		{
			MbEvent.EventHandlerRec eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent.EventHandlerRec eventHandlerRec2 = list;
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

		// Token: 0x04000176 RID: 374
		private MbEvent.EventHandlerRec _nonSerializedListenerList;

		// Token: 0x02000488 RID: 1160
		internal class EventHandlerRec
		{
			// Token: 0x17000D65 RID: 3429
			// (get) Token: 0x060041C4 RID: 16836 RVA: 0x00142FBF File Offset: 0x001411BF
			// (set) Token: 0x060041C5 RID: 16837 RVA: 0x00142FC7 File Offset: 0x001411C7
			internal Action Action { get; private set; }

			// Token: 0x17000D66 RID: 3430
			// (get) Token: 0x060041C6 RID: 16838 RVA: 0x00142FD0 File Offset: 0x001411D0
			// (set) Token: 0x060041C7 RID: 16839 RVA: 0x00142FD8 File Offset: 0x001411D8
			internal object Owner { get; private set; }

			// Token: 0x060041C8 RID: 16840 RVA: 0x00142FE1 File Offset: 0x001411E1
			public EventHandlerRec(object owner, Action action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040013A6 RID: 5030
			public MbEvent.EventHandlerRec Next;
		}
	}
}
