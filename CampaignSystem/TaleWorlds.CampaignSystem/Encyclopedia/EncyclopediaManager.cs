using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000158 RID: 344
	public class EncyclopediaManager
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001882 RID: 6274 RVA: 0x0007CA90 File Offset: 0x0007AC90
		// (set) Token: 0x06001883 RID: 6275 RVA: 0x0007CA98 File Offset: 0x0007AC98
		public IViewDataTracker ViewDataTracker { get; private set; }

		// Token: 0x06001884 RID: 6276 RVA: 0x0007CAA4 File Offset: 0x0007ACA4
		public void CreateEncyclopediaPages()
		{
			this._pages = new Dictionary<Type, EncyclopediaPage>();
			this.ViewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			List<Type> list = new List<Type>();
			List<Assembly> list2 = new List<Assembly>();
			Assembly assembly = typeof(EncyclopediaModelBase).Assembly;
			list2.Add(assembly);
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName[] referencedAssemblies = assembly2.GetReferencedAssemblies();
				for (int j = 0; j < referencedAssemblies.Length; j++)
				{
					if (referencedAssemblies[j].ToString() == assembly.GetName().ToString())
					{
						list2.Add(assembly2);
						break;
					}
				}
			}
			foreach (Assembly assembly3 in list2)
			{
				list.AddRange(assembly3.GetTypesSafe(null));
			}
			foreach (Type type in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type))
				{
					object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(OverrideEncyclopediaModel), false);
					for (int i = 0; i < customAttributesSafe.Length; i++)
					{
						OverrideEncyclopediaModel overrideEncyclopediaModel = customAttributesSafe[i] as OverrideEncyclopediaModel;
						if (overrideEncyclopediaModel != null)
						{
							EncyclopediaPage value = Activator.CreateInstance(type) as EncyclopediaPage;
							foreach (Type key in overrideEncyclopediaModel.PageTargetTypes)
							{
								this._pages.Add(key, value);
							}
						}
					}
				}
			}
			foreach (Type type2 in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type2))
				{
					object[] customAttributesSafe = type2.GetCustomAttributesSafe(typeof(EncyclopediaModel), false);
					for (int i = 0; i < customAttributesSafe.Length; i++)
					{
						EncyclopediaModel encyclopediaModel = customAttributesSafe[i] as EncyclopediaModel;
						if (encyclopediaModel != null)
						{
							EncyclopediaPage value2 = Activator.CreateInstance(type2) as EncyclopediaPage;
							foreach (Type key2 in encyclopediaModel.PageTargetTypes)
							{
								if (!this._pages.ContainsKey(key2))
								{
									this._pages.Add(key2, value2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0007CD40 File Offset: 0x0007AF40
		public IEnumerable<EncyclopediaPage> GetEncyclopediaPages()
		{
			return this._pages.Values.Distinct<EncyclopediaPage>();
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0007CD52 File Offset: 0x0007AF52
		public EncyclopediaPage GetPageOf(Type type)
		{
			return this._pages[type];
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0007CD60 File Offset: 0x0007AF60
		public string GetIdentifier(Type type)
		{
			return this._pages[type].GetIdentifier(type);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0007CD74 File Offset: 0x0007AF74
		public void GoToLink(string pageType, string stringID)
		{
			if (this._executeLink == null || string.IsNullOrEmpty(pageType))
			{
				return;
			}
			if (pageType == "Home" || pageType == "LastPage")
			{
				this._executeLink(pageType, null);
				return;
			}
			if (pageType == "ListPage")
			{
				EncyclopediaPage arg = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault((EncyclopediaPage e) => e.HasIdentifier(stringID));
				this._executeLink(pageType, arg);
				return;
			}
			EncyclopediaPage encyclopediaPage = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault((EncyclopediaPage e) => e.HasIdentifier(pageType));
			MBObjectBase @object = encyclopediaPage.GetObject(pageType, stringID);
			if (encyclopediaPage != null && encyclopediaPage.IsValidEncyclopediaItem(@object))
			{
				this._executeLink(pageType, @object);
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0007CE80 File Offset: 0x0007B080
		public void GoToLink(string link)
		{
			string[] array = link.ToString().Split(new char[]
			{
				'-'
			});
			this.GoToLink(array[0], array[1]);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0007CEB0 File Offset: 0x0007B0B0
		public void SetLinkCallback(Action<string, object> ExecuteLink)
		{
			this._executeLink = ExecuteLink;
		}

		// Token: 0x0400089C RID: 2204
		private Dictionary<Type, EncyclopediaPage> _pages;

		// Token: 0x0400089E RID: 2206
		public const string HOME_ID = "Home";

		// Token: 0x0400089F RID: 2207
		public const string LIST_PAGE_ID = "ListPage";

		// Token: 0x040008A0 RID: 2208
		public const string LAST_PAGE_ID = "LastPage";

		// Token: 0x040008A1 RID: 2209
		private Action<string, object> _executeLink;
	}
}
