import { createApp } from 'vue';
import App from './App.vue';
import router from '@/components/router';
import store from './components/stores';
import AppToast from '@/components/AppToast.vue'; // Cambié Notification por AppToast

const app = createApp(App);

// Configuración global del toast
app.config.globalProperties.$toast = {
  show: (message, type = 'info', timeout = 3000) => {
    const event = new CustomEvent('show-toast', {
      detail: { message, type, timeout }
    });
    window.dispatchEvent(event);
  }
};

app.use(store);
app.use(router);
app.component('AppToast', AppToast);

app.mount('#app');