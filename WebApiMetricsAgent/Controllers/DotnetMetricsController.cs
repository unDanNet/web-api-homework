using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/dotnet")]
	public class DotnetMetricsController : ControllerBase
	{
		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetErrorCountForTimePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			return Ok();
		}
	}
}