<!-- src/components/layout/Navbar.vue -->
<template>
    <nav class="bg-gray-800 text-white p-4 mb-6">
      <div class="container mx-auto flex justify-between items-center">
        <div class="font-bold text-xl">Sistema de Gestión</div>
        <div class="flex space-x-4">
          <router-link to="/productos" class="hover:text-blue-300">Productos</router-link>
          <router-link to="/clientes" class="hover:text-blue-300">Clientes</router-link>
          <router-link to="/ventas" class="hover:text-blue-300">Ventas</router-link>
        </div>
      </div>
    </nav>
  </template>