using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsManager.Controllers;
using WebApiMetricsManager.Models;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	[TestFixture]
	public class AgentsController_Tests
	{
		private AgentsController controller;
		private Mock<ILogger<AgentsController>> mock;
	
		[SetUp]
		public void Setup()
		{
			mock = new Mock<ILogger<AgentsController>>();
			
			controller = new AgentsController(mock.Object);
		}
	
		[Test]
		public void GetRegisteredAgents_ReturnsOk()
		{
			var result = controller.GetRegisteredAgents();
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	
		[Test]
		public void RegisterAgent_ReturnsOk()
		{
			var agentInfo = new AgentInfo {
				Id = 1,
				Address = new Uri("https://sample.uri")
			};
	
			var result = controller.RegisterAgent(agentInfo);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	
		[Test]
		public void EnableAgentById_ReturnsOk()
		{
			var agentId = 1;
	
			var result = controller.EnableAgentById(agentId);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	
		[Test]
		public void DisableAgentById_ReturnsOk()
		{
			var agentId = 1;
	
			var result = controller.DisableAgentById(agentId);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}