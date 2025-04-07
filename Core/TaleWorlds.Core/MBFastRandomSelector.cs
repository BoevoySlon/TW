using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000A5 RID: 165
	public class MBFastRandomSelector<T>
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0001BC7B File Offset: 0x00019E7B
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0001BC83 File Offset: 0x00019E83
		public ushort RemainingCount { get; private set; }

		// Token: 0x06000830 RID: 2096 RVA: 0x0001BC8C File Offset: 0x00019E8C
		public MBFastRandomSelector(ushort capacity = 32)
		{
			this.ReallocateIndexArray(capacity);
			this._list = null;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001BCA2 File Offset: 0x00019EA2
		public MBFastRandomSelector(MBReadOnlyList<T> list, ushort capacity = 32)
		{
			this.ReallocateIndexArray(capacity);
			this.Initialize(list);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		public void Initialize(MBReadOnlyList<T> list)
		{
			if (list != null && list.Count <= 65535)
			{
				this._list = list;
				this.TryExpand();
			}
			else
			{
				Debug.FailedAssert("Cannot initialize random selector as passed list is null or it exceeds " + ushort.MaxValue + " elements).", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\MBFastRandomSelector.cs", "Initialize", 63);
				this._list = null;
			}
			this.Reset();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001BD1C File Offset: 0x00019F1C
		public void Reset()
		{
			if (this._list != null)
			{
				if (this._currentVersion < 65535)
				{
					this._currentVersion += 1;
				}
				else
				{
					for (int i = 0; i < this._indexArray.Length; i++)
					{
						this._indexArray[i] = default(MBFastRandomSelector<T>.IndexEntry);
					}
					this._currentVersion = 1;
				}
				this.RemainingCount = (ushort)this._list.Count;
				return;
			}
			this._currentVersion = 1;
			this.RemainingCount = 0;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001BD9C File Offset: 0x00019F9C
		public void Pack()
		{
			if (this._list != null)
			{
				ushort num = (ushort)MathF.Max(32, this._list.Count);
				if (this._indexArray.Length != (int)num)
				{
					this.ReallocateIndexArray(num);
					return;
				}
			}
			else if (this._indexArray.Length != 32)
			{
				this.ReallocateIndexArray(32);
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001BDEC File Offset: 0x00019FEC
		public bool SelectRandom(out T selection, Predicate<T> conditions = null)
		{
			selection = default(T);
			if (this._list == null)
			{
				return false;
			}
			bool flag = false;
			while (this.RemainingCount > 0 && !flag)
			{
				ushort num = (ushort)MBRandom.RandomInt((int)this.RemainingCount);
				ushort num2 = this.RemainingCount - 1;
				MBFastRandomSelector<T>.IndexEntry indexEntry = this._indexArray[(int)num];
				T t = (indexEntry.Version == this._currentVersion) ? this._list[(int)indexEntry.Index] : this._list[(int)num];
				if (conditions == null || conditions(t))
				{
					flag = true;
					selection = t;
				}
				MBFastRandomSelector<T>.IndexEntry indexEntry2 = this._indexArray[(int)num2];
				this._indexArray[(int)num] = ((indexEntry2.Version == this._currentVersion) ? new MBFastRandomSelector<T>.IndexEntry(indexEntry2.Index, this._currentVersion) : new MBFastRandomSelector<T>.IndexEntry(num2, this._currentVersion));
				ushort remainingCount = this.RemainingCount;
				this.RemainingCount = remainingCount - 1;
			}
			return flag;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
		private void TryExpand()
		{
			if (this._indexArray.Length >= this._list.Count)
			{
				return;
			}
			ushort capacity = (ushort)(this._list.Count * 2);
			this.ReallocateIndexArray(capacity);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001BF21 File Offset: 0x0001A121
		private void ReallocateIndexArray(ushort capacity)
		{
			capacity = (ushort)MBMath.ClampInt((int)capacity, 32, 65535);
			this._indexArray = new MBFastRandomSelector<T>.IndexEntry[(int)capacity];
			this._currentVersion = 1;
		}

		// Token: 0x040004AD RID: 1197
		public const ushort MinimumCapacity = 32;

		// Token: 0x040004AE RID: 1198
		public const ushort MaximumCapacity = 65535;

		// Token: 0x040004AF RID: 1199
		private const ushort InitialVersion = 1;

		// Token: 0x040004B0 RID: 1200
		private const ushort MaximumVersion = 65535;

		// Token: 0x040004B2 RID: 1202
		private MBReadOnlyList<T> _list;

		// Token: 0x040004B3 RID: 1203
		private MBFastRandomSelector<T>.IndexEntry[] _indexArray;

		// Token: 0x040004B4 RID: 1204
		private ushort _currentVersion;

		// Token: 0x02000102 RID: 258
		public struct IndexEntry
		{
			// Token: 0x06000A5E RID: 2654 RVA: 0x00021A85 File Offset: 0x0001FC85
			public IndexEntry(ushort index, ushort version)
			{
				this.Index = index;
				this.Version = version;
			}

			// Token: 0x040006E7 RID: 1767
			public ushort Index;

			// Token: 0x040006E8 RID: 1768
			public ushort Version;
		}
	}
}
