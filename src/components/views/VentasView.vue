<!-- src/views/VentasView.vue -->
<template>
  <div class="ventas-view">
    <div class="header">
      <h1>Gestión de Ventas</h1>
      <button @click="mostrarFormularioCrear" class="btn-crear">Nueva Venta</button>
    </div>
    
    <!-- Lista de ventas -->
    <ventas-list
      :ventas="ventasFiltradas"
      :is-loading="isLoading"
      :filtro-cliente="filtroCliente"
      :filtro-fecha="filtroFecha"
      @ver="mostrarDetalleVenta"
      @editar="mostrarFormularioEditar"
      @eliminar="confirmarEliminar"
      @filtrar="actualizarFiltros"
    />
    
    <!-- Modal de detalle venta -->
    <venta-detalle
      :venta="ventaDetalle"
      :is-loading="loadingDetalle"
      :mostrar="modalDetalle"
      @cerrar="modalDetalle = false"
      @editar="mostrarFormularioEditar"
    />
    
    <!-- Modal de formulario crear/editar -->
    <ventas-form
      :venta="ventaForm"
      :clientes="clientes"
      :productos="productos"
      :modo-edicion="modoEdicion"
      :mostrar="modalForm"
      @cerrar="modalForm = false"
      @guardar="guardarVenta"
    />
    
    <!-- Modal de confirmación para eliminar -->
    <div v-if="modalEliminar" class="modal">
      <div class="modal-content">
        <div class="modal-header">
          <h3>Confirmar eliminación</h3>
          <button @click="modalEliminar = false" class="btn-close">×</button>
        </div>
        <p>¿Está seguro que desea eliminar la venta #{{ ventaSeleccionada?.id }}?</p>
        <div class="modal-actions">
          <button @click="modalEliminar = false" class="btn-secondary">Cancelar</button>
          <button @click="eliminarVenta" class="btn-danger">Eliminar</button>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped src= "@/assets/CSS/Venta/VentaView.css"></style>

<script>
import { ref, computed, onMounted } from 'vue';
<<<<<<< Updated upstream
import { useApi } from '@/components/composables/useApi';
import { useToast } from '@/components/composables/useToast';
import VentasList from '@/components/Sales/VentasList.vue';
import VentaDetalle from '@/components/Sales/VentaDetalle.vue';
import VentasForm from '@/components/Sales/VentasForm.vue';
=======
import { useApi } from '../composables/useApi';
import { useToast } from '../composables/useToast';
import VentasList from '../Sales/VentasList.vue';
import VentaDetalle from '../Sales/VentaDetalle.vue';
import VentasForm from '../Sales/VentasForm.vue';
>>>>>>> Stashed changes

