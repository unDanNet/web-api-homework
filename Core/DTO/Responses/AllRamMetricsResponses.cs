using System.Collections.Generic;
using Core.DTO.Entities;

namespace Core.DTO.Responses
{
	public class AllRamMetricsResponses
	{
		public List<RamMetricDto> Metrics { get; set; }
	}
}