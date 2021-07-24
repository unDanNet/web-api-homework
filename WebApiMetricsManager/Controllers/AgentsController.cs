using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsManager.Models;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/agents")]
	public class AgentsController : ControllerBase
	{
		private readonly ILogger<AgentsController> _logger;

		public AgentsController(ILogger<AgentsController> logger)
		{
			_logger = logger;
		}
		
		[HttpGet("list")]
		public IActionResult GetRegisteredAgents()
		{
			_logger.LogInformation("No arguments passed");
			
			return Ok();
		}
		
		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentInfo)} = {agentInfo}");
			
			return Ok();
		}

		[HttpPut("enable/{agentId}")]
		public IActionResult EnableAgentById([FromRoute] int agentId)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentId)} = {agentId}");
			
			return Ok();
		}

		[HttpPut("disable/{agentId}")]
		public IActionResult DisableAgentById([FromRoute] int agentId)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentId)} = {agentId}");
			return Ok();
		}
	}
}