<template>
    <div class="dashboard">
      <!-- Resumen de estadísticas -->
      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon">
            <i class="fas fa-box"></i>
          </div>
          <div class="stat-content">
            <h3 class="stat-value">{{ totalProductos }}</h3>
            <p class="stat-label">Productos</p>
          </div>
        </div>
        
        <div class="stat-card">
          <div class="stat-icon">
            <i class="fas fa-users"></i>
          </div>
          <div class="stat-content">
            <h3 class="stat-value">{{ totalClientes }}</h3>
            <p class="stat-label">Clientes</p>
          </div>
        </div>
        
        <div class="stat-card">
          <div class="stat-icon">
            <i class="fas fa-cash-register"></i>
          </div>
          <div class="stat-content">
            <h3 class="stat-value">{{ totalVentas }}</h3>
            <p class="stat-label">Ventas</p>
          </div>
        </div>
        
        <div class="stat-card">
          <div class="stat-icon">
            <i class="fas fa-dollar-sign"></i>
          </div>
          <div class="stat-content">
            <h3 class="stat-value">${{ ingresoTotal.toFixed(2) }}</h3>
            <p class="stat-label">Ingresos Totales</p>
          </div>
        </div>
      </div>
  
      <!-- Productos con poco stock -->
      <div class="card">
        <div class="card-header">
          <h2 class="card-title">Productos con Poco Stock</h2>
          <router-link to="/productos" class="btn btn-primary">Ver Todos</router-link>
        </div>
        <div v-if="loadingProductos" class="loading">
          <i class="fas fa-spinner fa-spin"></i> Cargando...
        </div>
        <div v-else-if="productosLowStock.length === 0" class="empty-state">
          <i class="fas fa-check-circle"></i>
          <p>Todos los productos tienen stock suficiente</p>
        </div>
        <table v-else class="table">
          <thead>
            <tr>
              <th>Producto</th>
              <th>Stock</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="producto in productosLowStock" :key="producto.id">
              <td>{{ producto.nombre }}</td>
              <td>{{ producto.stock }}</td>
              <td>
                <span class="badge" :class="getBadgeClass(producto.stock)">
                  {{ getStockStatus(producto.stock) }}
                </span>
              </td>
              <td>
                <router-link :to="`/productos/editar/${producto.id}`" class="btn-action">
                  <i class="fas fa-edit"></i>
                </router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <!-- Ventas recientes -->
      <div class="card">
        <div class="card-header">
          <h2 class="card-title">Ventas Recientes</h2>
          <router-link to="/ventas" class="btn btn-primary">Ver Todas</router-link>
        </div>
        <div v-if="loadingVentas" class="loading">
          <i class="fas fa-spinner fa-spin"></i> Cargando...
        </div>
        <div v-else-if="ventasRecientes.length === 0" class="empty-state">
          <i class="fas fa-receipt"></i>
          <p>No hay ventas recientes</p>
        </div>
        <table v-else class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Fecha</th>
              <th>Total</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="venta in ventasRecientes" :key="venta.id">
              <td>{{ venta.id }}</td>
              <td>{{ venta.cliente.nombre }}</td>
              <td>{{ formatDate(venta.fecha) }}</td>
              <td>${{ venta.total.toFixed(2) }}</td>
              <td>
                <router-link :to="`/ventas/${venta.id}`" class="btn-action">
                  <i class="fas fa-eye"></i>
                </router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  <script>
  import { useApi } from '../components/composables/useApi';
  
  export default {
    name: 'Dashboard',
    data() {
      return {
        totalProductos: 0,
        totalClientes: 0,
        totalVentas: 0,
        ingresoTotal: 0,
        productosLowStock: [],
        ventasRecientes: [],
        loadingProductos: true,
        loadingVentas: true
      };
    },
    created() {
      this.cargarDatos();
    },
    methods: {
      async cargarDatos() {
        const api = useApi();
        
        try {
          // Cargar productos con stock bajo
          this.loadingProductos = true;
          const responseProductos = await api.getProductosLowStock();
          this.productosLowStock = responseProductos.data;
          
          // Cargar total de productos
          const allProductos = await api.getProductos();
          this.totalProductos = allProductos.data.length;
        } catch (error) {
          console.error('Error al cargar productos:', error);
        } finally {
          this.loadingProductos = false;
        }
        
        try {
          // Cargar ventas recientes
          this.loadingVentas = true;
          const responseVentas = await api.getVentas();
          this.ventasRecientes = responseVentas.data
            .sort((a, b) => new Date(b.fecha) - new Date(a.fecha))
            .slice(0, 5); // Mostrar solo las 5 más recientes
          
          // Calcular total de ventas e ingresos
          this.totalVentas = responseVentas.data.length;
          this.ingresoTotal = responseVentas.data.reduce((sum, venta) => sum + venta.total, 0);
        } catch (error) {
          console.error('Error al cargar ventas:', error);
        } finally {
          this.loadingVentas = false;
        }
        
        try {
          // Cargar total de clientes
          const responseClientes = await api.getCliente();
          this.totalClientes = responseClientes.data.length;
        } catch (error) {
          console.error('Error al cargar clientes:', error);
        }
      },
      
      formatDate(dateString) {
        const date = new Date(dateString);
        return new Intl.DateTimeFormat('es-ES', { 
          day: '2-digit', 
          month: '2-digit', 
          year: 'numeric' 
        }).format(date);
      },
      
      getStockStatus(stock) {
        if (stock <= 0) return 'Sin Stock';
        if (stock < 5) return 'Crítico';
        if (stock < 10) return 'Bajo';
        return 'Normal';
      },
      
      getBadgeClass(stock) {
        if (stock <= 0) return 'badge-danger';
        if (stock < 5) return 'badge-danger';
        if (stock < 10) return 'badge-warning';
        return 'badge-success';
      }
    }
  };
  </script>
  
  <style scoped>
  .dashboard {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
  }
  
  .stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
    gap: 1.5rem;
    margin-bottom: 1rem;
  }
  
  .stat-card {
    background: #fff;
    border-radius: 8px;
    padding: 1.5rem;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
    display: flex;
    align-items: center;
  }
  
  .stat-icon {
    width: 48px;
    height: 48px;
    border-radius: 8px;
    background: rgba(52, 152, 219, 0.1);
    color: #3498db;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    margin-right: 1rem;
  }
  
  .stat-content {
    flex: 1;
  }
  
  .stat-value {
    font-size: 1.5rem;
    font-weight: 700;
    margin: 0;
    color: #2c3e50;
  }
  
  .stat-label {
    margin: 0;
    color: #7f8c8d;
    font-size: 0.875rem;
  }
  
  .loading {
    padding: 1.5rem;
    text-align: center;
    color: #7f8c8d;
  }
  
  .empty-state {
    padding: 2rem;
    text-align: center;
    color: #7f8c8d;
  }
  
  .empty-state i {
    font-size: 2.5rem;
    color: #bdc3c7;
    margin-bottom: 1rem;
  }
  
  .btn-action {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 32px;
    height: 32px;
    border-radius: 4px;
    color: #3498db;
    transition: all 0.2s ease;
  }
  
  .btn-action:hover {
    background: rgba(52, 152, 219, 0.1);
  }
  </style>