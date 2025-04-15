import { createRouter, createWebHistory } from 'vue-router'

const routes = 
[
    {
        path: '/',
        name: 'dashboard',
        component: () => import('@/views/DashboardView.vue')
      },
      {
        path: '/productos',
        name: 'products',
        component: () => import('@/views/ProductsView.vue')
      },
      {
        path: '/clientes',
        name: 'clients',
        component: () => import('@/views/ClientsView.vue')
      },
      {
        path: '/ventas',
        name: 'sales',
        component: () => import('@/views/SalesView.vue'),
        children: [
          {
            path: 'nueva',
            component: () => import('@/components/sales/NewSaleForm.vue')
          },
          {
            path: 'historial',
            component: () => import('@/components/sales/SalesHistory.vue')
          }
        ]
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router