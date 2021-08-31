using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class RamMetricJob : IJob
	{
		private readonly IRamMetricsRepository _repository;
		private readonly PerformanceCounter _ramCounter;
		
		public RamMetricJob(IRamMetricsRepository repository)
		{
			_repository = repository;
			_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			var memoryAvailable = Convert.ToInt32(_ramCounter.NextValue());
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			_repository.AddItem(new RamMetric {
				Time = time,
				MemoryAvailable = memoryAvailable
			});
			
			return Task.CompletedTask;
		}
	}
}