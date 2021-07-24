using System;

namespace WebApiMetricsAgent.Models.DTO
{
	public class HddMetricDto
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		public TimeSpan Time { get; set; }
	}
}