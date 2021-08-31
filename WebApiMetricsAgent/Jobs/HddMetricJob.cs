using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Jobs
{
	public class HddMetricJob : IJob
	{
		private readonly IHddMetricsRepository _repository;
		private readonly ILogger<HddMetricJob> _logger;
		
		public HddMetricJob(IHddMetricsRepository repository, ILogger<HddMetricJob> logger)
		{
			_repository = repository;
			_logger = logger;
		}
		
		public Task Execute(IJobExecutionContext context)
		{
			int spaceLeft;

			try
			{
				var driveInfo = new DriveInfo(DriveInfo.GetDrives()[0].Name);
				spaceLeft = Convert.ToInt32((driveInfo.AvailableFreeSpace / 1024) / 1024); // convert Bytes to MBytes
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to read available free space from the drive");
				return Task.CompletedTask;
			}

			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			_repository.AddItem(new HddMetric {
				SpaceLeft = spaceLeft,
				Time = time
			});
			
			return Task.CompletedTask;
		}
	}
}