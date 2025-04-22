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
<<<<<<< Updated upstream
</script>

<style scoped>
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
  max-height: 90vh;
  overflow-y: auto;
}

.modal-lg {
  width: 700px;
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

.loading, .no-data {
  text-align: center;
  padding: 20px;
  background-color: #f8f9fa;
  border-radius: 4px;
}

.venta-detalle-content {
  padding: 0;
}

.info-section {
  padding: 15px 20px;
  border-bottom: 1px solid #eee;
}

.info-group h4 {
  margin-top: 0;
  margin-bottom: 10px;
  color: #333;
}

.info-row {
  display: flex;
  margin-bottom: 8px;
}

.info-label {
  width: 100px;
  font-weight: bold;
}

.status-badge {
  display: inline-block;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.85em;
  text-transform: uppercase;
}

.status-badge.completada {
  background-color: #28a745;
  color: white;
}

.status-badge.pendiente {
  background-color: #ffc107;
  color: #212529;
}

.status-badge.cancelada {
  background-color: #dc3545;
  color: white;
}

.items-section {
  padding: 15px 20px;
  border-bottom: 1px solid #eee;
}

.items-section h4 {
  margin-top: 0;
  margin-bottom: 10px;
  color: #333;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
}

.items-table th, .items-table td {
  padding: 8px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.items-table th {
  background-color: #f8f9fa;
  font-weight: bold;
}

.totals-section {
  padding: 15px 20px;
  background-color: #f8f9fa;
}

.total-row {
  display: flex;
  justify-content: space-between;
  padding: 4px 0;
}

.total-label {
  font-weight: bold;
}

.grand-total {
  margin-top: 8px;
  padding-top: 8px;
  border-top: 2px solid #ddd;
  font-size: 1.1em;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 15px 20px;
  border-top: 1px solid #eee;
}

.btn-primary, .btn-secondary {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary {
  background-color: #4caf50;
  color: white;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}
</style>
=======
</script>
>>>>>>> Stashed changes
