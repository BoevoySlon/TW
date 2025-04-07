using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000381 RID: 897
	public class CompanionGrievanceBehavior : CampaignBehaviorBase
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x000E30FC File Offset: 0x000E12FC
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaided));
			CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDesertedBattle));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000E317C File Offset: 0x000E137C
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CompanionGrievanceBehavior.Grievance>>("_heroGrievances", ref this._heroGrievances);
			dataStore.SyncData<CompanionGrievanceBehavior.Grievance>("_currentGrievance", ref this._currentGrievance);
			dataStore.SyncData<CampaignTime[]>("_nextGrievableTimeForComplaintType", ref this._nextGrievableTimeForComplaintType);
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x000E31B4 File Offset: 0x000E13B4
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("companion_start_grievance", "start", "grievance_received", "{=PVyZ9vNB}{TITLE}, there is something I wish to discuss with you.", new ConversationSentence.OnConditionDelegate(this.companion_start_grievance_condition), null, 120, null);
			campaignGameStarter.AddPlayerLine("grievance_requested", "grievance_received", "grievance_noticed", "{=m72wpzG2}Go on, I'm listening.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_desert_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_desert_battle_condition), null, 100, "", "", "", "", null).Variation(new object[]
			{
				"{=7ldEMGn6}I don't like running away from a battle like that.",
				"DefaultTag",
				0
			}).Variation(new object[]
			{
				"{=EqLQtlca}The way we just ran from the enemy back there... I don't want to get a name for being a coward.",
				"PersonaSoftspokenTag",
				1
			}).Variation(new object[]
			{
				"{=sXwOLNo9}I don't like turning my back on the enemy like that. For me, death in battle is better than dishonor.",
				"PersonaCurtTag",
				1
			}).Variation(new object[]
			{
				"{=TnW3i5ul}We ran back there. I despise running. I prefer to be the wolf, not a rabbit.",
				"PersonaCurtTag",
				1,
				"KhuzaitTag",
				1
			}).Variation(new object[]
			{
				"{=IDJbtGks}We ran back there. It is shameful to turn your back on the enemy.",
				"PersonaCurtTag",
				1,
				"SturgianTag",
				1
			}).Variation(new object[]
			{
				"{=PTj1WtJx}The way we ran back there... It shames me to think of it. Next time, let us fight and die rather than let men call us cowards!",
				"PersonaEarnestTag",
				1
			}).Variation(new object[]
			{
				"{=UTRvswWE}As I recall, when you first hired me, it was to fight, not run away. Now, I'm sure what you did back there was sensible, but still, I've earned a bit of a reputation for bravery and I don't care to be called a coward. Those foes we can't beat - let's try to stay a little more clear of them next time, shall we?",
				"PersonaIronicTag",
				1
			}).Variation(new object[]
			{
				"{=v7OCHday}I suppose back there we had to run away to fight another day, as the hero Cathalac once did. If you remember the story, though, for the next three years he sat by himself in a bog, unable to look anyone else in the eye. So let's not do that too often, shall we?",
				"PersonaIronicTag",
				1,
				"BattanianTag",
				1
			}).Variation(new object[]
			{
				"{=u9tAQLUf}We ran away back there. I hope word does not get around. Not looking forward to seeing the snickers and grins on people's faces the next time we walk into a tavern. Stings worse than arrows, that does.",
				"PersonaIronicTag",
				1,
				"VlandianTag",
				1
			}).Variation(new object[]
			{
				"{=gfoqoGTn}The way we ran away back there... I may have told you that I hoped one day the poets would write odes about me. I had intended that they would praise my heroism, not my ability to scamper to safety.",
				"PersonaIronicTag",
				1,
				"AseraiTag",
				1
			});
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_wage_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_wage_condition), null, 100, "", "", "", "", null).Variation(new object[]
			{
				"{=yuqLzmL9}I should remind you that I expect to be paid as you had promised.",
				"DefaultTag",
				1
			}).Variation(new object[]
			{
				"{=zBfB5vw8}I hope you don't mind me saying this, but... Your men shed their blood for you. It bothers me to hear that their wages are late.",
				"PersonaSoftspokenTag",
				1
			}).Variation(new object[]
			{
				"{=Bhtt6XPv}Your men's wages are late. That's not the kind of company I like to fight in.",
				"PersonaCurtTag",
				1
			}).Variation(new object[]
			{
				"{=0mVwnCES}I must say something. Your men say their wages are late. We should take care that this doesn't happen.",
				"PersonaEarnestTag",
				1
			}).Variation(new object[]
			{
				"{=TzqxgiQl}The men say their wages are late. Best uphold your end of the bargain with them, or they might not keep theirs. It would especially be tricky if they chose to void their contract during a battle, wouldn't you say?",
				"PersonaIronicTag",
				1
			});
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_starve_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_starve_condition), null, 100, "", "", "", "", null).Variation(new object[]
			{
				"{=IPLyqdVX}I hear we're running low on food. We should watch our stocks better.",
				"DefaultTag",
				1
			}).Variation(new object[]
			{
				"{=ITboR6C1}The men say we're running low on food. We should be more careful of that.",
				"PersonaSoftspokenTag",
				1
			}).Variation(new object[]
			{
				"{=HkjaCc44}Your men say there's little to eat. They march, they fight. They deserve to eat.",
				"PersonaCurtTag",
				1
			}).Variation(new object[]
			{
				"{=acOOsQaC}The food's running out. That's not fair to the men. We should take care that the food doesn't run out.",
				"PersonaEarnestTag",
				1
			}).Variation(new object[]
			{
				"{=6UKdUrPs}The men say the food's running out. We expect them to die for us if needed. Least we can do is let them die on a full belly.",
				"PersonaIronicTag",
				1
			}).Variation(new object[]
			{
				"{=gChji1JO}About the food... These men are ready to spill their blood for you, but there won't be much blood in their veins to shed if their bellies are empty.",
				"PersonaIronicTag",
				1,
				"BattanianTag",
				1
			}).Variation(new object[]
			{
				"{=IgGQUms4}About our food situation... The general Aricaros used to say that an army marches on its stomach. Can't get far on an empty one.",
				"PersonaIronicTag",
				1,
				"EmpireTag",
				1
			}).Variation(new object[]
			{
				"{=ZNaQrIaP}About our food situation... We shouldn't let the men go hungry. A man's courage comes from his stomach, they say.",
				"PersonaIronicTag",
				1,
				"PersonaIronicTag",
				1
			});
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_raid_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_raid_condition), null, 100, "", "", "", "", null).Variation(new object[]
			{
				"{=zNvjSFaC}Pillaging villages is not what I signed up for.",
				"DefaultTag",
				1
			}).Variation(new object[]
			{
				"{=bpXgcBCp}What we did to that village... I don't like it. Those farmers, they're a lot like my people. I want to know it won't happen again.",
				"PersonaSoftspokenTag",
				1
			}).Variation(new object[]
			{
				"{=4bkLDxIU}What we did back there, to that village... I don't do that. I want no part of it.",
				"PersonaCurtTag",
				1
			}).Variation(new object[]
			{
				"{=VldAzBo5}I need to say something. What we did to that village - it was wrong. They're innocent farmers and they shouldn't have their homes and fields ransacked and burned like that. I won't do that again.",
				"PersonaEarnestTag",
				1
			}).Variation(new object[]
			{
				"{=pDa7kOja}I know war is cruel, but I don't want to make it crueler than necessary. I'd rather not have the blood of innocents on my conscience, if you don't mind. Let's not raid villages like that.",
				"PersonaIronicTag",
				1
			});
			campaignGameStarter.AddPlayerLine("grievance_1", "grievance_listened", "close_window", "{=OVeSBrhv}Very well, I will consider this when taking such actions.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_accepted_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("grievance_2", "grievance_listened", "close_window", "{=2wmKs6Is}As your leader I am able to decide the best course of action.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("grievance_3", "grievance_listened", "close_window", "{=fzKFQuFT}Perhaps you are not suitable for this party after all.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_rejected_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_repeat_grievance", "start", "grievance_repeated", "{=baeO5Zkk}{TITLE}... {GRIEVANCE_SHORT_DESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.companion_repeat_grievance_condition), null, 120, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_desert", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_desert_battle_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_wage", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_wage_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_starve", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_starve_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_raid", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_raid_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000E38F4 File Offset: 0x000E1AF4
		private bool companion_grievance_raid_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.VillageRaided;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000E390E File Offset: 0x000E1B0E
		private bool companion_grievance_starve_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.Starvation;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000E3928 File Offset: 0x000E1B28
		private bool companion_grievance_desert_battle_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.DesertedBattle;
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000E3942 File Offset: 0x000E1B42
		private bool companion_grievance_wage_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.NoWage;
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000E395C File Offset: 0x000E1B5C
		private bool companion_start_grievance_condition()
		{
			MBTextManager.SetTextVariable("TITLE", Hero.MainHero.IsFemale ? GameTexts.FindText("str_my_lady", null) : GameTexts.FindText("str_my_lord", null), false);
			return this._currentGrievance != null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero && this._currentGrievance.Count <= 1 && !this._currentGrievance.HasBeenSettled;
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000E39D8 File Offset: 0x000E1BD8
		private bool companion_repeat_grievance_condition()
		{
			if (this._currentGrievance != null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero)
			{
				MBTextManager.SetTextVariable("TITLE", ConversationHelper.HeroRefersToHero(Hero.OneToOneConversationHero, Hero.MainHero, true), false);
				MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=scJ2eVhS}What I said to you before... [default, should not appear]", false);
				if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.DesertedBattle)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=1G5M9nn2}What I mentioned about running from battle...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.NoWage)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=p78FaTqe}What I said about our wages being paid on time...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.Starvation)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=zfPQlDbQ}What I said about our food...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.VillageRaided)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=pQmUIjOQ}What I said about raiding villagers...", false);
				}
				MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=qNSOb7pJ}Once again, this is not something I'm happy with...", false);
				if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaEarnest)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=YWu5Xfgz}I don't feel you're taking my complaint seriously.", false);
				}
				else if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaIronic)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=wScKLt7F}Let me put things this way... It's not grown on me at all since the last time it happened.", false);
				}
				else if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=dpzbyUCa}I don't care for it any more than I did before.", false);
				}
				return Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero && this._currentGrievance.Count > 1;
			}
			return false;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000E3B64 File Offset: 0x000E1D64
		private void companion_grievance_accepted_consequence()
		{
			CompanionGrievanceBehavior.Grievance value = this._heroGrievances.FirstOrDefault((KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> t) => t.Value == this._currentGrievance && t.Key == this._currentGrievance.GrievingHero).Value;
			if (value != null)
			{
				value.HasBeenSettled = true;
				if (value.Count <= 1)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, 10, true);
				}
			}
			this._currentGrievance = null;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000E3BC4 File Offset: 0x000E1DC4
		private void companion_grievance_consequence()
		{
			if (this._currentGrievance.Count > 1)
			{
				int relationChange = this._currentGrievance.HasBeenSettled ? -5 : -2;
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, relationChange, true);
			}
			this._currentGrievance = null;
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000E3C11 File Offset: 0x000E1E11
		private void companion_grievance_rejected_consequence()
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, -15, true);
			this._currentGrievance = null;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000E3C32 File Offset: 0x000E1E32
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000E3C3C File Offset: 0x000E1E3C
		private void OnHourlyTick()
		{
			foreach (KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> keyValuePair in this._heroGrievances)
			{
				CompanionGrievanceBehavior.Grievance value = keyValuePair.Value;
				if (value.GrievingHero.PartyBelongedTo == MobileParty.MainParty && value.HaveGrievance && GameStateManager.Current.ActiveState is MapState && MobileParty.MainParty.MapEvent == null && PlayerEncounter.Current == null)
				{
					this._currentGrievance = value;
					CampaignMapConversation.OpenConversation(new ConversationCharacterData(Hero.MainHero.CharacterObject, null, false, false, false, false, false, false), new ConversationCharacterData(value.GrievingHero.CharacterObject, null, false, false, false, false, false, false));
					value.HaveGrievance = false;
					value.NextGrievanceTime = CampaignTime.HoursFromNow(100f + (float)MBRandom.RandomInt(100));
					break;
				}
			}
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000E3D34 File Offset: 0x000E1F34
		private void DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType eventType)
		{
			if (this._nextGrievableTimeForComplaintType[(int)eventType].IsFuture)
			{
				return;
			}
			foreach (Hero hero in Hero.MainHero.CompanionsInParty)
			{
				CompanionGrievanceBehavior.Grievance grievance;
				this._heroGrievances.TryGetValue(hero, out grievance);
				if (grievance == null)
				{
					CompanionGrievanceBehavior.GrievanceType grievanceTypeForCompanion = this.GetGrievanceTypeForCompanion(hero, eventType);
					if (grievanceTypeForCompanion != CompanionGrievanceBehavior.GrievanceType.Invalid)
					{
						grievance = new CompanionGrievanceBehavior.Grievance(hero, CampaignTime.Now, grievanceTypeForCompanion);
						this._heroGrievances.Add(hero, grievance);
						this._nextGrievableTimeForComplaintType[(int)eventType] = CampaignTime.HoursFromNow(100f);
						break;
					}
				}
				if (grievance != null && grievance.TypeOfGrievance == eventType && !grievance.HaveGrievance && grievance.NextGrievanceTime.IsPast)
				{
					grievance.HaveGrievance = true;
					this._nextGrievableTimeForComplaintType[(int)eventType] = CampaignTime.HoursFromNow(100f);
					grievance.Count++;
					break;
				}
			}
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000E3E38 File Offset: 0x000E2038
		private CompanionGrievanceBehavior.GrievanceType GetGrievanceTypeForCompanion(Hero companionHero, CompanionGrievanceBehavior.GrievanceType type)
		{
			if ((type == CompanionGrievanceBehavior.GrievanceType.DesertedBattle && companionHero.GetTraitLevel(DefaultTraits.Valor) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.Starvation && companionHero.GetTraitLevel(DefaultTraits.Generosity) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.NoWage && companionHero.GetTraitLevel(DefaultTraits.Generosity) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.VillageRaided && companionHero.GetTraitLevel(DefaultTraits.Mercy) > 0))
			{
				return type;
			}
			return CompanionGrievanceBehavior.GrievanceType.Invalid;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000E3E90 File Offset: 0x000E2090
		private void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.DesertedBattle);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000E3E9C File Offset: 0x000E209C
		private void OnVillageRaided(Village village)
		{
			PartyBase party = village.Settlement.Party;
			MapEvent mapEvent = (party != null) ? party.MapEvent : null;
			if (mapEvent != null)
			{
				using (IEnumerator<PartyBase> enumerator = mapEvent.InvolvedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == PartyBase.MainParty)
						{
							this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.VillageRaided);
						}
					}
				}
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000E3F0C File Offset: 0x000E210C
		private void OnDailyTick()
		{
			if (PartyBase.MainParty.IsStarving)
			{
				this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.Starvation);
			}
			if (MobileParty.MainParty.HasUnpaidWages > 0f)
			{
				this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.NoWage);
			}
			foreach (KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> keyValuePair in this._heroGrievances)
			{
				CompanionGrievanceBehavior.Grievance value = keyValuePair.Value;
				if (value.NextGrievanceTime.ElapsedWeeksUntilNow >= 8f)
				{
					value.HasBeenSettled = false;
					value.Count = 0;
				}
			}
		}

		// Token: 0x04001113 RID: 4371
		private Dictionary<Hero, CompanionGrievanceBehavior.Grievance> _heroGrievances = new Dictionary<Hero, CompanionGrievanceBehavior.Grievance>();

		// Token: 0x04001114 RID: 4372
		private CampaignTime[] _nextGrievableTimeForComplaintType = new CampaignTime[Enum.GetValues(typeof(CompanionGrievanceBehavior.GrievanceType)).Length];

		// Token: 0x04001115 RID: 4373
		private CompanionGrievanceBehavior.Grievance _currentGrievance;

		// Token: 0x04001116 RID: 4374
		private const float _baseGrievanceFrequencyInHours = 100f;

		// Token: 0x04001117 RID: 4375
		private const float _grievanceObsolescenceDurationInWeeks = 8f;

		// Token: 0x020006D0 RID: 1744
		public class CompanionGrievanceBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005706 RID: 22278 RVA: 0x00180481 File Offset: 0x0017E681
			public CompanionGrievanceBehaviorTypeDefiner() : base(80000)
			{
			}

			// Token: 0x06005707 RID: 22279 RVA: 0x0018048E File Offset: 0x0017E68E
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(CompanionGrievanceBehavior.GrievanceType), 1, null);
			}

			// Token: 0x06005708 RID: 22280 RVA: 0x001804A2 File Offset: 0x0017E6A2
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CompanionGrievanceBehavior.Grievance), 10, null);
			}

			// Token: 0x06005709 RID: 22281 RVA: 0x001804B7 File Offset: 0x0017E6B7
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<Hero, CompanionGrievanceBehavior.Grievance>));
			}
		}

		// Token: 0x020006D1 RID: 1745
		internal class Grievance
		{
			// Token: 0x170013AF RID: 5039
			// (get) Token: 0x0600570A RID: 22282 RVA: 0x001804C9 File Offset: 0x0017E6C9
			// (set) Token: 0x0600570B RID: 22283 RVA: 0x001804D1 File Offset: 0x0017E6D1
			[SaveableProperty(4)]
			public bool HaveGrievance { get; set; }

			// Token: 0x0600570C RID: 22284 RVA: 0x001804DA File Offset: 0x0017E6DA
			public Grievance(Hero hero, CampaignTime time, CompanionGrievanceBehavior.GrievanceType type)
			{
				this.GrievingHero = hero;
				this.NextGrievanceTime = time;
				this.TypeOfGrievance = type;
				this.HasBeenSettled = false;
				this.Count = 1;
				this.HaveGrievance = true;
			}

			// Token: 0x0600570D RID: 22285 RVA: 0x0018050C File Offset: 0x0017E70C
			internal static void AutoGeneratedStaticCollectObjectsGrievance(object o, List<object> collectedObjects)
			{
				((CompanionGrievanceBehavior.Grievance)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600570E RID: 22286 RVA: 0x0018051A File Offset: 0x0017E71A
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.GrievingHero);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.NextGrievanceTime, collectedObjects);
			}

			// Token: 0x0600570F RID: 22287 RVA: 0x00180539 File Offset: 0x0017E739
			internal static object AutoGeneratedGetMemberValueHaveGrievance(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).HaveGrievance;
			}

			// Token: 0x06005710 RID: 22288 RVA: 0x0018054B File Offset: 0x0017E74B
			internal static object AutoGeneratedGetMemberValueGrievingHero(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).GrievingHero;
			}

			// Token: 0x06005711 RID: 22289 RVA: 0x00180558 File Offset: 0x0017E758
			internal static object AutoGeneratedGetMemberValueNextGrievanceTime(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).NextGrievanceTime;
			}

			// Token: 0x06005712 RID: 22290 RVA: 0x0018056A File Offset: 0x0017E76A
			internal static object AutoGeneratedGetMemberValueTypeOfGrievance(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).TypeOfGrievance;
			}

			// Token: 0x06005713 RID: 22291 RVA: 0x0018057C File Offset: 0x0017E77C
			internal static object AutoGeneratedGetMemberValueHasBeenSettled(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).HasBeenSettled;
			}

			// Token: 0x06005714 RID: 22292 RVA: 0x0018058E File Offset: 0x0017E78E
			internal static object AutoGeneratedGetMemberValueCount(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).Count;
			}

			// Token: 0x04001C24 RID: 7204
			[SaveableField(1)]
			public Hero GrievingHero;

			// Token: 0x04001C25 RID: 7205
			[SaveableField(2)]
			public CampaignTime NextGrievanceTime;

			// Token: 0x04001C26 RID: 7206
			[SaveableField(3)]
			public CompanionGrievanceBehavior.GrievanceType TypeOfGrievance;

			// Token: 0x04001C28 RID: 7208
			[SaveableField(5)]
			public bool HasBeenSettled;

			// Token: 0x04001C29 RID: 7209
			[SaveableField(6)]
			public int Count;
		}

		// Token: 0x020006D2 RID: 1746
		internal enum GrievanceType
		{
			// Token: 0x04001C2B RID: 7211
			Invalid,
			// Token: 0x04001C2C RID: 7212
			NoWage,
			// Token: 0x04001C2D RID: 7213
			Starvation,
			// Token: 0x04001C2E RID: 7214
			VillageRaided,
			// Token: 0x04001C2F RID: 7215
			DesertedBattle
		}
	}
}
