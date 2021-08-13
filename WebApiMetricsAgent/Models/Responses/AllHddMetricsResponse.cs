using System.Collections.Generic;
using WebApiMetricsAgent.Models.DTO;

namespace WebApiMetricsAgent.Models.Responses
{
	public class AllHddMetricsResponse
	{
		public List<HddMetricDto> Metrics { get; set; }
	}
}