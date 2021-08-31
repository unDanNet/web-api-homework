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
	public class DotnetMetricJob : IJob
	{
		private readonly IAgentsRepository _agentsRepo;
		private readonly IDotnetMetricsRepository _dotnetMetricsRepo;
		private readonly IMetricsAgentClient _client;
		
		public DotnetMetricJob(IAgentsRepository agentsRepo, IDotnetMetricsRepository dotnetMetricsRepo, IMetricsAgentClient client)
		{
			_agentsRepo = agentsRepo;
			_dotnetMetricsRepo = dotnetMetricsRepo;
			_client = client;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			IList<AgentInfo> agents = _agentsRepo.GetAllItems();

			foreach (var agent in agents)
			{
				TimeSpan fromTime = _dotnetMetricsRepo.GetTimeOfLatestMetricByAgentId(agent.Id);
				TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

				AllDotnetMetricsResponses metricsFromAgent = await _client.GetAllDotnetMetricsAsync(new GetAllDotnetMetricsApiRequest {
					FromTime = fromTime,
					ToTime = toTime,
					AgentBaseAddress = agent.Url
				});

				foreach (var metricDto in metricsFromAgent.Metrics)
				{
					_dotnetMetricsRepo.AddItem(new DotnetMetric {
						AgentId = agent.Id,
						Time = metricDto.Time,
						ErrorsCount = metricDto.ErrorsCount
					});
				}
			}
		}
	}
}