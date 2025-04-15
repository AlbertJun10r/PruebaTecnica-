import { defineStore } from 'pinia'
import { useApi } from '@/composables/useApi'

export const useProductsStore = defineStore('products', {
  state: () => ({
    products: [],
    loading: false,
    error: null
  }),
  actions: {
    async fetchProducts() {
      this.loading = true
      try {
        const { data } = await useApi().get('/api/productos')
        this.products = data
      } catch (error) {
        this.error = error.message
      } finally {
        this.loading = false
      }
    },
    async createProduct(productData) {
      this.loading = true
      try {
        const { data } = await useApi().post('/api/productos', productData)
        this.products.push(data)
        return data
      } catch (error) {
        this.error = error.response?.data || error.message
        throw error
      } finally {
        this.loading = false
      }
    }
  }
})