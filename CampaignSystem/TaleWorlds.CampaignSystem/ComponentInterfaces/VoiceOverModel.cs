using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CA RID: 458
	public abstract class VoiceOverModel : GameModel
	{
		// Token: 0x06001BDA RID: 7130
		public abstract string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject);

		// Token: 0x06001BDB RID: 7131
		public abstract string GetAccentClass(CultureObject culture, bool isHighClass);
	}
}
