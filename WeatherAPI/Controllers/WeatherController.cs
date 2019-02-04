using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ComparerExtensions;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.OpenWeatherMap;

namespace WeatherAPI.Controllers {
    [ApiController]
    [Route ("api/forecast/[action]")]
    [ApiVersion ("1.1")]
    public class WeatherController : Controller {
        private readonly OpenWeatherMapApiHandler _handler;

        public WeatherController (OpenWeatherMapApiHandler openWeatherMapApiHandler) {
            _handler = openWeatherMapApiHandler;
        }

        [Route ("{city}"), ActionName ("city"), HttpGet]
        public async Task<ForecastResponse> GetForecastByCity (string city) {
            var data = new ForecastResponse ();
            if (string.IsNullOrEmpty (city)) {
                data.StatusMessage = "City Name not provided.";
                data.StatusCode = 400;
                return data;
            }
            var response = await _handler.GetForecastByCityAsync (city.Trim ());
            return ForecastResponse (data, response);
        }

        [Route ("{zipCode}"), ActionName ("zip"), HttpGet]
        public async Task<ForecastResponse> GetForecastByZipCode (string zipCode) {
            var data = new ForecastResponse ();
            if (!int.TryParse (zipCode, out _)) {
                data.StatusMessage = "Zip Code not provided.";
                data.StatusCode = 400;
                return data;
            }
            var response = await _handler.GetForecastByZipAsync (zipCode.Trim ());
            return ForecastResponse (data, response);
        }

        private static ForecastResponse ForecastResponse (ForecastResponse data, OpenWeatherMapResponse response) {
            if (response.StatusCode >= 200 && response.StatusCode < 300) {
                data.City = response.OpenWeatherMapForecast.City.Name;
                data.Country = response.OpenWeatherMapForecast.City.Country;

                IEqualityComparer<ForecastResponse.Data> comparer = KeyEqualityComparer<ForecastResponse.Data>.Using (date => date.Date);

                data.Forecasts = response.OpenWeatherMapForecast.Forecasts
                    .GroupBy (g => new ForecastResponse.Data {
                        Temperature = g.Main.Temp,
                            WindSpeed = g.Wind.Speed,
                            Humidity = g.Main.Humidity,
                            Weather = g.Weather[0].Main,
                            WeatherIcon = g.Weather[0].Icon,
                            Date = g.Dt.ToLongDateString ()
                    }, comparer)
                    .Select (forecast =>
                        new ForecastResponse.Data {
                            Temperature = forecast.Key.Temperature,
                                WindSpeed = forecast.Key.WindSpeed,
                                Humidity = forecast.Key.Humidity,
                                Weather = forecast.Key.Weather,
                                WeatherIcon = forecast.Key.WeatherIcon,
                                Date = forecast.Key.Date
                        }
                    )
                    .ToArray ();

                data.StatusCode = response.OpenWeatherMapForecast.Cod;
                data.StatusMessage = "Success";

                return data;
            }

            data.StatusMessage = response.Message;
            data.StatusCode = response.StatusCode;
            return data;
        }
    }
}