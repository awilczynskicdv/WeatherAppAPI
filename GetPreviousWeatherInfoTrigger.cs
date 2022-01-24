using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WeatherApp.Function
{
    public static class GetPreviousWeatherInfoTrigger
    {
        [FunctionName("GetPreviousWeatherInfoTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("WeatherappDb");
                log.LogInformation(connectionString);
                var db = new DatabaseContext(connectionString);
                var weather = db.GetPreviousWeatherInfo();
                return new JsonResult(weather);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }
    }
}
