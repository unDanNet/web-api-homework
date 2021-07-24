using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WebApiMetricsAgent.DAL.Interfaces;
using WebApiMetricsAgent.DAL.Models;

namespace WebApiMetricsAgent.DAL.Repositories
{
	public class RamMetricsRepository : IRamMetricsRepository
	{
		private const string CONNECTION_STRING = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
		private const string TABLE_NAME = "cpumetrics";
		
		public IList<RamMetric> GetAllItems()
		{
			var result = new List<RamMetric>();
			
			// open the connection to the database
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				// create the command that will be sent to the database
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME}"
				}; 
				
				// start reading the data from the table
				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						result.Add(new RamMetric{
							Id = reader.GetInt32(0),
							MemoryAvailable = reader.GetInt32(1),
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}
			
			return result;
		}

		public RamMetric GetItemById(int id)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();
				
				using var command = new SQLiteCommand(connection) {
					CommandText = $"SELECT * FROM {TABLE_NAME} WHERE id = @id",
				};

				command.Parameters.AddWithValue("@id", id);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					return reader.Read() ? new RamMetric {
						Id = reader.GetInt32(0), 
						MemoryAvailable = reader.GetInt32(1), 
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					} : null;
				}
			}
		}

		public IList<RamMetric> GetItemsByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
		{
			var result = new List<RamMetric>();
			
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
						result.Add(new RamMetric {
							Id = reader.GetInt32(0), 
							MemoryAvailable = reader.GetInt32(1), 
							Time = TimeSpan.FromSeconds(reader.GetInt32(2))
						});
					}
				}
			}

			return result;
		}

		public void AddItem(RamMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"INSERT INTO {TABLE_NAME}(memoryAvailable, time) VALUES(@memoryAvailable, @time)"
				};
				
				command.Parameters.AddWithValue("@memoryAvailable", item.MemoryAvailable); // add the value for the 'memoryAvailable' parameter
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds); // add the value for the 'time' parameter
				
				command.Prepare(); // prepare command for the execution
				command.ExecuteNonQuery(); // execute the command
			}
		}

		public void UpdateItem(RamMetric item)
		{
			using (var connection = new SQLiteConnection(CONNECTION_STRING))	
			{
				connection.Open();

				using var command = new SQLiteCommand(connection) {
					CommandText = $"UPDATE {TABLE_NAME} SET memoryAvailable = @memoryAvailable, time = @time WHERE id = @id"
				};

				command.Parameters.AddWithValue("@id", item.Id);
				command.Parameters.AddWithValue("@memoryAvailable", item.MemoryAvailable);
				command.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
				
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