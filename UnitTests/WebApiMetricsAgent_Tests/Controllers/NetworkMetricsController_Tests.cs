using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class NetworkMetricsController_Tests
	{
		private NetworkMetricsController controller;

		[SetUp]
		public void Setup()
		{
			controller = new NetworkMetricsController();
		}

		[Test]
		public void GetNetworkMetricsForTimePeriod_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			var result = controller.GetNetworkMetricsForTimePeriod(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}