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

<script>
import { ref, computed, onMounted } from 'vue';
import { useApi } from '@/components/composables/useApi';
import { useToast } from '@/components/composables/useToast';
import ProductForm from '@/components/Products/ProductForm.vue';

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

<style scoped>
.productos-container {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.productos-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 15px;
}

.productos-header h1 {
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

.productos-actions {
  display: flex;
  gap: 15px;
  align-items: center;
}

.search-container {
  position: relative;
}

.search-input {
  padding: 8px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 250px;
}

.productos-table-container {
  overflow-x: auto;
}

.productos-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 15px;
}

.productos-table th,
.productos-table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.productos-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #333;
}

.productos-table tr:hover {
  background-color: #f5f5f5;
}

.low-stock {
  color: #d32f2f;
  font-weight: bold;
}

.stock-alert {
  display: inline-block;
  width: 18px;
  height: 18px;
  background-color: #d32f2f;
  color: white;
  border-radius: 50%;
  text-align: center;
  line-height: 18px;
  font-size: 12px;
  margin-left: 5px;
}

.actions {
  display: flex;
  gap: 8px;
}

.btn-primary {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-primary:hover {
  background-color: #388e3c;
}

.btn-edit {
  background-color: #2196f3;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-edit:hover {
  background-color: #1976d2;
}

.btn-delete {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-delete:hover {
  background-color: #d32f2f;
}

@media (max-width: 768px) {
  .productos-header {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .productos-actions {
    width: 100%;
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }
  
  .search-input {
    width: 100%;
  }
  
  .actions {
    flex-direction: column;
    gap: 5px;
  }
  
  .productos-table th,
  .productos-table td {
    padding: 8px;
    font-size: 14px;
  }

  .modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  }

  .modal-content {
    background-color: white;
    padding: 0;
    border-radius: 4px;
    width: 500px;
    max-width: 90%;
  }

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
  }

  .modal-header h3 {
    margin: 0;
  }

  .btn-close {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #666;
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
    padding: 15px 20px;
    border-top: 1px solid #eee;
  }

  .btn-secondary, .btn-danger {
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

  .btn-secondary {
    background-color: #6c757d;
    color: white;
  }

  .btn-danger {
    background-color: #dc3545;
    color: white;
  }
}
</style>