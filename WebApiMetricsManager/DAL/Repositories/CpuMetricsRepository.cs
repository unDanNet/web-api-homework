using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Core.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApiMetricsManager.DAL.Interfaces;
using WebApiMetricsManager.DAL.Models;
using WebApiMetricsManager.DAL.TypeHandlers;

namespace WebApiMetricsManager.DAL.Repositories
{
	public class CpuMetricsRepository : ICpuMetricsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public CpuMetricsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());

			tableName = dbConfig.GetFullTableName("Cpu");
			connectionString = dbConfig.GetConnectionString("DefaultConnection");
		}

		public IList<CpuMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);
			
			return connection.Query<CpuMetric>($"SELECT AgentId, Id, Value, Time FROM {tableName}").ToList();
		}

		public CpuMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<CpuMetric>(
				$"SELECT AgentId, Id, Value, Time FROM {tableName} WHERE id = @id",
				new { id }
			);
		}

		public IList<CpuMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<CpuMetric>(
				$"SELECT AgentId, Id, Value, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds,
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(CpuMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(value, time, agentId) VALUES(@value, @time, @agentId)",
				new {
					value = item.Value, 
					time = item.Time.TotalSeconds,
					agentId = item.AgentId
				}
			);
		}

		public void UpdateItem(CpuMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET value = @value, time = @time, agentId = @agentId WHERE id = @id",
				new {
					value = item.Value,
					time = item.Time.TotalSeconds,
					id = item.Id,
					agentId = item.AgentId
				}
			);
		}

		public void DeleteItem(int itemId)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"DELETE FROM {tableName} WHERE id = @id",
				new {id = itemId}
			);
		}

		public bool IsEmpty()
		{
			using var connection = new SQLiteConnection(connectionString);

			var itemsAmount = connection.QuerySingle<int>($"SELECT COUNT(*) FROM {tableName}");

			return itemsAmount == 0;
		}

		public IList<CpuMetric> GetItemsByAgentId(int agentId, TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<CpuMetric>(
				$"SELECT Id, Value, Time, AgentId FROM {tableName} WHERE agentId = @agentId AND time >= @fromTime AND time <= @toTime",
				new {
					agentId, 
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public TimeSpan GetTimeOfLatestMetricByAgentId(int agentId)
		{
			using var connection = new SQLiteConnection(connectionString);

			if (IsEmpty()) {
				return new TimeSpan();
			}

			return connection.QuerySingle<TimeSpan>(
				$"SELECT MAX(Time) FROM {tableName} WHERE agentId = @agentId",
				new {agentId}
			);
		}
	}
}
