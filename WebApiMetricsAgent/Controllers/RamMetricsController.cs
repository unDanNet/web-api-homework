using Microsoft.AspNetCore.Mvc;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/ram")]
	public class RamMetricsController : ControllerBase
	{
		[HttpGet("available")]
		public IActionResult GetAvailableRamLeft()
		{
			return Ok();
		}
	}
}