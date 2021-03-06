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
	public class NetworkMetricsController_Tests
	{
		private NetworkMetricsController controller;
		private Mock<INetworkMetricsRepository> repositoryMock;
		private Mock<ILogger<NetworkMetricsController>> loggerMock;
	
		[SetUp]
		public void Setup()
		{
			repositoryMock = new Mock<INetworkMetricsRepository>();
			loggerMock = new Mock<ILogger<NetworkMetricsController>>();
			var mapper = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile())).CreateMapper();
			
			controller = new NetworkMetricsController(loggerMock.Object, repositoryMock.Object, mapper);
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