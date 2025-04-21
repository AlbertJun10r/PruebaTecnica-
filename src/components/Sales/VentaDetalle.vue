<!-- src/components/ventas/VentaDetalle.vue -->
<template>
    <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-10">
      <div class="bg-white p-6 rounded-lg shadow-lg w-full max-w-2xl max-h-screen overflow-y-auto">
        <div v-if="loading" class="text-center py-10">
          <p class="text-gray-600">Cargando detalles de la venta...</p>
        </div>
        <template v-else-if="venta">
          <div class="flex justify-between items-start mb-6">
            <h2 class="text-xl font-bold">Factura: {{ venta.numeroFactura || 'Sin número' }}</h2>
            
            <span 
              class="px-3 py-1 rounded-full text-sm font-medium"
              :class="getEstadoClass(venta.estado)"
            >
              {{ venta.estado || 'Pendiente' }}
            </span>
          </div>
          
          <div class="grid grid-cols-2 gap-6 mb-6">
            <div class="bg-gray-50 p-4 rounded">
              <h3 class="font-medium text-gray-700 mb-2">Información de venta</h3>
              <p><span class="text-gray-500">Fecha:</span> {{ formatearFecha(venta.fechaVenta) }}</p>
              <p><span class="text-gray-500">Método de pago:</span> {{ venta.metodoPago || 'No especificado' }}</p>
              <p v-if="venta.observaciones"><span class="text-gray-500">Observaciones:</span> {{ venta.observaciones }}</p>
            </div>
            
            <div class="bg-gray-50 p-4 rounded">
              <h3 class="font-medium text-gray-700 mb-2">Cliente</h3>
              <p><span class="text-gray-500">Nombre:</span> {{ venta.cliente?.nombre || 'No especificado' }}</p>
              <p v-if="venta.cliente?.email"><span class="text-gray-500">Email:</span> {{ venta.cliente?.email }}</p>
              <p v-if="venta.cliente?.telefono"><span class="text-gray-500">Teléfono:</span> {{ venta.cliente?.telefono }}</p>
            </div>
          </div>
          
          <div class="mb-6">
            <h3 class="font-medium text-gray-700 mb-3">Productos</h3>
            
            <table class="min-w-full bg-white border">
              <thead class="bg-gray-100">
                <tr>
                  <th class="py-2 px-4 text-left">Producto</th>
                  <th class="py-2 px-4 text-center">Cantidad</th>
                  <th class="py-2 px-4 text-right">Precio</th>
                  <th class="py-2 px-4 text-right">Subtotal</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item, index) in venta.detalles" :key="index" class="border-b">
                  <td class="py-2 px-4">
                    <div class="font-medium">{{ item.producto?.nombre || 'Producto no disponible' }}</div>
                    <div class="text-sm text-gray-500" v-if="item.producto?.codigo">{{ item.producto.codigo }}</div>
                  </td>
                  <td class="py-2 px-4 text-center">{{ item.cantidad }}</td>
                  <td class="py-2 px-4 text-right">{{ formatearPrecio(item.precioUnitario) }}</td>
                  <td class="py-2 px-4 text-right">{{ formatearPrecio(item.subtotal) }}</td>
                </tr>
              </tbody>
              <tfoot class="bg-gray-50">
                <tr>
                  <td colspan="3" class="py-2 px-4 text-right font-medium">Subtotal:</td>
                  <td class="py-2 px-4 text-right">{{ formatearPrecio(calcularSubtotal()) }}</td>
                </tr>
                <tr v-if="venta.impuestos">
                  <td colspan="3" class="py-2 px-4 text-right font-medium">Impuestos:</td>
                  <td class="py-2 px-4 text-right">{{ formatearPrecio(venta.impuestos) }}</td>
                </tr>
                <tr v-if="venta.descuento">
                  <td colspan="3" class="py-2 px-4 text-right font-medium">Descuento:</td>
                  <td class="py-2 px-4 text-right">{{ formatearPrecio(venta.descuento) }}</td>
                </tr>
                <tr class="bg-gray-100">
                  <td colspan="3" class="py-2 px-4 text-right font-bold">Total:</td>
                  <td class="py-2 px-4 text-right font-bold">{{ formatearPrecio(venta.total) }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
          
          <div class="flex justify-end">
            <button 
              @click="$emit('cerrar')" 
              class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
            >
              Cerrar
            </button>
          </div>
        </template>
        <div v-else class="text-center py-10">
          <p class="text-gray-600">No se pudieron cargar los detalles de la venta</p>
          <button 
            @click="$emit('cerrar')" 
            class="mt-4 px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300"
          >
            Cerrar
          </button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  defineProps({
    venta: {
      type: Object,
      default: null
    },
    loading: {
      type: Boolean,
      default: false
    }
  });
  
  defineEmits(['cerrar']);
  
  // Formatear fecha
  function formatearFecha(fecha) {
    if (!fecha) return 'Fecha no disponible';
    
    return new Date(fecha).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
  
  // Formatear precio como moneda
  function formatearPrecio(precio) {
    return new Intl.NumberFormat('es-MX', {
      style: 'currency',
      currency: 'MXN'
    }).format(precio || 0);
  }
  
  // Obtener clase CSS según el estado de la venta
  function getEstadoClass(estado) {
    switch (estado?.toLowerCase()) {
      case 'completada':
        return 'bg-green-100 text-green-800';
      case 'en proceso':
        return 'bg-blue-100 text-blue-800';
      case 'cancelada':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  }
  
  // Calcular subtotal (por si no viene en la respuesta)
  function calcularSubtotal() {
    if (!props.venta?.detalles) return 0;
    return props.venta.detalles.reduce((total, item) => total + (item.subtotal || 0), 0);
  }
  </script>