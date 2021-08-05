using System;
using Quartz;

namespace WebApiMetricsAgent.Jobs.Utils
{
	public class JobSchedule
	{
		public Type JobType { get; }
		public string CronExpression { get; }
		
		public JobSchedule(Type jobType, string cronExpression)
		{
			JobType = jobType;
			CronExpression = cronExpression;
		}
	}
}