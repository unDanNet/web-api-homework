using System;
using System.Text.Json.Serialization;

namespace Core.DTO.Entities
{
	public class RamMetricDto
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan Time { get; set; }
	}
}