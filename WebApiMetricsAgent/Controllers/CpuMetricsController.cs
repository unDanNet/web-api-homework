using System;
using System.Collections.Generic;
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
	/// Controller for gathering CPU metrics.
	/// </summary>
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


		/// <summary>
		/// Gather CPU metrics (% of processor time) at the specified time range.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/cpu/from/00.00:00:00/to/100000.00:00:00
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
			
			var metrics = _cpuMetricsRepository.GetItemsByTimePeriod(fromTime, toTime) ?? new List<CpuMetric>();
			
			var response = new AllCpuMetricsResponses { Metrics = new List<CpuMetricDto>() };

			foreach (CpuMetric metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}
			
			return Ok(response);
		}
	}
}