using System;

namespace WebApiMetricsAgent.DAL.Models
{
	public class HddMetric
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		public TimeSpan Time { get; set; }
	}
}