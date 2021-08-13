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
	[Route("api/metrics/ram")]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private readonly IRamMetricsRepository _ramMetricsRepository;
		private readonly IMapper _mapper;

		public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository ramMetricsRepository, IMapper mapper)
		{
			_logger = logger;
			_ramMetricsRepository = ramMetricsRepository;
			_mapper = mapper;
		}
		
		[HttpGet("available/from/{fromTime}/to/{toTime}")]
		public IActionResult GetAvailableRamLeft([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _ramMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<RamMetric>();
			
			var response = new AllRamMetricsResponses { Metrics = new List<RamMetricDto>() };

			foreach (RamMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
			}
			
			return Ok(response);
		}
	}
}