using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnixDateTimeConverter = WeatherAPI.Helpers.UnixDateTimeConverter;

namespace WeatherAPI.OpenWeatherMap {
    public class OpenWeatherMapForecast {
        [JsonProperty ("cod")]
        [JsonConverter (typeof (ParseStringConverter))]
        public long Cod { get; set; }

        [JsonProperty ("message")]
        public double Message { get; set; }

        [JsonProperty ("cnt")]
        public long Cnt { get; set; }

        [JsonProperty ("list")]
        public Forecast[] Forecasts { get; set; }

        [JsonProperty ("city")]
        public City City { get; set; }
    }

    public class City {
        [JsonProperty ("id")]
        public long Id { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("coord")]
        public Coord Coord { get; set; }

        [JsonProperty ("country")]
        public string Country { get; set; }
    }

    public class Coord {
        [JsonProperty ("lat")]
        public double Lat { get; set; }

        [JsonProperty ("lon")]
        public double Lon { get; set; }
    }

    public class Forecast {
        [JsonProperty ("dt")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime Dt { get; set; }

        [JsonProperty ("main")]
        public MainClass Main { get; set; }

        [JsonProperty ("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty ("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty ("wind")]
        public Wind Wind { get; set; }

        [JsonProperty ("rain")]
        public Rain Rain { get; set; }

        [JsonProperty ("snow")]
        public Snow Snow { get; set; }

        [JsonProperty ("sys")]
        public Sys Sys { get; set; }

        [JsonProperty ("dt_txt")]
        public DateTimeOffset DtTxt { get; set; }
    }

    public class Clouds {
        [JsonProperty ("all")]
        public long All { get; set; }
    }

    public class MainClass {
        [JsonProperty ("temp")]
        public double Temp { get; set; }

        [JsonProperty ("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty ("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty ("pressure")]
        public double Pressure { get; set; }

        [JsonProperty ("sea_level")]
        public double SeaLevel { get; set; }

        [JsonProperty ("grnd_level")]
        public double GrndLevel { get; set; }

        [JsonProperty ("humidity")]
        public long Humidity { get; set; }

        [JsonProperty ("temp_kf")]
        public long TempKf { get; set; }
    }

    public class Rain {
        [JsonProperty ("1h", NullValueHandling = NullValueHandling.Ignore)]
        public double? OneHour { get; set; }

        [JsonProperty ("2h", NullValueHandling = NullValueHandling.Ignore)]
        public double? TwoHour { get; set; }

        [JsonProperty ("3h", NullValueHandling = NullValueHandling.Ignore)]
        public double? ThreeHour { get; set; }
    }

    public class Snow {
        [JsonProperty ("1h", NullValueHandling = NullValueHandling.Ignore)]
        public double? OneHour { get; set; }

        [JsonProperty ("2h", NullValueHandling = NullValueHandling.Ignore)]
        public double? TwoHour { get; set; }

        [JsonProperty ("3h", NullValueHandling = NullValueHandling.Ignore)]
        public double? ThreeHour { get; set; }
    }

    public class Sys {
        [JsonProperty ("pod")]
        public string Pod { get; set; }
    }

    public class Weather {
        [JsonProperty ("id")]
        public long Id { get; set; }

        [JsonProperty ("main")]
        public string Main { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }

        [JsonProperty ("icon")]
        public string Icon { get; set; }
    }

    public class Wind {
        [JsonProperty ("speed")]
        public double Speed { get; set; }

        [JsonProperty ("deg")]
        public double Deg { get; set; }
    }

    public class OpenWeatherMapForecastResponse {
        public static OpenWeatherMapForecastResponse FromJson (string json) => JsonConvert.DeserializeObject<OpenWeatherMapForecastResponse> (json, Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson (this OpenWeatherMapForecastResponse self) => JsonConvert.SerializeObject (self, Converter.Settings);
    }

    internal static class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter {
        public override bool CanConvert (Type t) => t == typeof (long) || t == typeof (long?);

        public override object ReadJson (JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string> (reader);
            if (Int64.TryParse (value, out var l)) {
                return l;
            }
            throw new Exception ("Cannot unmarshal type long");
        }

        public override void WriteJson (JsonWriter writer, object untypedValue, JsonSerializer serializer) {
            if (untypedValue == null) {
                serializer.Serialize (writer, null);
                return;
            }
            var value = (long) untypedValue;
            serializer.Serialize (writer, value.ToString ());
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter ();
    }
}