using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class NetworkMetricJob : IJob
	{
		private readonly INetworkMetricsRepository _repository;
		private readonly PerformanceCounter _networkCounter;
		private readonly ILogger<NetworkMetricJob> _logger;
		
		public NetworkMetricJob(INetworkMetricsRepository repository, ILogger<NetworkMetricJob> logger)
		{
			_repository = repository;
			_logger = logger;
			_networkCounter = new PerformanceCounter(
				"Network Interface",
				"Packets/sec",
				"Realtek PCIe GbE Family Controller"
			);
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			try
			{
				var packetsPerSecond = Convert.ToInt32(_networkCounter.NextValue());
				var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
				_repository.AddItem(new NetworkMetric {
					Time = time,
					Value = packetsPerSecond
				});
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
			
			return Task.CompletedTask;
		}
	}
}