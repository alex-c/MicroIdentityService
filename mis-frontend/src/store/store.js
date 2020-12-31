import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

// Import modules
import ui from './modules/store-ui.js';
import user from './modules/store-user.js';

export default new Vuex.Store({
  state: {},
  mutations: {},
  modules: { ui, user },
});
