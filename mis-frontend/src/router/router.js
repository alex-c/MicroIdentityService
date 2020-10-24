import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

// Main layour views
import Public from '../views/Public.vue';
import Private from '../views/Private.vue';

// Private views
import Dashboard from '../views/private/Dashboard.vue';

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
    children: [
      {
        path: '/',
        component: Dashboard,
      },
    ],
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
