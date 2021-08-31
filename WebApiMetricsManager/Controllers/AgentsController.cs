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
		
		
		/// <summary>
		/// Get all registered agents in the system.
		/// </summary>
		/// <remarks>
		/// Request example:
		///
		///		GET api/agents/list
		/// 
		/// </remarks>
		/// <returns>The list of all registered agents.</returns>
		[HttpGet("list")]
		public IActionResult GetRegisteredAgents()
		{
			_logger.LogInformation("No arguments passed");

			IList<AgentInfo> result = _repository.GetAllItems();
			
			return Ok(result);
		}
		
		
		/// <summary>
		/// Register a new agent to gather metrics.
		/// </summary>
		/// <remarks>
		/// Request example:
		///
		///		POST api/agents/register?url=localhost:5000
		/// 
		/// </remarks>
		/// <returns>The list of all registered agents.</returns>
		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentInfo)} = {agentInfo}");
			
			_repository.AddItem(agentInfo);
			
			return Ok();
		}

		
		/// <summary>
		/// Enable the agent.
		/// </summary>
		/// <param name="agentId">Id of the agent to enable.</param>
		[HttpPut("enable/{agentId}")]
		public IActionResult EnableAgentById([FromRoute] int agentId)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentId)} = {agentId}");

			return Ok();
		}

		/// <summary>
		/// Disable the agent.
		/// </summary>
		/// <param name="agentId">Id of the agent to disable.</param>
		[HttpPut("disable/{agentId}")]
		public IActionResult DisableAgentById([FromRoute] int agentId)
		{
			_logger.LogInformation($"Arguments taken: {nameof(agentId)} = {agentId}");

			return Ok();
		}
	}
}