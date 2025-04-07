using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009F RID: 159
	public sealed class SkillEffect : PropertyObject
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x00051A26 File Offset: 0x0004FC26
		public static MBReadOnlyList<SkillEffect> All
		{
			get
			{
				return Campaign.Current.AllSkillEffects;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00051A32 File Offset: 0x0004FC32
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x00051A3A File Offset: 0x0004FC3A
		public SkillEffect.PerkRole PrimaryRole { get; private set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x00051A43 File Offset: 0x0004FC43
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x00051A4B File Offset: 0x0004FC4B
		public SkillEffect.PerkRole SecondaryRole { get; private set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x00051A54 File Offset: 0x0004FC54
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x00051A5C File Offset: 0x0004FC5C
		public float PrimaryBonus { get; private set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00051A65 File Offset: 0x0004FC65
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x00051A6D File Offset: 0x0004FC6D
		public float SecondaryBonus { get; private set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00051A76 File Offset: 0x0004FC76
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x00051A7E File Offset: 0x0004FC7E
		public SkillEffect.EffectIncrementType IncrementType { get; private set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00051A87 File Offset: 0x0004FC87
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x00051A8F File Offset: 0x0004FC8F
		public SkillObject[] EffectedSkills { get; private set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00051A98 File Offset: 0x0004FC98
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x00051AA0 File Offset: 0x0004FCA0
		public float PrimaryBaseValue { get; private set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x00051AA9 File Offset: 0x0004FCA9
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x00051AB1 File Offset: 0x0004FCB1
		public float SecondaryBaseValue { get; private set; }

		// Token: 0x060011A0 RID: 4512 RVA: 0x00051ABA File Offset: 0x0004FCBA
		public SkillEffect(string stringId) : base(stringId)
		{
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00051AC4 File Offset: 0x0004FCC4
		public void Initialize(TextObject description, SkillObject[] effectedSkills, SkillEffect.PerkRole primaryRole = SkillEffect.PerkRole.None, float primaryBonus = 0f, SkillEffect.PerkRole secondaryRole = SkillEffect.PerkRole.None, float secondaryBonus = 0f, SkillEffect.EffectIncrementType incrementType = SkillEffect.EffectIncrementType.AddFactor, float primaryBaseValue = 0f, float secondaryBaseValue = 0f)
		{
			base.Initialize(TextObject.Empty, description);
			this.PrimaryRole = primaryRole;
			this.SecondaryRole = secondaryRole;
			this.PrimaryBonus = primaryBonus;
			this.SecondaryBonus = secondaryBonus;
			this.IncrementType = incrementType;
			this.EffectedSkills = effectedSkills;
			this.PrimaryBaseValue = primaryBaseValue;
			this.SecondaryBaseValue = secondaryBaseValue;
			base.AfterInitialized();
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00051B21 File Offset: 0x0004FD21
		public float GetPrimaryValue(int skillLevel)
		{
			return MathF.Max(0f, this.PrimaryBaseValue + this.PrimaryBonus * (float)skillLevel);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00051B3D File Offset: 0x0004FD3D
		public float GetSecondaryValue(int skillLevel)
		{
			return MathF.Max(0f, this.SecondaryBaseValue + this.SecondaryBonus * (float)skillLevel);
		}

		// Token: 0x020004CB RID: 1227
		public enum EffectIncrementType
		{
			// Token: 0x040014A5 RID: 5285
			Invalid = -1,
			// Token: 0x040014A6 RID: 5286
			Add,
			// Token: 0x040014A7 RID: 5287
			AddFactor
		}

		// Token: 0x020004CC RID: 1228
		public enum PerkRole
		{
			// Token: 0x040014A9 RID: 5289
			None,
			// Token: 0x040014AA RID: 5290
			Ruler,
			// Token: 0x040014AB RID: 5291
			ClanLeader,
			// Token: 0x040014AC RID: 5292
			Governor,
			// Token: 0x040014AD RID: 5293
			ArmyCommander,
			// Token: 0x040014AE RID: 5294
			PartyLeader,
			// Token: 0x040014AF RID: 5295
			PartyOwner,
			// Token: 0x040014B0 RID: 5296
			Surgeon,
			// Token: 0x040014B1 RID: 5297
			Engineer,
			// Token: 0x040014B2 RID: 5298
			Scout,
			// Token: 0x040014B3 RID: 5299
			Quartermaster,
			// Token: 0x040014B4 RID: 5300
			PartyMember,
			// Token: 0x040014B5 RID: 5301
			Personal,
			// Token: 0x040014B6 RID: 5302
			Captain,
			// Token: 0x040014B7 RID: 5303
			NumberOfPerkRoles
		}
	}
}
