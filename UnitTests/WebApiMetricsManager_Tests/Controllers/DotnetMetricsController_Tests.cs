using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsManager.Controllers;
using WebApiMetricsManager.DAL.Interfaces;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	public class DotnetMetricsController_Tests
	{
		private DotNetMetricsController controller;
		private Mock<ILogger<DotNetMetricsController>> mock;
		private Mock<IDotnetMetricsRepository> dotnetMock;
	
		[SetUp]
		public void Setup()
		{
			mock = new Mock<ILogger<DotNetMetricsController>>();
			dotnetMock = new Mock<IDotnetMetricsRepository>();
			
			controller = new DotNetMetricsController(mock.Object, dotnetMock.Object);
		}
	
		[Test]
		public void GetMetricsFromAgent_ReturnsOk()
		{
			var agentId = 1;
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
	
			var result = controller.GetMetricsFromAgent(1, fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	
		[Test]
		public void GetMetricsFromAllCluster_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
	
			var result = controller.GetMetricsFromAllCluster(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}