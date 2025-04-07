using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C9 RID: 201
	internal class LocatorGrid<T> where T : ILocatable<T>
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x00055869 File Offset: 0x00053A69
		internal LocatorGrid(float gridNodeSize = 5f, int gridWidth = 32, int gridHeight = 32)
		{
			this._width = gridWidth;
			this._height = gridHeight;
			this._gridNodeSize = gridNodeSize;
			this._nodes = new T[this._width * this._height];
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005589E File Offset: 0x00053A9E
		private int MapCoordinates(int x, int y)
		{
			x %= this._width;
			if (x < 0)
			{
				x += this._width;
			}
			y %= this._height;
			if (y < 0)
			{
				y += this._height;
			}
			return y * this._width + x;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000558DC File Offset: 0x00053ADC
		internal bool CheckWhetherPositionsAreInSameNode(Vec2 pos1, ILocatable<T> locatable)
		{
			int num = this.Pos2NodeIndex(pos1);
			int locatorNodeIndex = locatable.LocatorNodeIndex;
			return num == locatorNodeIndex;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000558FC File Offset: 0x00053AFC
		internal bool UpdateLocator(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			Vec2 getPosition2D = locatable2.GetPosition2D;
			int num = this.Pos2NodeIndex(getPosition2D);
			if (num != locatable2.LocatorNodeIndex)
			{
				if (locatable2.LocatorNodeIndex >= 0)
				{
					this.RemoveFromList(locatable2);
				}
				this.AddToList(num, locatable);
				locatable2.LocatorNodeIndex = num;
				return true;
			}
			return false;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0005594C File Offset: 0x00053B4C
		private void RemoveFromList(ILocatable<T> locatable)
		{
			if (this._nodes[locatable.LocatorNodeIndex] == locatable)
			{
				this._nodes[locatable.LocatorNodeIndex] = locatable.NextLocatable;
				locatable.NextLocatable = default(T);
				return;
			}
			ILocatable<T> locatable2;
			if ((locatable2 = this._nodes[locatable.LocatorNodeIndex]) != null)
			{
				while (locatable2.NextLocatable != null)
				{
					if (locatable2.NextLocatable == locatable)
					{
						locatable2.NextLocatable = locatable.NextLocatable;
						locatable.NextLocatable = default(T);
						return;
					}
					locatable2 = locatable2.NextLocatable;
				}
				Debug.FailedAssert("cannot remove party from MapLocator: " + locatable.ToString(), "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\LocatorGrid.cs", "RemoveFromList", 134);
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00055A1C File Offset: 0x00053C1C
		private void AddToList(int nodeIndex, T locator)
		{
			T nextLocatable = this._nodes[nodeIndex];
			this._nodes[nodeIndex] = locator;
			locator.NextLocatable = nextLocatable;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00055A50 File Offset: 0x00053C50
		private T FindLocatableOnNextNode(ref LocatableSearchData<T> data)
		{
			T t = default(T);
			do
			{
				data.CurrentY++;
				if (data.CurrentY > data.MaxYInclusive)
				{
					data.CurrentY = data.MinY;
					data.CurrentX++;
				}
				if (data.CurrentX <= data.MaxXInclusive)
				{
					int num = this.MapCoordinates(data.CurrentX, data.CurrentY);
					t = this._nodes[num];
				}
			}
			while (t == null && data.CurrentX <= data.MaxXInclusive);
			return t;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00055ADC File Offset: 0x00053CDC
		internal T FindNextLocatable(ref LocatableSearchData<T> data)
		{
			if (data.CurrentLocatable != null)
			{
				data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				while (data.CurrentLocatable != null)
				{
					if (data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) < data.RadiusSquared)
					{
						break;
					}
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			while (data.CurrentLocatable == null && data.CurrentX <= data.MaxXInclusive)
			{
				data.CurrentLocatable = this.FindLocatableOnNextNode(ref data);
				while (data.CurrentLocatable != null && data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) >= data.RadiusSquared)
				{
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			return (T)((object)data.CurrentLocatable);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00055BC4 File Offset: 0x00053DC4
		internal LocatableSearchData<T> StartFindingLocatablesAroundPosition(Vec2 position, float radius)
		{
			int minX;
			int minY;
			int maxX;
			int maxY;
			this.GetBoundaries(position, radius, out minX, out minY, out maxX, out maxY);
			return new LocatableSearchData<T>(position, radius, minX, minY, maxX, maxY);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00055BEC File Offset: 0x00053DEC
		internal void RemoveLocatable(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			if (locatable2.LocatorNodeIndex >= 0)
			{
				this.RemoveFromList(locatable2);
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00055C10 File Offset: 0x00053E10
		private void GetBoundaries(Vec2 position, float radius, out int minX, out int minY, out int maxX, out int maxY)
		{
			Vec2 v = new Vec2(MathF.Min(radius, (float)(this._width - 1) * this._gridNodeSize * 0.5f), MathF.Min(radius, (float)(this._height - 1) * this._gridNodeSize * 0.5f));
			this.GetGridIndices(position - v, out minX, out minY);
			this.GetGridIndices(position + v, out maxX, out maxY);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00055C7D File Offset: 0x00053E7D
		private void GetGridIndices(Vec2 position, out int x, out int y)
		{
			x = MathF.Floor(position.x / this._gridNodeSize);
			y = MathF.Floor(position.y / this._gridNodeSize);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00055CA8 File Offset: 0x00053EA8
		private int Pos2NodeIndex(Vec2 position)
		{
			int x;
			int y;
			this.GetGridIndices(position, out x, out y);
			return this.MapCoordinates(x, y);
		}

		// Token: 0x04000669 RID: 1641
		private const float DefaultGridNodeSize = 5f;

		// Token: 0x0400066A RID: 1642
		private const int DefaultGridWidth = 32;

		// Token: 0x0400066B RID: 1643
		private const int DefaultGridHeight = 32;

		// Token: 0x0400066C RID: 1644
		private readonly T[] _nodes;

		// Token: 0x0400066D RID: 1645
		private readonly float _gridNodeSize;

		// Token: 0x0400066E RID: 1646
		private readonly int _width;

		// Token: 0x0400066F RID: 1647
		private readonly int _height;
	}
}
