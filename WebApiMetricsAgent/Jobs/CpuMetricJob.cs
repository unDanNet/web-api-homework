using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class CpuMetricJob : IJob
	{
		private readonly ICpuMetricsRepository _repository;
		private readonly PerformanceCounter _cpuCounter;
		private readonly ILogger<CpuMetricJob> _logger;
		
		public CpuMetricJob(ICpuMetricsRepository repository, ILogger<CpuMetricJob> logger)
		{
			_repository = repository;
			_logger = logger;
			_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			try
			{
				var value = Convert.ToInt32(_cpuCounter.NextValue());
				var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
				_repository.AddItem(new CpuMetric {
					Time = time,
					Value = value
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