using System;
using System.IO;
using System.Net;
using System.Text;
using Forecast;
using Newtonsoft.Json;

namespace WeatherAPI.OpenWeatherMap {
    public class OpenWeatherMapApiHandler {
        private static string END_POINT = "http://api.openweathermap.org/data/2.5/";

        private String Unit;
        private readonly string API_KEY;

        public OpenWeatherMapApiHandler (string key) {
            Unit = "metric";
            API_KEY = key;
        }

        public OpenWeatherMapForcast GetForcastByZip (string zipCode) {
            StringBuilder builder = new StringBuilder (END_POINT);
            builder.Append ("forecast");
            builder.AppendFormat ("?zip={0},de&appid={1}&units={2}", zipCode, API_KEY, Unit);
            HttpWebRequest apiRequest = WebRequest.Create (builder.ToString ()) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse () as HttpWebResponse) {
                var reader = new StreamReader (response.GetResponseStream ());
                apiResponse = reader.ReadToEnd ();
            }

            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var jsonModel = JsonConvert.DeserializeObject<OpenWeatherMapForcast> (apiResponse, settings);
            return jsonModel;
        }

        public OpenWeatherMapForcast GetForcastByCity (string cityName) {
            StringBuilder builder = new StringBuilder (END_POINT);
            builder.Append ("forecast");
            builder.AppendFormat ("?q={0}&appid={1}&units={2}", cityName, API_KEY, Unit);
            HttpWebRequest apiRequest = WebRequest.Create (builder.ToString ()) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse () as HttpWebResponse) {
                var reader = new StreamReader (response.GetResponseStream ());
                apiResponse = reader.ReadToEnd ();
            }

            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var jsonModel = JsonConvert.DeserializeObject<OpenWeatherMapForcast> (apiResponse, settings);
            return jsonModel;
        }

        private bool IsFloatOrInt (string value) {
            int intValue;
            float floatValue;
            return Int32.TryParse (value, out intValue) ||
                float.TryParse (value, out floatValue);
        }
    }
}