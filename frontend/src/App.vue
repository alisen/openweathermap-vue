<template>
  <v-app dark>
    <Toolbar/>
    <v-content>
      <v-container fluid>
        <div id="app">
          <router-view/>
        </div>
      </v-container>
    </v-content>
    <Footer/>
  </v-app>
</template>

<script>
import Footer from "../src/components/Footer.vue";
import Toolbar from "../src/components/Toolbar.vue";

export default {
  name: "App",
  components: { Footer, Toolbar }
};
</script>