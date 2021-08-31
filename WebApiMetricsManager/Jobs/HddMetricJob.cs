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
	public class HddMetricJob : IJob
	{
		private readonly IAgentsRepository _agentsRepo;
		private readonly IHddMetricsRepository _hddMetricsRepo;
		private readonly IMetricsAgentClient _client;
		
		public HddMetricJob(IAgentsRepository agentsRepo, IHddMetricsRepository hddMetricsRepo, IMetricsAgentClient client)
		{
			_agentsRepo = agentsRepo;
			_hddMetricsRepo = hddMetricsRepo;
			_client = client;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			IList<AgentInfo> agents = _agentsRepo.GetAllItems();

			foreach (var agent in agents)
			{
				TimeSpan fromTime = _hddMetricsRepo.GetTimeOfLatestMetricByAgentId(agent.Id);
				TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

				AllHddMetricsResponses metricsFromAgent = await _client.GetAllHddMetricsAsync(new GetAllHddMetricsApiRequest {
					FromTime = fromTime,
					ToTime = toTime,
					AgentBaseAddress = agent.Url
				});

				foreach (var metricDto in metricsFromAgent.Metrics)
				{
					_hddMetricsRepo.AddItem(new HddMetric {
						AgentId = agent.Id,
						Time = metricDto.Time,
						SpaceLeft = metricDto.SpaceLeft
					});
				}
			}
		}
	}
}