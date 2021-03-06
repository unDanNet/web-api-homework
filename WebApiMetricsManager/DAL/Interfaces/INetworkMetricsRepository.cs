using System;
using System.Collections.Generic;
using Core.Interfaces;
using WebApiMetricsManager.DAL.Models;

namespace WebApiMetricsManager.DAL.Interfaces
{
	public interface INetworkMetricsRepository : IRepository<NetworkMetric>
	{
		IList<NetworkMetric> GetItemsByAgentId(int agentId, TimeSpan fromTime, TimeSpan toTime);
		TimeSpan GetTimeOfLatestMetricByAgentId(int agentId);
	}
}