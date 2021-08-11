using System;

namespace Core.DTO.Entities
{
	public class NetworkMetricDto
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public TimeSpan Time { get; set; }
	}
}