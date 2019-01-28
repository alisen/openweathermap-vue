import Vue from 'vue'
import App from './App.vue'
import router from './router'
import Vuetify from 'vuetify'
import Vuex from 'vuex'

Vue.config.productionTip = false

Vue.use(Vuex);
Vue.use(Vuetify);
Vue.use(require('vue-moment'));

const store = new Vuex.Store({
  state: {
    searchedCities: [],
  },
  getters: {
    getCities: state => {
      return state.searchedCities
    }
  },
})

new Vue({
  render: h => h(App),
  store,
  router
}).$mount('#app')