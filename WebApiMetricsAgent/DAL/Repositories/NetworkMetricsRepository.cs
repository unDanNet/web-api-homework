using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using AutoMapper.Configuration;
using Core.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;
using WebApiMetricsAgent.DAL.TypeHandlers;

namespace WebApiMetricsAgent.DAL.Repositories
{
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public NetworkMetricsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());

			connectionString = dbConfig.GetFullTableName("Network");
			tableName = dbConfig.GetConnectionString("DefaultConnection");
		}

		public IList<NetworkMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<NetworkMetric>($"SELECT Id, Value, Time FROM {tableName}").ToList();
		}

		public NetworkMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<NetworkMetric>(
				$"SELECT Id, Value, Time FROM {tableName} WHERE id = @id",
				new {id}
			);
		}

		public IList<NetworkMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<NetworkMetric>(
				$"SELECT Id, Value, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(NetworkMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(value, time) VALUES(@value, @time)",
				new {
					value = item.Value,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(NetworkMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET value = @value, time = @time WHERE id = @id",
				new {
					value = item.Value,
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
	}
}