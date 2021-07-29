using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/network")]
	public class NetworkMetricsController : ControllerBase
	{
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetNetworkMetricsForTimePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			return Ok();
		}
	}
}