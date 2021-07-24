using System;

namespace WebApiMetricsAgent.Models.DTO
{
	public class CpuMetricDto
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public TimeSpan Time { get; set; }
	}
}