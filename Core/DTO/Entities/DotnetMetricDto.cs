using System;
using System.Text.Json.Serialization;

namespace Core.DTO.Entities
{
	public class DotnetMetricDto
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan Time { get; set; }
	}
}