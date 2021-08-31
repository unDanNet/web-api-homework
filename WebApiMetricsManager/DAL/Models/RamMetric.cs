using System;

namespace WebApiMetricsManager.DAL.Models
{
	public class RamMetric
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}