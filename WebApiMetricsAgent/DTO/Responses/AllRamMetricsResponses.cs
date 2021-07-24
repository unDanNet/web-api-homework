using System.Collections.Generic;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent.DTO.Responses
{
	public class AllRamMetricsResponses
	{
		public List<RamMetricDto> Metrics { get; set; }
	}
}