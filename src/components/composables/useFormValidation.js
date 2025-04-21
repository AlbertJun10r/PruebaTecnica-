import { ref, computed } from 'vue'
import * as validators from '../utils/validators'

export function useFormValidation(fields) {
  const errors = ref({})
  
  const validateField = (fieldName, value) => {
    const field = fields[fieldName]
    errors.value[fieldName] = []
    
    if (field.required && !validators.required(value)) {
      errors.value[fieldName].push('Este campo es requerido')
    }
    
    if (field.minLength && !validators.minLength(value, field.minLength)) {
      errors.value[fieldName].push(`Mínimo ${field.minLength} caracteres`)
    }
    
    if (field.type === 'email' && !validators.email(value)) {
      errors.value[fieldName].push('Ingrese un email válido')
    }
    
    if (field.type === 'number' && !validators.number(value)) {
      errors.value[fieldName].push('Ingrese un número válido')
    }
    
    // Puedes agregar más validaciones según necesites
  }
  
  const validateForm = () => {
    let isValid = true
    Object.keys(fields).forEach(fieldName => {
      validateField(fieldName, fields[fieldName].value)
      if (errors.value[fieldName].length > 0) {
        isValid = false
      }
    })
    return isValid
  }
  
  const hasErrors = computed(() => {
    return Object.values(errors.value).some(error => error.length > 0)
  })
  
  return { errors, validateField, validateForm, hasErrors }
}