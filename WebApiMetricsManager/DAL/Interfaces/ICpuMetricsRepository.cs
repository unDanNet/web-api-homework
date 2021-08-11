using System;
using System.Collections.Generic;
using Core.Interfaces;
using WebApiMetricsManager.DAL.Models;

namespace WebApiMetricsManager.DAL.Interfaces
{
	public interface ICpuMetricsRepository : IRepository<CpuMetric>
	{
		IList<CpuMetric> GetItemsByAgentId(int agentId, TimeSpan fromTime, TimeSpan toTime);
		TimeSpan GetTimeOfLatestMetricByAgentId(int agentId);
	}
}