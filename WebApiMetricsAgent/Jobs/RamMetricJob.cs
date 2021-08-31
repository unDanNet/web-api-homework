using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class RamMetricJob : IJob
	{
		private readonly IRamMetricsRepository _repository;
		private readonly PerformanceCounter _ramCounter;
		private readonly ILogger<RamMetricJob> _logger;
		
		public RamMetricJob(IRamMetricsRepository repository, ILogger<RamMetricJob> logger)
		{
			_repository = repository;
			_logger = logger;
			_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			try
			{
				var memoryAvailable = Convert.ToInt32(_ramCounter.NextValue());
				var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
				_repository.AddItem(new RamMetric {
					Time = time,
					MemoryAvailable = memoryAvailable
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