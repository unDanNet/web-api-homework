using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsManager.Client;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/metrics/hdd")]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private readonly IHddMetricsRepository _repository;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}
		
		
		/// <summary>
		/// Get HDD metrics (disk space left) at the specified time range from the agent with specified Id.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/hdd/agent/1/from/00.00:00:00/to/100000.00:00:00
		/// 
		/// </remarks>
		/// <param name="agentId">Id of the agent to get metrics from.</param>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>A list of metrics gathered during the specified time period from the specified agent.</returns>
		/// <response code="200">if metrics were got from the agent successfully.</response>
		/// <response code="400">if specified arguments were incorrect.</response>
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation("Starting new request to metrics agent");

			IList<HddMetric> result = _repository.GetItemsByAgentId(agentId, fromTime, toTime);
			
			return Ok(result);
		}


		/// <summary>
		/// Get HDD metrics (disk space left) at the specified time range from all registered agents.
		/// </summary>
		/// <remarks>
		/// Request example:
		/// 
		/// 	GET api/metrics/hdd/cluster/from/00.00:00:00/to/100000.00:00:00
		/// 
		/// </remarks>
		/// <param name="fromTime">The start time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <param name="toTime">The end time (passed from 01.01.1970 00:00:00) of the time range.</param>
		/// <returns>A list of metrics gathered during the specified time period from all registered agents.</returns>
		/// <response code="200">if metrics were got from the agents successfully.</response>
		/// <response code="400">if specified arguments were incorrect.</response>
		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");

			IList<HddMetric> result = _repository.GetItemsByTimePeriod(fromTime, toTime);

			return Ok(result);
		}
	}
}