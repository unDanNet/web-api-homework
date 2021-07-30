using Microsoft.Extensions.Configuration;

namespace Core.Extensions
{
	public static class ConfigurationExtensions
	{
		public static string GetFullTableName(this IConfiguration configuration, string shortName)
		{
			return configuration.GetSection("TablesNames")[shortName];
		}
	}
}