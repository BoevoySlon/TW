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
	// Token: 0x0200041F RID: 1055
	public class TransferPrisonerBarterable : Barterable
	{
		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06003FFB RID: 16379 RVA: 0x0013BCAD File Offset: 0x00139EAD
		public override string StringID
		{
			get
			{
				return "transfer_prisoner_barterable";
			}
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0013BCB4 File Offset: 0x00139EB4
		public TransferPrisonerBarterable(Hero prisonerCharacter, Hero owner, PartyBase ownerParty, Hero opponentHero, PartyBase otherParty) : base(owner, ownerParty)
		{
			this._prisonerCharacter = prisonerCharacter;
			this._opponentHero = opponentHero;
			this._otherParty = otherParty;
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x0013BCD8 File Offset: 0x00139ED8
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=g5bzJjd5}Transfer {PRISONER.NAME}", null);
				StringHelpers.SetCharacterProperties("PRISONER", this._prisonerCharacter.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0013BD0C File Offset: 0x00139F0C
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int num = Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(this._prisonerCharacter.CharacterObject, null);
			Hero originalOwner = base.OriginalOwner;
			if (faction != ((originalOwner != null) ? originalOwner.Clan : null))
			{
				Hero originalOwner2 = base.OriginalOwner;
				if (faction != ((originalOwner2 != null) ? originalOwner2.MapFaction : null) && faction != base.OriginalParty.MapFaction)
				{
					return num;
				}
			}
			return -num;
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0013BD76 File Offset: 0x00139F76
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new ImageIdentifier(CharacterCode.CreateFrom(this._prisonerCharacter.CharacterObject));
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0013BD8D File Offset: 0x00139F8D
		public override string GetEncyclopediaLink()
		{
			return this._prisonerCharacter.EncyclopediaLink;
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0013BD9C File Offset: 0x00139F9C
		public override void Apply()
		{
			if (this._otherParty != null && this._otherParty.MapFaction.IsAtWarWith(this._prisonerCharacter.MapFaction))
			{
				TransferPrisonerAction.Apply(this._prisonerCharacter.CharacterObject, base.OriginalParty, this._otherParty);
				return;
			}
			Debug.FailedAssert("Failed to transfer prisoner through barter", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\Barterables\\TransferPrisonerBarterable.cs", "Apply", 70);
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0013BE01 File Offset: 0x0013A001
		internal static void AutoGeneratedStaticCollectObjectsTransferPrisonerBarterable(object o, List<object> collectedObjects)
		{
			((TransferPrisonerBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0013BE0F File Offset: 0x0013A00F
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._prisonerCharacter);
			collectedObjects.Add(this._opponentHero);
			collectedObjects.Add(this._otherParty);
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0013BE3C File Offset: 0x0013A03C
		internal static object AutoGeneratedGetMemberValue_prisonerCharacter(object o)
		{
			return ((TransferPrisonerBarterable)o)._prisonerCharacter;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0013BE49 File Offset: 0x0013A049
		internal static object AutoGeneratedGetMemberValue_opponentHero(object o)
		{
			return ((TransferPrisonerBarterable)o)._opponentHero;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0013BE56 File Offset: 0x0013A056
		internal static object AutoGeneratedGetMemberValue_otherParty(object o)
		{
			return ((TransferPrisonerBarterable)o)._otherParty;
		}

		// Token: 0x040012AC RID: 4780
		[SaveableField(10)]
		private readonly Hero _prisonerCharacter;

		// Token: 0x040012AD RID: 4781
		[SaveableField(20)]
		private readonly Hero _opponentHero;

		// Token: 0x040012AE RID: 4782
		[SaveableField(30)]
		private readonly PartyBase _otherParty;
	}
}
