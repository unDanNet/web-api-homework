using System;

namespace WebApiMetricsManager.DTO.Requests
{
	public class GetAllHddMetricsApiRequest
	{
		public Uri AgentBaseAddress { get; set; }
		public TimeSpan FromTime { get; set; }
		public TimeSpan ToTime { get; set; }
	}
}