using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B4 RID: 180
	public class HeroExecutionSceneNotificationData : SceneNotificationData
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x0005387E File Offset: 0x00051A7E
		public Hero Executer { get; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00053886 File Offset: 0x00051A86
		public Hero Victim { get; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x0005388E File Offset: 0x00051A8E
		public override bool IsNegativeOptionShown { get; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x00053896 File Offset: 0x00051A96
		public override string SceneID
		{
			get
			{
				return "scn_execution_notification";
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x0005389D File Offset: 0x00051A9D
		public override TextObject NegativeText
		{
			get
			{
				return GameTexts.FindText("str_execution_negative_action", null);
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x000538AA File Offset: 0x00051AAA
		public override bool IsAffirmativeOptionShown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x000538AD File Offset: 0x00051AAD
		public override TextObject TitleText { get; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x000538B5 File Offset: 0x00051AB5
		public override TextObject AffirmativeText { get; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x000538BD File Offset: 0x00051ABD
		public override TextObject AffirmativeTitleText { get; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x000538C5 File Offset: 0x00051AC5
		public override TextObject AffirmativeHintText { get; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000538CD File Offset: 0x00051ACD
		public override TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x000538D5 File Offset: 0x00051AD5
		public override TextObject AffirmativeDescriptionText { get; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x000538DD File Offset: 0x00051ADD
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x06001226 RID: 4646 RVA: 0x000538E8 File Offset: 0x00051AE8
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			Equipment equipment = this.Victim.BattleEquipment.Clone(true);
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			ItemObject item = Items.All.FirstOrDefault((ItemObject i) => i.StringId == "execution_axe");
			Equipment equipment2 = this.Executer.BattleEquipment.Clone(true);
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(item, null, null, false));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			return new List<SceneNotificationData.SceneNotificationCharacter>
			{
				CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Victim, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false),
				CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Executer, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false)
			};
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00053A38 File Offset: 0x00051C38
		private HeroExecutionSceneNotificationData(Hero executingHero, Hero dyingHero, TextObject titleText, TextObject affirmativeTitleText, TextObject affirmativeActionText, TextObject affirmativeActionDescriptionText, TextObject affirmativeActionHintText, TextObject affirmativeActionHintExtendedText, bool isNegativeOptionShown, Action onAffirmativeAction, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			this.Executer = executingHero;
			this.Victim = dyingHero;
			this.TitleText = titleText;
			this.AffirmativeTitleText = affirmativeTitleText;
			this.AffirmativeText = affirmativeActionText;
			this.AffirmativeDescriptionText = affirmativeActionDescriptionText;
			this.AffirmativeHintText = affirmativeActionHintText;
			this.AffirmativeHintTextExtended = affirmativeActionHintExtendedText;
			this.IsNegativeOptionShown = isNegativeOptionShown;
			this.RelevantContext = relevantContextType;
			this._onAffirmativeAction = onAffirmativeAction;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00053AA0 File Offset: 0x00051CA0
		public override void OnAffirmativeAction()
		{
			if (this._onAffirmativeAction != null)
			{
				this._onAffirmativeAction();
				return;
			}
			if (this.Victim != Hero.MainHero)
			{
				KillCharacterAction.ApplyByExecution(this.Victim, this.Executer, true, true);
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00053AD8 File Offset: 0x00051CD8
		public static HeroExecutionSceneNotificationData CreateForPlayerExecutingHero(Hero dyingHero, Action onAffirmativeAction, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
			GameTexts.SetVariable("YEAR", CampaignTime.Now.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			TextObject textObject = GameTexts.FindText("str_execution_positive_action", null);
			textObject.SetCharacterProperties("DYING_HERO", dyingHero.CharacterObject, false);
			return new HeroExecutionSceneNotificationData(Hero.MainHero, dyingHero, GameTexts.FindText("str_executing_prisoner", null), GameTexts.FindText("str_executed_prisoner", null), textObject, GameTexts.FindText("str_execute_prisoner_desc", null), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, false), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, true), true, onAffirmativeAction, relevantContextType);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00053B7C File Offset: 0x00051D7C
		public static HeroExecutionSceneNotificationData CreateForInformingPlayer(Hero executingHero, Hero dyingHero, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
			GameTexts.SetVariable("YEAR", CampaignTime.Now.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			TextObject textObject = new TextObject("{=uYjEknNX}{VICTIM.NAME}'s execution by {EXECUTER.NAME}", null);
			textObject.SetCharacterProperties("VICTIM", dyingHero.CharacterObject, false);
			textObject.SetCharacterProperties("EXECUTER", executingHero.CharacterObject, false);
			return new HeroExecutionSceneNotificationData(executingHero, dyingHero, textObject, GameTexts.FindText("str_executed_prisoner", null), GameTexts.FindText("str_proceed", null), null, null, null, false, null, relevantContextType);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00053C18 File Offset: 0x00051E18
		private static TextObject GetExecuteTroopHintText(Hero dyingHero, bool showAll)
		{
			Dictionary<Clan, int> dictionary = new Dictionary<Clan, int>();
			GameTexts.SetVariable("LEFT", new TextObject("{=jxypVgl2}Relation Changes", null));
			string text = GameTexts.FindText("str_LEFT_colon", null).ToString();
			foreach (Clan clan in Clan.All)
			{
				foreach (Hero hero in clan.Heroes)
				{
					if (!hero.IsHumanPlayerCharacter && hero.IsAlive && hero != dyingHero && (!hero.IsLord || hero.Clan.Leader == hero))
					{
						bool flag;
						int relationChangeForExecutingHero = Campaign.Current.Models.ExecutionRelationModel.GetRelationChangeForExecutingHero(dyingHero, hero, out flag);
						if (relationChangeForExecutingHero != 0)
						{
							if (dictionary.ContainsKey(clan))
							{
								if (relationChangeForExecutingHero < dictionary[clan])
								{
									dictionary[clan] = relationChangeForExecutingHero;
								}
							}
							else
							{
								dictionary.Add(clan, relationChangeForExecutingHero);
							}
						}
					}
				}
			}
			GameTexts.SetVariable("newline", "\n");
			List<KeyValuePair<Clan, int>> list = (from change in dictionary
			orderby change.Value
			select change).ToList<KeyValuePair<Clan, int>>();
			int num = 0;
			foreach (KeyValuePair<Clan, int> keyValuePair in list)
			{
				Clan key = keyValuePair.Key;
				int value = keyValuePair.Value;
				GameTexts.SetVariable("LEFT", key.Name);
				GameTexts.SetVariable("RIGHT", value);
				string content = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", content);
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
				num++;
				if (!showAll && num == HeroExecutionSceneNotificationData.MaxShownRelationChanges)
				{
					TextObject content2 = new TextObject("{=DPTPuyip}And {NUMBER} more...", null);
					GameTexts.SetVariable("NUMBER", dictionary.Count - num);
					GameTexts.SetVariable("STR1", text);
					GameTexts.SetVariable("STR2", content2);
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					TextObject textObject = new TextObject("{=u12ocP9f}Hold '{EXTEND_KEY}' for more info.", null);
					textObject.SetTextVariable("EXTEND_KEY", GameTexts.FindText("str_game_key_text", "anyalt"));
					GameTexts.SetVariable("STR1", text);
					GameTexts.SetVariable("STR2", textObject);
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					break;
				}
			}
			return new TextObject("{=!}" + text, null);
		}

		// Token: 0x04000637 RID: 1591
		private readonly Action _onAffirmativeAction;

		// Token: 0x04000638 RID: 1592
		protected static int MaxShownRelationChanges = 8;
	}
}
