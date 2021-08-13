using System.Collections.Generic;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent.DTO.Responses
{
	public class AllDotnetMetricResponses
	{
		public List<DotnetMetricDto> Metrics { get; set; }
	}
}