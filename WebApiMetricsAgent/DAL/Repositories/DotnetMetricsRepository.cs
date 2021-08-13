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
	public class DotnetMetricsRepository : IDotnetMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "dotnetmetrics";

		public DotnetMetricsRepository()
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}


		public IList<DotnetMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<DotnetMetric>($"SELECT Id, ErrorsCount, Time FROM {TABLE_NAME}").ToList();
		}

		public DotnetMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.QuerySingle<DotnetMetric>(
				$"SELECT Id, ErrorsCount, Time FROM {TABLE_NAME} WHERE id = @id",
				new {id}
			);
		}

		public IList<DotnetMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<DotnetMetric>(
				$"SELECT Id, ErrorsCount, Time FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(DotnetMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"INSERT INTO {TABLE_NAME}(errorsCount, time) VALUES(@errorsCount, @time)",
				new {
					errorsCount = item.ErrorsCount,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(DotnetMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"UPDATE {TABLE_NAME} SET errorsCount = @errorsCount, time = @time WHERE id = @id",
				new {
					errorsCount = item.ErrorsCount,
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