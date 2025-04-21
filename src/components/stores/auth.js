import { defineStore } from 'pinia'
import { useRouter } from 'vue-router'
import { useToast } from '@/composables/useToast'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: localStorage.getItem('token') || null,
    isAuthenticated: !!localStorage.getItem('token')
  }),
  actions: {
    setAuthData({ user, token }) {
      this.user = user
      this.token = token
      this.isAuthenticated = true
      localStorage.setItem('token', token)
    },
    logout() {
      this.user = null
      this.token = null
      this.isAuthenticated = false
      localStorage.removeItem('token')
      const router = useRouter()
      router.push('/login')
    },
    async checkAuth() {
      if (!this.token) return false
      
      try {
        const { fetchData } = useApi()
        const response = await fetchData('/api/auth/check')
        this.user = response.user
        return true
      } catch (error) {
        this.logout()
        const { showToast } = useToast()
        showToast('Sesión expirada', 'error')
        return false
      }
    }
  }
})