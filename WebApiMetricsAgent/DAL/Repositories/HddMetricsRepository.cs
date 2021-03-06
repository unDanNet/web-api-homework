using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Core.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;
using WebApiMetricsAgent.DAL.TypeHandlers;

namespace WebApiMetricsAgent.DAL.Repositories
{
	public class HddMetricsRepository : IHddMetricsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public HddMetricsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());

			tableName = dbConfig.GetFullTableName("Hdd");
			connectionString = dbConfig.GetConnectionString("DefaultConnection");
		}

		public IList<HddMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<HddMetric>($"SELECT Id, SpaceLeft, Time FROM {tableName}").ToList();
		}

		public HddMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<HddMetric>(
				$"SELECT Id, SpaceLeft, Time FROM {tableName} WHERE id = @id",
				new {id}
			);
		}

		public IList<HddMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<HddMetric>(
				$"SELECT Id, SpaceLeft, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(HddMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(spaceLeft, time) VALUES(@spaceLeft, @time)",
				new {
					spaceLeft = item.SpaceLeft,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(HddMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET spaceLeft = @spaceLeft, time = @time WHERE id = @id",
				new {
					spaceLeft = item.SpaceLeft,
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
		
		public bool IsEmpty()
		{
			using var connection = new SQLiteConnection(connectionString);

			var itemsAmount = connection.QuerySingle<int>($"SELECT COUNT(*) FROM {tableName}");

			return itemsAmount == 0;
		}
	}
}