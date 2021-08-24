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
	/// Controller for gathering .NET metrics.
	/// </summary>
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
		
		
		/// <summary>
		/// Gather .NET metrics (error events raised) at the specified time range.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/dotnet/errors-count/from/00.00:00:00/to/100000.00:00:00
		/// 
		/// </remarks>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>A list of metrics gathered during the specified time period.</returns>
		/// <response code="200">if metrics were gathered successfully.</response>
		/// <response code="400">if specified arguments were incorrect.</response>
		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetErrorCount([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");

			var metrics = _dotnetMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<DotnetMetric>();
			
			var response = new AllDotnetMetricsResponses {Metrics = new List<DotnetMetricDto>()};

			foreach (DotnetMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<DotnetMetricDto>(metric));
			}
			
			return Ok(JsonSerializer.Serialize(response));
		}
	}
}