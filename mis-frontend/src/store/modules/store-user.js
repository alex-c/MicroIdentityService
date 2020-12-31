// JWT decoding utility
const jwtDecode = require('jwt-decode');

// Mutation and action types
import { SIGN_IN, SIGN_OUT } from '../actions.js';
import { SET_USER_DATA, PURGE_USER_DATA } from '../mutations.js';

// Load user data from local storage
const token = localStorage.getItem('token');
const identifier = localStorage.getItem('identifier');
const name = localStorage.getItem('name');

// Admin role detection
const MIS_ADMIN_ROLE = 'mis.admin';
function isAdmin(decodedToken) {
  if (Array.isArray(decodedToken.role)) {
    return decodedToken.role.includes(MIS_ADMIN_ROLE);
  } else if (typeof decodedToken.role === 'string') {
    return decodedToken.role === MIS_ADMIN_ROLE;
  }
  return false;
}

export default {
  state: {
    token: token,
    identifier: identifier,
    name: name,
  },
  mutations: {
    [SET_USER_DATA](state, data) {
      state.token = data.token;
      state.identifier = data.identifier;
      state.name = data.name;
    },
    [PURGE_USER_DATA](state) {
      state.token = null;
      state.identifier = null;
      state.name = null;
    },
  },
  actions: {
    [SIGN_IN]({ commit }, token) {
      const decodedToken = jwtDecode(token);
      if (isAdmin(decodedToken)) {
        const identifier = decodedToken.sub;
        const name = decodedToken.unique_name;
        localStorage.setItem('token', token);
        localStorage.setItem('identifier', identifier);
        localStorage.setItem('name', name);
        commit(SET_USER_DATA, { token, identifier, name });
      } else {
        return new Error();
      }
    },
    [SIGN_OUT]({ commit }) {
      localStorage.removeItem('token');
      localStorage.removeItem('identifier');
      localStorage.removeItem('name');
      commit(PURGE_USER_DATA);
    },
  },
  getters: {},
};
