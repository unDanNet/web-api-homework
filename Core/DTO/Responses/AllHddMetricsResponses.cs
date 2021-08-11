using System.Collections.Generic;
using Core.DTO.Entities;

namespace Core.DTO.Responses
{
	public class AllHddMetricsResponses
	{
		public List<HddMetricDto> Metrics { get; set; }
	}
}