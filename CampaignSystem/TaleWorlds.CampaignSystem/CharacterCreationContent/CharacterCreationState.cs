using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001DE RID: 478
	public class CharacterCreationState : PlayerGameState
	{
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x0007FA36 File Offset: 0x0007DC36
		// (set) Token: 0x06001C6F RID: 7279 RVA: 0x0007FA3E File Offset: 0x0007DC3E
		public CharacterCreation CharacterCreation
		{
			get
			{
				return this._characterCreation;
			}
			private set
			{
				this._characterCreation = value;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x0007FA47 File Offset: 0x0007DC47
		// (set) Token: 0x06001C71 RID: 7281 RVA: 0x0007FA4F File Offset: 0x0007DC4F
		public ICharacterCreationStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x0007FA58 File Offset: 0x0007DC58
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x0007FA60 File Offset: 0x0007DC60
		public CharacterCreationStageBase CurrentStage { get; private set; }

		// Token: 0x06001C74 RID: 7284 RVA: 0x0007FA6C File Offset: 0x0007DC6C
		public CharacterCreationState(CharacterCreationContentBase baseContent)
		{
			this.CharacterCreation = new CharacterCreation();
			this.CurrentCharacterCreationContent = baseContent;
			this.CurrentCharacterCreationContent.Initialize(this.CharacterCreation);
			this._stages = new List<KeyValuePair<int, Type>>();
			int key = 0;
			foreach (Type type in this.CurrentCharacterCreationContent.CharacterCreationStages)
			{
				if (type.IsSubclassOf(typeof(CharacterCreationStageBase)))
				{
					this._stages.Add(new KeyValuePair<int, Type>(key, type));
				}
				else
				{
					Debug.FailedAssert("Invalid character creation stage type: " + type.Name, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\CharacterCreationState.cs", ".ctor", 54);
				}
			}
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0007FB3C File Offset: 0x0007DD3C
		public CharacterCreationState()
		{
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x0007FB4B File Offset: 0x0007DD4B
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0007FB63 File Offset: 0x0007DD63
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._stageIndex == -1 && this.CharacterCreation != null)
			{
				this.NextStage();
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0007FB84 File Offset: 0x0007DD84
		public void FinalizeCharacterCreation()
		{
			this.CharacterCreation.ApplyFinalEffects();
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
			Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
			PartyBase.MainParty.SetVisualAsDirty();
			ICharacterCreationStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnCharacterCreationFinalized();
			}
			this.CurrentCharacterCreationContent.OnCharacterCreationFinalized();
			CampaignEventDispatcher.Instance.OnCharacterCreationIsOver();
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0007FBFC File Offset: 0x0007DDFC
		public void NextStage()
		{
			this._stageIndex++;
			CharacterCreationStageBase currentStage = this.CurrentStage;
			if (currentStage != null)
			{
				currentStage.OnFinalize();
			}
			this._furthestStageIndex = MathF.Max(this._furthestStageIndex, this._stageIndex);
			if (this._stageIndex == this._stages.Count)
			{
				this.FinalizeCharacterCreation();
				return;
			}
			Type value = this._stages[this._stageIndex].Value;
			this.CreateStage(value);
			this.Refresh();
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0007FC80 File Offset: 0x0007DE80
		public void PreviousStage()
		{
			CharacterCreationStageBase currentStage = this.CurrentStage;
			if (currentStage != null)
			{
				currentStage.OnFinalize();
			}
			this._stageIndex--;
			Type value = this._stages[this._stageIndex].Value;
			this.CreateStage(value);
			this.Refresh();
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0007FCD3 File Offset: 0x0007DED3
		private void CreateStage(Type type)
		{
			this.CurrentStage = (Activator.CreateInstance(type, new object[]
			{
				this
			}) as CharacterCreationStageBase);
			ICharacterCreationStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnStageCreated(this.CurrentStage);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0007FD08 File Offset: 0x0007DF08
		public void Refresh()
		{
			if (this.CurrentStage == null || this._stageIndex < 0 || this._stageIndex >= this._stages.Count)
			{
				Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\CharacterCreationState.cs", "Refresh", 139);
				return;
			}
			ICharacterCreationStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRefresh();
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0007FD63 File Offset: 0x0007DF63
		public int GetTotalStagesCount()
		{
			return this._stages.Count;
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0007FD70 File Offset: 0x0007DF70
		public int GetIndexOfCurrentStage()
		{
			return this._stageIndex;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x0007FD78 File Offset: 0x0007DF78
		public int GetFurthestIndex()
		{
			return this._furthestStageIndex;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x0007FD80 File Offset: 0x0007DF80
		public void GoToStage(int stageIndex)
		{
			if (stageIndex >= 0 && stageIndex < this._stages.Count && stageIndex != this._stageIndex && stageIndex <= this._furthestStageIndex)
			{
				this._stageIndex = stageIndex + 1;
				this.PreviousStage();
			}
		}

		// Token: 0x040008E8 RID: 2280
		private CharacterCreation _characterCreation;

		// Token: 0x040008E9 RID: 2281
		private ICharacterCreationStateHandler _handler;

		// Token: 0x040008EA RID: 2282
		private readonly List<KeyValuePair<int, Type>> _stages;

		// Token: 0x040008EB RID: 2283
		private int _stageIndex = -1;

		// Token: 0x040008ED RID: 2285
		private int _furthestStageIndex;

		// Token: 0x040008EE RID: 2286
		public readonly CharacterCreationContentBase CurrentCharacterCreationContent;
	}
}
