using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core
{
	// Token: 0x020000C6 RID: 198
	public class VirtualPlayer
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00020052 File Offset: 0x0001E252
		public static Dictionary<Type, object> PeerComponents
		{
			get
			{
				return VirtualPlayer._peerComponents;
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00020059 File Offset: 0x0001E259
		static VirtualPlayer()
		{
			VirtualPlayer.FindPeerComponents();
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002006C File Offset: 0x0001E26C
		private static void FindPeerComponents()
		{
			Debug.Print("Searching Peer Components", 0, Debug.DebugColor.White, 17592186044416UL);
			VirtualPlayer._peerComponentIds = new Dictionary<Type, uint>();
			VirtualPlayer._peerComponentTypes = new Dictionary<uint, Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (VirtualPlayer.CheckAssemblyForPeerComponent(assembly))
				{
					List<Type> typesSafe = assembly.GetTypesSafe(null);
					list.AddRange(from q in typesSafe
					where typeof(PeerComponent).IsAssignableFrom(q) && typeof(PeerComponent) != q
					select q);
				}
			}
			foreach (Type type in list)
			{
				uint djb = (uint)Common.GetDJB2(type.Name);
				VirtualPlayer._peerComponentIds.Add(type, djb);
				VirtualPlayer._peerComponentTypes.Add(djb, type);
			}
			Debug.Print("Found " + list.Count + " peer components", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00020194 File Offset: 0x0001E394
		private static bool CheckAssemblyForPeerComponent(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(PeerComponent));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000201E9 File Offset: 0x0001E3E9
		private static void EnsurePeerTypeList<T>() where T : PeerComponent
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(typeof(T)))
			{
				VirtualPlayer._peerComponents.Add(typeof(T), new List<T>());
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002021C File Offset: 0x0001E41C
		private static void EnsurePeerTypeList(Type type)
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(type))
			{
				IList value = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
				{
					type
				})) as IList;
				VirtualPlayer._peerComponents.Add(type, value);
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00020266 File Offset: 0x0001E466
		public static List<T> Peers<T>() where T : PeerComponent
		{
			VirtualPlayer.EnsurePeerTypeList<T>();
			return VirtualPlayer._peerComponents[typeof(T)] as List<T>;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00020286 File Offset: 0x0001E486
		public static void Reset()
		{
			VirtualPlayer._peerComponents = new Dictionary<Type, object>();
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00020292 File Offset: 0x0001E492
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x000202AD File Offset: 0x0001E4AD
		public string BannerCode
		{
			get
			{
				if (this._bannerCode == null)
				{
					this._bannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";
				}
				return this._bannerCode;
			}
			set
			{
				this._bannerCode = value;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x000202B6 File Offset: 0x0001E4B6
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x000202BE File Offset: 0x0001E4BE
		public BodyProperties BodyProperties { get; set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x000202C7 File Offset: 0x0001E4C7
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x000202CF File Offset: 0x0001E4CF
		public int Race { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x000202D8 File Offset: 0x0001E4D8
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x000202E0 File Offset: 0x0001E4E0
		public bool IsFemale { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000202E9 File Offset: 0x0001E4E9
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x000202F1 File Offset: 0x0001E4F1
		public PlayerId Id { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x000202FA File Offset: 0x0001E4FA
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x00020302 File Offset: 0x0001E502
		public int Index { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0002030B File Offset: 0x0001E50B
		public bool IsMine
		{
			get
			{
				return MBNetwork.MyPeer == this;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00020315 File Offset: 0x0001E515
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0002031D File Offset: 0x0001E51D
		public string UserName { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00020326 File Offset: 0x0001E526
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0002032E File Offset: 0x0001E52E
		public int ChosenBadgeIndex { get; set; }

		// Token: 0x060009CA RID: 2506 RVA: 0x00020337 File Offset: 0x0001E537
		public VirtualPlayer(int index, string name, PlayerId playerID, ICommunicator communicator)
		{
			this._peerEntitySystem = new EntitySystem<PeerComponent>();
			this.UserName = name;
			this.Index = index;
			this.Id = playerID;
			this.Communicator = communicator;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00020368 File Offset: 0x0001E568
		public T AddComponent<T>() where T : PeerComponent, new()
		{
			T t = this._peerEntitySystem.AddComponent<T>();
			t.Peer = this;
			t.TypeId = VirtualPlayer._peerComponentIds[typeof(T)];
			VirtualPlayer.EnsurePeerTypeList<T>();
			(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Add(t);
			this.Communicator.OnAddComponent(t);
			t.Initialize();
			return t;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000203F0 File Offset: 0x0001E5F0
		public PeerComponent AddComponent(Type peerComponentType)
		{
			PeerComponent peerComponent = this._peerEntitySystem.AddComponent(peerComponentType);
			peerComponent.Peer = this;
			peerComponent.TypeId = VirtualPlayer._peerComponentIds[peerComponentType];
			VirtualPlayer.EnsurePeerTypeList(peerComponentType);
			(VirtualPlayer._peerComponents[peerComponentType] as IList).Add(peerComponent);
			this.Communicator.OnAddComponent(peerComponent);
			peerComponent.Initialize();
			return peerComponent;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00020452 File Offset: 0x0001E652
		public PeerComponent AddComponent(uint componentId)
		{
			return this.AddComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00020465 File Offset: 0x0001E665
		public PeerComponent GetComponent(uint componentId)
		{
			return this.GetComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00020478 File Offset: 0x0001E678
		public T GetComponent<T>() where T : PeerComponent
		{
			return this._peerEntitySystem.GetComponent<T>();
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00020485 File Offset: 0x0001E685
		public PeerComponent GetComponent(Type peerComponentType)
		{
			return this._peerEntitySystem.GetComponent(peerComponentType);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00020494 File Offset: 0x0001E694
		public void RemoveComponent<T>(bool synched = true) where T : PeerComponent
		{
			T component = this._peerEntitySystem.GetComponent<T>();
			if (component != null)
			{
				this._peerEntitySystem.RemoveComponent(component);
				(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Remove(component);
				if (synched)
				{
					this.Communicator.OnRemoveComponent(component);
				}
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000204FA File Offset: 0x0001E6FA
		public void RemoveComponent(PeerComponent component)
		{
			this._peerEntitySystem.RemoveComponent(component);
			(VirtualPlayer._peerComponents[component.GetType()] as IList).Remove(component);
			this.Communicator.OnRemoveComponent(component);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002052F File Offset: 0x0001E72F
		public void OnDisconnect()
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00020534 File Offset: 0x0001E734
		public void SynchronizeComponentsTo(VirtualPlayer peer)
		{
			foreach (PeerComponent component in this._peerEntitySystem.Components)
			{
				this.Communicator.OnSynchronizeComponentTo(peer, component);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00020594 File Offset: 0x0001E794
		public void UpdateIndexForReconnectingPlayer(int playerIndex)
		{
			this.Index = playerIndex;
		}

		// Token: 0x040005BD RID: 1469
		private const string DefaultPlayerBannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";

		// Token: 0x040005BE RID: 1470
		private static Dictionary<Type, object> _peerComponents = new Dictionary<Type, object>();

		// Token: 0x040005BF RID: 1471
		private static Dictionary<Type, uint> _peerComponentIds;

		// Token: 0x040005C0 RID: 1472
		private static Dictionary<uint, Type> _peerComponentTypes;

		// Token: 0x040005C1 RID: 1473
		private string _bannerCode;

		// Token: 0x040005C6 RID: 1478
		public readonly ICommunicator Communicator;

		// Token: 0x040005C7 RID: 1479
		private EntitySystem<PeerComponent> _peerEntitySystem;

		// Token: 0x040005CB RID: 1483
		public Dictionary<int, List<int>> UsedCosmetics;
	}
}
