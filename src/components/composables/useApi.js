import axios from 'axios';

// Configuración base de axios
const api = axios.create({
  baseURL: "https://localhost:7056/", // Cambia esto por la URL base de tu API
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

export function useApi() {
  // Funciones para consumir tus endpoints
  // Cliente
  const getCliente = () => api.get('/api/Clientes');
  const getClienteById = (id) => api.get(`/api/Clientes/${id}`);
  const crearCliente = (datos) => api.post('/api/Clientes', datos);
  const actualizarCliente = (id, datos) => api.put(`/api/Clientes/${id}`, datos);
  const eliminarCliente = (id) => api.delete(`/api/Clientes/${id}`);
  
  // Productos
  const getProductos = () => api.get('/api/Productos');
  const getProductoById = (id) => api.get(`/api/Productos/${id}`);
  const crearProducto = (datos) => api.post('/api/Productos', datos);
  const actualizarProducto = (id, datos) => api.put(`/api/Productos/${id}`, datos);
  const eliminarProducto = (id) => api.delete(`/api/Productos/${id}`);
  const getProductosLowStock = () => api.get('/api/Productos/low-stock');
  const getProductosLowStockById = (id) => api.get(`/api/Productos/${id}/stock`);
  
  // Ventas
  const getVentas = () => api.get('/api/Ventas');
  const getVentaById = (id) => api.get(`/api/Ventas/${id}`);
  const crearVenta = (datos) => api.post('/api/Ventas', datos);
  const actualizarVenta = (id, datos) => api.put(`/api/Ventas/${id}`, datos);
  const eliminarVenta = (id) => api.delete(`/api/Ventas/${id}`);
  const getVentasByProducto = (id) => api.get(`/api/Clientes/${id}`);
  
  return {
    // Cliente
    getCliente,
    getClienteById,
    crearCliente,
    actualizarCliente,
    eliminarCliente,
    
    // Productos
    getProductos,
    getProductoById,
    crearProducto,
    actualizarProducto,
    eliminarProducto,
    getProductosLowStock,
    getProductosLowStockById,
    
    // Ventas
    getVentas,
    getVentaById,
    crearVenta,
    actualizarVenta,
    eliminarVenta,
    getVentasByProducto
  };
}






// // composables/useApi.js
// import { ref } from 'vue'
// import { useToast } from './useToast'
// import { useRouter } from 'vue-router'

// export function useApi() {
//   const { showToast } = useToast()
//   const router = useRouter()
//   const isLoading = ref(false)
//   const error = ref(null)
  
//   const fetchData = async (url, options = {}) => {
//     isLoading.value = true
//     error.value = null
    
//     try {
//       const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}${url}`, {
//         headers: {
//           'Content-Type': 'application/json',
//           'Authorization': `Bearer ${localStorage.getItem('token')}`
//         },
//         ...options
//       })
      
//       if (response.status === 401) {
//         localStorage.removeItem('token')
//         router.push('/login')
//         throw new Error('Sesión expirada')
//       }
      
//       if (!response.ok) {
//         const errorData = await response.json()
//         throw new Error(errorData.message || `HTTP error! status: ${response.status}`)
//       }
      
//       return await response.json()
//     } catch (err) {
//       error.value = err.message
//       showToast(err.message || 'Error al conectar con el servidor', 'error')
//       throw err
//     } finally {
//       isLoading.value = false
//     }
//   }
  
//   // Clientes
//   const getClientes = async () => await fetchData('/api/Clientes')
//   const getClienteById = async (id) => await fetchData(`/api/Clientes/${id}`)
//   const crearCliente = async (datos) => await fetchData('/api/Clientes', {
//     method: 'POST',
//     body: JSON.stringify(datos)
//   })
//   const actualizarCliente = async (id, datos) => await fetchData(`/api/Clientes/${id}`, {
//     method: 'PUT',
//     body: JSON.stringify(datos)
//   })
//   const eliminarCliente = async (id) => await fetchData(`/api/Clientes/${id}`, {
//     method: 'DELETE'
//   })

//   // Productos
//   const getProductos = async () => await fetchData('/api/Productos')
//   const getProductoById = async (id) => await fetchData(`/api/Productos/${id}`)
//   const crearProducto = async (datos) => await fetchData('/api/Productos', {
//     method: 'POST',
//     body: JSON.stringify(datos)
//   })
//   const actualizarProducto = async (id, datos) => await fetchData(`/api/Productos/${id}`, {
//     method: 'PUT',
//     body: JSON.stringify(datos)
//   })
//   const eliminarProducto = async (id) => await fetchData(`/api/Productos/${id}`, {
//     method: 'DELETE'
//   })
//   const getProductosLowStock = async () => await fetchData('/api/Productos/low-stock')
//   const getProductoLowStockById = async (id) => await fetchData(`/api/Productos/${id}/stock`)

//   // Ventas
//   const getVentas = async () => await fetchData('/api/Ventas')
//   const getVentaById = async (id) => await fetchData(`/api/Ventas/${id}`)
//   const crearVenta = async (datos) => await fetchData('/api/Ventas', {
//     method: 'POST',
//     body: JSON.stringify(datos)
//   })
//   const actualizarVenta = async (id, datos) => await fetchData(`/api/Ventas/${id}`, {
//     method: 'PUT',
//     body: JSON.stringify(datos)
//   })
//   const eliminarVenta = async (id) => await fetchData(`/api/Ventas/${id}`, {
//     method: 'DELETE'
//   })
//   const getVentasByProducto = async (id) => await fetchData(`/api/Ventas/producto/${id}`)

//   return {
//     // General
//     fetchData,
//     isLoading,
//     error,
    
//     // Clientes
//     getClientes,
//     getClienteById,
//     crearCliente,
//     actualizarCliente,
//     eliminarCliente,
    
//     // Productos
//     getProductos,
//     getProductoById,
//     crearProducto,
//     actualizarProducto,
//     eliminarProducto,
//     getProductosLowStock,
//     getProductoLowStockById,
    
//     // Ventas
//     getVentas,
//     getVentaById,
//     crearVenta,
//     actualizarVenta,
//     eliminarVenta,
//     getVentasByProducto
//   }
// }