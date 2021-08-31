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
	public class RamMetricJob : IJob
	{
		private readonly IAgentsRepository _agentsRepo;
		private readonly IRamMetricsRepository _ramMetricsRepo;
		private readonly IMetricsAgentClient _client;
		
		public RamMetricJob(IAgentsRepository agentsRepo, IRamMetricsRepository ramMetricsRepo, IMetricsAgentClient client)
		{
			_agentsRepo = agentsRepo;
			_ramMetricsRepo = ramMetricsRepo;
			_client = client;
		}

		public Task Execute(IJobExecutionContext context)
		{
			IList<AgentInfo> agents = _agentsRepo.GetAllItems();

			foreach (var agent in agents)
			{
				TimeSpan fromTime = _ramMetricsRepo.GetTimeOfLatestMetricByAgentId(agent.Id);
				TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

				AllRamMetricsResponses metricsFromAgent = _client.GetAllRamMetrics(new GetAllRamMetricsApiRequest {
					FromTime = fromTime,
					ToTime = toTime,
					AgentBaseAddress = agent.Url
				});

				foreach (var metricDto in metricsFromAgent.Metrics)
				{
					_ramMetricsRepo.AddItem(new RamMetric {
						AgentId = agent.Id,
						Time = metricDto.Time,
						MemoryAvailable = metricDto.MemoryAvailable
					});
				}
			}

			return Task.CompletedTask;
		}
	}
}