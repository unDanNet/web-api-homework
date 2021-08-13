using System.Collections.Generic;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent.DTO.Responses
{
	public class AllHddMetricsResponse
	{
		public List<HddMetricDto> Metrics { get; set; }
	}
}