<template>
    <div class="container mx-auto px-4 py-8">
      <!--<h1 class="text-3xl font-bold mb-8">Event Check-in System</h1>-->
      <!-- Incluir o componente de Communities -->
      <Communities />
    </div>
  </template>
  
  <script>
  // Importando o componente Communities.vue
  import Communities from './components/Communities.vue';
  
  export default {
    name: 'App',
    components: {
      Communities,
    },
  };
  </script>
  
  <style>
  /* Estilos globais podem ser definidos aqui */
  </style>
  