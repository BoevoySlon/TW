using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Core
{
	// Token: 0x0200001F RID: 31
	public class BodyPropertiesJsonConverter : JsonConverter
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00006A9B File Offset: 0x00004C9B
		public override bool CanConvert(Type objectType)
		{
			return typeof(BodyProperties).IsAssignableFrom(objectType);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006AB0 File Offset: 0x00004CB0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			BodyProperties bodyProperties;
			BodyProperties.FromString((string)JObject.Load(reader)["_data"], out bodyProperties);
			return bodyProperties;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00006AE0 File Offset: 0x00004CE0
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006AE4 File Offset: 0x00004CE4
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JProperty content = new JProperty("_data", ((BodyProperties)value).ToString());
			new JObject
			{
				content
			}.WriteTo(writer, Array.Empty<JsonConverter>());
		}
	}
}
