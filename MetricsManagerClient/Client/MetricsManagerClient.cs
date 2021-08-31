using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
		

		public async Task<IList<AgentInfo>> GetAllRegisteredAgentsAsync()
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/agents/list"
			);

			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<IList<AgentInfo>>(responseStream);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task<AgentInfo> RegisterAgentAsync(string agentUrl)
		{

			try
			{
				var testRequest = new HttpRequestMessage(HttpMethod.Get, agentUrl);

				await _client.SendAsync(testRequest);
			}
			catch (Exception e)
			{
				throw new HttpRequestException("Attempted to add an agent that does not respond to http requests.");
			}
			
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Post, 
				$"{managerServerUrl}/agents/register?url={agentUrl}&enabled=true"
			);

			
			try
			{
				HttpResponseMessage response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();
				
				return await JsonSerializer.DeserializeAsync<AgentInfo>(responseStream);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task EnableAgentAsync(int agentId)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Put, 
				$"{managerServerUrl}/agents/enable/{agentId}"
			);

			try
			{ 
				await _client.SendAsync(httpRequest);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task DisableAgentAsync(int agentId)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Put, 
				$"{managerServerUrl}/agents/disable/{agentId}"
			);

			try
			{
				await _client.SendAsync(httpRequest);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task<T> GetLastMetricAsync<T>(string type, int agentId)
		{
			var toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			var fromTime = toTime - TimeSpan.FromSeconds(60);
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				var metrics = await JsonSerializer.DeserializeAsync<IList<T>>(responseStream);
				
				return metrics[^1];
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task<IList<T>> GetMetricsFromSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan fromTime)
		{
			var toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<IList<T>>(responseStream);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task<IList<T>> GetMetricsToSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan toTime)
		{
			var fromTime = new TimeSpan();
			
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<IList<T>>(responseStream);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}

		public async Task<IList<T>> GetMetricsInSpecifiedTimeAsync<T>(string type, int agentId, TimeSpan fromTime, TimeSpan toTime)
		{
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{managerServerUrl}/metrics/{type}/agent/{agentId}/from/{fromTime}/to/{toTime}"
			);

			try
			{
				var response = await _client.SendAsync(httpRequest);

				await using var responseStream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<IList<T>>(responseStream);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				throw;
			}
		}
	}
}