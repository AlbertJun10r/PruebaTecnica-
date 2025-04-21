<template>
  <div class="app-container">
    <aside class="sidebar" :class="{ 'collapsed': sidebarCollapsed }">
      <div class="sidebar-header">
        <h1 class="sidebar-title" v-if="!sidebarCollapsed">Panel Admin</h1>
        <span class="sidebar-toggle" @click="toggleSidebar">
          <i class="fas" :class="sidebarCollapsed ? 'fa-chevron-right' : 'fa-chevron-left'"></i>
        </span>
      </div>
      
      <nav class="sidebar-nav">
        <router-link to="/" class="nav-item" title="Dashboard">
          <i class="fas fa-tachometer-alt"></i>
          <span v-if="!sidebarCollapsed">Dashboard</span>
        </router-link>
        <router-link to="/productos" class="nav-item" title="Productos">
          <i class="fas fa-box"></i>
          <span v-if="!sidebarCollapsed">Productos</span>
        </router-link>
        <router-link to="/clientes" class="nav-item" title="Clientes">
          <i class="fas fa-users"></i>
          <span v-if="!sidebarCollapsed">Clientes</span>
        </router-link>
        <router-link to="/ventas" class="nav-item" title="Ventas">
          <i class="fas fa-cash-register"></i>
          <span v-if="!sidebarCollapsed">Ventas</span>
        </router-link>
      </nav>
    </aside>

    <main class="main-content">
      <header class="header">
        <div class="header-left">
          <h2 class="page-title">{{ currentRouteName }}</h2>
        </div>
        <div class="header-right">
          <div class="user-profile">
            <span class="user-name">Admin</span>
            <div class="avatar">
              <i class="fas fa-user"></i>
            </div>
          </div>
        </div>
      </header>

      <div class="content">
        <router-view></router-view>
      </div>
      
          <!-- Componente Toast -->
    <AppToast :toast="toast" />
    </main>
  </div>
</template>

<script>
import { ref, onMounted, onBeforeUnmount } from 'vue';

export default {
  name: 'App',
  setup() {
    const sidebarCollapsed = ref(false);
    const toast = ref({
      show: false,
      message: '',
      type: 'info',
      timeout: 3000
    });

    const handleToastEvent = (event) => {
      const { message, type, timeout } = event.detail;
      toast.value = {
        show: true,
        message,
        type,
        timeout
      };
      
      // Ocultar el toast después del tiempo especificado
      setTimeout(() => {
        toast.value.show = false;
      }, timeout);
    };

    onMounted(() => {
      window.addEventListener('show-toast', handleToastEvent);
    });

    onBeforeUnmount(() => {
      window.removeEventListener('show-toast', handleToastEvent);
    });

    return {
      sidebarCollapsed,
      toast
    };
  },
  computed: {
    currentRouteName() {
      const routeName = this.$route.name;
      return routeName ? routeName.charAt(0).toUpperCase() + routeName.slice(1) : 'Dashboard';
    }
  },
  methods: {
    toggleSidebar() {
      this.sidebarCollapsed = !this.sidebarCollapsed;
    }
  }
};
</script>

<style>
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css');

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: #f5f7fb;
  color: #333;
}

.app-container {
  display: flex;
  min-height: 100vh;
}

.sidebar {
  background: #2c3e50;
  color: #ecf0f1;
  width: 250px;
  transition: width 0.3s ease;
  overflow: hidden;
}

.sidebar.collapsed {
  width: 60px;
}

.sidebar-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.sidebar-title {
  font-size: 1.2rem;
  font-weight: 500;
}

.sidebar-toggle {
  cursor: pointer;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.sidebar-nav {
  padding: 1rem 0;
}

.nav-item {
  display: flex;
  align-items: center;
  padding: 0.75rem 1rem;
  color: #bdc3c7;
  text-decoration: none;
  transition: all 0.2s ease;
}

.nav-item:hover, .nav-item.router-link-active {
  color: #ecf0f1;
  background: rgba(255, 255, 255, 0.1);
}

.nav-item i {
  margin-right: 1rem;
  width: 20px;
  text-align: center;
}

.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.header {
  background: #fff;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 1.5rem;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
}

.page-title {
  font-size: 1.5rem;
  font-weight: 500;
}

.user-profile {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.user-name {
  margin-right: 0.75rem;
  font-weight: 500;
}

.avatar {
  width: 36px;
  height: 36px;
  background: #e1e5e8;
  color: #2c3e50;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.content {
  padding: 1.5rem;
  flex: 1;
}

/* Estilos comunes para componentes */
.card {
  background: #fff;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  margin-bottom: 1.5rem;
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
}

.card-title {
  font-size: 1.25rem;
  font-weight: 500;
}

.btn {
  padding: 0.5rem 1rem;
  border-radius: 4px;
  font-weight: 500;
  cursor: pointer;
  border: none;
  transition: all 0.2s ease;
}

.btn-primary {
  background: #3498db;
  color: #fff;
}

.btn-primary:hover {
  background: #2980b9;
}

.btn-danger {
  background: #e74c3c;
  color: #fff;
}

.btn-danger:hover {
  background: #c0392b;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th,
.table td {
  padding: 0.75rem 1rem;
  text-align: left;
}

.table th {
  background: #f9f9f9;
  font-weight: 500;
}

.table tbody tr:hover {
  background: #f5f5f5;
}

.table tbody tr:not(:last-child) {
  border-bottom: 1px solid #eee;
}

.form-group {
  margin-bottom: 1rem;
}

.form-label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
}

.form-control {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

.form-control:focus {
  outline: none;
  border-color: #3498db;
}

.error-message {
  color: #e74c3c;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

.badge {
  padding: 0.25rem 0.5rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 500;
}

.badge-success {
  background: #2ecc71;
  color: #fff;
}

.badge-warning {
  background: #f39c12;
  color: #fff;
}

.badge-danger {
  background: #e74c3c;
  color: #fff;
}

/* Agrega estos estilos para el toast al final */
.toast {
  position: fixed;
  bottom: 20px;
  right: 20px;
  padding: 12px 24px;
  border-radius: 4px;
  color: white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  cursor: pointer;
  max-width: 300px;
  animation: slideIn 0.3s ease-out;
}

.toast-success {
  background-color: #4caf50;
}

.toast-error {
  background-color: #f44336;
}

.toast-warning {
  background-color: #ff9800;
}

.toast-info {
  background-color: #2196f3;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

@keyframes slideIn {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>