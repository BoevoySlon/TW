using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x0200001E RID: 30
	[JsonConverter(typeof(BodyPropertiesJsonConverter))]
	[Serializable]
	public struct BodyProperties
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000063BD File Offset: 0x000045BD
		public StaticBodyProperties StaticProperties
		{
			get
			{
				return this._staticBodyProperties;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000063C5 File Offset: 0x000045C5
		public DynamicBodyProperties DynamicProperties
		{
			get
			{
				return this._dynamicBodyProperties;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000063CD File Offset: 0x000045CD
		public float Age
		{
			get
			{
				return this._dynamicBodyProperties.Age;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000063DA File Offset: 0x000045DA
		public float Weight
		{
			get
			{
				return this._dynamicBodyProperties.Weight;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000063E7 File Offset: 0x000045E7
		public float Build
		{
			get
			{
				return this._dynamicBodyProperties.Build;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000063F4 File Offset: 0x000045F4
		public ulong KeyPart1
		{
			get
			{
				return this._staticBodyProperties.KeyPart1;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006410 File Offset: 0x00004610
		public ulong KeyPart2
		{
			get
			{
				return this._staticBodyProperties.KeyPart2;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000642C File Offset: 0x0000462C
		public ulong KeyPart3
		{
			get
			{
				return this._staticBodyProperties.KeyPart3;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006448 File Offset: 0x00004648
		public ulong KeyPart4
		{
			get
			{
				return this._staticBodyProperties.KeyPart4;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006464 File Offset: 0x00004664
		public ulong KeyPart5
		{
			get
			{
				return this._staticBodyProperties.KeyPart5;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006480 File Offset: 0x00004680
		public ulong KeyPart6
		{
			get
			{
				return this._staticBodyProperties.KeyPart6;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000649C File Offset: 0x0000469C
		public ulong KeyPart7
		{
			get
			{
				return this._staticBodyProperties.KeyPart7;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000064B8 File Offset: 0x000046B8
		public ulong KeyPart8
		{
			get
			{
				return this._staticBodyProperties.KeyPart8;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000064D3 File Offset: 0x000046D3
		public BodyProperties(DynamicBodyProperties dynamicBodyProperties, StaticBodyProperties staticBodyProperties)
		{
			this._dynamicBodyProperties = dynamicBodyProperties;
			this._staticBodyProperties = staticBodyProperties;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000064E4 File Offset: 0x000046E4
		public static bool FromXmlNode(XmlNode node, out BodyProperties bodyProperties)
		{
			float age = 30f;
			float weight = 0.5f;
			float build = 0.5f;
			if (node.Attributes.get_ItemOf("age") != null)
			{
				float.TryParse(node.Attributes.get_ItemOf("age").Value, out age);
			}
			if (node.Attributes.get_ItemOf("weight") != null)
			{
				float.TryParse(node.Attributes.get_ItemOf("weight").Value, out weight);
			}
			if (node.Attributes.get_ItemOf("build") != null)
			{
				float.TryParse(node.Attributes.get_ItemOf("build").Value, out build);
			}
			DynamicBodyProperties dynamicBodyProperties = new DynamicBodyProperties(age, weight, build);
			StaticBodyProperties staticBodyProperties;
			if (StaticBodyProperties.FromXmlNode(node, out staticBodyProperties))
			{
				bodyProperties = new BodyProperties(dynamicBodyProperties, staticBodyProperties);
				return true;
			}
			bodyProperties = default(BodyProperties);
			return false;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000065C0 File Offset: 0x000047C0
		public static bool FromString(string keyValue, out BodyProperties bodyProperties)
		{
			if (keyValue.StartsWith("<BodyProperties ", StringComparison.InvariantCultureIgnoreCase) || keyValue.StartsWith("<BodyPropertiesMax ", StringComparison.InvariantCultureIgnoreCase))
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.LoadXml(keyValue);
				}
				catch (XmlException)
				{
					bodyProperties = default(BodyProperties);
					return false;
				}
				if (xmlDocument.FirstChild.Name.Equals("BodyProperties", StringComparison.InvariantCultureIgnoreCase) || xmlDocument.FirstChild.Name.Equals("BodyPropertiesMax", StringComparison.InvariantCultureIgnoreCase))
				{
					BodyProperties.FromXmlNode(xmlDocument.FirstChild, out bodyProperties);
					float age = 20f;
					float weight = 0f;
					float build = 0f;
					if (xmlDocument.FirstChild.Attributes.get_ItemOf("age") != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes.get_ItemOf("age").Value, out age);
					}
					if (xmlDocument.FirstChild.Attributes.get_ItemOf("weight") != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes.get_ItemOf("weight").Value, out weight);
					}
					if (xmlDocument.FirstChild.Attributes.get_ItemOf("build") != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes.get_ItemOf("build").Value, out build);
					}
					bodyProperties = new BodyProperties(new DynamicBodyProperties(age, weight, build), bodyProperties.StaticProperties);
					return true;
				}
				bodyProperties = default(BodyProperties);
				return false;
			}
			Debug.FailedAssert("unknown body properties format:\n" + keyValue, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BodyProperties.cs", "FromString", 148);
			bodyProperties = default(BodyProperties);
			return false;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006768 File Offset: 0x00004968
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tattooTags)
		{
			return FaceGen.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tattooTags);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006788 File Offset: 0x00004988
		public static bool operator ==(BodyProperties a, BodyProperties b)
		{
			return a == b || (a != null && b != null && a._staticBodyProperties == b._staticBodyProperties && a._dynamicBodyProperties == b._dynamicBodyProperties);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000067DD File Offset: 0x000049DD
		public static bool operator !=(BodyProperties a, BodyProperties b)
		{
			return !(a == b);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000067EC File Offset: 0x000049EC
		public override string ToString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(150, "ToString");
			mbstringBuilder.Append<string>("<BodyProperties version=\"4\" ");
			mbstringBuilder.Append<string>(this._dynamicBodyProperties.ToString() + " ");
			mbstringBuilder.Append<string>(this._staticBodyProperties.ToString());
			mbstringBuilder.Append<string>(" />");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006878 File Offset: 0x00004A78
		public override bool Equals(object obj)
		{
			if (!(obj is BodyProperties))
			{
				return false;
			}
			BodyProperties bodyProperties = (BodyProperties)obj;
			return EqualityComparer<DynamicBodyProperties>.Default.Equals(this._dynamicBodyProperties, bodyProperties._dynamicBodyProperties) && EqualityComparer<StaticBodyProperties>.Default.Equals(this._staticBodyProperties, bodyProperties._staticBodyProperties);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000068C6 File Offset: 0x00004AC6
		public override int GetHashCode()
		{
			return (2041866711 * -1521134295 + EqualityComparer<DynamicBodyProperties>.Default.GetHashCode(this._dynamicBodyProperties)) * -1521134295 + EqualityComparer<StaticBodyProperties>.Default.GetHashCode(this._staticBodyProperties);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000068FC File Offset: 0x00004AFC
		public BodyProperties ClampForMultiplayer()
		{
			float age = MathF.Clamp(this.DynamicProperties.Age, 22f, 128f);
			DynamicBodyProperties dynamicBodyProperties = new DynamicBodyProperties(age, 0.5f, 0.5f);
			StaticBodyProperties staticProperties = this.StaticProperties;
			StaticBodyProperties staticBodyProperties = this.ClampHeightMultiplierFaceKey(staticProperties);
			return new BodyProperties(dynamicBodyProperties, staticBodyProperties);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000694C File Offset: 0x00004B4C
		private StaticBodyProperties ClampHeightMultiplierFaceKey(in StaticBodyProperties staticBodyProperties)
		{
			StaticBodyProperties staticBodyProperties2 = staticBodyProperties;
			ulong keyPart = staticBodyProperties2.KeyPart8;
			float num = (float)BodyProperties.GetBitsValueFromKey(keyPart, 19, 6) / 63f;
			if (num < 0.25f || num > 0.75f)
			{
				num = 0.5f;
				int inewValue = (int)(num * 63f);
				ulong num2 = BodyProperties.SetBits(keyPart, 19, 6, inewValue);
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart2 = staticBodyProperties2.KeyPart1;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart3 = staticBodyProperties2.KeyPart2;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart4 = staticBodyProperties2.KeyPart3;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart5 = staticBodyProperties2.KeyPart4;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart6 = staticBodyProperties2.KeyPart5;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart7 = staticBodyProperties2.KeyPart6;
				ulong keyPart8 = num2;
				staticBodyProperties2 = staticBodyProperties;
				return new StaticBodyProperties(keyPart2, keyPart3, keyPart4, keyPart5, keyPart6, keyPart7, keyPart8, staticBodyProperties2.KeyPart8);
			}
			return staticBodyProperties;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006A1C File Offset: 0x00004C1C
		private static ulong SetBits(in ulong ipart7, int startBit, int numBits, int inewValue)
		{
			ulong num = ipart7;
			ulong num2 = MathF.PowTwo64(numBits) - 1UL << startBit;
			return (num & ~num2) | (ulong)((ulong)((long)inewValue) << startBit);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006A48 File Offset: 0x00004C48
		private static int GetBitsValueFromKey(in ulong part, int startBit, int numBits)
		{
			ulong num = part >> startBit;
			ulong num2 = MathF.PowTwo64(numBits) - 1UL;
			return (int)(num & num2);
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006A6C File Offset: 0x00004C6C
		public static BodyProperties Default
		{
			get
			{
				return new BodyProperties(new DynamicBodyProperties(20f, 0f, 0f), default(StaticBodyProperties));
			}
		}

		// Token: 0x04000156 RID: 342
		private readonly DynamicBodyProperties _dynamicBodyProperties;

		// Token: 0x04000157 RID: 343
		private readonly StaticBodyProperties _staticBodyProperties;

		// Token: 0x04000158 RID: 344
		private const float DefaultAge = 30f;

		// Token: 0x04000159 RID: 345
		private const float DefaultWeight = 0.5f;

		// Token: 0x0400015A RID: 346
		private const float DefaultBuild = 0.5f;
	}
}
