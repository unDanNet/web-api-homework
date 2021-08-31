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
	/// Controller for gathering RAM metrics.
	/// </summary>
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
		
		/// <summary>
		/// Gather RAM metrics (MBytes of available memory) at the specified time range.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/ram/available/from/00.00:00:00/to/100000.00:00:00
		/// 
		/// </remarks>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>A list of metrics gathered during the specified time period.</returns>
		/// <response code="200">if metrics were gathered successfully.</response>
		/// <response code="400">if specified arguments were incorrect.</response>
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
			
			return Ok(JsonSerializer.Serialize(response));
		}
	}
}