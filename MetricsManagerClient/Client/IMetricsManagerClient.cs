using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetricsManagerClient.Models;

namespace MetricsManagerClient.Client
{
	public interface IMetricsManagerClient
	{
		public Task<IList<AgentInfo>> GetAllRegisteredAgentsAsync();
		
		public Task<AgentInfo> RegisterAgentAsync(string agentUrl);

		public Task EnableAgentAsync(int agentId);

		public Task DisableAgentAsync(int agentId);

		public Task<T> GetLastMetricAsync<T>(string type, int agentId);

		public Task<IList<T>> GetMetricsFromSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan fromTime);

		public Task<IList<T>> GetMetricsToSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan toTime);

		public Task<IList<T>> GetMetricsInSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan fromTime, TimeSpan toTime);
	}
}