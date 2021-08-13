using System;

namespace WebApiMetricsAgent.Models.DTO
{
	public class RamMetricDto
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		public TimeSpan Time { get; set; }
	}
}