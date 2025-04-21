<!-- src/views/VentaDetalleView.vue -->
<template>
  <div class="venta-detalle">
    <div class="header">
      <h1>Detalle de Venta #{{ ventaId }}</h1>
      <div class="header-actions">
        <router-link to="/ventas" class="btn-back">
          Volver a Ventas
        </router-link>
        <router-link :to="`/ventas/${ventaId}/editar`" class="btn-edit">
          Editar Venta
        </router-link>
      </div>
    </div>
    
    <div v-if="isLoading" class="loading">
      <p>Cargando detalles de la venta...</p>
    </div>
    
    <div v-else-if="!venta" class="no-data">
      <p>No se encontró la venta o hubo un error al cargarla</p>
    </div>
    
    <div v-else class="venta-content">
      <div class="info-section">
        <div class="info-group">
          <h3>Información General</h3>
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
              <span class="status-badge" :class="venta.estado.toLowerCase()">
                {{ venta.estado }}
              </span>
            </div>
          </div>
        </div>
      </div>
      
      <div class="items-section">
        <h3>Productos</h3>
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
          <div class="total-value">{{ formatCurrency(calcularSubtotal()) }}</div>
        </div>
        <div class="total-row">
          <div class="total-label">IVA (16%):</div>
          <div class="total-value">{{ formatCurrency(calcularIVA()) }}</div>
        </div>
        <div class="total-row grand-total">
          <div class="total-label">Total:</div>
          <div class="total-value">{{ formatCurrency(venta.total) }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { useApi } from '../composables/useApi';
import { useToast } from '../composables/useToast'; // Asumiendo que tienes este composable

export default {
  name: 'VentaDetalleView',
  
  setup() {
    const route = useRoute();
    const { getVentaById } = useApi();
    const { showToast } = useToast();
    
    const ventaId = computed(() => route.params.id);
    const venta = ref(null);
    const isLoading = ref(true);
    
    const cargarVenta = async () => {
      isLoading.value = true;
      try {
        const response = await getVentaById(ventaId.value);
        venta.value = response.data;
      } catch (error) {
        showToast('Error al cargar los detalles de la venta', 'error');
      } finally {
        isLoading.value = false;
      }
    };
    
    const formatDate = (dateString) => {
      const options = { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      };
      return new Date(dateString).toLocaleDateString('es-ES', options);
    };
    
    const formatCurrency = (value) => {
      return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN'
      }).format(value);
    };
    
    const calcularSubtotal = () => {
      if (!venta.value || !venta.value.items) return 0;
      return venta.value.items.reduce((total, item) => {
        return total + (item.cantidad * item.precioUnitario);
      }, 0);
    };
    
    const calcularIVA = () => {
      return calcularSubtotal() * 0.16;
    };
    
    onMounted(() => {
      cargarVenta();
    });
    
    return {
      ventaId,
      venta,
      isLoading,
      formatDate,
      formatCurrency,
      calcularSubtotal,
      calcularIVA
    };
  }
};
</script>

<style scoped>
.venta-detalle {
  padding: 20px;
  max-width: 900px;
  margin: 0 auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.header-actions {
  display: flex;
  gap: 10px;
}

.btn-back, .btn-edit {
  padding: 8px 16px;
  border-radius: 4px;
  text-decoration: none;
}

.btn-back {
  background-color: #6c757d;
  color: white;
}

.btn-edit {
  background-color: #ffc107;
  color: #212529;
}

.loading, .no-data {
  text-align: center;
  padding: 30px;
  background-color: #f8f9fa;
  border-radius: 4px;
}

.venta-content {
  background-color: white;
  border-radius: 4px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.info-section {
  padding: 20px;
  border-bottom: 1px solid #eee;
}

.info-group h3 {
  margin-top: 0;
  margin-bottom: 15px;
  color: #333;
}

.info-row {
  display: flex;
  margin-bottom: 10px;
}

.info-label {
  width: 120px;
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
  padding: 20px;
  border-bottom: 1px solid #eee;
}

</style>