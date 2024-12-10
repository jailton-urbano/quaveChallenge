import Vue from 'vue';
import Router from 'vue-router';
import Communities from '../components/Communities.vue';

Vue.use(Router);

const routes = [
  {
    path: '/',
    name: 'Communities',
    component: Communities,
  },
];

const router = new Router({
  routes, // Define as rotas
});

export default router;
