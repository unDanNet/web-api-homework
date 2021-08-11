using System;
using System.Collections.Generic;
using Core.DTO.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiMetricsManager.Client;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;
using WebApiMetricsManager.DTO.Requests;

namespace WebApiMetricsManager.Controllers
{
	[ApiController]
	[Route("api/metrics/dotnet")]
	public class DotNetMetricsController : ControllerBase
	{
		private readonly ILogger<DotNetMetricsController> _logger;
		private readonly IDotnetMetricsRepository _repository;

		public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotnetMetricsRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}
		
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation("Starting new request to metrics agent");

			IList<DotnetMetric> result = _repository.GetItemsByAgentId(agentId, fromTime, toTime);
			
			return Ok(result);
		}


		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation($"Arguments taken: {nameof(fromTime)} = {fromTime}, {nameof(toTime)} = {toTime}");

			IList<DotnetMetric> result = _repository.GetItemsByTimePeriod(fromTime, toTime);
			
			return Ok(result);
		}
	}
}