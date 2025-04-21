import { ref } from 'vue';

export function useToast() {
  const toast = ref({
    show: false,
    message: '',
    type: 'info', // Puede ser: 'success', 'error', 'warning', 'info'
    timeout: 3000 // Duración en milisegundos
  });

  const showToast = (message, type = 'info', timeout = 3000) => {
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

  return {
    toast,
    showToast
  };
}