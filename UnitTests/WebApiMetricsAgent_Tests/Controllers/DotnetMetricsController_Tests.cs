using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class DotnetMetricsController_Tests
	{
		private DotnetMetricsController controller;

		[SetUp]
		public void Setup()
		{
			controller = new DotnetMetricsController();
		}

		[Test]
		public void GetErrorCountForTimePeriod_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			var result = controller.GetErrorCountForTimePeriod(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}