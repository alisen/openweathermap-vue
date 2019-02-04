<template>
  <v-container fluid grid-list-xl>
    <v-layout row justify>
      <v-flex xl12>
        <v-card dark>
          <History/>
        </v-card>
      </v-flex>
      <v-flex md6>
        <v-card dark>
          <CityName/>
        </v-card>
      </v-flex>
      <v-flex md6>
        <v-card dark>
          <ZipCode/>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import ZipCode from "../components/ZipCode";
import CityName from "../components/CityName";
import History from "../components/History";

export default {
  components: { ZipCode, CityName, History }
};
</script>