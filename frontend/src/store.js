import Vue from 'vue'
import Vuex from 'vuex'
import pathify from './pathify'

// import helper function
import {
    make
} from 'vuex-pathify'

// setup store
const state = {
    forecastHistory: [],
    forecastsCity: null,
    forecastsZip: null
}

const actions = make.actions(state)
const getters = make.getters(state)
const mutations = {
    ...make.mutations(state),
    forecastHistoryPush(state, query) {
        state.forecastHistory.push(query);
    }
}

// pathify.debug()

// use store
Vue.use(Vuex)

// create store
const store = new Vuex.Store({
    // use the plugin
    plugins: [
        pathify.plugin
    ],

    // store properties
    state,
    actions,
    getters,
    mutations
})

export default store
window.store = store