using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000AB RID: 171
	public readonly struct CampaignSaveMetaDataArgs
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x0001C563 File Offset: 0x0001A763
		public CampaignSaveMetaDataArgs(string[] moduleName, params KeyValuePair<string, string>[] otherArgs)
		{
			this.ModuleNames = moduleName;
			this.OtherData = otherArgs;
		}

		// Token: 0x040004C3 RID: 1219
		public readonly string[] ModuleNames;

		// Token: 0x040004C4 RID: 1220
		public readonly KeyValuePair<string, string>[] OtherData;
	}
}
