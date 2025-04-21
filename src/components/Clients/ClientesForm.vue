<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-10">
    <div class="bg-white p-6 rounded-lg shadow-lg w-full max-w-md max-h-screen overflow-y-auto">
      <h2 class="text-xl font-bold mb-4">
        {{ formTitle }}
      </h2>
      
      <form @submit.prevent="guardar">
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="codigo">Código</label>
          <input
            v-model="formData.codigo"
            id="codigo"
            type="text"
            class="w-full border rounded px-3 py-2"
            required
          />
        </div>
        
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="nombre">Nombre</label>
          <input
            v-model="formData.nombre"
            id="nombre"
            type="text"
            class="w-full border rounded px-3 py-2"
            required
          />
        </div>
        
        <div class="grid grid-cols-2 gap-4">
          <div class="mb-4">
            <label class="block text-sm font-medium mb-1" for="telefono">Teléfono</label>
            <input
              v-model="formData.telefono"
              id="telefono"
              type="tel"
              class="w-full border rounded px-3 py-2"
              required
            />
          </div>
          
          <div class="mb-4">
            <label class="block text-sm font-medium mb-1" for="email">Email</label>
            <input
              v-model="formData.email"
              id="email"
              type="email"
              class="w-full border rounded px-3 py-2"
            />
          </div>
        </div>
        
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="direccion">Dirección</label>
          <textarea
            v-model="formData.direccion"
            id="direccion"
            class="w-full border rounded px-3 py-2"
            rows="3"
          ></textarea>
        </div>
        
        <div class="flex justify-end space-x-3 mt-6">
          <button
            type="button"
            @click="$emit('cancelar')"
            class="px-4 py-2 border rounded hover:bg-gray-100 transition-colors"
          >
            Cancelar
          </button>
          <button
            type="submit"
            class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
            :disabled="guardando"
          >
            {{ guardando ? 'Guardando...' : 'Guardar' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';

const props = defineProps({
  cliente: {
    type: Object,
    default: null
  }
});

const emit = defineEmits(['guardar', 'cancelar']);

const formData = ref({
  codigo: '',
  nombre: '',
  telefono: '',
  email: '',
  direccion: ''
});

const guardando = ref(false);

const formTitle = computed(() => {
  return props.cliente?.id ? 'Editar Cliente' : 'Nuevo Cliente';
});

onMounted(() => {
  if (props.cliente) {
    formData.value = { ...props.cliente };
  }
});

async function guardar() {
  guardando.value = true;
  try {
    emit('guardar', { ...formData.value });
  } catch (error) {
    console.error('Error al guardar:', error);
  } finally {
    guardando.value = false;
  }
}
</script>

<style scoped>
.fixed {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 50;
}

.bg-opacity-50 {
  background-color: rgba(0, 0, 0, 0.5);
}

.bg-white {
  background-color: white;
}

.p-6 {
  padding: 1.5rem;
}

.rounded-lg {
  border-radius: 0.5rem;
}

.shadow-lg {
  box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
}

.max-w-md {
  max-width: 28rem;
}

.max-h-screen {
  max-height: 100vh;
}

.overflow-y-auto {
  overflow-y: auto;
}

.text-xl {
  font-size: 1.25rem;
  line-height: 1.75rem;
}

.font-bold {
  font-weight: 700;
}

.mb-4 {
  margin-bottom: 1rem;
}

.block {
  display: block;
}

.text-sm {
  font-size: 0.875rem;
  line-height: 1.25rem;
}

.font-medium {
  font-weight: 500;
}

.mb-1 {
  margin-bottom: 0.25rem;
}

.w-full {
  width: 100%;
}

.border {
  border-width: 1px;
  border-color: #e5e7eb;
}

.rounded {
  border-radius: 0.25rem;
}

.px-3 {
  padding-left: 0.75rem;
  padding-right: 0.75rem;
}

.py-2 {
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
}

.grid {
  display: grid;
}

.grid-cols-2 {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.gap-4 {
  gap: 1rem;
}

.flex {
  display: flex;
}

.justify-end {
  justify-content: flex-end;
}

.space-x-3 > * + * {
  margin-left: 0.75rem;
}

.mt-6 {
  margin-top: 1.5rem;
}

.hover\:bg-gray-100:hover {
  background-color: #f3f4f6;
}

.transition-colors {
  transition-property: background-color, border-color, color, fill, stroke;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 150ms;
}

.bg-blue-500 {
  background-color: #3b82f6;
}

.text-white {
  color: white;
}

.hover\:bg-blue-600:hover {
  background-color: #2563eb;
}

:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>