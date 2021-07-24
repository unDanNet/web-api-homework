using System;

namespace WebApiMetricsAgent.DTO.Entities
{
	public class CpuMetricDto
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public TimeSpan Time { get; set; }
	}
}