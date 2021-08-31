using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class DotnetMetricJob : IJob
	{
		private readonly IDotnetMetricsRepository _repository;
		private readonly ILogger<DotnetMetricJob> _logger;
		private readonly PerformanceCounter _dotnetCounter;
		
		public DotnetMetricJob(IDotnetMetricsRepository repository, ILogger<DotnetMetricJob> logger)
		{
			_repository = repository;
			_logger = logger;
			_dotnetCounter = new PerformanceCounter(
				"ASP.NET Applications",
				"Error Events Raised",
				"__Total__"
			);
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			try
			{
				var errorsCount = Convert.ToInt32(_dotnetCounter.NextValue());
				var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
				_repository.AddItem(new DotnetMetric {
					ErrorsCount = errorsCount,
					Time = time
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