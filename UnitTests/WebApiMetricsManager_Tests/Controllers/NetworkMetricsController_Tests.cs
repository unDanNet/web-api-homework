using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.Controllers;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	[TestFixture]
	public class NetworkMetricsController_Tests
	{
		private NetworkMetricsController controller;
		private Mock<ILogger<NetworkMetricsController>> mock;
		private Mock<INetworkMetricsRepository> networkMock;
	
		[SetUp]
		public void Setup()
		{
			mock = new Mock<ILogger<NetworkMetricsController>>();
			networkMock = new Mock<INetworkMetricsRepository>();
			
			controller = new NetworkMetricsController(mock.Object, networkMock.Object);
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