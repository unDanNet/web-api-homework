using System;
using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using Core.DTO.Entities;
using Core.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/cpu")]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private readonly ICpuMetricsRepository _cpuMetricsRepository;
		private readonly IMapper _mapper;

		public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository cpuMetricsRepository, IMapper mapper)
		{
			_logger = logger;
			_cpuMetricsRepository = cpuMetricsRepository;
			_mapper = mapper;
		}


		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _cpuMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<CpuMetric>();
			
			var response = new AllCpuMetricsResponses { Metrics = new List<CpuMetricDto>() };

			foreach (CpuMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}
			
			return Ok(JsonSerializer.Serialize(response));
		}
	}
}