using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.SaveSystem.Save;

namespace TaleWorlds.Core
{
	// Token: 0x02000062 RID: 98
	[SaveableRootClass(5000)]
	public sealed class Game : IGameStateManagerOwner
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600068D RID: 1677 RVA: 0x00017550 File Offset: 0x00015750
		// (remove) Token: 0x0600068E RID: 1678 RVA: 0x00017584 File Offset: 0x00015784
		public static event Action OnGameCreated;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600068F RID: 1679 RVA: 0x000175B8 File Offset: 0x000157B8
		// (remove) Token: 0x06000690 RID: 1680 RVA: 0x000175F0 File Offset: 0x000157F0
		public event Action<ItemObject> OnItemDeserializedEvent;

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00017625 File Offset: 0x00015825
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0001762D File Offset: 0x0001582D
		public Game.State CurrentState { get; private set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00017636 File Offset: 0x00015836
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001763E File Offset: 0x0001583E
		public IMonsterMissionDataCreator MonsterMissionDataCreator { get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00017648 File Offset: 0x00015848
		public Monster DefaultMonster
		{
			get
			{
				Monster result;
				if ((result = this._defaultMonster) == null)
				{
					result = (this._defaultMonster = this.ObjectManager.GetFirstObject<Monster>());
				}
				return result;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00017673 File Offset: 0x00015873
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0001767B File Offset: 0x0001587B
		[SaveableProperty(3)]
		public GameType GameType { get; private set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x00017684 File Offset: 0x00015884
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x0001768C File Offset: 0x0001588C
		public DefaultSiegeEngineTypes DefaultSiegeEngineTypes { get; private set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00017695 File Offset: 0x00015895
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001769D File Offset: 0x0001589D
		public MBObjectManager ObjectManager { get; private set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x000176A6 File Offset: 0x000158A6
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x000176AE File Offset: 0x000158AE
		[SaveableProperty(8)]
		public BasicCharacterObject PlayerTroop { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000176B7 File Offset: 0x000158B7
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x000176BF File Offset: 0x000158BF
		[SaveableProperty(12)]
		internal MBFastRandom RandomGenerator { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000176C8 File Offset: 0x000158C8
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000176D0 File Offset: 0x000158D0
		public BasicGameModels BasicModels { get; private set; }

		// Token: 0x060006A2 RID: 1698 RVA: 0x000176DC File Offset: 0x000158DC
		public T AddGameModelsManager<T>(IEnumerable<GameModel> inputComponents) where T : GameModelsManager
		{
			T t = (T)((object)Activator.CreateInstance(typeof(T), new object[]
			{
				inputComponents
			}));
			this._gameModelManagers.Add(typeof(T), t);
			return t;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00017724 File Offset: 0x00015924
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0001772C File Offset: 0x0001592C
		public GameManagerBase GameManager { get; private set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00017735 File Offset: 0x00015935
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0001773D File Offset: 0x0001593D
		public GameTextManager GameTextManager { get; private set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00017746 File Offset: 0x00015946
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0001774E File Offset: 0x0001594E
		public GameStateManager GameStateManager { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00017757 File Offset: 0x00015957
		public bool CheatMode
		{
			get
			{
				return this.GameManager.CheatMode;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00017764 File Offset: 0x00015964
		public bool IsDevelopmentMode
		{
			get
			{
				return this.GameManager.IsDevelopmentMode;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00017771 File Offset: 0x00015971
		public bool IsEditModeOn
		{
			get
			{
				return this.GameManager.IsEditModeOn;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001777E File Offset: 0x0001597E
		public UnitSpawnPrioritizations UnitSpawnPrioritization
		{
			get
			{
				return this.GameManager.UnitSpawnPrioritization;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001778B File Offset: 0x0001598B
		public float ApplicationTime
		{
			get
			{
				return this.GameManager.ApplicationTime;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00017798 File Offset: 0x00015998
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0001779F File Offset: 0x0001599F
		public static Game Current
		{
			get
			{
				return Game._current;
			}
			internal set
			{
				Game._current = value;
				Action onGameCreated = Game.OnGameCreated;
				if (onGameCreated == null)
				{
					return;
				}
				onGameCreated();
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000177B6 File Offset: 0x000159B6
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x000177BE File Offset: 0x000159BE
		public IBannerVisualCreator BannerVisualCreator { get; set; }

		// Token: 0x060006B2 RID: 1714 RVA: 0x000177C7 File Offset: 0x000159C7
		public IBannerVisual CreateBannerVisual(Banner banner)
		{
			if (this.BannerVisualCreator == null)
			{
				return null;
			}
			return this.BannerVisualCreator.CreateBannerVisual(banner);
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x000177E0 File Offset: 0x000159E0
		public int NextUniqueTroopSeed
		{
			get
			{
				int nextUniqueTroopSeed = this._nextUniqueTroopSeed;
				this._nextUniqueTroopSeed = nextUniqueTroopSeed + 1;
				return nextUniqueTroopSeed;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000177FE File Offset: 0x000159FE
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00017806 File Offset: 0x00015A06
		public DefaultCharacterAttributes DefaultCharacterAttributes { get; private set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001780F File Offset: 0x00015A0F
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00017817 File Offset: 0x00015A17
		public DefaultSkills DefaultSkills { get; private set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00017820 File Offset: 0x00015A20
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00017828 File Offset: 0x00015A28
		public DefaultBannerEffects DefaultBannerEffects { get; private set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00017831 File Offset: 0x00015A31
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00017839 File Offset: 0x00015A39
		public DefaultItemCategories DefaultItemCategories { get; private set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00017842 File Offset: 0x00015A42
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x0001784A File Offset: 0x00015A4A
		public EventManager EventManager { get; private set; }

		// Token: 0x060006BE RID: 1726 RVA: 0x00017854 File Offset: 0x00015A54
		public Equipment GetDefaultEquipmentWithName(string equipmentName)
		{
			if (!this._defaultEquipments.ContainsKey(equipmentName))
			{
				Debug.FailedAssert("Equipment with name \"" + equipmentName + "\" could not be found.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\Game.cs", "GetDefaultEquipmentWithName", 130);
				return null;
			}
			return this._defaultEquipments[equipmentName].Clone(false);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000178A7 File Offset: 0x00015AA7
		public void SetDefaultEquipments(IReadOnlyDictionary<string, Equipment> defaultEquipments)
		{
			if (this._defaultEquipments == null)
			{
				this._defaultEquipments = defaultEquipments;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000178B8 File Offset: 0x00015AB8
		public static Game CreateGame(GameType gameType, GameManagerBase gameManager, int seed)
		{
			Game game = Game.CreateGame(gameType, gameManager);
			game.RandomGenerator = new MBFastRandom((uint)seed);
			return game;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000178D0 File Offset: 0x00015AD0
		private Game(GameType gameType, GameManagerBase gameManager, MBObjectManager objectManager)
		{
			this.GameType = gameType;
			Game.Current = this;
			this.GameType.CurrentGame = this;
			this.GameManager = gameManager;
			this.GameManager.Game = this;
			this.EventManager = new EventManager();
			this.ObjectManager = objectManager;
			this.RandomGenerator = new MBFastRandom();
			this.InitializeParameters();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001793C File Offset: 0x00015B3C
		public static Game CreateGame(GameType gameType, GameManagerBase gameManager)
		{
			MBObjectManager objectManager = MBObjectManager.Init();
			Game.RegisterTypes(gameType, objectManager);
			return new Game(gameType, gameManager, objectManager);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00017960 File Offset: 0x00015B60
		public static Game LoadSaveGame(LoadResult loadResult, GameManagerBase gameManager)
		{
			MBSaveLoad.OnStartGame(loadResult);
			MBObjectManager objectManager = MBObjectManager.Init();
			Game game = (Game)loadResult.Root;
			Game.RegisterTypes(game.GameType, objectManager);
			loadResult.InitializeObjects();
			MBObjectManager.Instance.ReInitialize();
			GC.Collect();
			game.ObjectManager = objectManager;
			game.BeginLoading(gameManager);
			return game;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000179B3 File Offset: 0x00015BB3
		[LoadInitializationCallback]
		private void OnLoad()
		{
			if (this.RandomGenerator == null)
			{
				this.RandomGenerator = new MBFastRandom();
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000179C8 File Offset: 0x00015BC8
		private void BeginLoading(GameManagerBase gameManager)
		{
			Game.Current = this;
			this.GameType.CurrentGame = this;
			this.GameManager = gameManager;
			this.GameManager.Game = this;
			this.EventManager = new EventManager();
			this.InitializeParameters();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00017A00 File Offset: 0x00015C00
		private void SaveAux(MetaData metaData, string saveName, ISaveDriver driver, Action<SaveResult> onSaveCompleted)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnBeforeSave();
			}
			SaveOutput saveOutput = SaveManager.Save(this, metaData, saveName, driver);
			if (!saveOutput.IsContinuing)
			{
				this.OnSaveCompleted(saveOutput, onSaveCompleted);
				return;
			}
			this._currentActiveSaveData = new Tuple<SaveOutput, Action<SaveResult>>(saveOutput, onSaveCompleted);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00017A80 File Offset: 0x00015C80
		private void OnSaveCompleted(SaveOutput finishedOutput, Action<SaveResult> onSaveCompleted)
		{
			finishedOutput.PrintStatus();
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnAfterSave();
			}
			Common.MemoryCleanupGC(false);
			if (onSaveCompleted != null)
			{
				onSaveCompleted(finishedOutput.Result);
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00017AF0 File Offset: 0x00015CF0
		public void Save(MetaData metaData, string saveName, ISaveDriver driver, Action<SaveResult> onSaveCompleted)
		{
			using (new PerformanceTestBlock("Save Process"))
			{
				this.SaveAux(metaData, saveName, driver, onSaveCompleted);
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00017B30 File Offset: 0x00015D30
		private void InitializeParameters()
		{
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
			this.GameType.InitializeParameters();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00017B56 File Offset: 0x00015D56
		void IGameStateManagerOwner.OnStateStackEmpty()
		{
			this.Destroy();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00017B60 File Offset: 0x00015D60
		public void Destroy()
		{
			this.CurrentState = Game.State.Destroying;
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameEnd();
			}
			this.GameManager.OnGameEnd(this);
			this.GameType.OnDestroy();
			this.ObjectManager.Destroy();
			this.EventManager.Clear();
			this.EventManager = null;
			GameStateManager.Current = null;
			this.GameStateManager = null;
			Game.Current = null;
			this.CurrentState = Game.State.Destroyed;
			this._currentActiveSaveData = null;
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00017C18 File Offset: 0x00015E18
		public void CreateGameManager()
		{
			this.GameStateManager = new GameStateManager(this, GameStateManager.GameStateManagerType.Game);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00017C27 File Offset: 0x00015E27
		public void OnStateChanged(GameState oldState)
		{
			this.GameType.OnStateChanged(oldState);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00017C35 File Offset: 0x00015E35
		public T AddGameHandler<T>() where T : GameHandler, new()
		{
			return this._gameEntitySystem.AddComponent<T>();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00017C42 File Offset: 0x00015E42
		public T GetGameHandler<T>() where T : GameHandler
		{
			return this._gameEntitySystem.GetComponent<T>();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00017C4F File Offset: 0x00015E4F
		public void RemoveGameHandler<T>() where T : GameHandler
		{
			this._gameEntitySystem.RemoveComponent<T>();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00017C5C File Offset: 0x00015E5C
		public void Initialize()
		{
			if (this._gameEntitySystem == null)
			{
				this._gameEntitySystem = new EntitySystem<GameHandler>();
			}
			this.GameTextManager = new GameTextManager();
			this.GameTextManager.LoadGameTexts();
			this._gameModelManagers = new Dictionary<Type, GameModelsManager>();
			GameTexts.Initialize(this.GameTextManager);
			this.GameType.OnInitialize();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00017CB4 File Offset: 0x00015EB4
		public static void RegisterTypes(GameType gameType, MBObjectManager objectManager)
		{
			if (gameType != null)
			{
				gameType.BeforeRegisterTypes(objectManager);
			}
			objectManager.RegisterType<Monster>("Monster", "Monsters", 2U, true, false);
			objectManager.RegisterType<SkeletonScale>("Scale", "Scales", 3U, true, false);
			objectManager.RegisterType<ItemObject>("Item", "Items", 4U, true, false);
			objectManager.RegisterType<ItemModifier>("ItemModifier", "ItemModifiers", 6U, true, false);
			objectManager.RegisterType<ItemModifierGroup>("ItemModifierGroup", "ItemModifierGroups", 7U, true, false);
			objectManager.RegisterType<CharacterAttribute>("CharacterAttribute", "CharacterAttributes", 8U, true, false);
			objectManager.RegisterType<SkillObject>("Skill", "Skills", 9U, true, false);
			objectManager.RegisterType<ItemCategory>("ItemCategory", "ItemCategories", 10U, true, false);
			objectManager.RegisterType<CraftingPiece>("CraftingPiece", "CraftingPieces", 11U, true, false);
			objectManager.RegisterType<CraftingTemplate>("CraftingTemplate", "CraftingTemplates", 12U, true, false);
			objectManager.RegisterType<SiegeEngineType>("SiegeEngineType", "SiegeEngineTypes", 13U, true, false);
			objectManager.RegisterType<WeaponDescription>("WeaponDescription", "WeaponDescriptions", 14U, true, false);
			objectManager.RegisterType<MBBodyProperty>("BodyProperty", "BodyProperties", 50U, true, false);
			objectManager.RegisterType<MBEquipmentRoster>("EquipmentRoster", "EquipmentRosters", 51U, true, false);
			objectManager.RegisterType<MBCharacterSkills>("SkillSet", "SkillSets", 52U, true, false);
			objectManager.RegisterType<BannerEffect>("BannerEffect", "BannerEffects", 53U, true, false);
			if (gameType != null)
			{
				gameType.OnRegisterTypes(objectManager);
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00017E0F File Offset: 0x0001600F
		public void SetBasicModels(IEnumerable<GameModel> models)
		{
			this.BasicModels = this.AddGameModelsManager<BasicGameModels>(models);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00017E20 File Offset: 0x00016020
		internal void OnTick(float dt)
		{
			if (GameStateManager.Current == this.GameStateManager)
			{
				this.GameStateManager.OnTick(dt);
				if (this._gameEntitySystem != null)
				{
					foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
					{
						try
						{
							gameHandler.OnTick(dt);
						}
						catch (Exception arg)
						{
							Debug.Print("Exception on gameHandler tick: " + arg, 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
				}
			}
			Action<float> afterTick = this.AfterTick;
			if (afterTick != null)
			{
				afterTick(dt);
			}
			Tuple<SaveOutput, Action<SaveResult>> currentActiveSaveData = this._currentActiveSaveData;
			if (currentActiveSaveData != null && !currentActiveSaveData.Item1.IsContinuing)
			{
				this.OnSaveCompleted(this._currentActiveSaveData.Item1, this._currentActiveSaveData.Item2);
				this._currentActiveSaveData = null;
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00017F18 File Offset: 0x00016118
		internal void OnGameNetworkBegin()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameNetworkBegin();
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00017F70 File Offset: 0x00016170
		internal void OnGameNetworkEnd()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameNetworkEnd();
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00017FC8 File Offset: 0x000161C8
		internal void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnEarlyPlayerConnect(peer);
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00018020 File Offset: 0x00016220
		internal void OnPlayerConnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnPlayerConnect(peer);
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00018078 File Offset: 0x00016278
		internal void OnPlayerDisconnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnPlayerDisconnect(peer);
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000180D0 File Offset: 0x000162D0
		public void OnGameStart()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameStart();
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00018128 File Offset: 0x00016328
		public bool DoLoading()
		{
			return this.GameType.DoLoadingForGameType();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00018135 File Offset: 0x00016335
		public void OnMissionIsStarting(string missionName, MissionInitializerRecord rec)
		{
			this.GameType.OnMissionIsStarting(missionName, rec);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00018144 File Offset: 0x00016344
		public void OnFinalize()
		{
			this.CurrentState = Game.State.Destroying;
			GameStateManager.Current.CleanStates(0);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00018158 File Offset: 0x00016358
		public void InitializeDefaultGameObjects()
		{
			this.DefaultCharacterAttributes = new DefaultCharacterAttributes();
			this.DefaultSkills = new DefaultSkills();
			this.DefaultBannerEffects = new DefaultBannerEffects();
			this.DefaultItemCategories = new DefaultItemCategories();
			this.DefaultSiegeEngineTypes = new DefaultSiegeEngineTypes();
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00018194 File Offset: 0x00016394
		public void LoadBasicFiles()
		{
			this.ObjectManager.LoadXML("Monsters", false);
			this.ObjectManager.LoadXML("SkeletonScales", false);
			this.ObjectManager.LoadXML("ItemModifiers", false);
			this.ObjectManager.LoadXML("ItemModifierGroups", false);
			this.ObjectManager.LoadXML("CraftingPieces", false);
			this.ObjectManager.LoadXML("WeaponDescriptions", false);
			this.ObjectManager.LoadXML("CraftingTemplates", false);
			this.ObjectManager.LoadXML("BodyProperties", false);
			this.ObjectManager.LoadXML("SkillSets", false);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001823A File Offset: 0x0001643A
		public void ItemObjectDeserialized(ItemObject itemObject)
		{
			Action<ItemObject> onItemDeserializedEvent = this.OnItemDeserializedEvent;
			if (onItemDeserializedEvent == null)
			{
				return;
			}
			onItemDeserializedEvent(itemObject);
		}

		// Token: 0x04000385 RID: 901
		public Action<float> AfterTick;

		// Token: 0x04000388 RID: 904
		private EntitySystem<GameHandler> _gameEntitySystem;

		// Token: 0x04000389 RID: 905
		private Monster _defaultMonster;

		// Token: 0x04000390 RID: 912
		private Dictionary<Type, GameModelsManager> _gameModelManagers;

		// Token: 0x04000394 RID: 916
		private static Game _current;

		// Token: 0x04000396 RID: 918
		[SaveableField(11)]
		private int _nextUniqueTroopSeed = 1;

		// Token: 0x0400039C RID: 924
		private IReadOnlyDictionary<string, Equipment> _defaultEquipments;

		// Token: 0x0400039D RID: 925
		private Tuple<SaveOutput, Action<SaveResult>> _currentActiveSaveData;

		// Token: 0x020000F6 RID: 246
		public enum State
		{
			// Token: 0x040006C6 RID: 1734
			Running,
			// Token: 0x040006C7 RID: 1735
			Destroying,
			// Token: 0x040006C8 RID: 1736
			Destroyed
		}
	}
}
