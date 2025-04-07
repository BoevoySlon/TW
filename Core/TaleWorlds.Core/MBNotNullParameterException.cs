using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009F RID: 159
	public class MBNotNullParameterException : MBException
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x0001BC3B File Offset: 0x00019E3B
		public MBNotNullParameterException(string parameterName) : base("The parameter must be null : " + parameterName)
		{
		}
	}
}
