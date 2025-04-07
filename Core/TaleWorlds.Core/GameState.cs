using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200006F RID: 111
	public abstract class GameState : MBObjectBase
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000187C7 File Offset: 0x000169C7
		public GameState Predecessor
		{
			get
			{
				return this.GameStateManager.FindPredecessor(this);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000187D5 File Offset: 0x000169D5
		public bool IsActive
		{
			get
			{
				return this.GameStateManager != null && this.GameStateManager.ActiveState == this;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x000187EF File Offset: 0x000169EF
		public IReadOnlyCollection<IGameStateListener> Listeners
		{
			get
			{
				return this._listeners.AsReadOnly();
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x000187FC File Offset: 0x000169FC
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00018804 File Offset: 0x00016A04
		public GameStateManager GameStateManager { get; internal set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001880D File Offset: 0x00016A0D
		public virtual bool IsMusicMenuState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00018810 File Offset: 0x00016A10
		public virtual bool IsMenuState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00018813 File Offset: 0x00016A13
		public virtual bool IsMission
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00018816 File Offset: 0x00016A16
		protected GameState()
		{
			this._listeners = new List<IGameStateListener>();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00018829 File Offset: 0x00016A29
		public bool RegisterListener(IGameStateListener listener)
		{
			if (this._listeners.Contains(listener))
			{
				return false;
			}
			this._listeners.Add(listener);
			return true;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00018848 File Offset: 0x00016A48
		public bool UnregisterListener(IGameStateListener listener)
		{
			return this._listeners.Remove(listener);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00018858 File Offset: 0x00016A58
		public T GetListenerOfType<T>()
		{
			using (List<IGameStateListener>.Enumerator enumerator = this._listeners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGameStateListener gameStateListener;
					if ((gameStateListener = enumerator.Current) is T)
					{
						return (T)((object)gameStateListener);
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000188C4 File Offset: 0x00016AC4
		internal void HandleInitialize()
		{
			this.OnInitialize();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnInitialize();
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001891C File Offset: 0x00016B1C
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00018920 File Offset: 0x00016B20
		internal void HandleFinalize()
		{
			this.OnFinalize();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnFinalize();
			}
			this._listeners = null;
			this.GameStateManager = null;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00018984 File Offset: 0x00016B84
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00018988 File Offset: 0x00016B88
		internal void HandleActivate()
		{
			GameState.NumberOfListenerActivations = 0;
			if (this.IsActive)
			{
				this.OnActivate();
				if (this.IsActive && this._listeners.Count != 0 && GameState.NumberOfListenerActivations == 0)
				{
					foreach (IGameStateListener gameStateListener in this._listeners)
					{
						gameStateListener.OnActivate();
					}
					GameState.NumberOfListenerActivations++;
				}
				if (!string.IsNullOrEmpty(GameStateManager.StateActivateCommand))
				{
					bool flag;
					CommandLineFunctionality.CallFunction(GameStateManager.StateActivateCommand, "", out flag);
				}
				Debug.ReportMemoryBookmark("GameState Activated: " + base.GetType().Name);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00018A50 File Offset: 0x00016C50
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x00018A58 File Offset: 0x00016C58
		public bool Activated { get; private set; }

		// Token: 0x06000747 RID: 1863 RVA: 0x00018A61 File Offset: 0x00016C61
		protected virtual void OnActivate()
		{
			this.Activated = true;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00018A6C File Offset: 0x00016C6C
		internal void HandleDeactivate()
		{
			this.OnDeactivate();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnDeactivate();
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00018AC4 File Offset: 0x00016CC4
		protected virtual void OnDeactivate()
		{
			this.Activated = false;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00018ACD File Offset: 0x00016CCD
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00018ACF File Offset: 0x00016CCF
		protected internal virtual void OnIdleTick(float dt)
		{
		}

		// Token: 0x040003AE RID: 942
		public int Level;

		// Token: 0x040003AF RID: 943
		private List<IGameStateListener> _listeners;

		// Token: 0x040003B0 RID: 944
		public static int NumberOfListenerActivations;
	}
}
