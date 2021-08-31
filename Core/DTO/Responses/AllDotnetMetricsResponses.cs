using System.Collections.Generic;
using Core.DTO.Entities;

namespace Core.DTO.Responses
{
	public class AllDotnetMetricsResponses
	{
		public List<DotnetMetricDto> Metrics { get; set; }
	}
}