using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsManager.Controllers;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	[TestFixture]
	public class HddMetricsController_Tests
	{
		private HddMetricsController controller;

		[SetUp]
		public void Setup()
		{
			controller = new HddMetricsController();
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