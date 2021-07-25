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
	public class HddMetricsRepository : IHddMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "hddmetrics";

		public HddMetricsRepository()
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		public IList<HddMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<HddMetric>($"SELECT Id, SpaceLeft, Time FROM {TABLE_NAME}").ToList();
		}

		public HddMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.QuerySingle<HddMetric>(
				$"SELECT Id, SpaceLeft, Time FROM {TABLE_NAME} WHERE id = @id",
				new {id}
			);
		}

		public IList<HddMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			return connection.Query<HddMetric>(
				$"SELECT Id, SpaceLeft, Time FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(HddMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"INSERT INTO {TABLE_NAME}(spaceLeft, time) VALUES(@spaceLeft, @time)",
				new {
					spaceLeft = item.SpaceLeft,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(HddMetric item)
		{
			using var connection = new SQLiteConnection(CONNECTION_STRING);

			connection.Execute(
				$"UPDATE {TABLE_NAME} SET spaceLeft = @spaceLeft, time = @time WHERE id = @id",
				new {
					spaceLeft = item.SpaceLeft,
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