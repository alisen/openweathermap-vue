namespace WeatherAPI.OpenWeatherMap {
    public class OpenWeatherMapResponse {
        public bool IsSuccessStatusCode { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public OpenWeatherMapForecast OpenWeatherMapForecast { get; set; }
    }
}