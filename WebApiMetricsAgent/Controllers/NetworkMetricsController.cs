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
	/// <summary>
	/// Controller for gathering Network metrics.
	/// </summary>
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
		
		
		/// <summary>
		/// Gather Network metrics (packets per second) at the specified time range.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/network/from/00.00:00:00/to/100000.00:00:00
		/// 
		/// </remarks>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>A list of metrics gathered during the specified time period.</returns>
		/// <response code="200">if metrics were gathered successfully.</response>
		/// <response code="400">if specified arguments were incorrect.</response>
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