using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000005 RID: 5
	public class AgentData
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000002B8
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020C0 File Offset: 0x000002C0
		public BasicCharacterObject AgentCharacter { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020C9 File Offset: 0x000002C9
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020D1 File Offset: 0x000002D1
		public Monster AgentMonster { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020DA File Offset: 0x000002DA
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020E2 File Offset: 0x000002E2
		public IBattleCombatant AgentOwnerParty { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020EB File Offset: 0x000002EB
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020F3 File Offset: 0x000002F3
		public Equipment AgentOverridenEquipment { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020FC File Offset: 0x000002FC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002104 File Offset: 0x00000304
		public int AgentEquipmentSeed { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000210D File Offset: 0x0000030D
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002115 File Offset: 0x00000315
		public bool AgentNoHorses { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000211E File Offset: 0x0000031E
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002126 File Offset: 0x00000326
		public string AgentMountKey { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000212F File Offset: 0x0000032F
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002137 File Offset: 0x00000337
		public bool AgentNoWeapons { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002140 File Offset: 0x00000340
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002148 File Offset: 0x00000348
		public bool AgentNoArmor { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002151 File Offset: 0x00000351
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002159 File Offset: 0x00000359
		public bool AgentFixedEquipment { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002162 File Offset: 0x00000362
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000216A File Offset: 0x0000036A
		public bool AgentCivilianEquipment { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002173 File Offset: 0x00000373
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000217B File Offset: 0x0000037B
		public uint AgentClothingColor1 { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002184 File Offset: 0x00000384
		// (set) Token: 0x0600001D RID: 29 RVA: 0x0000218C File Offset: 0x0000038C
		public uint AgentClothingColor2 { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002195 File Offset: 0x00000395
		// (set) Token: 0x0600001F RID: 31 RVA: 0x0000219D File Offset: 0x0000039D
		public bool BodyPropertiesOverriden { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000021A6 File Offset: 0x000003A6
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000021AE File Offset: 0x000003AE
		public BodyProperties AgentBodyProperties { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000021B7 File Offset: 0x000003B7
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000021BF File Offset: 0x000003BF
		public bool AgeOverriden { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000021C8 File Offset: 0x000003C8
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000021D0 File Offset: 0x000003D0
		public int AgentAge { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000021D9 File Offset: 0x000003D9
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000021E1 File Offset: 0x000003E1
		public bool GenderOverriden { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021EA File Offset: 0x000003EA
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000021F2 File Offset: 0x000003F2
		public bool AgentIsFemale { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021FB File Offset: 0x000003FB
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002203 File Offset: 0x00000403
		public int AgentRace { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000220C File Offset: 0x0000040C
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002214 File Offset: 0x00000414
		public IAgentOriginBase AgentOrigin { get; private set; }

		// Token: 0x0600002E RID: 46 RVA: 0x0000221D File Offset: 0x0000041D
		public AgentData(IAgentOriginBase agentOrigin) : this(agentOrigin.Troop)
		{
			this.AgentOrigin = agentOrigin;
			this.AgentCharacter = agentOrigin.Troop;
			this.AgentEquipmentSeed = agentOrigin.Seed;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000224C File Offset: 0x0000044C
		public AgentData(BasicCharacterObject characterObject)
		{
			this.AgentCharacter = characterObject;
			this.AgentRace = characterObject.Race;
			this.AgentMonster = FaceGen.GetBaseMonsterFromRace(this.AgentRace);
			this.AgentOwnerParty = null;
			this.AgentOverridenEquipment = null;
			this.AgentEquipmentSeed = 0;
			this.AgentNoHorses = false;
			this.AgentNoWeapons = false;
			this.AgentNoArmor = false;
			this.AgentFixedEquipment = false;
			this.AgentCivilianEquipment = false;
			this.AgentClothingColor1 = uint.MaxValue;
			this.AgentClothingColor2 = uint.MaxValue;
			this.BodyPropertiesOverriden = false;
			this.GenderOverriden = false;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000022D7 File Offset: 0x000004D7
		public AgentData Character(BasicCharacterObject characterObject)
		{
			this.AgentCharacter = characterObject;
			return this;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000022E1 File Offset: 0x000004E1
		public AgentData Monster(Monster monster)
		{
			this.AgentMonster = monster;
			return this;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000022EB File Offset: 0x000004EB
		public AgentData OwnerParty(IBattleCombatant owner)
		{
			this.AgentOwnerParty = owner;
			return this;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000022F5 File Offset: 0x000004F5
		public AgentData Equipment(Equipment equipment)
		{
			this.AgentOverridenEquipment = equipment;
			return this;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000022FF File Offset: 0x000004FF
		public AgentData EquipmentSeed(int seed)
		{
			this.AgentEquipmentSeed = seed;
			return this;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002309 File Offset: 0x00000509
		public AgentData NoHorses(bool noHorses)
		{
			this.AgentNoHorses = noHorses;
			return this;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002313 File Offset: 0x00000513
		public AgentData NoWeapons(bool noWeapons)
		{
			this.AgentNoWeapons = noWeapons;
			return this;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000231D File Offset: 0x0000051D
		public AgentData NoArmor(bool noArmor)
		{
			this.AgentNoArmor = noArmor;
			return this;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002327 File Offset: 0x00000527
		public AgentData FixedEquipment(bool fixedEquipment)
		{
			this.AgentFixedEquipment = fixedEquipment;
			return this;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002331 File Offset: 0x00000531
		public AgentData CivilianEquipment(bool civilianEquipment)
		{
			this.AgentCivilianEquipment = civilianEquipment;
			return this;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000233B File Offset: 0x0000053B
		public AgentData ClothingColor1(uint color)
		{
			this.AgentClothingColor1 = color;
			return this;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002345 File Offset: 0x00000545
		public AgentData ClothingColor2(uint color)
		{
			this.AgentClothingColor2 = color;
			return this;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000234F File Offset: 0x0000054F
		public AgentData BodyProperties(BodyProperties bodyProperties)
		{
			this.AgentBodyProperties = bodyProperties;
			this.BodyPropertiesOverriden = true;
			return this;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002360 File Offset: 0x00000560
		public AgentData Age(int age)
		{
			this.AgentAge = age;
			this.AgeOverriden = true;
			return this;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002371 File Offset: 0x00000571
		public AgentData TroopOrigin(IAgentOriginBase troopOrigin)
		{
			this.AgentOrigin = troopOrigin;
			if (((troopOrigin != null) ? troopOrigin.Troop : null) != null && !troopOrigin.Troop.IsHero)
			{
				this.EquipmentSeed(troopOrigin.Seed);
			}
			return this;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000023A3 File Offset: 0x000005A3
		public AgentData IsFemale(bool isFemale)
		{
			this.AgentIsFemale = isFemale;
			this.GenderOverriden = true;
			return this;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000023B4 File Offset: 0x000005B4
		public AgentData Race(int race)
		{
			this.AgentRace = race;
			this.GenderOverriden = true;
			return this;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000023C5 File Offset: 0x000005C5
		public AgentData MountKey(string mountKey)
		{
			this.AgentMountKey = mountKey;
			return this;
		}
	}
}
