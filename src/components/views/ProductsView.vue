<!-- src/views/ProductosView.vue -->
<template>
    <div class="productos-container">
      <h1 class="text-2xl font-bold mb-4">Gestión de Productos</h1>
      
      <ProductosList 
        :productos="productosFiltrados" 
        :loading="cargando"
        @editar="editarProducto" 
        @eliminar="confirmarEliminar" 
        @ver-stock="verDetallesStock"
      />
  
      <ProductoForm
        v-if="mostrarFormulario"
        :producto="productoSeleccionado"
        @guardar="guardarProducto"
        @cancelar="cerrarFormulario"
      />
  
      <!-- Modal de confirmación para eliminar -->
      <div v-if="mostrarConfirmacion" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-10">
        <div class="bg-white p-6 rounded-lg shadow-lg max-w-md">
          <h3 class="text-lg font-bold mb-4">Confirmar eliminación</h3>
          <p>¿Estás seguro de que deseas eliminar este producto?</p>
          <div class="mt-6 flex justify-end space-x-3">
            <button 
              @click="mostrarConfirmacion = false" 
              class="px-4 py-2 border rounded"
            >
              Cancelar
            </button>
            <button 
              @click="eliminarProducto" 
              class="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
            >
              Eliminar
            </button>
          </div>
        </div>
      </div>
  
      <!-- Modal detalles de stock -->
      <div v-if="mostrarDetallesStock" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-10">
        <div class="bg-white p-6 rounded-lg shadow-lg max-w-md">
          <h3 class="text-lg font-bold mb-4">Detalles de Stock</h3>
          <div v-if="stockLoading" class="text-center py-4">Cargando información...</div>
          <div v-else-if="detallesStock">
            <div class="mb-3">
              <p class="text-lg font-semibold">{{ detallesStock.nombre }}</p>
              <p class="text-gray-600">{{ detallesStock.descripcion }}</p>
            </div>
            <div class="grid grid-cols-2 gap-4 mb-4">
              <div>
                <p class="text-sm text-gray-500">Stock actual</p>
                <p class="font-bold text-xl" :class="{ 'text-red-500': detallesStock.cantidadStock < detallesStock.stockMinimo }">
                  {{ detallesStock.cantidadStock }}
                </p>
              </div>
              <div>
                <p class="text-sm text-gray-500">Stock mínimo</p>
                <p class="font-bold text-xl">{{ detallesStock.stockMinimo }}</p>
              </div>
            </div>
            <div class="bg-gray-100 p-3 rounded">
              <p class="text-sm">Estado: 
                <span v-if="detallesStock.cantidadStock < detallesStock.stockMinimo" class="text-red-500 font-medium">
                  Stock bajo - ¡Necesita reposición!
                </span>
                <span v-else class="text-green-500 font-medium">
                  Stock adecuado
                </span>
              </p>
            </div>
          </div>
          <div class="mt-6 flex justify-end">
            <button 
              @click="mostrarDetallesStock = false" 
              class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted, watch } from 'vue';
  import { useApi } from '../composables/useApi';
  import ProductosList from '@/components/Products/ProductList.vue';
  import ProductoForm from '@/components/Products/ProductForm.vue';
  
  const api = useApi();
  const productos = ref([]);
  const cargando = ref(false);
  const mostrarFormulario = ref(false);
  const productoSeleccionado = ref(null);
  const mostrarConfirmacion = ref(false);
  const productoIdEliminar = ref(null);
  const busqueda = ref('');
  const filtroStock = ref('todos');
  
  // Estado para el modal de detalles de stock
  const mostrarDetallesStock = ref(false);
  const stockLoading = ref(false);
  const detallesStock = ref(null);
  const productoIdStock = ref(null);
  
  // Filtrar productos basado en la búsqueda y el filtro de stock
  const productosFiltrados = computed(() => {
    let resultado = productos.value;
    
    // Filtrar por término de búsqueda
    if (busqueda.value) {
      const termino = busqueda.value.toLowerCase();
      resultado = resultado.filter(producto => 
        producto.nombre?.toLowerCase().includes(termino) || 
        producto.descripcion?.toLowerCase().includes(termino) ||
        producto.codigo?.toLowerCase().includes(termino)
      );
    }
    
    // Filtrar por stock
    if (filtroStock.value === 'bajo') {
      resultado = resultado.filter(producto => 
        producto.cantidadStock < producto.stockMinimo
      );
    }
    
    return resultado;
  });
  
  // Cargar los productos al montar el componente
  onMounted(async () => {
    await cargarProductos();
  });
  
  // Observar cambios en el filtro para recargar si es necesario
  watch(filtroStock, async (newValue) => {
    if (newValue === 'bajo') {
      await cargarProductosBajoStock();
    } else {
      await cargarProductos();
    }
  });
  
  // Cargar la lista de productos
  async function cargarProductos() {
    cargando.value = true;
    try {
      const response = await api.getProductos();
      productos.value = response.data;
    } catch (error) {
      console.error('Error al cargar productos:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Cargar productos con stock bajo
  async function cargarProductosBajoStock() {
    cargando.value = true;
    try {
      const response = await api.getProductosLowStock();
      productos.value = response.data;
    } catch (error) {
      console.error('Error al cargar productos con stock bajo:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Abrir formulario para crear un nuevo producto
  function abrirFormulario() {
    productoSeleccionado.value = null;
    mostrarFormulario.value = true;
  }
  
  // Abrir formulario para editar un producto existente
  function editarProducto(producto) {
    productoSeleccionado.value = { ...producto };
    mostrarFormulario.value = true;
  }
  
  // Cerrar el formulario
  function cerrarFormulario() {
    mostrarFormulario.value = false;
    productoSeleccionado.value = null;
  }
  
  // Guardar un producto (crear o actualizar)
  async function guardarProducto(producto) {
    cargando.value = true;
    try {
      if (producto.id) {
        // Actualizar producto existente
        await api.actualizarProducto(producto.id, producto);
      } else {
        // Crear nuevo producto
        await api.crearProducto(producto);
      }
      
      // Recargar la lista según el filtro actual
      if (filtroStock.value === 'bajo') {
        await cargarProductosBajoStock();
      } else {
        await cargarProductos();
      }
      cerrarFormulario();
    } catch (error) {
      console.error('Error al guardar producto:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Confirmar eliminación de un producto
  function confirmarEliminar(id) {
    productoIdEliminar.value = id;
    mostrarConfirmacion.value = true;
  }
  
  // Eliminar producto
  async function eliminarProducto() {
    cargando.value = true;
    try {
      await api.eliminarProducto(productoIdEliminar.value);
      
      // Recargar la lista según el filtro actual
      if (filtroStock.value === 'bajo') {
        await cargarProductosBajoStock();
      } else {
        await cargarProductos();
      }
      mostrarConfirmacion.value = false;
    } catch (error) {
      console.error('Error al eliminar producto:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Ver detalles de stock de un producto
  async function verDetallesStock(id) {
    productoIdStock.value = id;
    mostrarDetallesStock.value = true;
    stockLoading.value = true;
    
    try {
      const response = await api.getProductosLowStockById(id);
      detallesStock.value = response.data;
    } catch (error) {
      console.error('Error al cargar detalles de stock:', error);
      detallesStock.value = null;
    } finally {
      stockLoading.value = false;
    }
  }
  </script>