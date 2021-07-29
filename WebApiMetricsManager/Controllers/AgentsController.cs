using System;
using Microsoft.AspNetCore.Mvc;
using WebApiMetricsManager.Models;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/agents")]
	public class AgentsController : ControllerBase
	{
		[HttpGet("list")]
		public IActionResult GetRegisteredAgents()
		{
			return Ok();
		}
		
		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			return Ok();
		}

		[HttpPut("enable/{agentId}")]
		public IActionResult EnableAgentById([FromRoute] int agentId)
		{
			return Ok();
		}

		[HttpPut("disable/{agentId}")]
		public IActionResult DisableAgentById([FromRoute] int agentId)
		{
			return Ok();
		}
	}
}