using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BA RID: 186
	public class MarriageSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600124F RID: 4687 RVA: 0x000546ED File Offset: 0x000528ED
		public Hero GroomHero { get; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001250 RID: 4688 RVA: 0x000546F5 File Offset: 0x000528F5
		public Hero BrideHero { get; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x000546FD File Offset: 0x000528FD
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_wedding";
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x00054704 File Offset: 0x00052904
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				Hero hero = (this.GroomHero == Hero.MainHero) ? this.GroomHero : this.BrideHero;
				Hero hero2 = (hero == this.GroomHero) ? this.BrideHero : this.GroomHero;
				GameTexts.SetVariable("FIRST_HERO", hero.Name);
				GameTexts.SetVariable("SECOND_HERO", hero2.Name);
				return GameTexts.FindText("str_marriage_notification", null);
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x0005479D File Offset: 0x0005299D
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x06001254 RID: 4692 RVA: 0x000547A8 File Offset: 0x000529A8
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner,
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner
			};
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0005487C File Offset: 0x00052A7C
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.GroomHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.GroomHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string brideEquipmentIDFromCulture = MarriageSceneNotificationItem.GetBrideEquipmentIDFromCulture(this.BrideHero.Culture);
			Equipment overridenEquipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(brideEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.BrideHero, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("cutscene_monk");
			Equipment overriddenEquipment = @object.Equipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overriddenEquipment, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, overriddenEquipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			List<Hero> audienceMembers = this.GetAudienceMembers(this.BrideHero, this.GroomHero);
			for (int i = 0; i < audienceMembers.Count; i++)
			{
				Hero hero = audienceMembers[i];
				if (hero != null)
				{
					Equipment overridenEquipment3 = hero.CivilianEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment3, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, overridenEquipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000549ED File Offset: 0x00052BED
		public MarriageSceneNotificationItem(Hero groomHero, Hero brideHero, CampaignTime creationTime, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			this.GroomHero = groomHero;
			this.BrideHero = brideHero;
			this.RelevantContext = relevantContextType;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00054A14 File Offset: 0x00052C14
		private List<Hero> GetAudienceMembers(Hero brideHero, Hero groomHero)
		{
			Queue<Hero> groomSide = new Queue<Hero>();
			Queue<Hero> brideSide = new Queue<Hero>();
			List<Hero> list = new List<Hero>();
			Hero mother = groomHero.Mother;
			if (mother != null && mother.IsAlive)
			{
				groomSide.Enqueue(groomHero.Mother);
			}
			Hero father = groomHero.Father;
			if (father != null && father.IsAlive)
			{
				groomSide.Enqueue(groomHero.Father);
			}
			if (groomHero.Siblings != null)
			{
				foreach (Hero item in from s in groomHero.Siblings
				where s.IsAlive && !s.IsChild
				select s)
				{
					groomSide.Enqueue(item);
				}
			}
			if (groomHero.Children != null)
			{
				foreach (Hero item2 in from s in groomHero.Children
				where s.IsAlive && !s.IsChild
				select s)
				{
					groomSide.Enqueue(item2);
				}
			}
			Hero mother2 = brideHero.Mother;
			if (mother2 != null && mother2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Mother);
			}
			Hero father2 = brideHero.Father;
			if (father2 != null && father2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Father);
			}
			if (brideHero.Siblings != null)
			{
				foreach (Hero item3 in from s in brideHero.Siblings
				where s.IsAlive && !s.IsChild
				select s)
				{
					brideSide.Enqueue(item3);
				}
			}
			if (brideHero.Children != null)
			{
				foreach (Hero item4 in from s in brideHero.Children
				where s.IsAlive && !s.IsChild
				select s)
				{
					brideSide.Enqueue(item4);
				}
			}
			if (groomSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__4;
				Func<Hero, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((Hero h) => h.IsLord && !h.IsChild && h != groomHero && h != brideHero && h.IsFriend(groomHero) && !brideSide.Contains(h)));
				}
				foreach (Hero item5 in allAliveHeroes.Where(predicate).Take(MathF.Ceiling(3f - (float)groomSide.Count)))
				{
					groomSide.Enqueue(item5);
				}
			}
			if (brideSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes2 = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__5;
				Func<Hero, bool> predicate2;
				if ((predicate2 = <>9__5) == null)
				{
					predicate2 = (<>9__5 = ((Hero h) => h.IsLord && !h.IsChild && h != brideHero && h != groomHero && h.IsFriend(brideHero) && !groomSide.Contains(h)));
				}
				foreach (Hero item6 in allAliveHeroes2.Where(predicate2).Take(MathF.Ceiling(3f - (float)brideSide.Count)))
				{
					brideSide.Enqueue(item6);
				}
			}
			for (int i = 0; i < 6; i++)
			{
				bool flag = i <= 1 || i == 4;
				Queue<Hero> queue = flag ? brideSide : groomSide;
				if (queue.Count > 0 && queue.Peek() != null)
				{
					list.Add(queue.Dequeue());
				}
				else
				{
					list.Add(null);
				}
			}
			return list;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00054E7C File Offset: 0x0005307C
		private static string GetBrideEquipmentIDFromCulture(CultureObject brideCulture)
		{
			string stringId = brideCulture.StringId;
			if (stringId == "empire")
			{
				return "marriage_female_emp_cutscene_template";
			}
			if (stringId == "aserai")
			{
				return "marriage_female_ase_cutscene_template";
			}
			if (stringId == "battania")
			{
				return "marriage_female_bat_cutscene_template";
			}
			if (stringId == "khuzait")
			{
				return "marriage_female_khu_cutscene_template";
			}
			if (stringId == "sturgia")
			{
				return "marriage_female_stu_cutscene_template";
			}
			if (!(stringId == "vlandia"))
			{
				return "marriage_female_emp_cutscene_template";
			}
			return "marriage_female_vla_cutscene_template";
		}

		// Token: 0x0400064C RID: 1612
		private const int NumberOfAudienceHeroes = 6;

		// Token: 0x04000650 RID: 1616
		private readonly CampaignTime _creationCampaignTime;
	}
}
