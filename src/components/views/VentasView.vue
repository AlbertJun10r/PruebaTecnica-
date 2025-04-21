<!-- src/views/VentasView.vue -->
<template>
  <div class="ventas-view">
    <div class="header">
      <h1>Gestión de Ventas</h1>
      <router-link to="/ventas/crear" class="btn-crear">Nueva Venta</router-link>
    </div>
    
    <div class="filters">
      <input 
        type="text" 
        v-model="filtroCliente" 
        placeholder="Buscar por cliente..." 
        class="search-input"
      />
      <input 
        type="date" 
        v-model="filtroFecha" 
        placeholder="Filtrar por fecha" 
        class="date-input"
      />
    </div>
    
    <div v-if="isLoading" class="loading">
      <p>Cargando ventas...</p>
    </div>
    <div v-else-if="!ventas.length" class="no-data">
      <p>No hay ventas registradas</p>
    </div>
    <div v-else class="tabla-container">
      <table class="tabla-ventas">
        <thead>
          <tr>
            <th>ID</th>
            <th>Cliente</th>
            <th>Fecha</th>
            <th>Total</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="venta in ventasFiltradas" :key="venta.id">
            <td>{{ venta.id }}</td>
            <td>{{ venta.cliente.nombre }}</td>
            <td>{{ formatDate(venta.fecha) }}</td>
            <td>{{ formatCurrency(venta.total) }}</td>
            <td class="acciones">
              <router-link :to="`/ventas/${venta.id}`" class="btn-ver">
                Ver
              </router-link>
              <router-link :to="`/ventas/${venta.id}/editar`" class="btn-editar">
                Editar
              </router-link>
              <button @click="confirmarEliminar(venta)" class="btn-eliminar">
                Eliminar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Modal de confirmación para eliminar -->
    <div v-if="modalEliminar" class="modal">
      <div class="modal-content">
        <h3>Confirmar eliminación</h3>
        <p>¿Está seguro que desea eliminar la venta #{{ ventaSeleccionada?.id }}?</p>
        <div class="modal-actions">
          <button @click="modalEliminar = false" class="btn-secondary">Cancelar</button>
          <button @click="eliminarVenta" class="btn-danger">Eliminar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { useApi } from '../composables/useApi';
import { useToast } from '../composables/useToast'; // Asumiendo que tienes este composable

export default {
  name: 'VentasView',
  
  setup() {
    const { getVentas, eliminarVenta: apiEliminarVenta } = useApi();
    const { showToast } = useToast();
    
    const ventas = ref([]);
    const isLoading = ref(true);
    const filtroCliente = ref('');
    const filtroFecha = ref('');
    const modalEliminar = ref(false);
    const ventaSeleccionada = ref(null);
    
    const cargarVentas = async () => {
      isLoading.value = true;
      try {
        const response = await getVentas();
        ventas.value = response.data || [];
      } catch (error) {
        showToast('Error al cargar las ventas', 'error');
      } finally {
        isLoading.value = false;
      }
    };
    
    const ventasFiltradas = computed(() => {
      return ventas.value.filter(venta => {
        const matchCliente = !filtroCliente.value || 
          venta.cliente.nombre.toLowerCase().includes(filtroCliente.value.toLowerCase());
        
        const matchFecha = !filtroFecha.value || 
          venta.fecha.split('T')[0] === filtroFecha.value;
        
        return matchCliente && matchFecha;
      });
    });
    
    const formatDate = (dateString) => {
      const options = { year: 'numeric', month: 'long', day: 'numeric' };
      return new Date(dateString).toLocaleDateString('es-ES', options);
    };
    
    const formatCurrency = (value) => {
      return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN'
      }).format(value);
    };
    
    const confirmarEliminar = (venta) => {
      ventaSeleccionada.value = venta;
      modalEliminar.value = true;
    };
    
    const eliminarVenta = async () => {
      try {
        await apiEliminarVenta(ventaSeleccionada.value.id);
        showToast('Venta eliminada con éxito', 'success');
        modalEliminar.value = false;
        cargarVentas();
      } catch (error) {
        showToast('Error al eliminar la venta', 'error');
      }
    };
    
    onMounted(() => {
      cargarVentas();
    });
    
    return {
      ventas,
      isLoading,
      filtroCliente,
      filtroFecha,
      ventasFiltradas,
      modalEliminar,
      ventaSeleccionada,
      formatDate,
      formatCurrency,
      confirmarEliminar,
      eliminarVenta
    };
  }
};
</script>

<style scoped>
.ventas-view {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.btn-crear {
  background-color: #4caf50;
  color: white;
  padding: 10px 20px;
  text-decoration: none;
  border-radius: 4px;
}

.filters {
  display: flex;
  gap: 15px;
  margin-bottom: 20px;
}

.search-input, .date-input {
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.search-input {
  flex: 1;
}

.loading, .no-data {
  text-align: center;
  padding: 20px;
  background-color: #f8f9fa;
  border-radius: 4px;
}

.tabla-container {
  overflow-x: auto;
}

.tabla-ventas {
  width: 100%;
  border-collapse: collapse;
}

.tabla-ventas th, .tabla-ventas td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.tabla-ventas th {
  background-color: #f2f2f2;
  font-weight: bold;
}

.tabla-ventas tr:hover {
  background-color: #f5f5f5;
}

.acciones {
  display: flex;
  gap: 8px;
}

.btn-ver, .btn-editar, .btn-eliminar {
  padding: 6px 12px;
  border-radius: 4px;
  text-decoration: none;
  font-size: 0.9em;
  cursor: pointer;
  border: none;
}

.btn-ver {
  background-color: #17a2b8;
  color: white;
}

.btn-editar {
  background-color: #ffc107;
  color: #212529;
}

.btn-eliminar {
  background-color: #dc3545;
  color: white;
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
  padding: 20px;
  border-radius: 4px;
  width: 400px;
  max-width: 90%;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
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
</style>