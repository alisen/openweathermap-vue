<template>
  <v-container>
      <v-layout>
        <v-text-field
          label="City Name"
          hint="Enter city name e.g. 'London'"
          name="cityInput"
          v-model="cityName"
          required
        ></v-text-field>
        <v-btn
          round
          outline
          small
          v-on:click="loadForecastsbyCityName"
          :loading="loading"
          color="info"
          @click="loader = 'loading'"
        >City Search</v-btn>
      </v-layout>
      <v-layout justify>
        <v-card dark color="primary">
          <v-container xl4 sm4 offset-sm4 v-if="forecastsCity">
            <v-layout>
              <v-flex>
                <div class="headline text-xs-center">{{city}},{{country}}</div>
                <v-card v-for="(itemCity, i) in forecastsCity" :key="i">
                  <br>
                  <h3 class="text-xs-center" style="color:#ffc107">{{ itemCity.date }}</h3>
                  <h4 class="text-xs-center">{{ itemCity.weather }}</h4>
                  <h4 class="text-xs-center">
                    <img
                      v-bind:src="`https://openweathermap.org/img/w/${itemCity.weatherIcon}.png`"
                    >
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
      </v-layout>
  </v-container>
</template>

<script>
import API from "@/lib/API";
import { get, sync, commit } from "vuex-pathify";

export default {
  components: { API, get, sync, commit },
  data() {
    return {
      cityName: "",
      loader: null,
      loading: false
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
  computed: {
    ...sync("*")
  },
  methods: {
    loadForecastsbyCityName() {
      if (!!this.cityName) {
        API.getForecastFromCityName(this.cityName).then(resultCity => {
          this.$store.set("forecastHistoryPush", resultCity);
          this.forecastsCity = resultCity.forecasts;
          this.city = resultCity.city;
          this.country = resultCity.country;
          this.cityName = null;
          this.forecastsZip = null;
          if (resultCity.StatusMessage != null && resultCity.StatusCode > 200) {
            this.$toast.error(resultCity.StatusMessage.toUpperCase());
          }
          if (resultCity.StatusMessage == null && resultCity.StatusCode > 200) {
            this.$toast.error(resultCity.StatusCode);
          }
        });
      }
    }
  }
};
</script>