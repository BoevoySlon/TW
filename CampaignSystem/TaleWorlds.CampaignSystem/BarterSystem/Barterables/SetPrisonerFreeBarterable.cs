﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x0200041E RID: 1054
	public class SetPrisonerFreeBarterable : Barterable
	{
		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x0013BB3A File Offset: 0x00139D3A
		public override string StringID
		{
			get
			{
				return "set_prisoner_free_barterable";
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0013BB41 File Offset: 0x00139D41
		public SetPrisonerFreeBarterable(Hero prisonerCharacter, Hero captor, PartyBase ownerParty, Hero ransompayer) : base(captor, ownerParty)
		{
			this._prisonerCharacter = prisonerCharacter;
			this._ransompayer = ransompayer;
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x0013BB5A File Offset: 0x00139D5A
		public override TextObject Name
		{
			get
			{
				StringHelpers.SetCharacterProperties("PRISONER", this._prisonerCharacter.CharacterObject, null, false);
				return new TextObject("{=RwOzeGc3}Release {PRISONER.NAME}", null);
			}
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0013BB80 File Offset: 0x00139D80
		public override int GetUnitValueForFaction(IFaction faction)
		{
			float num = (float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(this._prisonerCharacter.CharacterObject, null) * (1f + MBMath.ClampFloat(this._prisonerCharacter.CaptivityStartTime.ElapsedWeeksUntilNow, 0f, 8f) * 0.3f) * 0.9f;
			if (faction == this._prisonerCharacter.MapFaction || faction == this._prisonerCharacter.Clan)
			{
				return (int)num;
			}
			if (faction.MapFaction == this._prisonerCharacter.PartyBelongedToAsPrisoner.MapFaction)
			{
				return (int)(-(int)num);
			}
			return 0;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0013BC20 File Offset: 0x00139E20
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new ImageIdentifier(CharacterCode.CreateFrom(this._prisonerCharacter.CharacterObject));
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0013BC37 File Offset: 0x00139E37
		public override string GetEncyclopediaLink()
		{
			return this._prisonerCharacter.EncyclopediaLink;
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0013BC44 File Offset: 0x00139E44
		public override void Apply()
		{
			if (this._prisonerCharacter.IsPrisoner)
			{
				EndCaptivityAction.ApplyByRansom(this._prisonerCharacter, this._ransompayer);
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0013BC64 File Offset: 0x00139E64
		internal static void AutoGeneratedStaticCollectObjectsSetPrisonerFreeBarterable(object o, List<object> collectedObjects)
		{
			((SetPrisonerFreeBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0013BC72 File Offset: 0x00139E72
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._prisonerCharacter);
			collectedObjects.Add(this._ransompayer);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0013BC93 File Offset: 0x00139E93
		internal static object AutoGeneratedGetMemberValue_prisonerCharacter(object o)
		{
			return ((SetPrisonerFreeBarterable)o)._prisonerCharacter;
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0013BCA0 File Offset: 0x00139EA0
		internal static object AutoGeneratedGetMemberValue_ransompayer(object o)
		{
			return ((SetPrisonerFreeBarterable)o)._ransompayer;
		}

		// Token: 0x040012AA RID: 4778
		[SaveableField(900)]
		private readonly Hero _prisonerCharacter;

		// Token: 0x040012AB RID: 4779
		[SaveableField(901)]
		private readonly Hero _ransompayer;
	}
}
