using Newtonsoft.Json;

namespace WeatherAPI.Models {
    public class ForecastResponse {

        [JsonProperty ("StatusCode")]
        public long StatusCode { get; set; }

        [JsonProperty ("StatusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty ("city")]
        public string City { get; set; }

        [JsonProperty ("country")]
        public string Country { get; set; }

        [JsonProperty ("forecasts")]
        public Data[] Forecasts { get; set; }

        public class Data {
            [JsonProperty ("temperature")]
            public double Temperature { get; set; }

            [JsonProperty ("humidity")]
            public double Humidity { get; set; }

            [JsonProperty ("weather")]
            public string Weather { get; set; }

            [JsonProperty ("weatherIcon")]
            public string WeatherIcon { get; set; }

            [JsonProperty ("windSpeed")]
            public double WindSpeed { get; set; }

            [JsonProperty ("date")]
            public string Date { get; set; }
        }
    }
}