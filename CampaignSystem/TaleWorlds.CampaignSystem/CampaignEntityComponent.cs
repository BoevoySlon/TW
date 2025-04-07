using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000030 RID: 48
	public class CampaignEntityComponent : IEntityComponent
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0001B340 File Offset: 0x00019540
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001B348 File Offset: 0x00019548
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001B350 File Offset: 0x00019550
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0001B352 File Offset: 0x00019552
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001B354 File Offset: 0x00019554
		public virtual void OnTick(float realDt, float dt)
		{
		}
	}
}
