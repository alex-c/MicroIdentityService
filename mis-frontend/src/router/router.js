import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

// Navigation guards
import { redirectAuthorizedUser, redirectUnauthorizedUser } from './route-guards.js';

// Main layour views
import Public from '../views/Public.vue';
import Private from '../views/Private.vue';

// Private views
import Dashboard from '../views/private/Dashboard.vue';
import Identities from '../views/private/Identities.vue';
import CreateIdentity from '../views/private/identities/CreateIdentity.vue';
import IdentityRoles from '../views/private/identities/IdentityRoles.vue';
import Domains from '../views/private/Domains.vue';
import Roles from '../views/private/Roles.vue';
import CreateRole from '../views/private/roles/CreateRole.vue';

const routes = [
  {
    path: '/',
    name: 'Public',
    component: Public,
    beforeEnter: redirectAuthorizedUser,
  },
  {
    path: '/private',
    component: Private,
    beforeEnter: redirectUnauthorizedUser,
    children: [
      {
        path: '/',
        redirect: '/dashboard',
      },
      {
        path: '/dashboard',
        component: Dashboard,
      },
      {
        path: '/identities',
        component: Identities,
      },
      {
        path: '/identities/create',
        component: CreateIdentity,
      },
      {
        path: '/identities/:id/roles',
        component: IdentityRoles,
      },
      {
        path: '/domains',
        component: Domains,
      },
      {
        path: '/roles',
        name: 'roles',
        component: Roles,
        props: true,
      },
      {
        path: '/roles/create',
        component: CreateRole,
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
