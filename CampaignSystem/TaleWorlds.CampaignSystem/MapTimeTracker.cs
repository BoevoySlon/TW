﻿using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000055 RID: 85
	internal class MapTimeTracker
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x00037DFD File Offset: 0x00035FFD
		internal static void AutoGeneratedStaticCollectObjectsMapTimeTracker(object o, List<object> collectedObjects)
		{
			((MapTimeTracker)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00037E0B File Offset: 0x0003600B
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00037E0D File Offset: 0x0003600D
		internal static object AutoGeneratedGetMemberValue_numTicks(object o)
		{
			return ((MapTimeTracker)o)._numTicks;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00037E1F File Offset: 0x0003601F
		internal static object AutoGeneratedGetMemberValue_deltaTimeInTicks(object o)
		{
			return ((MapTimeTracker)o)._deltaTimeInTicks;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00037E31 File Offset: 0x00036031
		internal long NumTicks
		{
			get
			{
				return this._numTicks;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00037E39 File Offset: 0x00036039
		internal long DeltaTimeInTicks
		{
			get
			{
				return this._deltaTimeInTicks;
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00037E41 File Offset: 0x00036041
		internal MapTimeTracker(CampaignTime initialMapTime)
		{
			this._numTicks = initialMapTime.NumTicks;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00037E56 File Offset: 0x00036056
		internal MapTimeTracker()
		{
			this._numTicks = 0L;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00037E66 File Offset: 0x00036066
		internal CampaignTime Now
		{
			get
			{
				return new CampaignTime(this._numTicks);
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00037E73 File Offset: 0x00036073
		internal void Tick(float seconds)
		{
			this._deltaTimeInTicks = (long)(seconds * 10000f);
			this._numTicks += this._deltaTimeInTicks;
		}

		// Token: 0x040002B1 RID: 689
		[SaveableField(0)]
		private long _numTicks;

		// Token: 0x040002B2 RID: 690
		[SaveableField(1)]
		private long _deltaTimeInTicks;
	}
}
