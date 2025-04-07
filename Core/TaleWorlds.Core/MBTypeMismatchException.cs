using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000099 RID: 153
	public class MBTypeMismatchException : MBException
	{
		// Token: 0x06000821 RID: 2081 RVA: 0x0001BBBC File Offset: 0x00019DBC
		public MBTypeMismatchException(string exceptionString) : base("Type Does not match with the expected one. " + exceptionString)
		{
		}
	}
}
