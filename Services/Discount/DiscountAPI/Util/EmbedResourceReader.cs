using System.IO;
using System.Reflection;

namespace DiscountAPI.Util
{
	public static class EmbedResourceReader
	{
		public static string GetScriptsByName(string scriptName)
		{
			var fullPath = $"DiscountAPI.SqlQueries.{scriptName}";
			var assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(fullPath))
			using (StreamReader reader = new StreamReader(stream))
			{
				string result = reader.ReadToEnd();
				return result;
			}
		}
	}
}
