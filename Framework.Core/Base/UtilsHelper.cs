using Newtonsoft.Json;

namespace Framework.Core.Base
{
	public static class UtilsHelpers
	{
		public static string ToJson(this object @object) => JsonConvert.SerializeObject(@object, Formatting.None);
	}
}
