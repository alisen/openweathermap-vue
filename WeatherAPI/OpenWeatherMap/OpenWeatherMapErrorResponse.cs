using Newtonsoft.Json;

namespace WeatherAPI.OpenWeatherMap {
    public class OpenWeatherMapErrorResponse {
        [JsonProperty ("cod")]
        public long Cod { get; set; }

        [JsonProperty ("message")]
        public string Message { get; set; }
    }
}