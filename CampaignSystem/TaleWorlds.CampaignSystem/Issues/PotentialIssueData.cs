using System;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000321 RID: 801
	public struct PotentialIssueData
	{
		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002E09 RID: 11785 RVA: 0x000C1228 File Offset: 0x000BF428
		public PotentialIssueData.StartIssueDelegate OnStartIssue { get; }

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002E0A RID: 11786 RVA: 0x000C1230 File Offset: 0x000BF430
		public string IssueId { get; }

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000C1238 File Offset: 0x000BF438
		public Type IssueType { get; }

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x000C1240 File Offset: 0x000BF440
		public IssueBase.IssueFrequency Frequency { get; }

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002E0D RID: 11789 RVA: 0x000C1248 File Offset: 0x000BF448
		public object RelatedObject { get; }

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000C1250 File Offset: 0x000BF450
		public bool IsValid
		{
			get
			{
				return this.OnStartIssue != null;
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000C125B File Offset: 0x000BF45B
		public PotentialIssueData(PotentialIssueData.StartIssueDelegate onStartIssue, Type issueType, IssueBase.IssueFrequency frequency, object relatedObject = null)
		{
			this.OnStartIssue = onStartIssue;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000C1286 File Offset: 0x000BF486
		public PotentialIssueData(Type issueType, IssueBase.IssueFrequency frequency)
		{
			this.OnStartIssue = null;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = null;
		}

		// Token: 0x02000682 RID: 1666
		// (Invoke) Token: 0x060054A8 RID: 21672
		public delegate IssueBase StartIssueDelegate(in PotentialIssueData pid, Hero issueOwner);
	}
}
