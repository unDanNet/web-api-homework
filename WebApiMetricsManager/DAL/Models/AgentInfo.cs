using System;

namespace WebApiMetricsManager.DAL.Models
{
	public class AgentInfo
	{
		public int Id { get; set; }
		public Uri Url { get; set; }
		public bool Enabled { get; set; }
	}
}