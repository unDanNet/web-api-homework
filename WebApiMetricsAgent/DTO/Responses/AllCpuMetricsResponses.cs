using System.Collections.Generic;
using WebApiMetricsAgent.DTO.Entities;

namespace WebApiMetricsAgent.DTO.Responses
{
	public class AllCpuMetricsResponses
	{
		public List<CpuMetricDto> Metrics { get; set; }
	}
}