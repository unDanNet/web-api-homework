using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/metrics/dotnet")]
	public class DotNetMetricsController : ControllerBase
	{
		private readonly ILogger<DotNetMetricsController> _logger;

		public DotNetMetricsController(ILogger<DotNetMetricsController> logger)
		{
			_logger = logger;
		}
		
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentId)} = {agentId}, {nameof(fromTime)} = {fromTime}, " +
			                       $"{nameof(toTime)} = {toTime}");
			
			return Ok();
		}


		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			return Ok();
		}
	}
}