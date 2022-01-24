using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace WeatherApp.Function
{
    public static class AddWeatherinfoToDbTrigger
    {
        [FunctionName("AddWeatherinfoToDbTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string APIKey = System.Environment.GetEnvironmentVariable("OpenweathermapAPIKey", EnvironmentVariableTarget.Process);
            var cityName = req.Query["city"];
            var lon = req.Query["lon"];
            var lat = req.Query["lat"];

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&lon={lon}&lat={lat}&appid={APIKey}"))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            var info = JsonConvert.DeserializeObject<Root>(data);
                            if (info.cod == 200)
                            {
                                string connectionString = Environment.GetEnvironmentVariable("WeatherappDb");
                                ObjectResult result;
                                string responseMessage ="";
                                var db = new DatabaseContext(connectionString);
                                db.AddWeatherInfo(info.coord.lat, info.coord.lon, info.name, info.sys.country, info.main.temp, info.main.feels_like, info.main.pressure, info.main.humidity, info.wind.speed, info.visibility, DateTime.Now);
                                responseMessage = "Weather info succesfully added to db";
                                result = new OkObjectResult(responseMessage);
                                return result;
                            }
                            else 
                            {
                                return new OkObjectResult($"OpenweathermapAPI returned status code: {info.cod}");
                            }
                        }
                    }
                }
            } catch(Exception exception)
            {
                Console.WriteLine("Exception Hit");
                Console.WriteLine(exception);
            }
            return new BadRequestObjectResult("Something went wrong");
        }
    }
}
