using System;

namespace WebApiMetricsManager.DAL.Models
{
	public class HddMetric
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}