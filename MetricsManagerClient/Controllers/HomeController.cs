using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetricsManagerClient.Client;
using MetricsManagerClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManagerClient.Controllers
{
	[Route("")]
	[Route("home")]
	public class HomeController : Controller
	{
		private readonly IMetricsManagerClient _client;
		
		public HomeController(IMetricsManagerClient client)
		{
			_client = client;
		}

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
			try
			{
				IList<AgentInfo> allAgents = await _client.GetAllRegisteredAgentsAsync();
				
				return View(allAgents);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("add-agent")]
		public async Task<IActionResult> AddAgent(string url)
		{
			try
			{
				AgentInfo addedAgent = await _client.RegisterAgentAsync(url);
				
				return PartialView("_TableRow", addedAgent);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpPut("enable-agent")]
		public async Task<IActionResult> EnableAgent(int id)
		{
			try
			{
				await _client.EnableAgentAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpPut("disable-agent")]
		public async Task<IActionResult> DisableAgent(int id)
		{
			try
			{
				await _client.DisableAgentAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("get-all-agents")]
		public async Task<IActionResult> GetAllAgents()
		{
			try
			{
				IList<AgentInfo> allAgents = await _client.GetAllRegisteredAgentsAsync();
				
				return PartialView("_DropdownList", allAgents);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}