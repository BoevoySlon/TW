﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues.IssueQuestTasks
{
	// Token: 0x02000325 RID: 805
	public class DefeatPartyQuestTask : QuestTaskBase
	{
		// Token: 0x06002E29 RID: 11817 RVA: 0x000C15E8 File Offset: 0x000BF7E8
		internal static void AutoGeneratedStaticCollectObjectsDefeatPartyQuestTask(object o, List<object> collectedObjects)
		{
			((DefeatPartyQuestTask)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000C15F6 File Offset: 0x000BF7F6
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._partiesToDefeat);
			collectedObjects.Add(this._partyType);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000C1617 File Offset: 0x000BF817
		internal static object AutoGeneratedGetMemberValue_partiesToDefeat(object o)
		{
			return ((DefeatPartyQuestTask)o)._partiesToDefeat;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000C1624 File Offset: 0x000BF824
		internal static object AutoGeneratedGetMemberValue_targetNumParties(object o)
		{
			return ((DefeatPartyQuestTask)o)._targetNumParties;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000C1636 File Offset: 0x000BF836
		internal static object AutoGeneratedGetMemberValue_deferatedNumParties(object o)
		{
			return ((DefeatPartyQuestTask)o)._deferatedNumParties;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000C1648 File Offset: 0x000BF848
		internal static object AutoGeneratedGetMemberValue_partyType(object o)
		{
			return ((DefeatPartyQuestTask)o)._partyType;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000C1655 File Offset: 0x000BF855
		internal static object AutoGeneratedGetMemberValue_finishOnFail(object o)
		{
			return ((DefeatPartyQuestTask)o)._finishOnFail;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000C1668 File Offset: 0x000BF868
		public DefeatPartyQuestTask(List<PartyBase> partiesToDefeat, Action onSucceededAction, Action onFailedAction, Action onCanceledAction, DialogFlow dialogFlow = null, bool finishOnFail = false) : base(dialogFlow, onSucceededAction, onFailedAction, onCanceledAction)
		{
			this._partiesToDefeat = new List<PartyBase>();
			foreach (PartyBase item in partiesToDefeat)
			{
				this._partiesToDefeat.Add(item);
			}
			this._targetNumParties = partiesToDefeat.Count;
			this._partyType = partiesToDefeat[0].Name;
			this._finishOnFail = finishOnFail;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000C16F8 File Offset: 0x000BF8F8
		public DefeatPartyQuestTask(DefeatPartyQuestTask.PartyConditionDelegateType partyCondition, DefeatPartyQuestTask.OnPartyDefeatedDelegateType onPartyDefeated, int targetNumParties, Action onSucceededAction, Action onFailedAction, Action onCanceledAction, DialogFlow dialogFlow = null) : base(dialogFlow, onSucceededAction, onFailedAction, onCanceledAction)
		{
			this._partyConditionDelegate = partyCondition;
			this._targetNumParties = targetNumParties;
			this._onPartyDefeatedDelegate = onPartyDefeated;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000C1720 File Offset: 0x000BF920
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent)
			{
				if (PartyBase.MainParty.Side == mapEvent.WinningSide)
				{
					foreach (PartyBase partyBase in mapEvent.InvolvedParties)
					{
						if (partyBase.Side == mapEvent.DefeatedSide)
						{
							if (this._partyConditionDelegate == null && this._partiesToDefeat.Contains(partyBase))
							{
								this._partiesToDefeat.Remove(partyBase);
								this._deferatedNumParties++;
							}
							else if (this._partyConditionDelegate != null && this._partyConditionDelegate(partyBase))
							{
								this._deferatedNumParties++;
							}
							if (this._onPartyDefeatedDelegate != null)
							{
								this._onPartyDefeatedDelegate(partyBase);
							}
						}
					}
					if (this._deferatedNumParties >= this._targetNumParties)
					{
						base.Finish(QuestTaskBase.FinishStates.Success);
						return;
					}
				}
				else if (PartyBase.MainParty.Side == mapEvent.DefeatedSide && this._finishOnFail)
				{
					foreach (PartyBase item in mapEvent.InvolvedParties)
					{
						if (this._partyConditionDelegate == null && this._partiesToDefeat.Contains(item))
						{
							base.Finish(QuestTaskBase.FinishStates.Fail);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000C188C File Offset: 0x000BFA8C
		protected override void OnFinished()
		{
			if (this._partiesToDefeat != null)
			{
				this._partiesToDefeat.Clear();
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000C18A1 File Offset: 0x000BFAA1
		public override void SetReferences()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
		}

		// Token: 0x04000DCF RID: 3535
		private DefeatPartyQuestTask.PartyConditionDelegateType _partyConditionDelegate;

		// Token: 0x04000DD0 RID: 3536
		private DefeatPartyQuestTask.OnPartyDefeatedDelegateType _onPartyDefeatedDelegate;

		// Token: 0x04000DD1 RID: 3537
		[SaveableField(40)]
		private readonly List<PartyBase> _partiesToDefeat;

		// Token: 0x04000DD2 RID: 3538
		[SaveableField(41)]
		private int _targetNumParties;

		// Token: 0x04000DD3 RID: 3539
		[SaveableField(42)]
		private int _deferatedNumParties;

		// Token: 0x04000DD4 RID: 3540
		[SaveableField(43)]
		private TextObject _partyType;

		// Token: 0x04000DD5 RID: 3541
		[SaveableField(44)]
		private bool _finishOnFail;

		// Token: 0x02000683 RID: 1667
		// (Invoke) Token: 0x060054AC RID: 21676
		public delegate bool PartyConditionDelegateType(PartyBase defeatedParty);

		// Token: 0x02000684 RID: 1668
		// (Invoke) Token: 0x060054B0 RID: 21680
		public delegate void OnPartyDefeatedDelegateType(PartyBase party);
	}
}
