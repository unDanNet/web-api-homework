using System;
using System.Text.Json.Serialization;
using Core;

namespace WebApiMetricsManager.DAL.Models
{
	public class RamMetric
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan Time { get; set; }
		public int AgentId { get; set; }
	}
}