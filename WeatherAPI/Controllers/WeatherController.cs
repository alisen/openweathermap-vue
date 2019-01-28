using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.OpenWeatherMap;

namespace WeatherAPI.Controllers {
    [Route ("api/")]
    public class WeatherController : Controller {
        private OpenWeatherMapApiHandler handler = null;

        public WeatherController (OpenWeatherMapApiHandler openWeatherMapApiHandler) {
            handler = openWeatherMapApiHandler;
        }

        [Route ("forecast/city/{city}"), ActionName ("GetForecastByCity"), HttpGet]
        public Frontend.ForecastData GetForecastByCity (string city) {
            Frontend.ForecastData data = new Frontend.ForecastData ();
            if (string.IsNullOrEmpty (city)) {
                data.Code = Frontend.ReturnCode.ERROR;
                return data;
            }
            var resp = handler.GetForcastByCity (city);
            if (resp == null) {
                data.Code = Frontend.ReturnCode.ERROR;
                return data;
            }
            data.City = resp.City.Name;
            data.Country = resp.City.Country;

            var list = new LinkedList<Frontend.ForecastData.Data> ();

            int lastDay = -1;
            foreach (var forecast in resp.Forecasts) {
                if (forecast.Dt.Day != lastDay) {
                    Frontend.ForecastData.Data d = new Frontend.ForecastData.Data ();
                    d.Temperature = forecast.Main.Temp;
                    d.WindSpeed = forecast.Wind.Speed;
                    d.Humidity = forecast.Main.Humidity;
                    d.Weather = forecast.Weather[0].Main;
                    d.WeatherIcon = forecast.Weather[0].Icon;
                    d.Date = forecast.Dt.Date.ToString ();
                    list.AddLast (d);
                    lastDay = forecast.Dt.Day;
                }
            }
            data.Forecasts = list.ToArray ();
            data.Code = Frontend.ReturnCode.SUCCESS;

            return data;
        }

        [Route ("forecast/zip/{zipCode}"), ActionName ("GetForecastByZipCode"), HttpGet]
        public Frontend.ForecastData GetForecastByZipCode (string zipCode) {
            Frontend.ForecastData data = new Frontend.ForecastData ();
            int id;
            if (!int.TryParse (zipCode, out id)) {
                data.Code = Frontend.ReturnCode.ERROR;
                return data;
            }
            var resp = handler.GetForcastByZip (zipCode);
            if (resp == null) {
                data.Code = Frontend.ReturnCode.ERROR;
                return data;
            }
            data.City = resp.City.Name;
            data.Country = resp.City.Country;

            var list = new LinkedList<Frontend.ForecastData.Data> ();

            int lastDay = -1;
            foreach (var forecast in resp.Forecasts) {
                if (forecast.Dt.Day != lastDay) {
                    Frontend.ForecastData.Data d = new Frontend.ForecastData.Data ();
                    d.Temperature = forecast.Main.Temp;
                    d.WindSpeed = forecast.Wind.Speed;
                    d.Humidity = forecast.Main.Humidity;
                    d.Weather = forecast.Weather[0].Main;
                    d.WeatherIcon = forecast.Weather[0].Icon;
                    d.Date = forecast.Dt.Date.ToString ();
                    list.AddLast (d);
                    lastDay = forecast.Dt.Day;
                }
            }
            data.Forecasts = list.ToArray ();
            data.Code = Frontend.ReturnCode.SUCCESS;

            return data;
        }
    }
}