export default {
  name: 'VentasView',
  
  components: {
    VentasList,
    VentaDetalle,
    VentasForm
  },
  
  setup() {
    const { 
      getVentas, getVentaById, getCliente, getProductos,
      crearVenta, actualizarVenta, eliminarVenta: apiEliminarVenta 
    } = useApi();
    const { showToast } = useToast();
    
    // Estados para el listado de ventas
    const ventas = ref([]);
    const isLoading = ref(true);
    const filtroCliente = ref('');
    const filtroFecha = ref('');
    
    // Estados para el modal de eliminar
    const modalEliminar = ref(false);
    const ventaSeleccionada = ref(null);
    
    // Estados para el modal de detalle
    const modalDetalle = ref(false);
    const ventaDetalle = ref(null);
    const loadingDetalle = ref(false);
    
    // Estados para el formulario
    const modalForm = ref(false);
    const modoEdicion = ref(false);
    const ventaForm = ref({
      id: null,
      clienteId: '',
      fecha: new Date().toISOString().split('T')[0],
      items: [
        { productoId: '', cantidad: 1, precioUnitario: 0, subtotal: 0 }
      ]
    });
    const clientes = ref([]);
    const productos = ref([]);
    
    // Filtrado de ventas
    const ventasFiltradas = computed(() => {
      return ventas.value.filter(venta => {
        const matchCliente = !filtroCliente.value || 
          venta.cliente.nombre.toLowerCase().includes(filtroCliente.value.toLowerCase());
        
        const matchFecha = !filtroFecha.value || 
          venta.fecha.split('T')[0] === filtroFecha.value;
        
        return matchCliente && matchFecha;
      });
    });
    
    // Funciones para el listado de ventas
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
    
    const actualizarFiltros = (filtros) => {
      filtroCliente.value = filtros.cliente;
      filtroFecha.value = filtros.fecha;
    };
    
    // Funciones para eliminar venta
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
    
    // Funciones para mostrar detalle de venta
    const mostrarDetalleVenta = async (id) => {
      modalDetalle.value = true;
      loadingDetalle.value = true;
      
      try {
        const response = await getVentaById(id);
        ventaDetalle.value = response.data;
      } catch (error) {
        showToast('Error al cargar los detalles de la venta', 'error');
      } finally {
        loadingDetalle.value = false;
      }
    };
    
    // Funciones para el formulario de venta
    const cargarDatosIniciales = async () => {
      try {
        const [clientesRes, productosRes] = await Promise.all([
          getCliente(),
          getProductos()
        ]);
        
        clientes.value = clientesRes.data || [];
        productos.value = productosRes.data || [];
      } catch (error) {
        showToast('Error al cargar datos iniciales', 'error');
      }
    };
    
    const mostrarFormularioCrear = async () => {
      modoEdicion.value = false;
      resetearFormulario();
      await cargarDatosIniciales();
      modalForm.value = true;
    };
    
    const mostrarFormularioEditar = async (id) => {
      modoEdicion.value = true;
      resetearFormulario();
      await cargarDatosIniciales();
      
      try {
        const response = await getVentaById(id);
        const ventaData = response.data;
        
        ventaForm.value = {
          id: ventaData.id,
          clienteId: ventaData.clienteId,
          fecha: ventaData.fecha.split('T')[0],
          items: ventaData.items.map(item => ({
            productoId: item.productoId,
            cantidad: item.cantidad,
            precioUnitario: item.precioUnitario,
            subtotal: item.cantidad * item.precioUnitario
          }))
        };
        
        modalForm.value = true;
      } catch (error) {
        showToast('Error al cargar la venta para editar', 'error');
      }
    };
    
    const resetearFormulario = () => {
      ventaForm.value = {
        id: null,
        clienteId: '',
        fecha: new Date().toISOString().split('T')[0],
        items: [
          { productoId: '', cantidad: 1, precioUnitario: 0, subtotal: 0 }
        ]
      };
    };
    
    const guardarVenta = async (ventaData) => {
      try {
        // Verificar stock disponible
        for (const item of ventaData.items) {
          const producto = productos.value.find(p => p.id === item.productoId);
          if (producto && item.cantidad > producto.stock) {
            showToast(`Stock insuficiente para ${producto.nombre}`, 'error');
            return;
          }
        }
        
        if (modoEdicion.value) {
          await actualizarVenta(ventaData.id, ventaData);
          showToast('Venta actualizada con éxito', 'success');
        } else {
          await crearVenta(ventaData);
          showToast('Venta registrada con éxito', 'success');
        }
        
        modalForm.value = false;
        cargarVentas();
      } catch (error) {
        showToast('Error al guardar la venta', 'error');
      }
    };
    
    onMounted(() => {
      cargarVentas();
    });
    
    return {
      // Estados
      ventas,
      isLoading,
      filtroCliente,
      filtroFecha,
      ventasFiltradas,
      modalEliminar,
      ventaSeleccionada,
      modalDetalle,
      ventaDetalle,
      loadingDetalle,
      modalForm,
      modoEdicion,
      ventaForm,
      clientes,
      productos,
      
      // Funciones
      actualizarFiltros,
      confirmarEliminar,
      eliminarVenta,
      mostrarDetalleVenta,
      mostrarFormularioCrear,
      mostrarFormularioEditar,
      guardarVenta
    };
  }
};
<<<<<<< Updated upstream
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
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

/* Estilos para el modal de eliminar */
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
</style>
=======
</script>
>>>>>>> Stashed changes
