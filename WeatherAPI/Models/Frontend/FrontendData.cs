namespace Frontend {
    public static class ReturnCode {
        public const string ERROR = "ERROR";
        public const string SUCCESS = "SUCCESS";
    }

    public struct FrontendData {
        public string City { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Weather { get; set; }
        public string WeatherIcon { get; set; }
        public string Code { get; set; }
    }

    public struct ForecastData {
        public string Code { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Data[] Forecasts { get; set; }

        public struct Data {
            public double Temperature { get; set; }
            public double Humidity { get; set; }
            public string Weather { get; set; }
            public string WeatherIcon { get; set; }
            public double WindSpeed { get; set; }
            public string Date { get; set; }
        }
    }
}