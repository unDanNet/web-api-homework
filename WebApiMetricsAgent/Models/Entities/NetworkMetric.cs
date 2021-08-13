using System;

namespace WebApiMetricsAgent.Models.Entities
{
	public class NetworkMetric
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public TimeSpan Time { get; set; }
	}
}