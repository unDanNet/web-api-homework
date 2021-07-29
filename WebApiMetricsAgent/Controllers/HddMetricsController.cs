using Microsoft.AspNetCore.Mvc;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/hdd")]
	public class HddMetricsController : ControllerBase
	{
		[HttpGet("left")]
		public IActionResult GetAvailableDiskSpaceLeft()
		{
			return Ok();
		}
	}
}