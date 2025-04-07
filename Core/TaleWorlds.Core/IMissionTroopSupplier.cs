using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000B0 RID: 176
	public interface IMissionTroopSupplier
	{
		// Token: 0x06000898 RID: 2200
		IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate);

		// Token: 0x06000899 RID: 2201
		IEnumerable<IAgentOriginBase> GetAllTroops();

		// Token: 0x0600089A RID: 2202
		BasicCharacterObject GetGeneralCharacter();

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600089B RID: 2203
		int NumRemovedTroops { get; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600089C RID: 2204
		int NumTroopsNotSupplied { get; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600089D RID: 2205
		bool AnyTroopRemainsToBeSupplied { get; }

		// Token: 0x0600089E RID: 2206
		int GetNumberOfPlayerControllableTroops();
	}
}
