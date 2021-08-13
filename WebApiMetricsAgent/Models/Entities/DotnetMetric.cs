using System;

namespace WebApiMetricsAgent.Models.Entities
{
	public class DotnetMetric
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		public TimeSpan Time { get; set; }
	}
}