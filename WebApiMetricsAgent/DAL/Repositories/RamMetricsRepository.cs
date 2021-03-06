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
	public class RamMetricsRepository : IRamMetricsRepository
	{
		private readonly string connectionString;
		private readonly string tableName;

		public RamMetricsRepository(IConfigurationRoot dbConfig)
		{
			SqlMapper.AddTypeHandler(new TimeSpanHandler());

			tableName = dbConfig.GetFullTableName("Ram");
			connectionString = dbConfig.GetConnectionString("DefaultConnection");
		}

		public IList<RamMetric> GetAllItems()
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<RamMetric>($"SELECT Id, MemoryAvailable, Time FROM {tableName}").ToList();
		}

		public RamMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<RamMetric>(
				$"SELECT Id, MemoryAvailable, Time FROM {tableName} WHERE id = @id",
				new {id}
			);
		}

		public IList<RamMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<RamMetric>(
				$"SELECT Id, MemoryAvailable, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
				new {
					fromTime = fromTime.TotalSeconds, 
					toTime = toTime.TotalSeconds
				}
			).ToList();
		}

		public void AddItem(RamMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"INSERT INTO {tableName}(memoryAvailable, time) VALUES(@memoryAvailable, @time)",
				new {
					memoryAvailable = item.MemoryAvailable,
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(RamMetric item)
		{
			using var connection = new SQLiteConnection(connectionString);

			connection.Execute(
				$"UPDATE {tableName} SET memoryAvailable = @memoryAvailable, time = @time WHERE id = @id",
				new {
					memoryAvailable = item.MemoryAvailable,
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