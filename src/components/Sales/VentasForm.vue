<!-- src/components/ventas/VentasForm.vue -->
<template>
  <div v-if="mostrar" class="modal">
    <div class="modal-content modal-xl">
      <div class="modal-header">
        <h3>{{ modoEdicion ? 'Editar Venta' : 'Nueva Venta' }}</h3>
        <button @click="cerrar" class="btn-close">×</button>
      </div>
      
      <form @submit.prevent="guardar" class="venta-form">
        <div class="form-group">
          <label for="cliente">Cliente:</label>
          <select 
            id="cliente" 
            v-model="ventaLocal.clienteId" 
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
          <div v-for="(item, index) in ventaLocal.items" :key="index" class="producto-item">
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
                @change="calcularSubtotalItem(index)"
              />
              
              <input 
                type="number" 
                v-model.number="item.precioUnitario" 
                min="0.01" 
                step="0.01" 
                class="form-control" 
                placeholder="Precio" 
                required
                @change="calcularSubtotalItem(index)"
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
            v-model="ventaLocal.fecha" 
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
        
        <div class="modal-actions">
          <button type="button" @click="cerrar" class="btn-secondary">Cancelar</button>
          <button type="submit" class="btn-primary">{{ modoEdicion ? 'Actualizar' : 'Guardar' }}</button>
        </div>
      </form>
    </div>
  </div>
</template>
<style scoped src= "@/assets/CSS/Venta/VentaForm.css"></style>

<script>
import { reactive, toRef, watch } from 'vue';

export default {
  name: 'VentasForm',
  
  props: {
    venta: {
      type: Object,
      default: () => ({
        id: null,
        clienteId: '',
        fecha: new Date().toISOString().split('T')[0],
        items: [
          { productoId: '', cantidad: 1, precioUnitario: 0, subtotal: 0 }
        ]
      })
    },
    clientes: {
      type: Array,
      default: () => []
    },
    productos: {
      type: Array,
      default: () => []
    },
    modoEdicion: {
      type: Boolean,
      default: false
    },
    mostrar: {
      type: Boolean,
      default: false
    }
  },
  
  emits: ['cerrar', 'guardar'],
  
  setup(props, { emit }) {
    const ventaLocal = reactive({...props.venta});
    
    watch(() => props.venta, (newVal) => {
      Object.assign(ventaLocal, JSON.parse(JSON.stringify(newVal)));
    }, { deep: true });
    
    const cerrar = () => {
      emit('cerrar');
    };
    
    const actualizarPrecioUnitario = (index) => {
      const item = ventaLocal.items[index];
      const producto = props.productos.find(p => p.id === item.productoId);
      
      if (producto) {
        item.precioUnitario = producto.precio;
        calcularSubtotalItem(index);
      }
    };
    
    const calcularSubtotalItem = (index) => {
      const item = ventaLocal.items[index];
      item.subtotal = item.cantidad * item.precioUnitario;
    };
    
    const agregarProducto = () => {
      ventaLocal.items.push({
        productoId: '',
        cantidad: 1,
        precioUnitario: 0,
        subtotal: 0
      });
    };
    
    const eliminarProducto = (index) => {
      if (ventaLocal.items.length > 1) {
        ventaLocal.items.splice(index, 1);
      } else {
        // Podría mostrar un mensaje aquí, pero lo simplificamos
        console.warn('La venta debe tener al menos un producto');
      }
    };
    
    const calcularTotal = () => {
      return ventaLocal.items.reduce((total, item) => total + item.subtotal, 0);
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
    
    const guardar = () => {
      // Preparar datos para enviar
      const ventaData = {
        id: ventaLocal.id,
        clienteId: ventaLocal.clienteId,
        fecha: ventaLocal.fecha,
        items: ventaLocal.items.map(item => ({
          productoId: item.productoId,
          cantidad: item.cantidad,
          precioUnitario: item.precioUnitario
        })),
        total: calcularTotalConIVA()
      };
      
      emit('guardar', ventaData);
    };
    
    return {
      ventaLocal,
      cerrar,
      actualizarPrecioUnitario,
      calcularSubtotalItem,
      agregarProducto,
      eliminarProducto,
      calcularTotal,
      calcularIVA,
      calcularTotalConIVA,
      formatCurrency,
      guardar
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

.modal-xl {
  width: 900px;
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

.venta-form {
  padding: 20px;
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

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 15px 0 0;
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
