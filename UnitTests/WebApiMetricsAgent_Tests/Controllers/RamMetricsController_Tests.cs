using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Repositories;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class RamMetricsController_Tests
	{
		private RamMetricsController controller;
		private Mock<IRamMetricsRepository> repositoryMock;
		private Mock<ILogger<RamMetricsController>> loggerMock;
	
		[SetUp]
		public void Setup()
		{
			repositoryMock = new Mock<IRamMetricsRepository>();
			loggerMock = new Mock<ILogger<RamMetricsController>>();
			
			controller = new RamMetricsController(loggerMock.Object, repositoryMock.Object);
		}

		[Test]
		public void GetAvailableRamLeft__ShouldCall_GetItemsByTimePeriod_From_Repository()
		{
			repositoryMock.Setup(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())
			).Verifiable();

			var result = controller.GetAvailableRamLeft(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(50));
			
			repositoryMock.Verify(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
				Times.AtMostOnce
			);
		}
	
		[Test]
		public void GetAvailableRamLeft__ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
			
			var result = controller.GetAvailableRamLeft(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}