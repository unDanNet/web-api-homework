using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;
using WebApiMetricsAgent.Repositories;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class HddMetricsController_Tests
	{
		private HddMetricsController controller;
		private Mock<IHddMetricsRepository> repositoryMock;
		private Mock<ILogger<HddMetricsController>> loggerMock;
	
		[SetUp]
		public void Setup()
		{
			repositoryMock = new Mock<IHddMetricsRepository>();
			loggerMock = new Mock<ILogger<HddMetricsController>>();
			
			controller = new HddMetricsController(loggerMock.Object, repositoryMock.Object);
		}

		[Test]
		public void GetAvailableDiskSpaceLeft__ShouldCall_GetItemsByTimePeriod_From_Repository()
		{
			repositoryMock.Setup(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())
			).Verifiable();

			var result = controller.GetAvailableDiskSpaceLeft(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(50));
			
			repositoryMock.Verify(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
				Times.AtMostOnce
			);
		}
	
		[Test]
		public void GetAvailableDiskSpaceLeft_ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
			
			var result = controller.GetAvailableDiskSpaceLeft(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}