using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/agents")]
	public class AgentsController : ControllerBase
	{
		private readonly ILogger<AgentsController> _logger;
		private readonly IAgentsRepository _repository;

		public AgentsController(ILogger<AgentsController> logger, IAgentsRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}
		
		[HttpGet("list")]
		public IActionResult GetRegisteredAgents()
		{
			_logger.LogInformation("No arguments passed");

			IList<AgentInfo> result = _repository.GetAllItems();
			
			return Ok(result);
		}
		
		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentInfo)} = {agentInfo}");
			
			_repository.AddItem(agentInfo);
			
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