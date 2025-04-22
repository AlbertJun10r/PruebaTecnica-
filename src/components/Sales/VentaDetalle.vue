<!-- src/components/ventas/VentaDetalle.vue -->
<template>
  <div v-if="mostrar" class="modal">
    <div class="modal-content modal-lg">
      <div class="modal-header">
        <h3>Detalle de Venta #{{ venta?.id }}</h3>
        <button @click="cerrar" class="btn-close">×</button>
      </div>
      
      <div v-if="isLoading" class="loading">
        <p>Cargando detalles de la venta...</p>
      </div>
      
      <div v-else-if="!venta" class="no-data">
        <p>No se encontró la venta o hubo un error al cargarla</p>
      </div>
      
      <div v-else class="venta-detalle-content">
        <div class="info-section">
          <div class="info-group">
            <h4>Información General</h4>
            <div class="info-row">
              <div class="info-label">Cliente:</div>
              <div class="info-value">{{ venta.cliente.nombre }}</div>
            </div>
            <div class="info-row">
              <div class="info-label">Fecha:</div>
              <div class="info-value">{{ formatDate(venta.fecha) }}</div>
            </div>
            <div class="info-row">
              <div class="info-label">Estado:</div>
              <div class="info-value">
                <span class="status-badge" :class="venta.estado?.toLowerCase()">
                  {{ venta.estado || 'Completada' }}
                </span>
              </div>
            </div>
          </div>
        </div>
        
        <div class="items-section">
          <h4>Productos</h4>
          <table class="items-table">
            <thead>
              <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Precio Unitario</th>
                <th>Subtotal</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in venta.items" :key="index">
                <td>{{ item.producto.nombre }}</td>
                <td>{{ item.cantidad }}</td>
                <td>{{ formatCurrency(item.precioUnitario) }}</td>
                <td>{{ formatCurrency(item.cantidad * item.precioUnitario) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        
        <div class="totals-section">
          <div class="total-row">
            <div class="total-label">Subtotal:</div>
            <div class="total-value">{{ formatCurrency(calcularSubtotal(venta.items)) }}</div>
          </div>
          <div class="total-row">
            <div class="total-label">IVA (16%):</div>
            <div class="total-value">{{ formatCurrency(calcularIVA(venta.items)) }}</div>
          </div>
          <div class="total-row grand-total">
            <div class="total-label">Total:</div>
            <div class="total-value">{{ formatCurrency(venta.total) }}</div>
          </div>
        </div>
        
        <div class="modal-actions">
          <button @click="cerrar" class="btn-secondary">Cerrar</button>
          <button @click="$emit('editar', venta.id)" class="btn-primary">Editar</button>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped src= "@/assets/CSS/Venta/VentaDetalle.css"></style>


<script>
import { toRef } from 'vue';

export default {
  name: 'VentaDetalle',
  
  props: {
    venta: {
      type: Object,
      default: null
    },
    isLoading: {
      type: Boolean,
      default: false
    },
    mostrar: {
      type: Boolean,
      default: false
    }
  },
  
  emits: ['cerrar', 'editar'],
  
  setup(props, { emit }) {
    const mostrarModal = toRef(props, 'mostrar');
    
    const cerrar = () => {
      emit('cerrar');
    };
    
    const calcularSubtotal = (items) => {
      if (!items) return 0;
      return items.reduce((total, item) => {
        return total + (item.cantidad * item.precioUnitario);
      }, 0);
    };
    
    const calcularIVA = (items) => {
      return calcularSubtotal(items) * 0.16;
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
      mostrarModal,
      cerrar,
      calcularSubtotal,
      calcularIVA,
      formatDate,
      formatCurrency
    };
  }
};
</script>

