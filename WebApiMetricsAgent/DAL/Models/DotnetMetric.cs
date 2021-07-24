using System;

namespace WebApiMetricsAgent.DAL.Models
{
	public class DotnetMetric
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		public TimeSpan Time { get; set; }
	}
}