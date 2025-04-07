using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000148 RID: 328
	public class DefaultVoiceOverModel : VoiceOverModel
	{
		// Token: 0x06001844 RID: 6212 RVA: 0x0007B864 File Offset: 0x00079A64
		public override string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject)
		{
			if (voiceObject == null)
			{
				return "";
			}
			foreach (string str in voiceObject.VoicePaths)
			{
				Debug.Print("[VOICEOVER]Sound path search for: " + str, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			string text = "";
			string value = character.StringId + "_" + (CharacterObject.PlayerCharacter.IsFemale ? "female" : "male");
			foreach (string text2 in voiceObject.VoicePaths)
			{
				if (text2.Contains(value))
				{
					text = text2;
					break;
				}
				if (text2.Contains(character.StringId + "_"))
				{
					text = text2;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				string accentClass = this.GetAccentClass(character.Culture, ConversationTagHelper.UsesHighRegister(character));
				Debug.Print("accentClass: " + accentClass, 0, Debug.DebugColor.White, 17592186044416UL);
				string text3 = character.IsFemale ? "female" : "male";
				string stringId = character.GetPersona().StringId;
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				list2.Add(string.Concat(new string[]
				{
					".+\\\\",
					accentClass,
					"_",
					text3,
					"_",
					stringId,
					"_.+"
				}));
				list2.Add(string.Concat(new string[]
				{
					".+\\\\",
					accentClass,
					"_",
					text3,
					"_generic_.+"
				}));
				this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
				if (list.IsEmpty<string>())
				{
					list2.Clear();
					list2.Add(string.Concat(new string[]
					{
						".+\\\\",
						accentClass,
						"_",
						stringId,
						"_.+"
					}));
					list2.Add(".+\\\\" + accentClass + "_generic_.+");
					list2.Add(string.Concat(new string[]
					{
						".+\\\\",
						text3,
						"_",
						stringId,
						"_.+"
					}));
					list2.Add(".+\\\\" + text3 + "_generic_.+");
					this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
					if (list.IsEmpty<string>())
					{
						list2.Clear();
						list2.Add(".+\\\\" + stringId + "_.+");
						list2.Add(".+\\\\generic_.+");
						list2.Add(".+" + accentClass + "_.+");
						this.CheckPossibleMatches(voiceObject, list2, ref list, true, character.IsFemale);
					}
				}
				if (!list.IsEmpty<string>())
				{
					if (character.IsHero)
					{
						text = list[character.HeroObject.RandomInt(list.Count)];
					}
					else if (MobileParty.ConversationParty != null)
					{
						text = list[MobileParty.ConversationParty.RandomInt(list.Count)];
					}
					else
					{
						text = list.GetRandomElement<string>();
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text, 0, Debug.DebugColor.White, 17592186044416UL);
			text = text.Replace("$PLATFORM", "PC");
			return text + ".ogg";
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0007BC14 File Offset: 0x00079E14
		private void CheckPossibleMatches(VoiceObject voiceObject, List<string> possibleMatches, ref List<string> possibleVoicePaths, bool doubleCheckForGender = false, bool isFemale = false)
		{
			foreach (string text in possibleMatches)
			{
				Regex regex = new Regex(text, 1);
				foreach (string text2 in voiceObject.VoicePaths)
				{
					if (regex.Match(text2).Success && !possibleVoicePaths.Contains(text2))
					{
						if (doubleCheckForGender && (text2.Contains("_male") || text2.Contains("_female")))
						{
							string value = isFemale ? "_female" : "_male";
							if (text2.Contains(value))
							{
								possibleVoicePaths.Add(text2);
							}
						}
						else
						{
							possibleVoicePaths.Add(text2);
						}
					}
				}
			}
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0007BD08 File Offset: 0x00079F08
		public override string GetAccentClass(CultureObject culture, bool isHighClass)
		{
			if (culture.StringId == "empire")
			{
				if (isHighClass)
				{
					return "imperial_high";
				}
				return "imperial_low";
			}
			else
			{
				if (culture.StringId == "vlandia")
				{
					return "vlandian";
				}
				if (culture.StringId == "sturgia")
				{
					return "sturgian";
				}
				if (culture.StringId == "khuzait")
				{
					return "khuzait";
				}
				if (culture.StringId == "aserai")
				{
					return "aserai";
				}
				if (culture.StringId == "battania")
				{
					return "battanian";
				}
				if (culture.StringId == "forest_bandits")
				{
					return "forest_bandits";
				}
				if (culture.StringId == "sea_raiders")
				{
					return "sea_raiders";
				}
				if (culture.StringId == "mountain_bandits")
				{
					return "mountain_bandits";
				}
				if (culture.StringId == "desert_bandits")
				{
					return "desert_bandits";
				}
				if (culture.StringId == "steppe_bandits")
				{
					return "steppe_bandits";
				}
				if (culture.StringId == "looters")
				{
					return "looters";
				}
				return "";
			}
		}

		// Token: 0x04000880 RID: 2176
		private const string ImperialHighClass = "imperial_high";

		// Token: 0x04000881 RID: 2177
		private const string ImperialLowClass = "imperial_low";

		// Token: 0x04000882 RID: 2178
		private const string VlandianClass = "vlandian";

		// Token: 0x04000883 RID: 2179
		private const string SturgianClass = "sturgian";

		// Token: 0x04000884 RID: 2180
		private const string KhuzaitClass = "khuzait";

		// Token: 0x04000885 RID: 2181
		private const string AseraiClass = "aserai";

		// Token: 0x04000886 RID: 2182
		private const string BattanianClass = "battanian";

		// Token: 0x04000887 RID: 2183
		private const string ForestBanditClass = "forest_bandits";

		// Token: 0x04000888 RID: 2184
		private const string SeaBanditClass = "sea_raiders";

		// Token: 0x04000889 RID: 2185
		private const string MountainBanditClass = "mountain_bandits";

		// Token: 0x0400088A RID: 2186
		private const string DesertBanditClass = "desert_bandits";

		// Token: 0x0400088B RID: 2187
		private const string SteppeBanditClass = "steppe_bandits";

		// Token: 0x0400088C RID: 2188
		private const string LootersClass = "looters";

		// Token: 0x0400088D RID: 2189
		private const string Male = "male";

		// Token: 0x0400088E RID: 2190
		private const string Female = "female";

		// Token: 0x0400088F RID: 2191
		private const string GenericPersonaId = "generic";
	}
}
