using System;
using System.Data;
using Dapper;

namespace WebApiMetricsManager.DAL.TypeHandlers
{
	public class UriHandler : SqlMapper.TypeHandler<Uri>
	{
		public override void SetValue(IDbDataParameter parameter, Uri value)
		{
			parameter.Value = value;
		}

		public override Uri Parse(object value)
		{
			return new Uri(Convert.ToString(value)!);
		}
	}
}