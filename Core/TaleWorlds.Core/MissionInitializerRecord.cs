using System;
using System.Runtime.InteropServices;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000AE RID: 174
	public struct MissionInitializerRecord : ISerializableObject
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x0001CE6C File Offset: 0x0001B06C
		public MissionInitializerRecord(string name)
		{
			this.TerrainType = -1;
			this.DamageToPlayerMultiplier = 1f;
			this.DamageToFriendsMultiplier = 1f;
			this.DamageFromPlayerToFriendsMultiplier = 1f;
			this.TimeOfDay = 6f;
			this.NeedsRandomTerrain = false;
			this.RandomTerrainSeed = 0;
			this.SceneName = name;
			this.SceneLevels = "";
			this.PlayingInCampaignMode = false;
			this.EnableSceneRecording = false;
			this.SceneUpgradeLevel = 0;
			this.SceneHasMapPatch = false;
			this.PatchCoordinates = Vec2.Zero;
			this.PatchEncounterDir = Vec2.Zero;
			this.DoNotUseLoadingScreen = false;
			this.DisableDynamicPointlightShadows = false;
			this.DecalAtlasGroup = 0;
			this.AtmosphereOnCampaign = AtmosphereInfo.GetInvalidAtmosphereInfo();
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001CF20 File Offset: 0x0001B120
		void ISerializableObject.DeserializeFrom(IReader reader)
		{
			this.SceneName = reader.ReadString();
			this.SceneLevels = reader.ReadString();
			this.TimeOfDay = reader.ReadFloat();
			this.NeedsRandomTerrain = reader.ReadBool();
			this.RandomTerrainSeed = reader.ReadInt();
			this.EnableSceneRecording = reader.ReadBool();
			this.SceneUpgradeLevel = reader.ReadInt();
			this.PlayingInCampaignMode = reader.ReadBool();
			this.DisableDynamicPointlightShadows = reader.ReadBool();
			this.DoNotUseLoadingScreen = reader.ReadBool();
			if (reader.ReadBool())
			{
				this.AtmosphereOnCampaign = AtmosphereInfo.GetInvalidAtmosphereInfo();
				this.AtmosphereOnCampaign.DeserializeFrom(reader);
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
		void ISerializableObject.SerializeTo(IWriter writer)
		{
			writer.WriteString(this.SceneName);
			writer.WriteString(this.SceneLevels);
			writer.WriteFloat(this.TimeOfDay);
			writer.WriteBool(this.NeedsRandomTerrain);
			writer.WriteInt(this.RandomTerrainSeed);
			writer.WriteBool(this.EnableSceneRecording);
			writer.WriteInt(this.SceneUpgradeLevel);
			writer.WriteBool(this.PlayingInCampaignMode);
			writer.WriteBool(this.DisableDynamicPointlightShadows);
			writer.WriteBool(this.DoNotUseLoadingScreen);
			writer.WriteInt(this.DecalAtlasGroup);
			bool isValid = this.AtmosphereOnCampaign.IsValid;
			writer.WriteBool(isValid);
			if (isValid)
			{
				this.AtmosphereOnCampaign.SerializeTo(writer);
			}
		}

		// Token: 0x040004D0 RID: 1232
		public int TerrainType;

		// Token: 0x040004D1 RID: 1233
		public float DamageToPlayerMultiplier;

		// Token: 0x040004D2 RID: 1234
		public float DamageToFriendsMultiplier;

		// Token: 0x040004D3 RID: 1235
		public float DamageFromPlayerToFriendsMultiplier;

		// Token: 0x040004D4 RID: 1236
		public float TimeOfDay;

		// Token: 0x040004D5 RID: 1237
		[MarshalAs(UnmanagedType.U1)]
		public bool NeedsRandomTerrain;

		// Token: 0x040004D6 RID: 1238
		public int RandomTerrainSeed;

		// Token: 0x040004D7 RID: 1239
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string SceneName;

		// Token: 0x040004D8 RID: 1240
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string SceneLevels;

		// Token: 0x040004D9 RID: 1241
		[MarshalAs(UnmanagedType.U1)]
		public bool PlayingInCampaignMode;

		// Token: 0x040004DA RID: 1242
		[MarshalAs(UnmanagedType.U1)]
		public bool EnableSceneRecording;

		// Token: 0x040004DB RID: 1243
		public int SceneUpgradeLevel;

		// Token: 0x040004DC RID: 1244
		[MarshalAs(UnmanagedType.U1)]
		public bool SceneHasMapPatch;

		// Token: 0x040004DD RID: 1245
		public Vec2 PatchCoordinates;

		// Token: 0x040004DE RID: 1246
		public Vec2 PatchEncounterDir;

		// Token: 0x040004DF RID: 1247
		[MarshalAs(UnmanagedType.U1)]
		public bool DoNotUseLoadingScreen;

		// Token: 0x040004E0 RID: 1248
		[MarshalAs(UnmanagedType.U1)]
		public bool DisableDynamicPointlightShadows;

		// Token: 0x040004E1 RID: 1249
		public int DecalAtlasGroup;

		// Token: 0x040004E2 RID: 1250
		public AtmosphereInfo AtmosphereOnCampaign;
	}
}
