using System;
using System.Collections.Generic;
using MetricsManagerClient.Models;

namespace MetricsManagerClient.Client
{
	public interface IMetricsManagerClient
	{
		public IList<AgentInfo> GetAllRegisteredAgents();
		
		public AgentInfo RegisterAgent(string agentUrl);

		public void EnableAgent(int agentId);

		public void DisableAgent(int agentId);

		public T GetLastMetric<T>(string type, int agentId);

		public IList<T> GetMetricsFromSpecifiedTime<T>(string type, int agentId, TimeSpan fromTime);

		public IList<T> GetMetricsToSpecifiedTime<T>(string type, int agentId, TimeSpan toTime);

		public IList<T> GetMetricsInSpecifiedTime<T>(string type, int agentId, TimeSpan fromTime, TimeSpan toTime);
	}
}