using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000BB RID: 187
	public class SceneNotificationData
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0001FB61 File Offset: 0x0001DD61
		public virtual string SceneID { get; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001FB69 File Offset: 0x0001DD69
		public virtual string SoundEventPath { get; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0001FB71 File Offset: 0x0001DD71
		public virtual TextObject TitleText { get; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001FB79 File Offset: 0x0001DD79
		public virtual TextObject AffirmativeDescriptionText { get; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0001FB81 File Offset: 0x0001DD81
		public virtual TextObject NegativeDescriptionText { get; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0001FB89 File Offset: 0x0001DD89
		public virtual TextObject AffirmativeHintText { get; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0001FB91 File Offset: 0x0001DD91
		public virtual TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0001FB99 File Offset: 0x0001DD99
		public virtual TextObject AffirmativeTitleText { get; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001FBA1 File Offset: 0x0001DDA1
		public virtual TextObject NegativeTitleText { get; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001FBA9 File Offset: 0x0001DDA9
		public virtual TextObject AffirmativeText { get; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0001FBB1 File Offset: 0x0001DDB1
		public virtual TextObject NegativeText { get; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001FBB9 File Offset: 0x0001DDB9
		public virtual bool IsAffirmativeOptionShown { get; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001FBC1 File Offset: 0x0001DDC1
		public virtual bool IsNegativeOptionShown { get; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001FBC9 File Offset: 0x0001DDC9
		public virtual bool PauseActiveState { get; } = 1;

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001FBD1 File Offset: 0x0001DDD1
		public virtual SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x0600098C RID: 2444 RVA: 0x0001FBD9 File Offset: 0x0001DDD9
		public virtual void OnAffirmativeAction()
		{
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0001FBDB File Offset: 0x0001DDDB
		public virtual void OnNegativeAction()
		{
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0001FBDD File Offset: 0x0001DDDD
		public virtual void OnCloseAction()
		{
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001FBDF File Offset: 0x0001DDDF
		public virtual IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>();
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001FBE6 File Offset: 0x0001DDE6
		public virtual IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			return new List<SceneNotificationData.SceneNotificationCharacter>();
		}

		// Token: 0x02000108 RID: 264
		public readonly struct SceneNotificationCharacter
		{
			// Token: 0x06000A6E RID: 2670 RVA: 0x00021B62 File Offset: 0x0001FD62
			public SceneNotificationCharacter(BasicCharacterObject character, Equipment overriddenEquipment = null, BodyProperties overriddenBodyProperties = default(BodyProperties), bool useCivilianEquipment = false, uint customColor1 = 4294967295U, uint customColor2 = 4294967295U, bool useHorse = false)
			{
				this.Character = character;
				this.OverriddenEquipment = overriddenEquipment;
				this.OverriddenBodyProperties = overriddenBodyProperties;
				this.UseCivilianEquipment = useCivilianEquipment;
				this.CustomColor1 = customColor1;
				this.CustomColor2 = customColor2;
				this.UseHorse = useHorse;
			}

			// Token: 0x040006F5 RID: 1781
			public readonly BasicCharacterObject Character;

			// Token: 0x040006F6 RID: 1782
			public readonly Equipment OverriddenEquipment;

			// Token: 0x040006F7 RID: 1783
			public readonly BodyProperties OverriddenBodyProperties;

			// Token: 0x040006F8 RID: 1784
			public readonly bool UseCivilianEquipment;

			// Token: 0x040006F9 RID: 1785
			public readonly bool UseHorse;

			// Token: 0x040006FA RID: 1786
			public readonly uint CustomColor1;

			// Token: 0x040006FB RID: 1787
			public readonly uint CustomColor2;
		}

		// Token: 0x02000109 RID: 265
		public enum RelevantContextType
		{
			// Token: 0x040006FD RID: 1789
			Any,
			// Token: 0x040006FE RID: 1790
			MPLobby,
			// Token: 0x040006FF RID: 1791
			CustomBattle,
			// Token: 0x04000700 RID: 1792
			Mission,
			// Token: 0x04000701 RID: 1793
			Map
		}
	}
}
