﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000069 RID: 105
	public class VisualTrackerManager
	{
		// Token: 0x06000E0F RID: 3599 RVA: 0x00044CA2 File Offset: 0x00042EA2
		internal static void AutoGeneratedStaticCollectObjectsVisualTrackerManager(object o, List<object> collectedObjects)
		{
			((VisualTrackerManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00044CB0 File Offset: 0x00042EB0
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._trackedObjects);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00044CBE File Offset: 0x00042EBE
		internal static object AutoGeneratedGetMemberValue_trackedObjects(object o)
		{
			return ((VisualTrackerManager)o)._trackedObjects;
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00044CCB File Offset: 0x00042ECB
		public MBReadOnlyList<TrackedObject> TrackedObjects
		{
			get
			{
				return this._trackedObjects;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x00044CD3 File Offset: 0x00042ED3
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x00044CDB File Offset: 0x00042EDB
		[CachedData]
		public int TrackedObjectsVersion { get; private set; }

		// Token: 0x06000E15 RID: 3605 RVA: 0x00044CE4 File Offset: 0x00042EE4
		public VisualTrackerManager()
		{
			this._trackedObjects = new MBList<TrackedObject>();
			this.Initialize();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00044CFD File Offset: 0x00042EFD
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData)
		{
			this.Initialize();
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00044D05 File Offset: 0x00042F05
		private void Initialize()
		{
			this.TrackedObjectsVersion = 0;
			this.CheckObjectValidities();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00044D14 File Offset: 0x00042F14
		private void CheckObjectValidities()
		{
			foreach (TrackedObject trackedObject in this.TrackedObjects.ToList<TrackedObject>())
			{
				if (trackedObject.Object == null)
				{
					this._trackedObjects.Remove(trackedObject);
				}
			}
			int trackedObjectsVersion = this.TrackedObjectsVersion;
			this.TrackedObjectsVersion = trackedObjectsVersion + 1;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00044D8C File Offset: 0x00042F8C
		public void RegisterObject(ITrackableCampaignObject obj)
		{
			if (obj != null)
			{
				bool flag = false;
				foreach (TrackedObject trackedObject in this.TrackedObjects)
				{
					if (trackedObject.Object == obj)
					{
						flag = true;
						trackedObject.TrackerCount++;
					}
				}
				if (!flag)
				{
					TrackedObject item = new TrackedObject(obj);
					this._trackedObjects.Add(item);
					int trackedObjectsVersion = this.TrackedObjectsVersion;
					this.TrackedObjectsVersion = trackedObjectsVersion + 1;
				}
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00044E20 File Offset: 0x00043020
		public bool CheckTracked(ITrackableBase obj)
		{
			using (List<TrackedObject>.Enumerator enumerator = this.TrackedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Object == obj)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00044E7C File Offset: 0x0004307C
		public bool CheckTracked(BasicCharacterObject agentCharacter)
		{
			using (List<TrackedObject>.Enumerator enumerator = this._trackedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Object.CheckTracked(agentCharacter))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00044EDC File Offset: 0x000430DC
		public void RemoveTrackedObject(ITrackableBase obj, bool forceRemove = false)
		{
			for (int i = this._trackedObjects.Count - 1; i >= 0; i--)
			{
				TrackedObject trackedObject = this._trackedObjects[i];
				if (trackedObject.Object == obj)
				{
					trackedObject.TrackerCount--;
					if (trackedObject.TrackerCount <= 0 || forceRemove)
					{
						this._trackedObjects.RemoveAt(i);
						int trackedObjectsVersion = this.TrackedObjectsVersion;
						this.TrackedObjectsVersion = trackedObjectsVersion + 1;
					}
				}
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00044F54 File Offset: 0x00043154
		private void ResetTracker()
		{
			this._trackedObjects.Clear();
			int trackedObjectsVersion = this.TrackedObjectsVersion;
			this.TrackedObjectsVersion = trackedObjectsVersion + 1;
		}

		// Token: 0x04000410 RID: 1040
		[SaveableField(0)]
		private readonly MBList<TrackedObject> _trackedObjects;
	}
}
