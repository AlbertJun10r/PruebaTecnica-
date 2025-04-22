<!-- src/components/ventas/VentasList.vue -->
<template>
  <div class="ventas-list">
    <div class="filters">
      <input 
        type="text" 
        v-model="filtroClienteLocal" 
        placeholder="Buscar por cliente..." 
        class="search-input"
        @input="actualizarFiltros"
      />
      <input 
        type="date" 
        v-model="filtroFechaLocal" 
        placeholder="Filtrar por fecha" 
        class="date-input"
        @input="actualizarFiltros"
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
          <tr v-for="venta in ventas" :key="venta.id">
            <td>{{ venta.id }}</td>
            <td>{{ venta.cliente.nombre }}</td>
            <td>{{ formatDate(venta.fecha) }}</td>
            <td>{{ formatCurrency(venta.total) }}</td>
            <td class="acciones">
              <button @click="$emit('ver', venta.id)" class="btn-ver">
                Ver
              </button>
              <button @click="$emit('editar', venta.id)" class="btn-editar">
                Editar
              </button>
              <button @click="$emit('eliminar', venta)" class="btn-eliminar">
                Eliminar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<style scoped src= "@/assets/CSS/Venta/VentaList.css"></style>

<script>
import { ref, watch } from 'vue';

export default {
  name: 'VentasList',
  
  props: {
    ventas: {
      type: Array,
      required: true
    },
    isLoading: {
      type: Boolean,
      default: false
    },
    filtroCliente: {
      type: String,
      default: ''
    },
    filtroFecha: {
      type: String,
      default: ''
    }
  },
  
  emits: ['ver', 'editar', 'eliminar', 'filtrar'],
  
  setup(props, { emit }) {
    const filtroClienteLocal = ref(props.filtroCliente);
    const filtroFechaLocal = ref(props.filtroFecha);
    
    watch(() => props.filtroCliente, (newVal) => {
      filtroClienteLocal.value = newVal;
    });
    
    watch(() => props.filtroFecha, (newVal) => {
      filtroFechaLocal.value = newVal;
    });
    
    const actualizarFiltros = () => {
      emit('filtrar', {
        cliente: filtroClienteLocal.value,
        fecha: filtroFechaLocal.value
      });
    };
    
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
    
    return {
      filtroClienteLocal,
      filtroFechaLocal,
      actualizarFiltros,
      formatDate,
      formatCurrency
    };
  }
};
</script>
<<<<<<< Updated upstream

<style scoped>
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
</style>
=======
>>>>>>> Stashed changes
