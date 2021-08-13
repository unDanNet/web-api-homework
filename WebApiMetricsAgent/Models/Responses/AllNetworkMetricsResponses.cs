using System.Collections.Generic;
using WebApiMetricsAgent.Models.DTO;

namespace WebApiMetricsAgent.Models.Responses
{
	public class AllNetworkMetricsResponses
	{
		public List<NetworkMetricDto> Metrics { get; set; }
	}
}