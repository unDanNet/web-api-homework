using System;

namespace WebApiMetricsAgent.Models.DTO
{
	public class DotnetMetricDto
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		public TimeSpan Time { get; set; }
	}
}