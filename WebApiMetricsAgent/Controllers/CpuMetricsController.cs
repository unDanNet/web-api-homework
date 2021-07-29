using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/cpu")]
	public class CpuMetricsController : ControllerBase
	{
		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsForTimePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, [FromRoute] int percentile)
		{
			return Ok();
		}


		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsForTimePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			return Ok();
		}
	}
}