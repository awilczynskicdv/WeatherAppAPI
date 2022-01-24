using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeatherApp{
    public class WeatherInfo{

        [JsonProperty("lon")]
        public string Longtitude { get; set; }
        [JsonProperty("lat")]
        public string Latitude { get; set; }
        [JsonProperty("name")]
        public string CityName {get; set;}
        [JsonProperty("country")]
        public string Country {get; set;}
        [JsonProperty("temp")]
        public string Temp { get; set; }
        [JsonProperty("feels_like")]
        public string TempFeelsLike { get; set; }
        [JsonProperty("pressure")]
        public int Pressure {get; set;}
        [JsonProperty("humidity")]
        public int Humidity {get; set;}
        [JsonProperty("speed")]
        public string WindSpeed {get; set;}
        [JsonProperty("visibility")]
        public int Visibility {get; set;}
        public string CreatedOn { get; set; } = DateTime.Now.ToString();

    }
}