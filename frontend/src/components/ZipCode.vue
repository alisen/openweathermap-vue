<template>
  <v-container>
    <v-layout>
      <v-text-field
        label="Zip Code"
        hint="Enter zip code e.g. '09496'"
        name="zipInput"
        v-model="zipCode"
        required
      ></v-text-field>
      <v-btn
        round
        outline
        small
        v-on:click="loadForecastsByZipCode"
        :loading="loading3"
        :disabled="loading3"
        color="info"
        @click="loader = 'loading3'"
      >Zip Search</v-btn>
    </v-layout>
    <v-layout>
      <v-card dark color="primary">
        <v-container xl4 sm4 offset-sm4 v-if="forecastsZip">
          <v-flex>
            <v-flex>
              <div class="headline text-xs-center">{{city}},{{country}}</div>
              <v-card v-for="(itemZip, i) in forecastsZip" :key="i">
                <br>
                <h3 class="text-xs-center" style="color:#ffc107">{{ itemZip.date }}</h3>
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
          </v-flex>
        </v-container>
      </v-card>
    </v-layout>
  </v-container>
</template>

<script>
import API from "@/lib/API";
import { get, sync, commit } from "vuex-pathify";

export default {
  components: { API, sync },
  data() {
    return {
      zipCode: "",
      loader: null,
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
  computed: {
    ...sync("*")
  },
  methods: {
    loadForecastsByZipCode() {
      if (!!this.zipCode) {
        API.getForecastFromZipCode(this.zipCode).then(resultZip => {
          this.$store.set("forecastHistoryPush", resultZip);
          this.forecastsZip = resultZip.forecasts;
          this.city = resultZip.city;
          this.country = resultZip.country;
          this.zipCode = null;
          this.forecastsCity = null;
          if (resultZip.StatusMessage != null && resultZip.StatusCode > 200) {
            this.$toast.error(resultZip.StatusMessage.toUpperCase());
          }
          if (resultZip.StatusMessage == null && resultZip.StatusCode > 200) {
            this.$toast.error(resultZip.StatusCode);
          }
        });
      }
    }
  }
};
</script>