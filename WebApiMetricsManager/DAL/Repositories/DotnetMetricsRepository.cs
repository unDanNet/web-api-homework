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
public class DotnetMetricsRepository : IDotnetMetricsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public DotnetMetricsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());

			tableName = dbConfig.GetFullTableName("Dotnet");
			connectionString = dbConfig.GetConnectionString("DefaultConnection");
		}


		public IList<DotnetMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<DotnetMetric>($"SELECT Id, ErrorsCount, Time FROM {tableName}").ToList();
		}

		public DotnetMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<DotnetMetric>(
				$"SELECT Id, ErrorsCount, Time FROM {tableName} WHERE id = @id",
				new {id}
			);
		}

		public IList<DotnetMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<DotnetMetric>(
				$"SELECT Id, ErrorsCount, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(DotnetMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(errorsCount, time) VALUES(@errorsCount, @time)",
				new {
					errorsCount = item.ErrorsCount,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(DotnetMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET errorsCount = @errorsCount, time = @time WHERE id = @id",
				new {
					errorsCount = item.ErrorsCount,
					time = item.Time.TotalSeconds,
					id = item.Id
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

		public IList<DotnetMetric> GetItemsByAgentId(int agentId, TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<DotnetMetric>(
				$"SELECT Id, ErrorsCount, Time, AgentId FROM {tableName} " +
				"WHERE agentId = @agentId AND time >= @fromTime AND time <= @toTime",
				new {agentId, fromTime, toTime}
			).ToList();
		}

		public TimeSpan GetTimeOfLatestMetricByAgentId(int agentId)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<TimeSpan>(
				$"SELECT MAX(Time) FROM {tableName} WHERE agentId = @agentId",
				new {agentId}
			);
		}
	}
}