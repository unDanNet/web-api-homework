using System;
using System.Collections.Generic;
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
		public IActionResult Index()
		{
			try
			{
				IList<AgentInfo> allAgents = _client.GetAllRegisteredAgents();
				
				return View(allAgents);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("add-agent")]
		public IActionResult AddAgent(string url)
		{
			try
			{
				AgentInfo addedAgent = _client.RegisterAgent(url);
				
				return PartialView("_TableRow", addedAgent);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpPut("enable-agent")]
		public IActionResult EnableAgent(int id)
		{
			try
			{
				_client.EnableAgent(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpPut("disable-agent")]
		public IActionResult DisableAgent(int id)
		{
			try
			{
				_client.DisableAgent(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("get-all-agents")]
		public IActionResult GetAllAgents()
		{
			try
			{
				IList<AgentInfo> allAgents = _client.GetAllRegisteredAgents();
				
				return PartialView("_DropdownList", allAgents);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}