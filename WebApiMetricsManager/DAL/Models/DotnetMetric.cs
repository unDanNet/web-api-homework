using System;
using System.Text.Json.Serialization;
using Core;

namespace WebApiMetricsManager.DAL.Models
{
	public class DotnetMetric
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}