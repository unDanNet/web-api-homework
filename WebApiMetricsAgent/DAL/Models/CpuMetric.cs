using System;

namespace WebApiMetricsAgent.DAL.Models
{
	public class CpuMetric
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public TimeSpan Time { get; set; }
	}
}