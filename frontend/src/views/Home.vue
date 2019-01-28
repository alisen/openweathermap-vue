<template>
  <v-layout row wrap>
    <v-flex xl4 sm4 offset-sm4>
      <v-layout justify>
        <v-text-field
          label="City Name"
          hint="Enter city name e.g. 'London'"
          id="cityInput"
          name="cityInput"
          v-model="cityName"
        ></v-text-field>
        <v-btn
          round
          outline
          small
          v-on:click="byCityName"
          :loading="loading"
          :disabled="loading"
          color="info"
          @click="loader = 'loading'"
        >City Search</v-btn>
      </v-layout>
      <v-divider class="mt-6"></v-divider>
      <v-layout justify>
        <v-text-field
          label="Zip Code"
          hint="Enter zip code e.g. '09496'"
          id="zipInput"
          name="zipInput"
          v-model="zipCode"
        ></v-text-field>
        <v-btn
          round
          outline
          small
          v-on:click="byZipCode"
          :loading="loading3"
          :disabled="loading3"
          color="info"
          @click="loader = 'loading3'"
        >Zip Search</v-btn>
      </v-layout>
      <v-divider class="mt-6"></v-divider>
      <v-card dark color="primary">
        <v-container xl4 sm4 offset-sm4 fluid v-if="forecastsCity">
          <v-layout row>
            <v-flex>
              <div class="headline text-xs-center">{{city}},{{country}}</div>
              <v-card v-for="itemCity in forecastsCity">
                <br>
                <h3
                  class="text-xs-center"
                  style="color:#ffc107"
                >{{ itemCity.date | moment("DD.MM.YYYY, dddd") }}</h3>
                <h4 class="text-xs-center">{{ itemCity.weather }}</h4>
                <h4 class="text-xs-center">
                  <img v-bind:src="`https://openweathermap.org/img/w/${itemCity.weatherIcon}.png`">
                </h4>
                <h4 class="text-xs-center">Temperature : {{itemCity.temperature}}&deg;C</h4>
                <h4 class="text-xs-center">Humidity : {{itemCity.humidity}}%</h4>
                <h4 class="text-xs-center">WindSpeed : {{itemCity.windSpeed }} m/s</h4>
                <br>
              </v-card>
            </v-flex>
          </v-layout>
        </v-container>
      </v-card>
      <v-card dark color="primary">
        <v-container xl4 sm4 offset-sm4 fluid v-if="forecastsZip">
          <v-layout row>
            <v-flex>
              <div class="headline text-xs-center">{{city}},{{country}}</div>
              <v-card v-for="itemZip in forecastsZip">
                <br>
                <h3
                  class="text-xs-center"
                  style="color:#ffc107"
                >{{ itemZip.date | moment("DD.MM.YYYY, dddd") }}</h3>
                <h4 class="text-xs-center">{{ itemZip.weather }}</h4>
                <h4 class="text-xs-center">
                  <img v-bind:src="`https://openweathermap.org/img/w/${itemZip.weatherIcon}.png`">
                </h4>
                <h4 class="text-xs-center">Temperature : {{itemZip.temperature}}&deg;C</h4>
                <h4 class="text-xs-center">Humidity : {{itemZip.humidity}}%</h4>
                <h4 class="text-xs-center">WindSpeed : {{itemZip.windSpeed }} m/s</h4>
                <br>
              </v-card>
            </v-flex>
          </v-layout>
        </v-container>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script>
import API from "@/lib/API";

export default {
  components: {},
  data() {
    return {
      cityName: "",
      zipCode: "",
      city: "",
      country: "",
      forecastsCity: null,
      forecastsZip: null,
      loader: null,
      loading: false,
      loading3: false
    };
  },
  watch: {
    loader() {
      const l = this.loader;
      this[l] = !this[l];

      setTimeout(() => (this[l] = false), 500);

      this.loader = null;
    }
  },
  computed: {},
  mounted() {},
  methods: {
    loadWeather(cityName) {
      API.getForecastFromCityName(cityName).then(resultCity => {
        this.forecastsCity = resultCity.forecasts;
        this.city = resultCity.city;
        this.country = resultCity.country;
        console.log(resultCity);
        this.cityName = null;
        this.forecastsZip = null;
      });
    },
    byCityName() {
      API.getForecastFromCityName(this.cityName).then(resultCity => {
        this.loadWeather(this.cityName);
      });
    },
    loadWeatherByZipCode(zipCode) {
      API.getForecastFromZipCode(zipCode).then(resultZip => {
        this.forecastsZip = resultZip.forecasts;
        this.city = resultZip.city;
        this.country = resultZip.country;
        console.log(resultZip);
        this.zipCode = null;
        this.forecastsCity = null;
      });
    },
    byZipCode() {
      API.getForecastFromZipCode(this.zipCode).then(resultZip => {
        this.loadWeatherByZipCode(this.zipCode);
      });
    }
  }
};
</script>