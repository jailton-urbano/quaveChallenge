import Vue from 'vue';
import App from './App.vue';
import router from './router';
import './styles/main.css';

Vue.config.productionTip = false;

new Vue({
  render: h => h(App),
  router // Adiciona o router à instância Vue
}).$mount('#app');
