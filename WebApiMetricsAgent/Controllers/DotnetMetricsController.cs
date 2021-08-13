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
	[Route("api/metrics/dotnet")]
	public class DotnetMetricsController : ControllerBase
	{
		private readonly ILogger<DotnetMetricsController> _logger;
		private readonly IDotnetMetricsRepository _dotnetMetricsRepository;

		public DotnetMetricsController(ILogger<DotnetMetricsController> logger, IDotnetMetricsRepository dotnetMetricsRepository)
		{
			_logger = logger;
			_dotnetMetricsRepository = dotnetMetricsRepository;
		}
		
		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetErrorCount([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");

			var metrics = _dotnetMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<DotnetMetric>();
			
			var response = new AllDotnetMetricResponses {Metrics = new List<DotnetMetricDto>()};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new DotnetMetricDto {
					Id = metric.Id,
					ErrorsCount = metric.ErrorsCount,
					Time = metric.Time
				});
			}
			
			return Ok(response);
		}
	}
}