using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using MetricsManagerClient.Models;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient.Client
{
	public class MetricsManagerClient : IMetricsManagerClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<MetricsManagerClient> _logger;

		private const string managerServerUrl = "http://localhost:5002/api";
		
		public MetricsManagerClient(HttpClient client, ILogger<MetricsManagerClient> logger)
		{
			_client = client;
			_logger = logger;
		}

		public IList<AgentInfo> GetAllRegisteredAgents()
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/agents/list"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;

				return JsonSerializer.DeserializeAsync<IList<AgentInfo>>(responseStream).Result;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public AgentInfo RegisterAgent(string agentUrl)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Post, 
				$"{managerServerUrl}/agents/register?url={agentUrl}&enabled=true"
			);

			try
			{
				HttpResponseMessage response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				
				return JsonSerializer.DeserializeAsync<AgentInfo>(responseStream).Result;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public void EnableAgent(int agentId)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Put, 
				$"{managerServerUrl}/agents/enable/{agentId}"
			);

			try
			{ 
				_client.SendAsync(httpRequest);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public void DisableAgent(int agentId)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Put, 
				$"{managerServerUrl}/agents/disable/{agentId}"
			);

			try
			{
				_client.SendAsync(httpRequest);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public T GetLastMetric<T>(string type, int agentId)
		{
			var toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			var fromTime = toTime - TimeSpan.FromSeconds(60);
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;

				var metrics = JsonSerializer.DeserializeAsync<IList<T>>(responseStream).Result;
				
				return metrics[^1];
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public IList<T> GetMetricsFromSpecifiedTime<T>(string type, int agentId, TimeSpan fromTime)
		{
			var toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;

				return JsonSerializer.DeserializeAsync<IList<T>>(responseStream).Result;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public IList<T> GetMetricsToSpecifiedTime<T>(string type, int agentId, TimeSpan toTime)
		{
			var fromTime = new TimeSpan();
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;

				return JsonSerializer.DeserializeAsync<IList<T>>(responseStream).Result;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public IList<T> GetMetricsInSpecifiedTime<T>(string type, int agentId, TimeSpan fromTime, TimeSpan toTime)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = _client.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;

				return JsonSerializer.DeserializeAsync<IList<T>>(responseStream).Result;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}
	}
}