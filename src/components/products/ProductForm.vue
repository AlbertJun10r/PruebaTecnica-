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
        
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="descripcion">Descripción</label>
          <textarea
            v-model="formData.descripcion"
            id="descripcion"
            class="w-full border rounded px-3 py-2"
            rows="3"
          ></textarea>
        </div>
        
        <div class="grid grid-cols-2 gap-4">
          <div class="mb-4">
            <label class="block text-sm font-medium mb-1" for="precio">Precio</label>
            <input
              v-model.number="formData.precio"
              id="precio"
              type="number"
              step="0.01"
              min="0"
              class="w-full border rounded px-3 py-2"
              required
            />
          </div>
          
          <div class="mb-4">
            <label class="block text-sm font-medium mb-1" for="cantidadStock">Stock actual</label>
            <input
              v-model.number="formData.cantidadStock"
              id="cantidadStock"
              type="number"
              step="1"
              min="0"
              class="w-full border rounded px-3 py-2"
              required
            />
          </div>
        </div>
        
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="stockMinimo">Stock mínimo</label>
          <input
            v-model.number="formData.stockMinimo"
            id="stockMinimo"
            type="number"
            step="1"
            min="0"
            class="w-full border rounded px-3 py-2"
            required
          />
          <p class="text-sm text-gray-500 mt-1">
            Nivel mínimo de stock para alertas de reposición
          </p>
        </div>
        
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1" for="categoria">Categoría</label>
          <input
            v-model="formData.categoria"
            id="categoria"
            type="text"
            class="w-full border rounded px-3 py-2"
          />
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
<style scoped src= "@/assets/CSS/Producto/ProductForm.css"></style>

<script setup>
import { ref, computed, onMounted } from 'vue';

const props = defineProps({
  producto: {
    type: Object,
    default: null
  }
});

const emit = defineEmits(['guardar', 'cancelar']);

const formData = ref({
  codigo: '',
  nombre: '',
  descripcion: '',
  precio: 0,
  cantidadStock: 0,
  stockMinimo: 5,
  categoria: ''
});

const guardando = ref(false);

const formTitle = computed(() => {
  return props.producto?.id ? 'Editar Producto' : 'Nuevo Producto';
});

onMounted(() => {
  if (props.producto) {
    formData.value = { ...props.producto };
  }
});

async function guardar() {
  guardando.value = true;
  try {
    formData.value.precio = Number(formData.value.precio);
    formData.value.cantidadStock = Number(formData.value.cantidadStock);
    formData.value.stockMinimo = Number(formData.value.stockMinimo);
    
    emit('guardar', { ...formData.value });
  } catch (error) {
    console.error('Error al guardar:', error);
  } finally {
    guardando.value = false;
  }
}
</script>