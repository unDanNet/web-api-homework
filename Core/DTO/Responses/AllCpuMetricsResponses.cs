using System.Collections.Generic;
using Core.DTO.Entities;

namespace Core.DTO.Responses
{
	public class AllCpuMetricsResponses
	{
		public List<CpuMetricDto> Metrics { get; set; }
	}
}