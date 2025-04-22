<template>
  <div class="clientes-container">
    <div class="clientes-header">
      <h1>Clientes</h1>
      <div class="clientes-actions">
        <button @click="mostrarFormulario" class="btn-primary">
          Nuevo Cliente
        </button>
        <div class="search-container">
          <input 
            v-model="searchQuery" 
            placeholder="Buscar cliente..." 
            class="search-input"
          />
        </div>
      </div>
    </div>

    <div class="clientes-table-container">
      <table class="clientes-table">
        <thead>
          <tr>
            <th>Código</th>
            <th>Nombre</th>
            <th>Teléfono</th>
            <th>Email</th>
            <th>Dirección</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cliente in clientesFiltrados" :key="cliente.id">
            <td>{{ cliente.codigo }}</td>
            <td>{{ cliente.nombre }}</td>
            <td>{{ cliente.telefono }}</td>
            <td>{{ cliente.email }}</td>
            <td>{{ cliente.direccion }}</td>
            <td class="actions">
              <button @click="editarCliente(cliente)" class="btn-edit">
                Editar
              </button>
              <button @click="confirmarEliminar(cliente)" class="btn-delete">
                Eliminar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <ClienteForm 
      v-if="mostrarForm" 
      :cliente="clienteSeleccionado" 
      @guardar="guardarCliente" 
      @cancelar="ocultarFormulario" 
    />
    
    <!-- Modal de confirmación para eliminar -->
    <div v-if="modalEliminar" class="modal">
      <div class="modal-content">
        <div class="modal-header">
          <h3>Confirmar eliminación</h3>
          <button @click="modalEliminar = false" class="btn-close">×</button>
        </div>
        <p>¿Está seguro que desea eliminar el cliente "{{ clienteSeleccionado?.nombre }}"?</p>
        <div class="modal-actions">
          <button @click="modalEliminar = false" class="btn-secondary">Cancelar</button>
          <button @click="eliminarClienteConfirmado" class="btn-danger">Eliminar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped src= "@/assets/CSS/Cliente/ClienteList.css"></style>

<script>
import { ref, computed, onMounted } from 'vue';
import { useApi } from '@/components/composables/useApi';
import { useToast } from '@/components/composables/useToast';
import ClienteForm from '@/components/Clients/ClientesForm.vue';

export default {
  components: {
    ClienteForm
  },
  setup() {
    const { getClientes, crearCliente, actualizarCliente, eliminarCliente } = useApi();
    const { showToast } = useToast();
    
    const clientes = ref([]);
    const searchQuery = ref('');
    const mostrarForm = ref(false);
    const clienteSeleccionado = ref(null);
    
    // Estado para el modal de eliminar
    const modalEliminar = ref(false);

    const clientesFiltrados = computed(() => {
      if (!searchQuery.value) return clientes.value;
      const query = searchQuery.value.toLowerCase();
      return clientes.value.filter(c => 
        c.nombre.toLowerCase().includes(query) || 
        c.codigo.toLowerCase().includes(query) ||
        c.email.toLowerCase().includes(query)
      );
    });

    const cargarClientes = async () => {
      try {
        const response = await getClientes();
        clientes.value = response.data || [];
      } catch (error) {
        showToast('Error al cargar los clientes', 'error');
      }
    };

    const mostrarFormulario = () => {
      clienteSeleccionado.value = null;
      mostrarForm.value = true;
    };

    const ocultarFormulario = () => {
      mostrarForm.value = false;
    };

    const editarCliente = (cliente) => {
      clienteSeleccionado.value = cliente;
      mostrarForm.value = true;
    };

    const guardarCliente = async (clienteData) => {
      try {
        if (clienteData.id) {
          await actualizarCliente(clienteData.id, clienteData);
          showToast('Cliente actualizado con éxito', 'success');
        } else {
          await crearCliente(clienteData);
          showToast('Cliente creado con éxito', 'success');
        }
        await cargarClientes();
        ocultarFormulario();
      } catch (error) {
        showToast('Error al guardar el cliente', 'error');
      }
    };
    
    // Nuevas funciones para manejar el modal de eliminar
    const confirmarEliminar = (cliente) => {
      clienteSeleccionado.value = cliente;
      modalEliminar.value = true;
    };
    
    const eliminarClienteConfirmado = async () => {
      try {
        await eliminarCliente(clienteSeleccionado.value.id);
        showToast('Cliente eliminado con éxito', 'success');
        modalEliminar.value = false;
        await cargarClientes();
      } catch (error) {
        showToast('Error al eliminar el cliente', 'error');
        console.error(error);
        if (error.response?.data?.message) {
          showToast(error.response.data.message, 'error');
        }
      }
    };

    onMounted(() => {
      cargarClientes();
    });

    return {
      clientes,
      searchQuery,
      clientesFiltrados,
      mostrarForm,
      clienteSeleccionado,
      mostrarFormulario,
      ocultarFormulario,
      editarCliente,
      guardarCliente,
      modalEliminar,
      confirmarEliminar,
      eliminarClienteConfirmado
    };
  }
};
</script>