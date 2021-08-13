using System;

namespace WebApiMetricsAgent.DAL.Models
{
	public class RamMetric
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		public TimeSpan Time { get; set; }
	}
}