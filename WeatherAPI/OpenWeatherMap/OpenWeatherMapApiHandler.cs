using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherAPI.OpenWeatherMap {
    public class OpenWeatherMapApiHandler {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _unit;
        private string _apiKey;
        private string _baseUrl;

        public OpenWeatherMapApiHandler (HttpClient httpClient, IConfiguration configuration) {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public virtual async Task<OpenWeatherMapResponse> GetForecastByZipAsync (string zipCode) {
            ParameterChecker ();
            var endpointUrl = $"{_baseUrl}forecast?zip={zipCode},de&units={_unit}&appid={_apiKey}";

            var response = await _httpClient.GetAsync (endpointUrl);

            if (response.IsSuccessStatusCode) return await ForecastResponse (response);
            var error = await response.Content.ReadAsAsync<OpenWeatherMapErrorResponse> ();
            return new OpenWeatherMapResponse {
                Message = error.Message,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = (int) response.StatusCode,
                    OpenWeatherMapForecast = null
            };
        }

        public virtual async Task<OpenWeatherMapResponse> GetForecastByCityAsync (string cityName) {
            ParameterChecker ();
            var endpointUrl = $"{_baseUrl}forecast?q={cityName}&units={_unit}&appid={_apiKey}";

            var response = await _httpClient.GetAsync (endpointUrl);

            if (response.IsSuccessStatusCode) return await ForecastResponse (response);
            var error = await response.Content.ReadAsAsync<OpenWeatherMapErrorResponse> ();
            return new OpenWeatherMapResponse {
                Message = error.Message,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = (int) response.StatusCode,
                    OpenWeatherMapForecast = null
            };
        }

        private static async Task<OpenWeatherMapResponse> ForecastResponse (HttpResponseMessage response) {
            var forecastResponse = await response.Content.ReadAsAsync<OpenWeatherMapForecast> ();
            return new OpenWeatherMapResponse {
                Message = forecastResponse.Message.ToString (),
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = (int) response.StatusCode,
                    OpenWeatherMapForecast = forecastResponse,
            };
        }

        private void ParameterChecker () {
            _unit = _configuration.GetValue<string> ("OpenWeatherMapUnit");
            _apiKey = _configuration.GetValue<string> ("OpenWeatherMapApiKey");
            _baseUrl = _configuration.GetValue<string> ("OpenWeatherMapBaseUrl");

            if (string.IsNullOrWhiteSpace (_unit)) {
                throw new ArgumentException ("There is no unit in the user secret store.");
            }

            if (string.IsNullOrWhiteSpace (_apiKey)) {
                throw new ArgumentException ("There is no OpenWeatherMap API key in the user secret store.");
            }

            if (string.IsNullOrWhiteSpace (_baseUrl)) {
                throw new ArgumentException ("There is no baseUrl in the user secret store.");
            }
        }
    }
}