using System.Collections.Generic;
using WebApiMetricsAgent.Models.DTO;

namespace WebApiMetricsAgent.Models.Responses
{
	public class AllDotnetMetricResponses
	{
		public List<DotnetMetricDto> Metrics { get; set; }
	}
}