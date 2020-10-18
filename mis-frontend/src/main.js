import Vue from 'vue';
import App from './App.vue';
import router from './router/router.js';
import store from './store/store.js';
Vue.config.productionTip = false;

// Element UI
import Element from 'element-ui';
import './style/theme.scss';
Vue.use(Element);

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app');
