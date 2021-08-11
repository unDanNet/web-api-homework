using System;
using System.Collections.Generic;
using Core.Interfaces;
using WebApiMetricsManager.DAL.Models;

namespace WebApiMetricsManager.DAL.Interfaces
{
	public interface IDotnetMetricsRepository : IRepository<DotnetMetric>
	{
		IList<DotnetMetric> GetItemsByAgentId(int agentId, TimeSpan fromTime, TimeSpan toTime);
		TimeSpan GetTimeOfLatestMetricByAgentId(int agentId);
	}
}