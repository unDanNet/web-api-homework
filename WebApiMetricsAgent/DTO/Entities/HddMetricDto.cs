using System;

namespace WebApiMetricsAgent.DTO.Entities
{
	public class HddMetricDto
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		public TimeSpan Time { get; set; }
	}
}