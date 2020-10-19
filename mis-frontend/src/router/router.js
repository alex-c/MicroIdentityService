import Vue from 'vue';
import VueRouter from 'vue-router';
import Public from '../views/Public.vue';
import Private from '../views/Private.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'Public',
    component: Public,
  },
  {
    path: '/private',
    name: 'Private',
    component: Private,
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
