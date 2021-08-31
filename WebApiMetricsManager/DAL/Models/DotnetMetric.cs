using System;

namespace WebApiMetricsManager.DAL.Models
{
	public class DotnetMetric
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}