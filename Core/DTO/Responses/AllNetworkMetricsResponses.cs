using System.Collections.Generic;
using Core.DTO.Entities;

namespace Core.DTO.Responses
{
	public class AllNetworkMetricsResponses
	{
		public List<NetworkMetricDto> Metrics { get; set; }
	}
}