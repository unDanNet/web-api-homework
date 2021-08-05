using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class CpuMetricJob : IJob
	{
		private readonly ICpuMetricsRepository _repository;
		private readonly PerformanceCounter _cpuCounter;
		
		public CpuMetricJob(ICpuMetricsRepository repository)
		{
			_repository = repository;
			_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			var value = Convert.ToInt32(_cpuCounter.NextValue());
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			_repository.AddItem(new CpuMetric {
				Time = time,
				Value = value
			});
			
			return Task.CompletedTask;
		}
	}
}