using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class DotnetMetricJob : IJob
	{
		private readonly IDotnetMetricsRepository _repository;
		private readonly PerformanceCounter _dotnetCounter;
		
		public DotnetMetricJob(IDotnetMetricsRepository repository)
		{
			_repository = repository;
			_dotnetCounter = new PerformanceCounter(
				"ASP.NET Applications",
				"Error Events Raised",
				"__Total__"
			);
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			var errorsCount = Convert.ToInt32(_dotnetCounter.NextValue());
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			_repository.AddItem(new DotnetMetric {
				ErrorsCount = errorsCount,
				Time = time
			});
			
			return Task.CompletedTask;
		}
	}
}