using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000073 RID: 115
	public static class GameTexts
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x00019988 File Offset: 0x00017B88
		public static void Initialize(GameTextManager gameTextManager)
		{
			GameTexts._gameTextManager = gameTextManager;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00019990 File Offset: 0x00017B90
		public static TextObject FindText(string id, string variation = null)
		{
			return GameTexts._gameTextManager.FindText(id, variation);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001999E File Offset: 0x00017B9E
		public static bool TryGetText(string id, out TextObject textObject, string variation = null)
		{
			return GameTexts._gameTextManager.TryGetText(id, variation, out textObject);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000199AD File Offset: 0x00017BAD
		public static IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			return GameTexts._gameTextManager.FindAllTextVariations(id);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000199BA File Offset: 0x00017BBA
		public static void SetVariable(string variableName, string content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000199C4 File Offset: 0x00017BC4
		public static void SetVariable(string variableName, float content)
		{
			MBTextManager.SetTextVariable(variableName, content);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000199CD File Offset: 0x00017BCD
		public static void SetVariable(string variableName, int content)
		{
			MBTextManager.SetTextVariable(variableName, content);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000199D6 File Offset: 0x00017BD6
		public static void SetVariable(string variableName, TextObject content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000199E0 File Offset: 0x00017BE0
		public static void ClearInstance()
		{
			GameTexts._gameTextManager = null;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000199E8 File Offset: 0x00017BE8
		public static GameTexts.GameTextHelper AddGameTextWithVariation(string id)
		{
			return new GameTexts.GameTextHelper(id);
		}

		// Token: 0x040003BE RID: 958
		private static GameTextManager _gameTextManager;

		// Token: 0x020000FF RID: 255
		public class GameTextHelper
		{
			// Token: 0x06000A57 RID: 2647 RVA: 0x000218FF File Offset: 0x0001FAFF
			public GameTextHelper(string id)
			{
				this._id = id;
			}

			// Token: 0x06000A58 RID: 2648 RVA: 0x0002190E File Offset: 0x0001FB0E
			public GameTexts.GameTextHelper Variation(string text, params object[] propertiesAndWeights)
			{
				GameTexts._gameTextManager.AddGameText(this._id).AddVariation(text, propertiesAndWeights);
				return this;
			}

			// Token: 0x06000A59 RID: 2649 RVA: 0x00021928 File Offset: 0x0001FB28
			public static TextObject MergeTextObjectsWithComma(List<TextObject> textObjects, bool includeAnd)
			{
				return GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(textObjects, new TextObject("{=kfdxjIad}, ", null), includeAnd ? new TextObject("{=eob9goyW} and ", null) : null);
			}

			// Token: 0x06000A5A RID: 2650 RVA: 0x0002194C File Offset: 0x0001FB4C
			public static TextObject MergeTextObjectsWithSymbol(List<TextObject> textObjects, TextObject symbol, TextObject lastSymbol = null)
			{
				int count = textObjects.Count;
				TextObject textObject;
				if (count == 0)
				{
					textObject = TextObject.Empty;
				}
				else if (count == 1)
				{
					textObject = textObjects[0];
				}
				else
				{
					string text = "{=!}";
					for (int i = 0; i < textObjects.Count - 2; i++)
					{
						text = string.Concat(new object[]
						{
							text,
							"{VAR_",
							i,
							"}{SYMBOL}"
						});
					}
					text = string.Concat(new object[]
					{
						text,
						"{VAR_",
						textObjects.Count - 2,
						"}{LAST_SYMBOL}{VAR_",
						textObjects.Count - 1,
						"}"
					});
					textObject = new TextObject(text, null);
					for (int j = 0; j < textObjects.Count; j++)
					{
						textObject.SetTextVariable("VAR_" + j, textObjects[j]);
					}
					textObject.SetTextVariable("SYMBOL", symbol);
					textObject.SetTextVariable("LAST_SYMBOL", lastSymbol ?? symbol);
				}
				return textObject;
			}

			// Token: 0x040006E0 RID: 1760
			private string _id;
		}
	}
}
