<!-- src/views/ClientesView.vue -->
<template>
    <div class="clientes-container">
      <h1 class="text-2xl font-bold mb-4">Gestión de Clientes</h1>
      

  
      <ClientesList 
        :clientes="clientesFiltrados" 
        :loading="cargando"
        @editar="editarCliente" 
        @eliminar="confirmarEliminar" 
      />
  
      <ClienteForm
        v-if="mostrarFormulario"
        :cliente="clienteSeleccionado"
        @guardar="guardarCliente"
        @cancelar="cerrarFormulario"
      />
  
      <!-- Modal de confirmación para eliminar -->
      <div v-if="mostrarConfirmacion" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-10">
        <div class="bg-white p-6 rounded-lg shadow-lg max-w-md">
          <h3 class="text-lg font-bold mb-4">Confirmar eliminación</h3>
          <p>¿Estás seguro de que deseas eliminar este cliente?</p>
          <div class="mt-6 flex justify-end space-x-3">
            <button 
              @click="mostrarConfirmacion = false" 
              class="px-4 py-2 border rounded"
            >
              Cancelar
            </button>
            <button 
              @click="eliminarCliente" 
              class="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
            >
              Eliminar
            </button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import { useApi } from '../composables/useApi';
  import ClientesList from '../Clients/ClientesList.vue';
  import ClienteForm from '../Clients/ClientesForm.vue';
  
  const api = useApi();
  const clientes = ref([]);
  const cargando = ref(false);
  const mostrarFormulario = ref(false);
  const clienteSeleccionado = ref(null);
  const mostrarConfirmacion = ref(false);
  const clienteIdEliminar = ref(null);
  const busqueda = ref('');
  
  // Filtrar clientes basado en la búsqueda
  const clientesFiltrados = computed(() => {
    if (!busqueda.value) return clientes.value;
    
    const termino = busqueda.value.toLowerCase();
    return clientes.value.filter(cliente => 
      cliente.nombre?.toLowerCase().includes(termino) || 
      cliente.email?.toLowerCase().includes(termino) ||
      cliente.telefono?.includes(termino)
    );
  });
  
  // Cargar los clientes al montar el componente
  onMounted(async () => {
    await cargarClientes();
  });
  
  // Cargar la lista de clientes
  async function cargarClientes() {
    cargando.value = true;
    try {
      const response = await api.getCliente();
      clientes.value = response.data;
    } catch (error) {
      console.error('Error al cargar clientes:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Abrir formulario para crear un nuevo cliente
  function abrirFormulario() {
    clienteSeleccionado.value = null;
    mostrarFormulario.value = true;
  }
  
  // Abrir formulario para editar un cliente existente
  function editarCliente(cliente) {
    clienteSeleccionado.value = { ...cliente };
    mostrarFormulario.value = true;
  }
  
  // Cerrar el formulario
  function cerrarFormulario() {
    mostrarFormulario.value = false;
    clienteSeleccionado.value = null;
  }
  
  // Guardar un cliente (crear o actualizar)
  async function guardarCliente(cliente) {
    cargando.value = true;
    try {
      if (cliente.id) {
        // Actualizar cliente existente
        await api.actualizarCliente(cliente.id, cliente);
      } else {
        // Crear nuevo cliente
        await api.crearCliente(cliente);
      }
      await cargarClientes();
      cerrarFormulario();
    } catch (error) {
      console.error('Error al guardar cliente:', error);
    } finally {
      cargando.value = false;
    }
  }
  
  // Confirmar eliminación de un cliente
  function confirmarEliminar(id) {
    clienteIdEliminar.value = id;
    mostrarConfirmacion.value = true;
  }
  
  // Eliminar cliente
  async function eliminarCliente() {
    cargando.value = true;
    try {
      await api.eliminarCliente(clienteIdEliminar.value);
      await cargarClientes();
      mostrarConfirmacion.value = false;
    } catch (error) {
      console.error('Error al eliminar cliente:', error);
    } finally {
      cargando.value = false;
    }
  }
  </script>