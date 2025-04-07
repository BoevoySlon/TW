using System;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001B RID: 27
	public static class PersuasionHelper
	{
		// Token: 0x060000ED RID: 237 RVA: 0x0000C265 File Offset: 0x0000A465
		public static TextObject ShowSuccess(PersuasionOptionArgs optionArgs, bool showToPlayer = true)
		{
			return TextObject.Empty;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000C26C File Offset: 0x0000A46C
		public static TextObject GetDefaultPersuasionOptionReaction(PersuasionOptionResult optionResult)
		{
			TextObject result;
			if (optionResult == PersuasionOptionResult.CriticalSuccess)
			{
				result = new TextObject("{=yNSqDwse}Well... I can't argue with that.", null);
			}
			else if (optionResult == PersuasionOptionResult.Failure || optionResult == PersuasionOptionResult.Miss)
			{
				result = new TextObject("{=mZmCmC6q}I don't think so.", null);
			}
			else if (optionResult == PersuasionOptionResult.CriticalFailure)
			{
				result = new TextObject("{=zqapPfSK}No.. No.", null);
			}
			else
			{
				result = ((MBRandom.RandomFloat > 0.5f) ? new TextObject("{=AmBEgOyq}I see...", null) : new TextObject("{=hq13B7Ok}Yes.. You might be correct.", null));
			}
			return result;
		}
	}
}
