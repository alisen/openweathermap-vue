import Vue from 'vue'
import store from './store'
import App from './App.vue'
import router from './router'
import Vuetify from 'vuetify'
import VuetifyToast from 'vuetify-toast-snackbar'

Vue.config.productionTip = false

Vue.use(Vuetify);
Vue.use(VuetifyToast, {
  x: 'top',
  y: 'bottom',
  color: 'info',
  icon: 'info',
  timeout: 3000,
  dismissable: true,
  autoHeight: true,
  multiLine: true,
  vertical: false,
  property: '$toast'
});

window.store = store
window.app = new Vue({
  store,
  router,
  render: h => h(App)
}).$mount('#app')