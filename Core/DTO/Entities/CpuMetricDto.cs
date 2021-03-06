using System;
using System.Text.Json.Serialization;

namespace Core.DTO.Entities
{
	public class CpuMetricDto
	{
		public int Id { get; set; }
		public int Value { get; set; }
		[JsonConverter(typeof(TimeSpanJsonConverter))] public TimeSpan Time { get; set; }
	}
}