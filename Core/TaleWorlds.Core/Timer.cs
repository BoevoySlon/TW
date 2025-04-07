using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C1 RID: 193
	public class Timer
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0001FF1C File Offset: 0x0001E11C
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0001FF24 File Offset: 0x0001E124
		public float Duration { get; protected set; }

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001FF2D File Offset: 0x0001E12D
		public Timer(float gameTime, float duration, bool autoReset = true)
		{
			this._startTime = gameTime;
			this._latestGameTime = gameTime;
			this._autoReset = autoReset;
			this.Duration = duration;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001FF54 File Offset: 0x0001E154
		public virtual bool Check(float gameTime)
		{
			this._latestGameTime = gameTime;
			if (this.Duration <= 0f)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				this._startTime = gameTime;
				return true;
			}
			bool result = false;
			if (this.ElapsedTime() >= this.Duration)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				if (this._autoReset)
				{
					while (this.ElapsedTime() >= this.Duration)
					{
						this._startTime += this.Duration;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001FFD4 File Offset: 0x0001E1D4
		public float ElapsedTime()
		{
			return this._latestGameTime - this._startTime;
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0001FFE3 File Offset: 0x0001E1E3
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0001FFEB File Offset: 0x0001E1EB
		public float PreviousDeltaTime { get; private set; }

		// Token: 0x060009AA RID: 2474 RVA: 0x0001FFF4 File Offset: 0x0001E1F4
		public void Reset(float gameTime)
		{
			this.Reset(gameTime, this.Duration);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00020003 File Offset: 0x0001E203
		public void Reset(float gameTime, float newDuration)
		{
			this._startTime = gameTime;
			this._latestGameTime = gameTime;
			this.Duration = newDuration;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002001A File Offset: 0x0001E21A
		public void AdjustStartTime(float deltaTime)
		{
			this._startTime += deltaTime;
		}

		// Token: 0x040005A1 RID: 1441
		private float _startTime;

		// Token: 0x040005A2 RID: 1442
		private float _latestGameTime;

		// Token: 0x040005A3 RID: 1443
		private bool _autoReset;
	}
}
