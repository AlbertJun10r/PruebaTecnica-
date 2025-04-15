import axios from 'axios'

export const useApi = () => {
  const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7056/',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }
  })
  
  // Interceptor para manejar errores globalmente
  api.interceptors.response.use(
    response => response,
    error => {
      if (error.response?.status === 401) {
        // Manejar autenticación
      }
      return Promise.reject(error)
    }
  )
  
  // Funciones para consumir tus endpoints
  //Cliente
  const getCliente = () => api.get('/api/Clientes')
  const getClienteById = (id) => api.get(`/api/Clientes/{id}`)
  const crearCliente = (datos) => api.post('/api/Clientes', datos)
  const actualizarCliente = (id, datos) => api.put(`/api/Clientes/${id}`, datos)
  const eliminarCliente = (id) => api.delete(`/api/Clientes/${id}`)

  //Productos
  const getProductos = () => api.get('/api/Productos')
  const getProductoById = (id) => api.get(`/api/Productos/${id}`)
  const crearProducto = (datos) => api.post('/api/Productos', datos)
  const actualizarProducto = (id, datos) => api.put(`/api/Productos/${id}`, datos)
  const eliminarProducto = (id) => api.delete(`/api/Productos/${id}`)
  const getProductosLowStock = () => api.get('/api/Productos/low-stock')
  const getProductosLowStockById = () => api.get(`/api/Productos/${id}/stock`)
  // const getProductosByCategoria = (id) => api.get(`/api/Productos/Categoria/${id}`)
  // const getCategorias = () => api.get('/api/Categorias')
  // const getCategoriaById = (id) => api.get(`/api/Categorias/${id}`)

  //Ventas
  const getVentas = () => api.get('/api/Ventas')
  const getVentaById = (id) => api.get(`/api/Ventas/${id}`)
  const crearVenta = (datos) => api.post('/api/Ventas', datos)
  const actualizarVenta = (id, datos) => api.put(`/api/Ventas/${id}`, datos)
  const eliminarVenta = (id) => api.delete(`/api/Ventas/${id}`)
  const getVentasByProducto = (id) => api.get(`/api/Clientes/${id}`)

  return {
    //Cliente
    getCliente,
    getClienteById,
    crearCliente,
    actualizarCliente,
    eliminarCliente,
    //Productos
    getProductos,
    getProductoById,
    crearProducto,
    actualizarProducto,
    eliminarProducto,
    getProductosLowStock,
    getProductosLowStockById,
    // getProductosByCategoria,
    // getCategorias,
    // getCategoriaById,

    //Ventas
    getVentas,
    getVentaById,
    crearVenta,
    actualizarVenta,
    eliminarVenta,
    getVentasByProducto
  }
}