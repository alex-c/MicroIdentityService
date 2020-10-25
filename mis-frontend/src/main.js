import Vue from 'vue';
import router from './router/router.js';
import store from './store/store.js';
Vue.config.productionTip = false;

// Element UI
import Element from 'element-ui';
import './style/theme.scss';
import enLocale from 'element-ui/lib/locale/lang/en';
Vue.use(Element, {
  i18n: (key, value) => i18n.t(key, value),
});

// Internationalization
import VueI18n from 'vue-i18n';
Vue.use(VueI18n);

// Load internationalization messages
import enMessages from './i18n/en.json';
const messages = {
  en: Object.assign(enMessages, enLocale),
};

// Configure internationalization
let language = localStorage.getItem('language') || 'en';
const i18n = new VueI18n({
  locale: language,
  fallbackLocale: 'en',
  messages,
});

// Mount app
import App from './App.vue';
new Vue({
  router,
  store,
  i18n,
  render: h => h(App),
}).$mount('#app');
