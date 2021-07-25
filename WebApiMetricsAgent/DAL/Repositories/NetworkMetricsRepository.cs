using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;
using WebApiMetricsAgent.DAL.TypeHandlers;

namespace WebApiMetricsAgent.DAL.Repositories
{
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "networkmetrics";

		public NetworkMetricsRepository()
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		public IList<NetworkMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<NetworkMetric>($"SELECT Id, Value, Time FROM {TABLE_NAME}").ToList();
		}

		public NetworkMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.QuerySingle<NetworkMetric>(
				$"SELECT Id, Value, Time FROM {TABLE_NAME} WHERE id = @id",
				new {id}
			);
		}

		public IList<NetworkMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<NetworkMetric>(
				$"SELECT Id, Value, Time FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(NetworkMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"INSERT INTO {TABLE_NAME}(value, time) VALUES(@value, @time)",
				new {
					value = item.Value,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(NetworkMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"UPDATE {TABLE_NAME} SET value = @value, time = @time WHERE id = @id",
				new {
					value = item.Value,
					time = item.Time.TotalSeconds,
					id = item.Id
				}
			);
		}

		public void DeleteItem(int itemId)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"DELETE FROM {TABLE_NAME} WHERE id = @id",
				new {id = itemId}
			);
		}
	}
}