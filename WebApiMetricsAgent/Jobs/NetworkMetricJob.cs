using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class NetworkMetricJob : IJob
	{
		private readonly INetworkMetricsRepository _repository;
		private readonly PerformanceCounter _networkCounter;
		
		public NetworkMetricJob(INetworkMetricsRepository repository)
		{
			_repository = repository;
			_networkCounter = new PerformanceCounter(
				"Network Interface",
				"Packets/sec", 
				"Realtek PCIe GbE Family Controller"
			);
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			var packetsPerSecond = Convert.ToInt32(_networkCounter.NextValue());
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			_repository.AddItem(new NetworkMetric {
				Time = time,
				Value = packetsPerSecond
			});
			
			return Task.CompletedTask;
		}
	}
}