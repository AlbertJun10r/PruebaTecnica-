<template>
    <v-card>
      <v-card-title>
        {{ productId ? 'Editar Producto' : 'Nuevo Producto' }}
      </v-card-title>
      
      <v-form @submit.prevent="submitForm">
        <v-card-text>
          <v-text-field
            v-model="form.nombre"
            label="Nombre"
            :rules="[rules.required]"
          />
          
          <v-textarea
            v-model="form.descripcion"
            label="Descripción"
            rows="2"
          />
          
          <v-text-field
            v-model="form.precio"
            label="Precio"
            type="number"
            min="0.01"
            step="0.01"
            :rules="[rules.required, rules.minValue(0.01)]"
          />
          
          <v-text-field
            v-model="form.stock"
            label="Stock"
            type="number"
            min="0"
            :rules="[rules.required, rules.minValue(0)]"
          />
        </v-card-text>
        
        <v-card-actions>
          <v-spacer />
          <v-btn color="secondary" @click="$router.go(-1)">
            Cancelar
          </v-btn>
          <v-btn 
            color="primary" 
            type="submit"
            :loading="productsStore.loading"
          >
            Guardar
          </v-btn>
        </v-card-actions>
      </v-form>
    </v-card>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useProductsStore } from '@/stores/products'
  
  const route = useRoute()
  const router = useRouter()
  const productsStore = useProductsStore()
  
  const productId = route.params.id
  const form = ref({
    nombre: '',
    descripcion: '',
    precio: 0,
    stock: 0
  })
  
  const rules = {
    required: value => !!value || 'Campo requerido',
    minValue: min => value => value >= min || `El valor mínimo es ${min}`
  }
  
  const submitForm = async () => {
    try {
      if (productId) {
        await productsStore.updateProduct(productId, form.value)
      } else {
        await productsStore.createProduct(form.value)
      }
      router.push('/productos')
    } catch (error) {
      // El error ya es manejado por el store
    }
  }
  </script>