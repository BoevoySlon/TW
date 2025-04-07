using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009B RID: 155
	public class MBOutOfRangeException : MBException
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0001BBEF File Offset: 0x00019DEF
		public MBOutOfRangeException(string parameterName) : base("The given value is out of range : " + parameterName)
		{
		}
	}
}
