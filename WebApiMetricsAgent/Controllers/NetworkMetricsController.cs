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
	[Route("api/metrics/network")]
	public class NetworkMetricsController : ControllerBase
	{
		private readonly ILogger<NetworkMetricsController> _logger;
		private readonly INetworkMetricsRepository _networkMetricsRepository;
		private readonly IMapper _mapper;

		public NetworkMetricsController(
			ILogger<NetworkMetricsController> logger,
			INetworkMetricsRepository networkMetricsRepository, 
			IMapper mapper
		) {
			_logger = logger;
			_networkMetricsRepository = networkMetricsRepository;
			_mapper = mapper;
		}
		
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");
			
			var metrics = _networkMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<NetworkMetric>();
			
			var response = new AllNetworkMetricsResponses {Metrics = new List<NetworkMetricDto>()};

			foreach (NetworkMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
			}
			
			return Ok(JsonSerializer.Serialize(response));
		}
	}
}