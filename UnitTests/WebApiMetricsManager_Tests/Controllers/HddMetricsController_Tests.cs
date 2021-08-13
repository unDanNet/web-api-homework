﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsManager.Controllers;

namespace UnitTests.WebApiMetricsManager_Tests.Controllers
{
	[TestFixture]
	public class HddMetricsController_Tests
	{
		private HddMetricsController controller;
		private Mock<ILogger<HddMetricsController>> mock;
	
		[SetUp]
		public void Setup()
		{
			mock = new Mock<ILogger<HddMetricsController>>();
			
			controller = new HddMetricsController(mock.Object);
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