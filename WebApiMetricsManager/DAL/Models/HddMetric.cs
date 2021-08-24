using System;
using System.Text.Json.Serialization;
using Core;

namespace WebApiMetricsManager.DAL.Models
{
	public class HddMetric
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}