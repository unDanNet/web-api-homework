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
	public class RamMetricsRepository : IRamMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "rammetrics";

		public RamMetricsRepository()
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		public IList<RamMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<RamMetric>($"SELECT Id, MemoryAvailable, Time FROM {TABLE_NAME}").ToList();
		}

		public RamMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.QuerySingle<RamMetric>(
				$"SELECT Id, MemoryAvailable, Time FROM {TABLE_NAME} WHERE id = @id",
				new {id}
			);
		}

		public IList<RamMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<RamMetric>(
				$"SELECT Id, MemoryAvailable, Time FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(RamMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"INSERT INTO {TABLE_NAME}(memoryAvailable, time) VALUES(@memoryAvailable, @time)",
				new {
					memoryAvailable = item.MemoryAvailable,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(RamMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"UPDATE {TABLE_NAME} SET memoryAvailable = @memoryAvailable, time = @time WHERE id = @id",
				new {
					memoryAvailable = item.MemoryAvailable,
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