<!-- src/components/VentasForm.vue -->
<template>
  <div class="ventas-form">
    <h2>{{ isEditing ? 'Editar Venta' : 'Nueva Venta' }}</h2>
    
    <form @submit.prevent="guardarVenta">
      <div class="form-group">
        <label for="cliente">Cliente:</label>
        <select 
          id="cliente" 
          v-model="venta.clienteId" 
          class="form-control" 
          required
        >
          <option value="" disabled>Seleccione un cliente</option>
          <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">
            {{ cliente.nombre }}
          </option>
        </select>
      </div>
      
      <div class="form-group">
        <label>Productos:</label>
        <div v-for="(item, index) in venta.items" :key="index" class="producto-item">
          <div class="producto-row">
            <select 
              v-model="item.productoId" 
              class="form-control" 
              required
              @change="actualizarPrecioUnitario(index)"
            >
              <option value="" disabled>Seleccione un producto</option>
              <option v-for="producto in productos" :key="producto.id" :value="producto.id">
                {{ producto.nombre }} (Stock: {{ producto.stock }})
              </option>
            </select>
            
            <input 
              type="number" 
              v-model.number="item.cantidad" 
              min="1" 
              class="form-control" 
              placeholder="Cantidad" 
              required
              @change="calcularSubtotal(index)"
            />
            
            <input 
              type="number" 
              v-model.number="item.precioUnitario" 
              min="0.01" 
              step="0.01" 
              class="form-control" 
              placeholder="Precio" 
              required
              @change="calcularSubtotal(index)"
            />
            
            <div class="subtotal">
              {{ formatCurrency(item.subtotal) }}
            </div>
            
            <button 
              type="button" 
              @click="eliminarProducto(index)" 
              class="btn-remove"
            >
              ✖
            </button>
          </div>
        </div>
        
        <button 
          type="button" 
          @click="agregarProducto" 
          class="btn-secondary"
        >
          Agregar Producto
        </button>
      </div>
      
      <div class="form-group">
        <label for="fecha">Fecha:</label>
        <input 
          type="date" 
          id="fecha" 
          v-model="venta.fecha" 
          class="form-control" 
          required
        />
      </div>
      
      <div class="totales">
        <div class="total-item">
          <span>Subtotal:</span>
          <span>{{ formatCurrency(calcularTotal()) }}</span>
        </div>
        <div class="total-item">
          <span>IVA (16%):</span>
          <span>{{ formatCurrency(calcularIVA()) }}</span>
        </div>
        <div class="total-item total-final">
          <span>Total:</span>
          <span>{{ formatCurrency(calcularTotalConIVA()) }}</span>
        </div>
      </div>
      
      <div class="form-actions">
        <button type="button" @click="cancelar" class="btn-secondary">Cancelar</button>
        <button type="submit" class="btn-primary">{{ isEditing ? 'Actualizar' : 'Guardar' }}</button>
      </div>
    </form>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useApi } from '../composables/useApi';
import { useToast } from '../composables/useToast'; // Asumiendo que tienes este composable

