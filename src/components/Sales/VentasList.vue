<template>
  <div class="container mx-auto px-4 py-6">
    <div class="flex flex-col space-y-6">
      <!-- Encabezado -->
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-800">Productos</h1>
          <p class="text-gray-600">Gestiona el inventario de productos</p>
        </div>
        
        <div class="flex flex-col sm:flex-row gap-3">
          <div class="relative flex-1 min-w-[200px]">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
              </svg>
            </div>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Buscar productos..."
              class="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            />
          </div>
          
          <button
            @click="mostrarFormulario"
            class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors flex items-center justify-center gap-2"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Nuevo Producto
          </button>
        </div>
      </div>
      
      <!-- Tabla de productos -->
      <div class="bg-white rounded-xl shadow overflow-hidden">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Código</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Precio</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Stock</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Categoría</th>
                <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="producto in productosFiltrados" :key="producto.id" class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ producto.codigo }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ producto.nombre }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ formatCurrency(producto.precio) }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium" :class="{'text-red-600': producto.cantidadStock <= producto.stockMinimo}">
                  <div class="flex items-center gap-2">
                    {{ producto.cantidadStock }}
                    <span v-if="producto.cantidadStock <= producto.stockMinimo" class="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800">
                      Alerta
                    </span>
                  </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ producto.categoria }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <div class="flex justify-end gap-2">
                    <button @click="editarProducto(producto)" class="text-blue-600 hover:text-blue-900">
                      Editar
                    </button>
                    <button @click="handleEliminarProducto(producto.id)" class="text-red-600 hover:text-red-900">
                      Eliminar
                    </button>
                  </div>
                </td>
              </tr>
              
              <tr v-if="productosFiltrados.length === 0">
                <td colspan="6" class="px-6 py-4 text-center text-sm text-gray-500">
                  No se encontraron productos
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    
    <!-- Modal de formulario -->
    <VentaForm 
      v-if="mostrarForm" 
      :producto="productoSeleccionado" 
      @guardar="guardarProducto" 
      @cancelar="ocultarFormulario" 
    />
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { useApi } from '@/components/composables/useApi';
import { useToast } from '@/components/composables/useToast';
import VentaForm from '@/components/Sales/VentasForm.vue';

export default {
  components: {
    VentaForm
  },
  setup() {
    const { getProductos, crearProducto, actualizarProducto, eliminarProducto } = useApi();
    const { showToast } = useToast();
    
    const productos = ref([]);
    const searchQuery = ref('');
    const mostrarForm = ref(false);
    const productoSeleccionado = ref(null);

    const productosFiltrados = computed(() => {
      if (!searchQuery.value) return productos.value;
      const query = searchQuery.value.toLowerCase();
      return productos.value.filter(p => 
        p.nombre.toLowerCase().includes(query) || 
        p.codigo.toLowerCase().includes(query) ||
        (p.categoria && p.categoria.toLowerCase().includes(query))
      );
    });

    const formatCurrency = (value) => {
      return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN'
      }).format(value);
    };

    const cargarProductos = async () => {
      try {
        const response = await getProductos();
        productos.value = response.data || [];
      } catch (error) {
        showToast('Error al cargar los productos', 'error');
      }
    };

    const mostrarFormulario = () => {
      productoSeleccionado.value = null;
      mostrarForm.value = true;
    };

    const ocultarFormulario = () => {
      mostrarForm.value = false;
    };

    const editarProducto = (producto) => {
      productoSeleccionado.value = producto;
      mostrarForm.value = true;
    };

    const guardarProducto = async (productoData) => {
      try {
        if (productoData.id) {
          await actualizarProducto(productoData.id, productoData);
          showToast('Producto actualizado con éxito', 'success');
        } else {
          await crearProducto(productoData);
          showToast('Producto creado con éxito', 'success');
        }
        await cargarProductos();
        ocultarFormulario();
      } catch (error) {
        showToast('Error al guardar el producto', 'error');
      }
    };

    const handleEliminarProducto = async (id) => {
      if (confirm('¿Estás seguro de eliminar este producto?')) {
        try {
          await eliminarProducto(id);
          showToast('Producto eliminado con éxito', 'success');
          await cargarProductos();
        } catch (error) {
          showToast('Error al eliminar el producto', 'error');
          console.error(error);
          if (error.response?.data?.message) {
            showToast(error.response.data.message, 'error');
          }
        }
      }
    };

    onMounted(() => {
      cargarProductos();
    });

    return {
      productos,
      searchQuery,
      productosFiltrados,
      mostrarForm,
      productoSeleccionado,
      formatCurrency,
      mostrarFormulario,
      ocultarFormulario,
      editarProducto,
      guardarProducto,
      handleEliminarProducto
    };
  }
};
</script>

<style scoped>
.container {
  max-width: 1200px;
}

.rounded-xl {
  border-radius: 0.75rem;
}

.shadow {
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06);
}

.transition-colors {
  transition-property: background-color, border-color, color, fill, stroke;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 150ms;
}

.focus\:ring-2:focus {
  --tw-ring-offset-shadow: var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color);
  --tw-ring-shadow: var(--tw-ring-inset) 0 0 0 calc(2px + var(--tw-ring-offset-width)) var(--tw-ring-color);
  box-shadow: var(--tw-ring-offset-shadow), var(--tw-ring-shadow), var(--tw-shadow, 0 0 #0000);
}

.focus\:ring-blue-500:focus {
  --tw-ring-color: #3b82f6;
}
</style>