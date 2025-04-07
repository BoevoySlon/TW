using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009A RID: 154
	public class MBUnderFlowException : MBException
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x0001BBCF File Offset: 0x00019DCF
		public MBUnderFlowException() : base("The given value is less than the expected value.")
		{
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001BBDC File Offset: 0x00019DDC
		public MBUnderFlowException(string parameterName) : base("The given value is less than the expected value : " + parameterName)
		{
		}
	}
}
