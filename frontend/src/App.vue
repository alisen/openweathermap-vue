<template>
  <v-app dark>
    <v-toolbar color="info" dark fixed app>
      <v-toolbar-title>Alisen's OpenWeatherMap by Vue.js</v-toolbar-title>
    </v-toolbar>
    <v-content>
      <v-container>
        <v-layout>
          <v-flex>
            <div id="app">
              <router-view/>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
    </v-content>
    <v-footer color="info" app>
      <span class="white--text">Alisen Erdogan &copy; 2019</span>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: "app"
};
</script>