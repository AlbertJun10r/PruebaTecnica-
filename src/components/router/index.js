import { createRouter, createWebHistory } from 'vue-router';

// Importar componentes
import Dashboard from '@/components/Dashboard.vue';
import ProductosView from '../views/ProductsView.vue';
import ClientesView from '../views/ClientesView.vue';
import VentasView from '../views/VentasView.vue';

const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: Dashboard
  },
  {
    path: '/clientes',
    name: 'clientes',
    component: ClientesView,
    meta: {
      title: 'Gestión de Clientes'
    }
  },
  {
    path: '/productos',
    name: 'productos',
    component: ProductosView,
    meta: {
      title: 'Gestión de Productos'
    }
  },
  {
    path: '/ventas',
    name: 'ventas',
    component: VentasView,
    meta: {
      title: 'Gestión de Ventas'
    }
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// Cambiar el título de la página según la ruta
router.beforeEach((to, from, next) => {
  document.title = to.meta.title || 'Sistema de Gestión';
  next();
});

export default router;


// import { createRouter, createWebHistory } from 'vue-router';

// // Importar componentes
// import Dashboard from '@/components/Dashboard.vue';
// import ProductosView from '../views/ProductsView.vue';
// import VentasList from '@/views/ventas/VentasList.vue';
// import VentasForm from '@/views/ventas/VentasForm.vue';
// import VentasDetalle from '@/views/ventas/VentasDetalle.vue';
// import ClientesView from '../views/ClientesView.vue';

// const routes = [
//   {
//     path: '/',
//     name: 'dashboard',
//     component: Dashboard
//   },
//   {
//     path: '/productos',
//     name: 'productos',
//     component: ProductosList
//   },
//   {
//     path: '/productos/nuevo',
//     name: 'nuevoProducto',
//     component: ProductosForm
//   },
//   {
//     path: '/productos/editar/:id',
//     name: 'editarProducto',
//     component: ProductosForm
//   },
//   {
//     path: '/clientes',
//     name: 'clientes',
//     component: ClientesView
//   },
//   {
//     path: '/ventas',
//     name: 'ventas',
//     component: VentasList
//   },
//   {
//     path: '/ventas/nueva',
//     name: 'nuevaVenta',
//     component: VentasForm
//   },
//   {
//     path: '/ventas/:id',
//     name: 'detalleVenta',
//     component: VentasDetalle
//   }
// ];

// const router = createRouter({
//   history: createWebHistory(process.env.BASE_URL),
//   routes
// });

// export default router;