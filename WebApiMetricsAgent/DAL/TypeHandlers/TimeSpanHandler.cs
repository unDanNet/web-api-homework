using System;
using System.Data;
using Dapper;

namespace WebApiMetricsAgent.DAL.TypeHandlers
{
	public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
	{
		public override void SetValue(IDbDataParameter parameter, TimeSpan value)
		{
			parameter.Value = value;
		}

		public override TimeSpan Parse(object value)
		{
			return TimeSpan.FromSeconds(Convert.ToInt64(value));
		}
	}
}