using System.Collections.Generic;
using WebApiMetricsAgent.Models.DTO;

namespace WebApiMetricsAgent.Models.Responses
{
	public class AllCpuMetricsResponses
	{
		public List<CpuMetricDto> Metrics { get; set; }
	}
}