export default {
  name: 'VentasForm',
  
  setup() {
    const router = useRouter();
    const route = useRoute();
    const { 
      getCliente, getProductos, getVentaById, 
      crearVenta, actualizarVenta 
    } = useApi();
    const { showToast } = useToast();
    
    const ventaId = computed(() => route.params.id);
    const isEditing = computed(() => !!ventaId.value);
    
    const clientes = ref([]);
    const productos = ref([]);
    const venta = ref({
      clienteId: '',
      fecha: new Date().toISOString().split('T')[0],
      items: [
        { productoId: '', cantidad: 1, precioUnitario: 0, subtotal: 0 }
      ],
      total: 0
    });
    
    const cargarClientes = async () => {
      try {
        const response = await getCliente();
        clientes.value = response.data || [];
      } catch (error) {
        showToast('Error al cargar los clientes', 'error');
      }
    };
    
    const cargarProductos = async () => {
      try {
        const response = await getProductos();
        productos.value = response.data || [];
      } catch (error) {
        showToast('Error al cargar los productos', 'error');
      }
    };
    
    const cargarVenta = async () => {
      if (!isEditing.value) return;
      
      try {
        const response = await getVentaById(ventaId.value);
        const ventaData = response.data;
        
        venta.value = {
          clienteId: ventaData.clienteId,
          fecha: ventaData.fecha.split('T')[0],
          items: ventaData.items.map(item => ({
            productoId: item.productoId,
            cantidad: item.cantidad,
            precioUnitario: item.precioUnitario,
            subtotal: item.cantidad * item.precioUnitario
          })),
          total: ventaData.total
        };
      } catch (error) {
        showToast('Error al cargar la venta', 'error');
        router.push('/ventas');
      }
    };
    
    const actualizarPrecioUnitario = (index) => {
      const item = venta.value.items[index];
      const producto = productos.value.find(p => p.id === item.productoId);
      
      if (producto) {
        item.precioUnitario = producto.precio;
        calcularSubtotal(index);
      }
    };
    
    const calcularSubtotal = (index) => {
      const item = venta.value.items[index];
      item.subtotal = item.cantidad * item.precioUnitario;
    };
    
    const calcularTotal = () => {
      return venta.value.items.reduce((total, item) => total + item.subtotal, 0);
    };
    
    const calcularIVA = () => {
      return calcularTotal() * 0.16;
    };
    
    const calcularTotalConIVA = () => {
      return calcularTotal() + calcularIVA();
    };
    
    const formatCurrency = (value) => {
      return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN'
      }).format(value);
    };
    
    const agregarProducto = () => {
      venta.value.items.push({
        productoId: '',
        cantidad: 1,
        precioUnitario: 0,
        subtotal: 0
      });
    };
    
    const eliminarProducto = (index) => {
      if (venta.value.items.length > 1) {
        venta.value.items.splice(index, 1);
      } else {
        showToast('La venta debe tener al menos un producto', 'warning');
      }
    };
    
    const guardarVenta = async () => {
      try {
        // Verificar stock disponible
        for (const item of venta.value.items) {
          const producto = productos.value.find(p => p.id === item.productoId);
          if (producto && item.cantidad > producto.stock) {
            showToast(`Stock insuficiente para ${producto.nombre}`, 'error');
            return;
          }
        }
        
        // Preparar datos para enviar
        const ventaData = {
          clienteId: venta.value.clienteId,
          fecha: venta.value.fecha,
          items: venta.value.items.map(item => ({
            productoId: item.productoId,
            cantidad: item.cantidad,
            precioUnitario: item.precioUnitario
          })),
          total: calcularTotalConIVA()
        };
        
        if (isEditing.value) {
          await actualizarVenta(ventaId.value, ventaData);
          showToast('Venta actualizada con éxito', 'success');
        } else {
          await crearVenta(ventaData);
          showToast('Venta registrada con éxito', 'success');
        }
        
        router.push('/ventas');
      } catch (error) {
        showToast('Error al guardar la venta', 'error');
      }
    };
    
    const cancelar = () => {
      router.push('/ventas');
    };
    
    onMounted(async () => {
      await Promise.all([cargarClientes(), cargarProductos()]);
      if (isEditing.value) {
        await cargarVenta();
      }
    });
    
    return {
      venta,
      clientes,
      productos,
      isEditing,
      agregarProducto,
      eliminarProducto,
      actualizarPrecioUnitario,
      calcularSubtotal,
      calcularTotal,
      calcularIVA,
      calcularTotalConIVA,
      formatCurrency,
      guardarVenta,
      cancelar
    };
  }
};
</script>

<style scoped>
.ventas-form {
  padding: 20px;
  max-width: 900px;
  margin: 0 auto;
}

h2 {
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-control {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-sizing: border-box;
}

.producto-item {
  margin-bottom: 10px;
}

.producto-row {
  display: grid;
  grid-template-columns: 3fr 1fr 1fr 1fr 40px;
  gap: 10px;
  align-items: center;
}

.subtotal {
  text-align: right;
  font-weight: bold;
}

.btn-remove {
  background-color: #ff4d4d;
  color: white;
  border: none;
  border-radius: 4px;
  width: 30px;
  height: 30px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}

.totales {
  background-color: #f8f9fa;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
}

.total-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 5px;
}

.total-final {
  font-weight: bold;
  font-size: 1.2em;
  border-top: 1px solid #ddd;
  padding-top: 10px;
  margin-top: 10px;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.btn-primary, .btn-secondary {
  padding: 10px 20px;
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