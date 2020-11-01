import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

import { SET_COLLAPSED_UI, SET_MENU_DRAWER_OPEN, SET_SETTINGS_DRAWER_OPEN } from './mutations.js';

export default new Vuex.Store({
  state: {
    collapsedUi: false, // Holds whether the UI is collapsed to fit smaller screens
    menuOpen: true,
    settings: {
      drawerOpen: false,
    },
  },
  mutations: {
    [SET_COLLAPSED_UI](state, payload) {
      state.collapsedUi = payload;
      if (state.collapsedUi) {
        state.menuOpen = false;
      }
    },
    [SET_MENU_DRAWER_OPEN](state, payload) {
      state.menuOpen = payload;
    },
    [SET_SETTINGS_DRAWER_OPEN](state, payload) {
      state.settings.drawerOpen = payload;
    },
  },
  actions: {},
  modules: {},
});
