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

<style scoped src= "@/assets/CSS/Cliente/ClientesForm.css"></style>

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