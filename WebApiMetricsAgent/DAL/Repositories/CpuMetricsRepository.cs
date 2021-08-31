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
			
			return connection.Query<CpuMetric>($"SELECT Id, Value, Time FROM {tableName}").ToList();
		}

		public CpuMetric GetItemById(int id)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.QuerySingle<CpuMetric>(
				$"SELECT Id, Value, Time FROM {tableName} WHERE id = @id",
				new { id }
			);
		}

		public IList<CpuMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			using var connection = new SQLiteConnection(connectionString);

			return connection.Query<CpuMetric>(
				$"SELECT Id, Value, Time FROM {tableName} WHERE time >= @fromTime AND time <= @toTime",
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
				$"INSERT INTO {tableName}(value, time) VALUES(@value, @time)",
				new {
					value = item.Value, 
					time = item.Time.TotalSeconds
				}
			);
		}

		public void UpdateItem(CpuMetric item)
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
		
		public bool IsEmpty()
		{
			using var connection = new SQLiteConnection(connectionString);

			var itemsAmount = connection.QuerySingle<int>($"SELECT COUNT(*) FROM {tableName}");

			return itemsAmount == 0;
		}
	}
}