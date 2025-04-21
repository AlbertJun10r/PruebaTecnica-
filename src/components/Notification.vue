<template>
    <transition name="notification-fade">
      <div v-if="show" class="notification" :class="typeClass">
        <div class="notification-icon">
          <i class="fas" :class="iconClass"></i>
        </div>
        <div class="notification-content">
          {{ mensaje }}
        </div>
        <button class="notification-close" @click="close">
          <i class="fas fa-times"></i>
        </button>
      </div>
    </transition>
  </template>
  
  <script>
  import { mapState } from 'vuex';
  
  export default {
    name: 'Notification',
    data() {
      return {
        timer: null
      };
    },
    computed: {
      ...mapState({
        show: state => state.notificacion.show,
        mensaje: state => state.notificacion.mensaje,
        tipo: state => state.notificacion.tipo
      }),
      typeClass() {
        return `notification-${this.tipo}`;
      },
      iconClass() {
        switch (this.tipo) {
          case 'success':
            return 'fa-check-circle';
          case 'error':
            return 'fa-exclamation-circle';
          case 'warning':
            return 'fa-exclamation-triangle';
          default:
            return 'fa-info-circle';
        }
      }
    },
    watch: {
      show(newVal) {
        if (newVal) {
          this.startTimer();
        } else {
          clearTimeout(this.timer);
        }
      }
    },
    methods: {
      close() {
        this.$store.commit('CLEAR_NOTIFICACION');
      },
      startTimer() {
        clearTimeout(this.timer);
        this.timer = setTimeout(() => {
          this.close();
        }, 5000); // Auto-close after 5 seconds
      }
    }
  };
  </script>
  
  <style scoped>
  .notification {
    position: fixed;
    top: 20px;
    right: 20px;
    width: 300px;
    padding: 15px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    z-index: 1000;
    display: flex;
    align-items: center;
  }
  
  .notification-success {
    background-color: #d4edda;
    color: #155724;
    border-left: 4px solid #28a745;
  }
  
  .notification-error {
    background-color: #f8d7da;
    color: #721c24;
    border-left: 4px solid #dc3545;
  }
  
  .notification-warning {
    background-color: #fff3cd;
    color: #856404;
    border-left: 4px solid #ffc107;
  }
  
  .notification-icon {
    margin-right: 12px;
    font-size: 1.25rem;
  }
  
  .notification-content {
    flex: 1;
  }
  
  .notification-close {
    background: none;
    border: none;
    cursor: pointer;
    opacity: 0.6;
    transition: opacity 0.2s;
    color: inherit;
    padding: 0;
    margin-left: 10px;
  }
  
  .notification-close:hover {
    opacity: 1;
  }
  
  /* Animation */
  .notification-fade-enter-active,
  .notification-fade-leave-active {
    transition: all 0.3s ease;
  }
  
  .notification-fade-enter-from,
  .notification-fade-leave-to {
    opacity: 0;
    transform: translateX(30px);
  }
  </style>