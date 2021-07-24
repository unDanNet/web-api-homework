using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.DAL.Repositories
{
	public class DotnetMetricsRepository : IDotnetMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "dotnetmetrics";
		
		
		public IList<DotnetMetric> GetAllItems()
		{
			var result = new List<DotnetMetric>();

			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME}"
				};

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(new DotnetMetric {
							Id = reader.GetInt32(0),
							ErrorsCount = reader.GetInt32(1),
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}

			return result;
		}

		public DotnetMetric GetItemById(int id)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME} WHERE id = @id"
				};

				command.Parameters.AddWithValue("@id", id);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					return reader.Read() ? new DotnetMetric {
						Id = reader.GetInt32(0),
						ErrorsCount = reader.GetInt32(1),
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					} : null;
				}
			}
		}

		public IList<DotnetMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			var result = new List<DotnetMetric>();
			
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME} WHERE time >= @fromTime AND time <= @toTime"
				};

				command.Parameters.AddWithValue("@fromTime", fromTime.TotalSeconds);
				command.Parameters.AddWithValue("@toTime", toTime.TotalSeconds);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(new DotnetMetric {
							Id = reader.GetInt32(0),
							ErrorsCount = reader.GetInt32(1),
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}

			return result;
		}

		public void AddItem(DotnetMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"INSERT INTO {TABLE_NAME}(errorsCount, time) VALUES(@errorsCount, @time)"
				};

				command.Parameters.AddWithValue("@errorsCount", item.ErrorsCount);
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
				
				command.Prepare();
				command.ExecuteNonQuery();
			}
		}

		public void UpdateItem(DotnetMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"UPDATE {TABLE_NAME} SET errorsCount = @errorsCount, time = @time WHERE id = @id"
				};

				command.Parameters.AddWithValue("@errorsCount", item.ErrorsCount);
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
				command.Parameters.AddWithValue("@id", item.Id);

				command.Prepare();
				command.ExecuteNonQuery();
			}
		}

		public void DeleteItem(int itemId)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"DELETE FROM {TABLE_NAME} WHERE id = @id"
				};

				command.Parameters.AddWithValue("@id", itemId);
				
				command.Prepare();
				command.ExecuteNonQuery();
			}
		}
	}
}