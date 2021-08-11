using System;

namespace Core.DTO.Entities
{
	public class DotnetMetricDto
	{
		public int Id { get; set; }
		public int ErrorsCount { get; set; }
		public TimeSpan Time { get; set; }
	}
}