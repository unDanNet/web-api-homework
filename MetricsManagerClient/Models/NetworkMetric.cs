using System;
using System.Text.Json.Serialization;
using Core;

namespace MetricsManagerClient.Models
{
	public class NetworkMetric
	{
		public int Id { get; set; }
		public int Value { get; set; }
		[JsonConverter(typeof(TimeSpanJsonConverter))] public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}