import { createStore } from 'vuex';
import { useApi } from '@/components/composables/useApi';

export default createStore({
  state: {
    productos: [],
    clientes: [],
    ventas: [],
    productosLoading: false,
    clientesLoading: false,
    ventasLoading: false,
    notificacion: {
      show: false,
      mensaje: '',
      tipo: 'success' // success, error, warning
    }
  },
  
  getters: {
    // Productos
    getProductoById: (state) => (id) => {
      return state.productos.find(producto => producto.id === parseInt(id));
    },
    
    // Clientes
    getClienteById: (state) => (id) => {
      return state.clientes.find(cliente => cliente.id === parseInt(id));
    },
    
    // Ventas
    getVentaById: (state) => (id) => {
      return state.ventas.find(venta => venta.id === parseInt(id));
    }
  },
  
  mutations: {
    // Productos
    SET_PRODUCTOS(state, productos) {
      state.productos = productos;
    },
    ADD_PRODUCTO(state, producto) {
      state.productos.push(producto);
    },
    UPDATE_PRODUCTO(state, productoActualizado) {
      const index = state.productos.findIndex(p => p.id === productoActualizado.id);
      if (index !== -1) {
        state.productos.splice(index, 1, productoActualizado);
      }
    },
    REMOVE_PRODUCTO(state, id) {
      state.productos = state.productos.filter(producto => producto.id !== id);
    },
    SET_PRODUCTOS_LOADING(state, isLoading) {
      state.productosLoading = isLoading;
    },
    
    // Clientes
    SET_CLIENTES(state, clientes) {
      state.clientes = clientes;
    },
    ADD_CLIENTE(state, cliente) {
      state.clientes.push(cliente);
    },
    UPDATE_CLIENTE(state, clienteActualizado) {
      const index = state.clientes.findIndex(c => c.id === clienteActualizado.id);
      if (index !== -1) {
        state.clientes.splice(index, 1, clienteActualizado);
      }
    },
    REMOVE_CLIENTE(state, id) {
      state.clientes = state.clientes.filter(cliente => cliente.id !== id);
    },
    SET_CLIENTES_LOADING(state, isLoading) {
      state.clientesLoading = isLoading;
    },
    
    // Ventas
    SET_VENTAS(state, ventas) {
      state.ventas = ventas;
    },
    ADD_VENTA(state, venta) {
      state.ventas.push(venta);
    },
    UPDATE_VENTA(state, ventaActualizada) {
      const index = state.ventas.findIndex(v => v.id === ventaActualizada.id);
      if (index !== -1) {
        state.ventas.splice(index, 1, ventaActualizada);
      }
    },
    REMOVE_VENTA(state, id) {
      state.ventas = state.ventas.filter(venta => venta.id !== id);
    },
    SET_VENTAS_LOADING(state, isLoading) {
      state.ventasLoading = isLoading;
    },
    

    SET_NOTIFICACION(state, { mensaje, tipo }) {
      state.notificacion = {
        show: true,
        mensaje,
        tipo
      };
    },
    CLEAR_NOTIFICACION(state) {
      state.notificacion.show = false;
    }
  },
  
  actions: {
    // Productos
    async fetchProductos({ commit }) {
      const api = useApi();
      commit('SET_PRODUCTOS_LOADING', true);
      
      try {
        const response = await api.getProductos();
        commit('SET_PRODUCTOS', response.data);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar productos: ' + error.message,
          tipo: 'error'
        });
        throw error;
      } finally {
        commit('SET_PRODUCTOS_LOADING', false);
      }
    },
    
    async fetchProductoById({ commit }, id) {
      const api = useApi();
      
      try {
        const response = await api.getProductoById(id);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar el producto: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async createProducto({ commit }, productoData) {
      const api = useApi();
      
      try {
        const response = await api.crearProducto(productoData);
        commit('ADD_PRODUCTO', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Producto creado exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al crear el producto: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async updateProducto({ commit }, { id, data }) {
      const api = useApi();
      
      try {
        const response = await api.actualizarProducto(id, data);
        commit('UPDATE_PRODUCTO', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Producto actualizado exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al actualizar el producto: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async deleteProducto({ commit }, id) {
      const api = useApi();
      
      try {
        await api.eliminarProducto(id);
        commit('REMOVE_PRODUCTO', id);
        commit('SET_NOTIFICACION', {
          mensaje: 'Producto eliminado exitosamente',
          tipo: 'success'
        });
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al eliminar el producto: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    // Clientes
    async fetchClientes({ commit }) {
      const api = useApi();
      commit('SET_CLIENTES_LOADING', true);
      
      try {
        const response = await api.getCliente();
        commit('SET_CLIENTES', response.data);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar clientes: ' + error.message,
          tipo: 'error'
        });
        throw error;
      } finally {
        commit('SET_CLIENTES_LOADING', false);
      }
    },
    
    async fetchClienteById({ commit }, id) {
      const api = useApi();
      
      try {
        const response = await api.getClienteById(id);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar el cliente: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async createCliente({ commit }, clienteData) {
      const api = useApi();
      
      try {
        const response = await api.crearCliente(clienteData);
        commit('ADD_CLIENTE', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Cliente creado exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al crear el cliente: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async updateCliente({ commit }, { id, data }) {
      const api = useApi();
      
      try {
        const response = await api.actualizarCliente(id, data);
        commit('UPDATE_CLIENTE', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Cliente actualizado exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al actualizar el cliente: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async deleteCliente({ commit }, id) {
      const api = useApi();
      
      try {
        await api.eliminarCliente(id);
        commit('REMOVE_CLIENTE', id);
        commit('SET_NOTIFICACION', {
          mensaje: 'Cliente eliminado exitosamente',
          tipo: 'success'
        });
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al eliminar el cliente: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    // Ventas
    async fetchVentas({ commit }) {
      const api = useApi();
      commit('SET_VENTAS_LOADING', true);
      
      try {
        const response = await api.getVentas();
        commit('SET_VENTAS', response.data);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar ventas: ' + error.message,
          tipo: 'error'
        });
        throw error;
      } finally {
        commit('SET_VENTAS_LOADING', false);
      }
    },
    
    async fetchVentaById({ commit }, id) {
      const api = useApi();
      
      try {
        const response = await api.getVentaById(id);
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al cargar la venta: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async createVenta({ commit }, ventaData) {
      const api = useApi();
      
      try {
        const response = await api.crearVenta(ventaData);
        commit('ADD_VENTA', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Venta registrada exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al registrar la venta: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async updateVenta({ commit }, { id, data }) {
      const api = useApi();
      
      try {
        const response = await api.actualizarVenta(id, data);
        commit('UPDATE_VENTA', response.data);
        commit('SET_NOTIFICACION', {
          mensaje: 'Venta actualizada exitosamente',
          tipo: 'success'
        });
        return response.data;
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al actualizar la venta: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    },
    
    async deleteVenta({ commit }, id) {
      const api = useApi();
      
      try {
        await api.eliminarVenta(id);
        commit('REMOVE_VENTA', id);
        commit('SET_NOTIFICACION', {
          mensaje: 'Venta eliminada exitosamente',
          tipo: 'success'
        });
      } catch (error) {
        commit('SET_NOTIFICACION', {
          mensaje: 'Error al eliminar la venta: ' + error.message,
          tipo: 'error'
        });
        throw error;
      }
    }
  }
});