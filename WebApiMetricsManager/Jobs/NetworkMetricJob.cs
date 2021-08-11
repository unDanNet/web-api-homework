using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.Responses;
using Quartz;
using WebApiMetricsManager.Client;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Jobs
{
	public class NetworkMetricJob
	{
		private readonly IAgentsRepository _agentsRepo;
		private readonly INetworkMetricsRepository _networkMetricsRepo;
		private readonly IMetricsAgentClient _client;
		
		public NetworkMetricJob(IAgentsRepository agentsRepo, INetworkMetricsRepository networkMetricsRepo, IMetricsAgentClient client)
		{
			_agentsRepo = agentsRepo;
			_networkMetricsRepo = networkMetricsRepo;
			_client = client;
		}

		public Task Execute(IJobExecutionContext context)
		{
			IList<AgentInfo> agents = _agentsRepo.GetAllItems();

			foreach (var agent in agents)
			{
				TimeSpan fromTime = _networkMetricsRepo.GetTimeOfLatestMetricByAgentId(agent.Id);
				TimeSpan toTime = TimeSpan.FromSeconds(DateTime.UtcNow.Second);

				AllNetworkMetricsResponses metricsFromAgent = _client.GetAllNetworkMetrics(new GetAllNetworkMetricsApiRequest {
					FromTime = fromTime,
					ToTime = toTime,
					AgentBaseAddress = agent.Url
				});

				foreach (var metricDto in metricsFromAgent.Metrics)
				{
					_networkMetricsRepo.AddItem(new NetworkMetric {
						AgentId = agent.Id,
						Time = metricDto.Time,
						Value = metricDto.Value
					});
				}
			}

			return Task.CompletedTask;
		}
	}
}