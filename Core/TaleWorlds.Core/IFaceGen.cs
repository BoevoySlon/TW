using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007E RID: 126
	public interface IFaceGen
	{
		// Token: 0x060007BF RID: 1983
		BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags);

		// Token: 0x060007C0 RID: 1984
		void GenerateParentBody(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties);

		// Token: 0x060007C1 RID: 1985
		void SetBody(ref BodyProperties bodyProperties, int build, int weight);

		// Token: 0x060007C2 RID: 1986
		void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo);

		// Token: 0x060007C3 RID: 1987
		void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor);

		// Token: 0x060007C4 RID: 1988
		BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age);

		// Token: 0x060007C5 RID: 1989
		BodyMeshMaturityType GetMaturityTypeWithAge(float age);

		// Token: 0x060007C6 RID: 1990
		int GetRaceCount();

		// Token: 0x060007C7 RID: 1991
		int GetRaceOrDefault(string raceId);

		// Token: 0x060007C8 RID: 1992
		string GetBaseMonsterNameFromRace(int race);

		// Token: 0x060007C9 RID: 1993
		string[] GetRaceNames();

		// Token: 0x060007CA RID: 1994
		Monster GetMonster(string monsterID);

		// Token: 0x060007CB RID: 1995
		Monster GetMonsterWithSuffix(int race, string suffix);

		// Token: 0x060007CC RID: 1996
		Monster GetBaseMonsterFromRace(int race);
	}
}
