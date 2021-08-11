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
	public class CpuMetricJob : IJob
	{
		private readonly IAgentsRepository _agentsRepo;
		private readonly ICpuMetricsRepository _cpuMetricsRepo;
		private readonly IMetricsAgentClient _client;
		
		public CpuMetricJob(IAgentsRepository agentsRepo, ICpuMetricsRepository cpuMetricsRepo, IMetricsAgentClient client)
		{
			_agentsRepo = agentsRepo;
			_cpuMetricsRepo = cpuMetricsRepo;
			_client = client;
		}

		public Task Execute(IJobExecutionContext context)
		{
			IList<AgentInfo> agents = _agentsRepo.GetAllItems();

			foreach (var agent in agents)
			{
				TimeSpan fromTime = _cpuMetricsRepo.GetTimeOfLatestMetricByAgentId(agent.Id);
				TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

				AllCpuMetricsResponses metricsFromAgent = _client.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest {
					FromTime = fromTime,
					ToTime = toTime,
					AgentBaseAddress = agent.Url
				});

				foreach (var metricDto in metricsFromAgent.Metrics)
				{
					_cpuMetricsRepo.AddItem(new CpuMetric {
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