using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class CpuMetricsController_Tests
	{
		private CpuMetricsController controller;
		
		[SetUp]
		public void Setup()
		{
			controller = new CpuMetricsController();
		}

		
		[Test]
		public void GetMetricsForTimePeriod_WithPercentiles_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
			var percentile = 50;
			
			var result = controller.GetMetricsForTimePeriod(fromTime, toTime, percentile);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}

		[Test]
		public void GetMetricsForTimePeriod_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			var result = controller.GetMetricsForTimePeriod(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}