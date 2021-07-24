using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsAgent.Models.DTO;
using WebApiMetricsAgent.Models.Entities;
using WebApiMetricsAgent.Models.Responses;
using WebApiMetricsAgent.Repositories;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/network")]
	public class NetworkMetricsController : ControllerBase
	{
		private readonly ILogger<NetworkMetricsController> _logger;
		private readonly INetworkMetricsRepository _networkMetricsRepository;

		public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository networkMetricsRepository)
		{
			_logger = logger;
			_networkMetricsRepository = networkMetricsRepository;
		}
		
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _networkMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<NetworkMetric>();
			
			var response = new AllNetworkMetricsResponses {Metrics = new List<NetworkMetricDto>()};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new NetworkMetricDto {
					Id = metric.Id,
					Value = metric.Value,
					Time = metric.Time
				});
			}
			
			return Ok(response);
		}
	}
}