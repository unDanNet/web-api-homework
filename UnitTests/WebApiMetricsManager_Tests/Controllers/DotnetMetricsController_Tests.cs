using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsManager.Controllers;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	public class DotnetMetricsController_Tests
	{
		private DotNetMetricsController controller;

		[SetUp]
		public void Setup()
		{
			controller = new DotNetMetricsController();
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