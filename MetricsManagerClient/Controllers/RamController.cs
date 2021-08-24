using System;
using System.Linq;
using MetricsManagerClient.Client;
using MetricsManagerClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManagerClient.Controllers
{
	[Route("ram")]
	public class RamController : Controller
	{
		private readonly IMetricsManagerClient _client;
		private const string metricType = "ram";
		
		public RamController(IMetricsManagerClient client)
		{
			_client = client;
		}
		
		[HttpPost("get-last-metric")]
		public IActionResult GetLastMetric(int agentId)
		{
			try
			{
				var metric = _client.GetLastMetric<RamMetric>(metricType, agentId);

				return new JsonResult(new MetricDto {
					Time = metric.Time.TotalMilliseconds,
					Value = metric.MemoryAvailable
				});
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("get-metrics-from-specified-time")]
		public IActionResult GetMetricsFromSpecifiedTime(int agentId, DateTimeOffset startTime)
		{
			var fromTime = TimeSpan.FromSeconds(startTime.ToUnixTimeSeconds());

			try
			{
				var metrics = _client.GetMetricsFromSpecifiedTime<RamMetric>(metricType, agentId, fromTime);
				
				return new JsonResult(
					metrics.Select(m => new MetricDto {
						Time = m.Time.TotalMilliseconds,
						Value = m.MemoryAvailable
					}).ToList()
				);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[HttpPost("get-metrics-to-specified-time")]
		public IActionResult GetMetricsToSpecifiedTime(int agentId, DateTimeOffset endTime)
		{
			var toTime = TimeSpan.FromSeconds(endTime.ToUnixTimeSeconds());

			try
			{
				var metrics = _client.GetMetricsToSpecifiedTime<RamMetric>(metricType, agentId, toTime);

				return new JsonResult(
					metrics.Select(m => new MetricDto {
						Time = m.Time.TotalMilliseconds,
						Value = m.MemoryAvailable
					}).ToList()
				);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[HttpPost("get-metrics-in-specified-period")]
		public IActionResult GetMetricsInSpecifiedPeriod(int agentId, DateTimeOffset startTime, DateTimeOffset endTime)
		{
			var fromTime = TimeSpan.FromSeconds(startTime.ToUnixTimeSeconds());
			var toTime = TimeSpan.FromSeconds(endTime.ToUnixTimeSeconds());

			try
			{
				var metrics = _client.GetMetricsInSpecifiedTime<RamMetric>(metricType, agentId, fromTime, toTime);
			
				return new JsonResult(
					metrics.Select(m => new MetricDto {
						Time = m.Time.TotalMilliseconds,
						Value = m.MemoryAvailable
					}).ToList()
				);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}