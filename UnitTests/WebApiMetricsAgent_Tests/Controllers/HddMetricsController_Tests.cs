using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
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
		public void GetAvailableDiskSpaceLeft_ReturnsOk()
		{
			var result = controller.GetAvailableDiskSpaceLeft();
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}