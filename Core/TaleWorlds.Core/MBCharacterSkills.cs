using System;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000095 RID: 149
	public class MBCharacterSkills : MBObjectBase
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0001B70C File Offset: 0x0001990C
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x0001B714 File Offset: 0x00019914
		public CharacterSkills Skills { get; private set; }

		// Token: 0x0600080B RID: 2059 RVA: 0x0001B71D File Offset: 0x0001991D
		public MBCharacterSkills()
		{
			this.Skills = new CharacterSkills();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001B730 File Offset: 0x00019930
		public void Init(MBObjectManager objectManager, XmlNode node)
		{
			base.Initialize();
			this.Skills.Deserialize(objectManager, node);
			base.AfterInitialized();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001B74B File Offset: 0x0001994B
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.Skills.Deserialize(objectManager, node);
		}
	}
}
