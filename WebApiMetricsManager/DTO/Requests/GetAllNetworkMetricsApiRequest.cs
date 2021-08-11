using System;

namespace WebApiMetricsManager.DTO.Requests
{
	public class GetAllNetworkMetricsApiRequest
	{
		public Uri AgentBaseAddress { get; set; }
		public TimeSpan FromTime { get; set; }
		public TimeSpan ToTime { get; set; }
	}
}