<template>
    <transition name="fade">
      <div 
        v-if="toast.show" 
        class="toast" 
        :class="`toast-${toast.type}`"
        @click="toast.show = false"
      >
        <div class="toast-content">
          <span>{{ toast.message }}</span>
        </div>
      </div>
    </transition>
  </template>
  
  <script>
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'AppToast',
    props: {
      toast: {
        type: Object,
        required: true
      }
    }
  });
  </script>
  
  <style scoped>
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