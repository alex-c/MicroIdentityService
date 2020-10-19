import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

import { SET_SETTINGS_DRAWER_OPEN } from './mutations.js';

export default new Vuex.Store({
  state: {
    settings: {
      drawerOpen: false,
    },
  },
  mutations: {
    [SET_SETTINGS_DRAWER_OPEN](state, payload) {
      state.settings.drawerOpen = payload;
    },
  },
  actions: {},
  modules: {},
});
