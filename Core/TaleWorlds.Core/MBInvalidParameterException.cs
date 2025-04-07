using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009D RID: 157
	public class MBInvalidParameterException : MBException
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x0001BC15 File Offset: 0x00019E15
		public MBInvalidParameterException(string parameterName) : base("The parameter must be valid : " + parameterName)
		{
		}
	}
}
