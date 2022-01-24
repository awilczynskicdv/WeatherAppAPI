  
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WeatherApp{
    public class DatabaseContext
    {
        private readonly string connectionString;
        private const string Query = "Select * from WeatherInfo";

        public DatabaseContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddWeatherInfo(double Longtitude, double Latitude, string CityName, string Country, double Temp, double TempFeelsLike,
                                    int Pressure, int Humidity, double WindSpeed, int Visibility, DateTime CreatedOn)
        {
            string QueryAdd = $"Insert into WeatherInfo values('{Longtitude}', '{Latitude}', '{CityName}', '{Country}', '{Temp}', '{TempFeelsLike}', '{Pressure}', '{Humidity}', '{WindSpeed}', '{Visibility}', '{CreatedOn}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(QueryAdd, connection);
                connection.Open();     
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<WeatherInfo> GetPreviousWeatherInfo()
        {
            var weather = new List<WeatherInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    weather.Add(new WeatherInfo
                    {
                        Longtitude = reader["Longtitude"].ToString(),
                        Latitude = reader["Latitude"].ToString(),
                        CityName = reader["CityName"].ToString(),
                        Country = reader["Country"].ToString(),
                        Temp = reader["Temp"].ToString(),
                        TempFeelsLike = reader["TempFeelsLike"].ToString(),
                        Pressure = (int)reader["Pressure"],
                        Humidity = (int)reader["Humidity"],
                        WindSpeed = reader["WindSpeed"].ToString(),
                        Visibility = (int)reader["Visibility"],
                        CreatedOn = reader["CreatedOn"].ToString()

                    });
                }
                reader.Close();
            }

            return weather;
        }


    }
}