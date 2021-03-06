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
	[Route("api/metrics/hdd")]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private readonly IHddMetricsRepository _hddMetricsRepository;
		private readonly IMapper _mapper;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository hddMetricsRepository, IMapper mapper)
		{
			_logger = logger;
			_hddMetricsRepository = hddMetricsRepository;
			_mapper = mapper;
		}
		
		[HttpGet("left/from/{fromTime}/to/{toTime}")]
		public IActionResult GetAvailableDiskSpaceLeft(TimeSpan fromTime, TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _hddMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<HddMetric>();
			
			var response = new AllHddMetricsResponses {Metrics = new List<HddMetricDto>()};

			foreach (HddMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
			}
			
			return Ok(JsonSerializer.Serialize(response));
		}
	}
}