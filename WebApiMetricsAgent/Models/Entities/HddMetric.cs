using System;

namespace WebApiMetricsAgent.Models.Entities
{
	public class HddMetric
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		public TimeSpan Time { get; set; }
	}
}