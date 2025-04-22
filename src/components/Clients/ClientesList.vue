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

<style scoped>
.clientes-container {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.clientes-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 15px;
}

.clientes-header h1 {
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

.clientes-actions {
  display: flex;
  gap: 15px;
  align-items: center;
}

.search-container {
  position: relative;
}

.search-input {
  padding: 8px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 250px;
}

.clientes-table-container {
  overflow-x: auto;
}

.clientes-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 15px;
}

.clientes-table th,
.clientes-table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.clientes-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #333;
}

.clientes-table tr:hover {
  background-color: #f5f5f5;
}

.actions {
  display: flex;
  gap: 8px;
}

.btn-primary {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-primary:hover {
  background-color: #388e3c;
}

.btn-edit {
  background-color: #2196f3;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-edit:hover {
  background-color: #1976d2;
}

.btn-delete {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-delete:hover {
  background-color: #d32f2f;
}

@media (max-width: 768px) {
  .clientes-header {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .clientes-actions {
    width: 100%;
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }
  
  .search-input {
    width: 100%;
  }
  
  .actions {
    flex-direction: column;
    gap: 5px;
  }
  
  .clientes-table th,
  .clientes-table td {
    padding: 8px;
    font-size: 14px;
  }

    /* Estilos para el modal de eliminar */
  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal-content {
    background-color: white;
    padding: 0;
    border-radius: 4px;
    width: 500px;
    max-width: 90%;
  }

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
  }

  .modal-header h3 {
    margin: 0;
  }

  .btn-close {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #666;
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
    padding: 15px 20px;
    border-top: 1px solid #eee;
  }

  .btn-secondary, .btn-danger {
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

  .btn-secondary {
    background-color: #6c757d;
    color: white;
  }

  .btn-danger {
    background-color: #dc3545;
    color: white;
  }
}
</style>