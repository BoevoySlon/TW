using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x0200034F RID: 847
	public class DefaultTraits
	{
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000CD119 File Offset: 0x000CB319
		private static DefaultTraits Instance
		{
			get
			{
				return Campaign.Current.DefaultTraits;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x000CD125 File Offset: 0x000CB325
		public static TraitObject Frequency
		{
			get
			{
				return DefaultTraits.Instance._traitFrequency;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x000CD131 File Offset: 0x000CB331
		public static TraitObject Mercy
		{
			get
			{
				return DefaultTraits.Instance._traitMercy;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000CD13D File Offset: 0x000CB33D
		public static TraitObject Valor
		{
			get
			{
				return DefaultTraits.Instance._traitValor;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x000CD149 File Offset: 0x000CB349
		public static TraitObject Honor
		{
			get
			{
				return DefaultTraits.Instance._traitHonor;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003001 RID: 12289 RVA: 0x000CD155 File Offset: 0x000CB355
		public static TraitObject Generosity
		{
			get
			{
				return DefaultTraits.Instance._traitGenerosity;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x000CD161 File Offset: 0x000CB361
		public static TraitObject Calculating
		{
			get
			{
				return DefaultTraits.Instance._traitCalculating;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x000CD16D File Offset: 0x000CB36D
		public static TraitObject PersonaCurt
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaCurt;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x000CD179 File Offset: 0x000CB379
		public static TraitObject PersonaEarnest
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaEarnest;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x000CD185 File Offset: 0x000CB385
		public static TraitObject PersonaIronic
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaIronic;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000CD191 File Offset: 0x000CB391
		public static TraitObject PersonaSoftspoken
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaSoftspoken;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x000CD19D File Offset: 0x000CB39D
		public static TraitObject Surgery
		{
			get
			{
				return DefaultTraits.Instance._traitSurgery;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x000CD1A9 File Offset: 0x000CB3A9
		public static TraitObject SergeantCommandSkills
		{
			get
			{
				return DefaultTraits.Instance._traitSergeantCommandSkills;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x000CD1B5 File Offset: 0x000CB3B5
		public static TraitObject RogueSkills
		{
			get
			{
				return DefaultTraits.Instance._traitRogueSkills;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x000CD1C1 File Offset: 0x000CB3C1
		public static TraitObject Siegecraft
		{
			get
			{
				return DefaultTraits.Instance._traitEngineerSkills;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000CD1CD File Offset: 0x000CB3CD
		public static TraitObject ScoutSkills
		{
			get
			{
				return DefaultTraits.Instance._traitScoutSkills;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x000CD1D9 File Offset: 0x000CB3D9
		public static TraitObject Blacksmith
		{
			get
			{
				return DefaultTraits.Instance._traitBlacksmith;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x000CD1E5 File Offset: 0x000CB3E5
		public static TraitObject Fighter
		{
			get
			{
				return DefaultTraits.Instance._traitFighter;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000CD1F1 File Offset: 0x000CB3F1
		public static TraitObject Commander
		{
			get
			{
				return DefaultTraits.Instance._traitCommander;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x000CD1FD File Offset: 0x000CB3FD
		public static TraitObject Politician
		{
			get
			{
				return DefaultTraits.Instance._traitPolitician;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x000CD209 File Offset: 0x000CB409
		public static TraitObject Manager
		{
			get
			{
				return DefaultTraits.Instance._traitManager;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000CD215 File Offset: 0x000CB415
		public static TraitObject Trader
		{
			get
			{
				return DefaultTraits.Instance._traitTraderSkills;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000CD221 File Offset: 0x000CB421
		public static TraitObject KnightFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitKnightFightingSkills;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x000CD22D File Offset: 0x000CB42D
		public static TraitObject CavalryFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitCavalryFightingSkills;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000CD239 File Offset: 0x000CB439
		public static TraitObject HorseArcherFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitHorseArcherFightingSkills;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x000CD245 File Offset: 0x000CB445
		public static TraitObject HopliteFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitHopliteFightingSkills;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x000CD251 File Offset: 0x000CB451
		public static TraitObject ArcherFIghtingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitArcherFIghtingSkills;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x000CD25D File Offset: 0x000CB45D
		public static TraitObject CrossbowmanStyle
		{
			get
			{
				return DefaultTraits.Instance._traitCrossbowmanStyle;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x000CD269 File Offset: 0x000CB469
		public static TraitObject PeltastFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitPeltastFightingSkills;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000CD275 File Offset: 0x000CB475
		public static TraitObject HuscarlFightingSkills
		{
			get
			{
				return DefaultTraits.Instance._traitHuscarlFightingSkills;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x000CD281 File Offset: 0x000CB481
		public static TraitObject WandererEquipment
		{
			get
			{
				return DefaultTraits.Instance._traitWandererEquipment;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x000CD28D File Offset: 0x000CB48D
		public static TraitObject GentryEquipment
		{
			get
			{
				return DefaultTraits.Instance._traitGentryEquipment;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000CD299 File Offset: 0x000CB499
		public static TraitObject RomanHair
		{
			get
			{
				return DefaultTraits.Instance._traitRomanHair;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000CD2A5 File Offset: 0x000CB4A5
		public static TraitObject FrankishHair
		{
			get
			{
				return DefaultTraits.Instance._traitFrankishHair;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000CD2B1 File Offset: 0x000CB4B1
		public static TraitObject CelticHair
		{
			get
			{
				return DefaultTraits.Instance._traitCelticHair;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000CD2BD File Offset: 0x000CB4BD
		public static TraitObject RusHair
		{
			get
			{
				return DefaultTraits.Instance._traitRusHair;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000CD2C9 File Offset: 0x000CB4C9
		public static TraitObject ArabianHair
		{
			get
			{
				return DefaultTraits.Instance._traitArabianHair;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000CD2D5 File Offset: 0x000CB4D5
		public static TraitObject SteppeHair
		{
			get
			{
				return DefaultTraits.Instance._traitSteppeHair;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000CD2E1 File Offset: 0x000CB4E1
		public static TraitObject Thug
		{
			get
			{
				return DefaultTraits.Instance._traitThug;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000CD2ED File Offset: 0x000CB4ED
		public static TraitObject Smuggler
		{
			get
			{
				return DefaultTraits.Instance._traitSmuggler;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x000CD2F9 File Offset: 0x000CB4F9
		public static TraitObject Egalitarian
		{
			get
			{
				return DefaultTraits.Instance._traitEgalitarian;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000CD305 File Offset: 0x000CB505
		public static TraitObject Oligarchic
		{
			get
			{
				return DefaultTraits.Instance._traitOligarchic;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000CD311 File Offset: 0x000CB511
		public static TraitObject Authoritarian
		{
			get
			{
				return DefaultTraits.Instance._traitAuthoritarian;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000CD31D File Offset: 0x000CB51D
		public static IEnumerable<TraitObject> Personality
		{
			get
			{
				return DefaultTraits.Instance._personality;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x000CD329 File Offset: 0x000CB529
		public static IEnumerable<TraitObject> SkillCategories
		{
			get
			{
				return DefaultTraits.Instance._skillCategories;
			}
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000CD338 File Offset: 0x000CB538
		public DefaultTraits()
		{
			this.RegisterAll();
			this._personality = new TraitObject[]
			{
				this._traitMercy,
				this._traitValor,
				this._traitHonor,
				this._traitGenerosity,
				this._traitCalculating
			};
			this._skillCategories = new TraitObject[]
			{
				this._traitCommander,
				this._traitFighter,
				this._traitPolitician,
				this._traitManager
			};
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000CD3BC File Offset: 0x000CB5BC
		public void RegisterAll()
		{
			this._traitFrequency = this.Create("Frequency");
			this._traitMercy = this.Create("Mercy");
			this._traitValor = this.Create("Valor");
			this._traitHonor = this.Create("Honor");
			this._traitGenerosity = this.Create("Generosity");
			this._traitCalculating = this.Create("Calculating");
			this._traitPersonaCurt = this.Create("curt");
			this._traitPersonaIronic = this.Create("ironic");
			this._traitPersonaEarnest = this.Create("earnest");
			this._traitPersonaSoftspoken = this.Create("softspoken");
			this._traitCommander = this.Create("Commander");
			this._traitFighter = this.Create("BalancedFightingSkills");
			this._traitPolitician = this.Create("Politician");
			this._traitManager = this.Create("Manager");
			this._traitTraderSkills = this.Create("Trader");
			this._traitSurgery = this.Create("Surgeon");
			this._traitTracking = this.Create("Tracking");
			this._traitBlacksmith = this.Create("Blacksmith");
			this._traitSergeantCommandSkills = this.Create("SergeantCommandSkills");
			this._traitEngineerSkills = this.Create("EngineerSkills");
			this._traitRogueSkills = this.Create("RogueSkills");
			this._traitScoutSkills = this.Create("ScoutSkills");
			this._traitKnightFightingSkills = this.Create("KnightFightingSkills");
			this._traitCavalryFightingSkills = this.Create("CavalryFightingSkills");
			this._traitHorseArcherFightingSkills = this.Create("HorseArcherFightingSkills");
			this._traitHopliteFightingSkills = this.Create("HopliteFightingSkills");
			this._traitArcherFIghtingSkills = this.Create("ArcherFIghtingSkills");
			this._traitCrossbowmanStyle = this.Create("CrossbowmanStyle");
			this._traitPeltastFightingSkills = this.Create("PeltastFightingSkills");
			this._traitHuscarlFightingSkills = this.Create("HuscarlFightingSkills");
			this._traitScarred = this.Create("Scarred");
			this._traitRomanHair = this.Create("RomanHair");
			this._traitCelticHair = this.Create("CelticHair");
			this._traitArabianHair = this.Create("ArabianHair");
			this._traitRusHair = this.Create("RusHair");
			this._traitFrankishHair = this.Create("FrankishHair");
			this._traitSteppeHair = this.Create("SteppeHair");
			this._traitWandererEquipment = this.Create("WandererEquipment");
			this._traitGentryEquipment = this.Create("GentryEquipment");
			this._traitThug = this.Create("Thug");
			this._traitSmuggler = this.Create("Smuggler");
			this._traitEgalitarian = this.Create("Egalitarian");
			this._traitOligarchic = this.Create("Oligarchic");
			this._traitAuthoritarian = this.Create("Authoritarian");
			this.InitializeAll();
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000CD6BB File Offset: 0x000CB8BB
		private TraitObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<TraitObject>(new TraitObject(stringId));
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000CD6D4 File Offset: 0x000CB8D4
		private void InitializeAll()
		{
			this._traitFrequency.Initialize(new TextObject("{=vsoyhPnl}Frequency", null), new TextObject("{=!}Frequency Description", null), true, 0, 20);
			this._traitMercy.Initialize(new TextObject("{=2I2uKJlw}Mercy", null), new TextObject("{=Au7VCWTa}Mercy represents your general aversion to suffering and your willingness to help strangers or even enemies.", null), false, -2, 2);
			this._traitValor.Initialize(new TextObject("{=toQLHG6x}Valor", null), new TextObject("{=Ugm9nO49}Valor represents your reputation for risking your life to win glory or wealth or advance your cause.", null), false, -2, 2);
			this._traitHonor.Initialize(new TextObject("{=0oGz5rVx}Honor", null), new TextObject("{=1vYgkaaK}Honor represents your reputation for respecting your formal commitments, like keeping your word and obeying the law.", null), false, -2, 2);
			this._traitGenerosity.Initialize(new TextObject("{=IuWu5Bu7}Generosity", null), new TextObject("{=IKzqzPDS}Generosity represents your loyalty to your kin and those who serve you, and your gratitude to those who have done you a favor.", null), false, -2, 2);
			this._traitCalculating.Initialize(new TextObject("{=5sMBbn7y}Calculating", null), new TextObject("{=QKjF5gTR}Calculating represents your ability to control your emotions for the sake of your long-term interests.", null), false, -2, 2);
			this._traitPersonaCurt.Initialize(new TextObject("{=!}PersonaCurt", null), new TextObject("{=!}PersonaCurt Description", null), false, -2, 2);
			this._traitPersonaIronic.Initialize(new TextObject("{=!}PersonaIronic", null), new TextObject("{=!}PersonaIronic Description", null), false, -2, 2);
			this._traitPersonaEarnest.Initialize(new TextObject("{=!}PersonaEarnest", null), new TextObject("{=!}PersonaEarnest Description", null), false, -2, 2);
			this._traitPersonaSoftspoken.Initialize(new TextObject("{=!}PersonaSoftspoken", null), new TextObject("{=!}PersonaSoftspoken Description", null), false, -2, 2);
			this._traitCommander.Initialize(new TextObject("{=RvKwdXWs}Commander", null), new TextObject("{=!}Commander Description", null), true, 0, 20);
			this._traitFighter.Initialize(new TextObject("{=!}BalancedFightingSkills", null), new TextObject("{=!}BalancedFightingSkills Description", null), true, 0, 20);
			this._traitPolitician.Initialize(new TextObject("{=4Ybbhzjw}Politician", null), new TextObject("{=!}Politician Description", null), true, 0, 20);
			this._traitManager.Initialize(new TextObject("{=6RYVOb0c}Manager", null), new TextObject("{=!}Manager Description", null), true, 0, 20);
			this._traitSurgery.Initialize(new TextObject("{=QBPrRdQJ}Surgeon", null), new TextObject("{=!}Surgeon Description", null), true, 0, 20);
			this._traitTracking.Initialize(new TextObject("{=dx0hmeH6}Tracking", null), new TextObject("{=!}Tracking Description", null), true, 0, 20);
			this._traitBlacksmith.Initialize(new TextObject("{=bNnQt4jN}Blacksmith", null), new TextObject("{=!}Blacksmith Description", null), true, 0, 20);
			this._traitSergeantCommandSkills.Initialize(new TextObject("{=!}SergeantCommandSkills", null), new TextObject("{=!}SergeantCommandSkills Description", null), true, 0, 20);
			this._traitEngineerSkills.Initialize(new TextObject("{=!}EngineerSkills", null), new TextObject("{=!}EngineerSkills Description", null), true, 0, 20);
			this._traitRogueSkills.Initialize(new TextObject("{=!}RogueSkills", null), new TextObject("{=!}RogueSkills Description", null), true, 0, 20);
			this._traitScoutSkills.Initialize(new TextObject("{=!}ScoutSkills", null), new TextObject("{=!}ScoutSkills Description", null), true, 0, 20);
			this._traitTraderSkills.Initialize(new TextObject("{=!}TraderSkills", null), new TextObject("{=!}Trader Description", null), true, 0, 20);
			this._traitKnightFightingSkills.Initialize(new TextObject("{=!}KnightFightingSkills", null), new TextObject("{=!}KnightFightingSkills Description", null), true, 0, 20);
			this._traitCavalryFightingSkills.Initialize(new TextObject("{=!}CavalryFightingSkills", null), new TextObject("{=!}CavalryFightingSkills Description", null), true, 0, 20);
			this._traitHorseArcherFightingSkills.Initialize(new TextObject("{=!}HorseArcherFightingSkills", null), new TextObject("{=!}HorseArcherFightingSkills Description", null), true, 0, 20);
			this._traitHopliteFightingSkills.Initialize(new TextObject("{=!}HopliteFightingSkills", null), new TextObject("{=!}HopliteFightingSkills Description", null), true, 0, 20);
			this._traitArcherFIghtingSkills.Initialize(new TextObject("{=!}ArcherFIghtingSkills", null), new TextObject("{=!}ArcherFIghtingSkills Description", null), true, 0, 20);
			this._traitCrossbowmanStyle.Initialize(new TextObject("{=!}CrossbowmanStyle", null), new TextObject("{=!}CrossbowmanStyle Description", null), true, 0, 20);
			this._traitPeltastFightingSkills.Initialize(new TextObject("{=!}PeltastFightingSkills", null), new TextObject("{=!}PeltastFightingSkills Description", null), true, 0, 20);
			this._traitHuscarlFightingSkills.Initialize(new TextObject("{=!}HuscarlFightingSkills", null), new TextObject("{=!}HuscarlFightingSkills Description", null), true, 0, 20);
			this._traitScarred.Initialize(new TextObject("{=!}Scarred", null), new TextObject("{=!}Scarred Description", null), true, 0, 20);
			this._traitRomanHair.Initialize(new TextObject("{=!}RomanHair", null), new TextObject("{=!}RomanHair Description", null), true, 0, 20);
			this._traitCelticHair.Initialize(new TextObject("{=!}CelticHair", null), new TextObject("{=!}CelticHair Description", null), true, 0, 20);
			this._traitArabianHair.Initialize(new TextObject("{=!}ArabianHair", null), new TextObject("{=!}ArabianHair Description", null), true, 0, 20);
			this._traitRusHair.Initialize(new TextObject("{=!}RusHair", null), new TextObject("{=!}RusHair Description", null), true, 0, 20);
			this._traitFrankishHair.Initialize(new TextObject("{=!}FrankishHair", null), new TextObject("{=!}FrankishHair Description", null), true, 0, 20);
			this._traitSteppeHair.Initialize(new TextObject("{=!}SteppeHair", null), new TextObject("{=!}SteppeHair Description", null), true, 0, 20);
			this._traitWandererEquipment.Initialize(new TextObject("{=!}WandererEquipment", null), new TextObject("{=!}WandererEquipment Description", null), true, 0, 20);
			this._traitGentryEquipment.Initialize(new TextObject("{=!}GentryEquipment", null), new TextObject("{=!}GentryEquipment Description", null), true, 0, 20);
			this._traitThug.Initialize(new TextObject("{=thugtrait}Thug", null), new TextObject("{=Fjnw9ooa}Indicates a gang member specialized in extortion", null), true, 0, 20);
			this._traitSmuggler.Initialize(new TextObject("{=eeWx1yYd}Smuggler", null), new TextObject("{=87c7IhkZ}Indicates a gang member specialized in smuggling", null), true, 0, 20);
			this._traitEgalitarian.Initialize(new TextObject("{=HMFb1gaq}Egalitarian", null), new TextObject("{=!}Egalitarian Description", null), false, 0, 20);
			this._traitOligarchic.Initialize(new TextObject("{=hR6Zo6pD}Oligarchic", null), new TextObject("{=!}Oligarchic Description", null), false, 0, 20);
			this._traitAuthoritarian.Initialize(new TextObject("{=NaMPa4ML}Authoritarian", null), new TextObject("{=!}Authoritarian Description", null), false, 0, 20);
		}

		// Token: 0x04000FA7 RID: 4007
		private const int MaxPersonalityTraitValue = 2;

		// Token: 0x04000FA8 RID: 4008
		private const int MinPersonalityTraitValue = -2;

		// Token: 0x04000FA9 RID: 4009
		private const int MaxHiddenTraitValue = 20;

		// Token: 0x04000FAA RID: 4010
		private const int MinHiddenTraitValue = 0;

		// Token: 0x04000FAB RID: 4011
		private TraitObject _traitMercy;

		// Token: 0x04000FAC RID: 4012
		private TraitObject _traitValor;

		// Token: 0x04000FAD RID: 4013
		private TraitObject _traitHonor;

		// Token: 0x04000FAE RID: 4014
		private TraitObject _traitGenerosity;

		// Token: 0x04000FAF RID: 4015
		private TraitObject _traitCalculating;

		// Token: 0x04000FB0 RID: 4016
		private TraitObject _traitPersonaCurt;

		// Token: 0x04000FB1 RID: 4017
		private TraitObject _traitPersonaEarnest;

		// Token: 0x04000FB2 RID: 4018
		private TraitObject _traitPersonaIronic;

		// Token: 0x04000FB3 RID: 4019
		private TraitObject _traitPersonaSoftspoken;

		// Token: 0x04000FB4 RID: 4020
		private TraitObject _traitEgalitarian;

		// Token: 0x04000FB5 RID: 4021
		private TraitObject _traitOligarchic;

		// Token: 0x04000FB6 RID: 4022
		private TraitObject _traitAuthoritarian;

		// Token: 0x04000FB7 RID: 4023
		private TraitObject _traitSurgery;

		// Token: 0x04000FB8 RID: 4024
		private TraitObject _traitTracking;

		// Token: 0x04000FB9 RID: 4025
		private TraitObject _traitSergeantCommandSkills;

		// Token: 0x04000FBA RID: 4026
		private TraitObject _traitRogueSkills;

		// Token: 0x04000FBB RID: 4027
		private TraitObject _traitEngineerSkills;

		// Token: 0x04000FBC RID: 4028
		private TraitObject _traitBlacksmith;

		// Token: 0x04000FBD RID: 4029
		private TraitObject _traitScoutSkills;

		// Token: 0x04000FBE RID: 4030
		private TraitObject _traitTraderSkills;

		// Token: 0x04000FBF RID: 4031
		private TraitObject _traitScarred;

		// Token: 0x04000FC0 RID: 4032
		private TraitObject _traitRomanHair;

		// Token: 0x04000FC1 RID: 4033
		private TraitObject _traitCelticHair;

		// Token: 0x04000FC2 RID: 4034
		private TraitObject _traitRusHair;

		// Token: 0x04000FC3 RID: 4035
		private TraitObject _traitArabianHair;

		// Token: 0x04000FC4 RID: 4036
		private TraitObject _traitFrankishHair;

		// Token: 0x04000FC5 RID: 4037
		private TraitObject _traitSteppeHair;

		// Token: 0x04000FC6 RID: 4038
		private TraitObject _traitFrequency;

		// Token: 0x04000FC7 RID: 4039
		private TraitObject _traitCommander;

		// Token: 0x04000FC8 RID: 4040
		private TraitObject _traitFighter;

		// Token: 0x04000FC9 RID: 4041
		private TraitObject _traitPolitician;

		// Token: 0x04000FCA RID: 4042
		private TraitObject _traitManager;

		// Token: 0x04000FCB RID: 4043
		private TraitObject _traitKnightFightingSkills;

		// Token: 0x04000FCC RID: 4044
		private TraitObject _traitCavalryFightingSkills;

		// Token: 0x04000FCD RID: 4045
		private TraitObject _traitHorseArcherFightingSkills;

		// Token: 0x04000FCE RID: 4046
		private TraitObject _traitArcherFIghtingSkills;

		// Token: 0x04000FCF RID: 4047
		private TraitObject _traitCrossbowmanStyle;

		// Token: 0x04000FD0 RID: 4048
		private TraitObject _traitPeltastFightingSkills;

		// Token: 0x04000FD1 RID: 4049
		private TraitObject _traitHopliteFightingSkills;

		// Token: 0x04000FD2 RID: 4050
		private TraitObject _traitHuscarlFightingSkills;

		// Token: 0x04000FD3 RID: 4051
		private TraitObject _traitWandererEquipment;

		// Token: 0x04000FD4 RID: 4052
		private TraitObject _traitGentryEquipment;

		// Token: 0x04000FD5 RID: 4053
		private TraitObject _traitThug;

		// Token: 0x04000FD6 RID: 4054
		private TraitObject _traitSmuggler;

		// Token: 0x04000FD7 RID: 4055
		private readonly TraitObject[] _personality;

		// Token: 0x04000FD8 RID: 4056
		private readonly TraitObject[] _skillCategories;
	}
}
