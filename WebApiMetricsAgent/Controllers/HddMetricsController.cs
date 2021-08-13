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
	[Route("api/metrics/hdd")]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private readonly IHddMetricsRepository _hddMetricsRepository;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository hddMetricsRepository)
		{
			_logger = logger;
			_hddMetricsRepository = hddMetricsRepository;
		}
		
		[HttpGet("left/from/{fromTime}/to/{toTime}")]
		public IActionResult GetAvailableDiskSpaceLeft(TimeSpan fromTime, TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _hddMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<HddMetric>();
			
			var response = new AllHddMetricsResponse {Metrics = new List<HddMetricDto>()};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new HddMetricDto {
					Id = metric.Id,
					SpaceLeft = metric.SpaceLeft,
					Time = metric.Time
				});
			}
			
			return Ok(response);
		}
	}
}