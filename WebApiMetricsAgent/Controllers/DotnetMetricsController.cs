using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;
using WebApiMetricsAgent.DAL.Repositories;
using WebApiMetricsAgent.DTO.Entities;
using WebApiMetricsAgent.DTO.Responses;

namespace WebApiMetricsAgent.Controllers
{
	[ApiController]
	[Route("api/metrics/dotnet")]
	public class DotnetMetricsController : ControllerBase
	{
		private readonly ILogger<DotnetMetricsController> _logger;
		private readonly IDotnetMetricsRepository _dotnetMetricsRepository;
		private readonly IMapper _mapper;

		public DotnetMetricsController (
			ILogger<DotnetMetricsController> logger, 
			IDotnetMetricsRepository dotnetMetricsRepository,
			IMapper mapper
		) {
			_logger = logger;
			_dotnetMetricsRepository = dotnetMetricsRepository;
			_mapper = mapper;
		}
		
		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetErrorCount([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");

			var metrics = _dotnetMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<DotnetMetric>();
			
			var response = new AllDotnetMetricResponses {Metrics = new List<DotnetMetricDto>()};

			foreach (DotnetMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<DotnetMetricDto>(metric));
			}
			
			return Ok(response);
		}
	}
}