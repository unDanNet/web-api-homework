using System;
using System.Text.Json.Serialization;

namespace Core.DTO.Entities
{
	public class HddMetricDto
	{
		public int Id { get; set; }
		public int SpaceLeft { get; set; }
		[JsonConverter(typeof(TimeSpanJsonConverter))] public TimeSpan Time { get; set; }
	}
}