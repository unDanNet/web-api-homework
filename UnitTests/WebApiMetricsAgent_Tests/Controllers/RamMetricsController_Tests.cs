using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiMetricsAgent.Controllers;

namespace UnitTests.WebApiMetricsAgent_Tests.Controllers
{
	[TestFixture]
	public class RamMetricsController_Tests
	{
		private RamMetricsController controller;

		[SetUp]
		public void Setup()
		{
			controller = new RamMetricsController();
		}

		[Test]
		public void GetAvailableRamLeft()
		{
			var result = controller.GetAvailableRamLeft();
			
			Assert.IsInstanceOf<IActionResult>(result);
		}
	}
}