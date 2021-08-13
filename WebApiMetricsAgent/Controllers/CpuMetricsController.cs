using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsAgent.Interfaces;
using WebApiMetricsAgent.Models.DTO;
using WebApiMetricsAgent.Models.Entities;
using WebApiMetricsAgent.Models.Responses;
using WebApiMetricsAgent.Repositories;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/cpu")]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private readonly ICpuMetricsRepository _cpuMetricsRepository;

		public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository cpuMetricsRepository)
		{
			_logger = logger;
			_cpuMetricsRepository = cpuMetricsRepository;
		}


		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _cpuMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<CpuMetric>();
			
			var response = new AllCpuMetricsResponses { Metrics = new List<CpuMetricDto>() };

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new CpuMetricDto {
					Time = metric.Time,
					Id = metric.Id,
					Value = metric.Value
				});
			}
			
			return Ok(response);
		}
	}
}