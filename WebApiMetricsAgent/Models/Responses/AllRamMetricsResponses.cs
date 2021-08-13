using System.Collections.Generic;
using WebApiMetricsAgent.Models.DTO;

namespace WebApiMetricsAgent.Models.Responses
{
	public class AllRamMetricsResponses
	{
		public List<RamMetricDto> Metrics { get; set; }
	}
}