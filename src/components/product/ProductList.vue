<template>
    <v-card>
      <v-card-title>
        <v-row>
          <v-col cols="12" md="6">
            <h2>Lista de Productos</h2>
          </v-col>
          <v-col cols="12" md="6" class="text-right">
            <v-btn color="primary" @click="$router.push('/productos/nuevo')">
              Nuevo Producto
            </v-btn>
          </v-col>
        </v-row>
      </v-card-title>
      
      <v-data-table
        :headers="headers"
        :items="productsStore.products"
        :loading="productsStore.loading"
      >
        <template v-slot:item.actions="{ item }">
          <v-btn icon @click="editProduct(item.id)">
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
          <v-btn icon color="error" @click="deleteProduct(item.id)">
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </v-card>
  </template>
  
  <script setup>
  import { useProductsStore } from '@/stores/products'
  
  const productsStore = useProductsStore()
  
  const headers = [
    { title: 'ID', value: 'id' },
    { title: 'Nombre', value: 'nombre' },
    { title: 'Precio', value: 'precio' },
    { title: 'Stock', value: 'stock' },
    { title: 'Acciones', value: 'actions', sortable: false }
  ]
  
  // Cargar productos al montar el componente
  onMounted(() => {
    productsStore.fetchProducts()
  })
  </script>