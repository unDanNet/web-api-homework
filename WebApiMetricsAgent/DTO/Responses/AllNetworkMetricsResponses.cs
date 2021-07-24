using System.Collections.Generic;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent.DTO.Responses
{
	public class AllNetworkMetricsResponses
	{
		public List<NetworkMetricDto> Metrics { get; set; }
	}
}