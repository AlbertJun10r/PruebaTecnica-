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