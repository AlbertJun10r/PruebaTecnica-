<template>
  <div class="productos-container">
    <div class="productos-header">
      <h1>Productos</h1>
      <div class="productos-actions">
        <button @click="mostrarFormulario" class="btn-primary">
          Nuevo Producto
        </button>
        <div class="search-container">
          <input 
            v-model="searchQuery" 
            placeholder="Buscar producto..." 
            class="search-input"
          />
        </div>
      </div>
    </div>

    <div class="productos-table-container">
      <table class="productos-table">
        <thead>
          <tr>
            <th>Código</th>
            <th>Nombre</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Categoría</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="producto in productosFiltrados" :key="producto.id">
            <td>{{ producto.codigo }}</td>
            <td>{{ producto.nombre }}</td>
            <td>{{ formatCurrency(producto.precio) }}</td>
            <td :class="{'low-stock': producto.cantidadStock <= producto.stockMinimo}">
              {{ producto.cantidadStock }}
              <span v-if="producto.cantidadStock <= producto.stockMinimo" class="stock-alert">!</span>
            </td>
            <td>{{ producto.categoria }}</td>
            <td class="actions">
              <button @click="editarProducto(producto)" class="btn-edit">
                Editar
              </button>
              <button @click="confirmarEliminar(producto)" class="btn-delete">
                Eliminar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <ProductForm 
      v-if="mostrarForm" 
      :producto="productoSeleccionado" 
      @guardar="guardarProducto" 
      @cancelar="ocultarFormulario" 
    />
    
    <!-- Modal de confirmación para eliminar -->
    <div v-if="modalEliminar" class="modal">
      <div class="modal-content">
        <div class="modal-header">
          <h3>Confirmar eliminación</h3>
          <button @click="modalEliminar = false" class="btn-close">×</button>
        </div>
        <p>¿Está seguro que desea eliminar el producto "{{ productoSeleccionado?.nombre }}"?</p>
        <div class="modal-actions">
          <button @click="modalEliminar = false" class="btn-secondary">Cancelar</button>
          <button @click="eliminarProductoConfirmado" class="btn-danger">Eliminar</button>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped src= "@/assets/CSS/Producto/ProductList.css"></style>

<script>
import { ref, computed, onMounted } from 'vue';
import { useApi } from '@/components/composables/useApi';
import { useToast } from '@/components/composables/useToast';
import ProductForm from './ProductForm.vue';

export default {
  components: {
    ProductForm
  },
  setup() {
    const { getProductos, crearProducto, actualizarProducto, eliminarProducto } = useApi();
    const { showToast } = useToast();
    
    const productos = ref([]);
    const searchQuery = ref('');
    const mostrarForm = ref(false);
    const productoSeleccionado = ref(null);
    
    // Estado para el modal de eliminar
    const modalEliminar = ref(false);

    const productosFiltrados = computed(() => {
      if (!searchQuery.value) return productos.value;
      const query = searchQuery.value.toLowerCase();
      return productos.value.filter(p => 
        p.nombre.toLowerCase().includes(query) || 
        p.codigo.toLowerCase().includes(query) ||
        p.categoria.toLowerCase().includes(query)
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
    
    // Nuevas funciones para manejar el modal de eliminar
    const confirmarEliminar = (producto) => {
      productoSeleccionado.value = producto;
      modalEliminar.value = true;
    };
    
    const eliminarProductoConfirmado = async () => {
      try {
        await eliminarProducto(productoSeleccionado.value.id);
        showToast('Producto eliminado con éxito', 'success');
        modalEliminar.value = false;
        await cargarProductos();
      } catch (error) {
        showToast('Error al eliminar el producto', 'error');
        console.error(error);
        if (error.response?.data?.message) {
          showToast(error.response.data.message, 'error');
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
      modalEliminar,
      confirmarEliminar,
      eliminarProductoConfirmado
    };
  }
};
</script>