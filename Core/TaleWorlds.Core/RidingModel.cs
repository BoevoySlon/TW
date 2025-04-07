using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006B RID: 107
	public abstract class RidingModel : GameModel
	{
		// Token: 0x0600072B RID: 1835
		public abstract float CalculateAcceleration(in EquipmentElement mountElement, in EquipmentElement harnessElement, int ridingSkill);
	}
}
