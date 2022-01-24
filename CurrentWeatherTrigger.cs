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
    public static class CurrentWeatherTrigger
    {
        [FunctionName("CurrentWeatherTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
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
                    using (HttpResponseMessage res = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&lon={lon}&lat={lat}&appid={APIKey}&units=metric"))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            var info = JsonConvert.DeserializeObject<Root>(data);
                            if (info.cod == 200)
                            {
                                return new OkObjectResult(info);
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
