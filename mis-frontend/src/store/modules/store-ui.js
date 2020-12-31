import { SET_COLLAPSED_UI, SET_MENU_DRAWER_OPEN, SET_SETTINGS_DRAWER_OPEN } from '../mutations.js';

export default {
  state: {
    collapsedUi: false, // Holds whether the UI is collapsed to fit smaller screens
    menuOpen: true,
    settingsDrawerOpen: false,
  },
  mutations: {
    [SET_COLLAPSED_UI](state, payload) {
      state.collapsedUi = payload;
    },
    [SET_MENU_DRAWER_OPEN](state, payload) {
      state.menuOpen = payload;
    },
    [SET_SETTINGS_DRAWER_OPEN](state, payload) {
      state.settingsDrawerOpen = payload;
    },
  },
  actions: {},
  getters: {},
};
