using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApiMetricsAgent;
using WebApiMetricsAgent.Controllers;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Repositories;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class DotnetMetricsController_Tests
	{
		private DotnetMetricsController controller;
		private Mock<IDotnetMetricsRepository> repositoryMock;
		private Mock<ILogger<DotnetMetricsController>> loggerMock;
	
		[SetUp]
		public void Setup()
		{
			repositoryMock = new Mock<IDotnetMetricsRepository>();
			loggerMock = new Mock<ILogger<DotnetMetricsController>>();
			var mapper = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile())).CreateMapper();
			
			controller = new DotnetMetricsController(loggerMock.Object, repositoryMock.Object, mapper);
		}

		[Test]
		public void GetErrorCount__ShouldCall_GetItemsByTimePeriod_From_Repository()
		{
			repositoryMock.Setup(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())
			).Verifiable();

			var result = controller.GetErrorCount(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(50));
			
			repositoryMock.Verify(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
				Times.AtMostOnce
			);
		}
	
		[Test]
		public void GetErrorCount__ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
	
			var result = controller.GetErrorCount(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}