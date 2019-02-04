<template>
  <v-container fluid grid-list-md>
    <p>History</p>
    <div v-for="(item,i) in forecastHistory" :key="i">
      <h4>{{item.city}} {{item.country}}</h4>
      <table>
        <thead>
          <tr class="text-xs-center">
            <th v-for="(dayItem,j) in item.forecasts" :key="j">
              <h5>{{dayItem.date}}</h5>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td v-for="(dayItem,k) in item.forecasts" :key="k" class="text-xs-center">
              <h5 class="text-xs-center">{{dayItem.temperature }} °C</h5>
              <h5 class="text-xs-center">{{dayItem.humidity }} %</h5>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </v-container>
</template>

<script>
import { sync } from "vuex-pathify";

export default {
  components: { sync },
  computed: {
    ...sync("*")
  }
};
</script>