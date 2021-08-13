using System;

namespace WebApiMetricsAgent.Models.Entities
{
	public class RamMetric
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		public TimeSpan Time { get; set; }
	}
}