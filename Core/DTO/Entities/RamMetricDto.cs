using System;

namespace Core.DTO.Entities
{
	public class RamMetricDto
	{
		public int Id { get; set; }
		public int MemoryAvailable { get; set; }
		public TimeSpan Time { get; set; }
	}
}