using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WebApiMetricsAgent;
using WebApiMetricsAgent.Controllers;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Repositories;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class CpuMetricsController_Tests
	{
		private CpuMetricsController controller;
		private Mock<ICpuMetricsRepository> repositoryMock;
		private Mock<ILogger<CpuMetricsController>> loggerMock;
		
		[SetUp]
		public void Setup()
		{
			repositoryMock = new Mock<ICpuMetricsRepository>();
			loggerMock = new Mock<ILogger<CpuMetricsController>>();
			var mapper = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile())).CreateMapper();
			
			controller = new CpuMetricsController(loggerMock.Object, repositoryMock.Object, mapper);
		}


		[Test]
		public void GetMetrics__ShouldCall_GetItemsByTimePeriod_From_Repository()
		{
			repositoryMock.Setup(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())
			).Verifiable();

			var result = controller.GetMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(50));
			
			repositoryMock.Verify(
				repository => repository.GetItemsByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
				Times.AtMostOnce
			);
		}
		

		[Test]
		public void GetMetrics__ReturnsOk()
		{
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			var result = controller.GetMetrics(fromTime, toTime);
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}