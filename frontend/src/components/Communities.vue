<template>
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-8">Event Check-in System</h1>
  
      <!-- Label e combo para selecionar o evento -->
      <div>
        <label for="event" class="block text-lg font-medium mb-2">Selecione o Evento</label>
        <select v-model="selectedEvent" @change="fetchEventDetails" id="event" class="border rounded p-2">
          <option v-for="event in events" :key="event.id" :value="event.id">
            {{ event.name }}
          </option>
        </select>
      </div>
  
      <!-- Resumo do Evento -->
      <div v-if="eventSummary" class="mt-8">
        <h2 class="text-2xl font-semibold mb-4">Resumo do Evento</h2>
        <p><strong>Total de Pessoas:</strong> {{ eventSummary.totalPeople }}</p>
        <p><strong>Pessoas com Check-in:</strong> {{ eventSummary.checkedInPeople }}</p>
        <p><strong>Pessoas com Check-out:</strong> {{ eventSummary.checkedOutPeople }}</p>
        <p><strong>Pessoas Sem Check-in:</strong> {{ eventSummary.noCheckedInPeople }}</p>
      </div>
  
      <!-- Tabela para exibir as empresas e a quantidade de participantes -->
      <!-- Tabela para exibir as empresas e a quantidade de participantes -->
        <div v-if="Object.keys(companies).length > 0" class="mt-8">
        <h2 class="text-2xl font-semibold mb-4">Empresas no Evento</h2>
        <table class="min-w-full table-auto border-collapse">
            <thead>
            <tr>
                <th class="px-4 py-2 border-b">Empresa</th>
                <th class="px-4 py-2 border-b">Quantidade de Participantes</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(entry, index) in paginatedCompanies" :key="index">
                <td class="px-4 py-2 border-b">{{ entry.company }}</td>
                <td class="px-4 py-2 border-b">{{ entry.count }}</td>
            </tr>
            </tbody>
        </table>
        
        <!-- Navegação da paginação -->
        <div class="flex justify-between items-center mt-4">
            <button 
            @click="previousPage"
            :disabled="currentPage === 1"
            class="px-4 py-2 bg-blue-500 text-white rounded disabled:opacity-50 disabled:cursor-not-allowed">
            Página Anterior
            </button>
            <span>Página {{ currentPage }} de {{ totalPages }}</span>
            <button 
            @click="nextPage"
            :disabled="currentPage === totalPages"
            class="px-4 py-2 bg-blue-500 text-white rounded disabled:opacity-50 disabled:cursor-not-allowed">
            Próxima Página
            </button>
        </div>
        </div>

  
      <!-- Tabela para exibir as pessoas -->
      <div v-if="people.length > 0" class="mt-8">
        <h2 class="text-2xl font-semibold mb-4" >Pessoas no Evento</h2>
        <table class="min-w-full table-auto border-collapse">
          <thead>
            <tr>
              <th class="px-4 py-2 border-b">Nome</th>
              <th class="px-4 py-2 border-b">Empresa</th>
              <th class="px-4 py-2 border-b">Cargo</th>
              <th class="px-4 py-2 border-b">Check-in</th>
              <th class="px-4 py-2 border-b">Check-out</th>
              <th class="px-4 py-2 border-b">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="person in people" :key="person.id">
              <td class="px-4 py-2 border-b">{{ person.firstName }} {{ person.lastName }}</td>
              <td class="px-4 py-2 border-b">{{ person.companyName || 'Não especificado' }}</td>
              <td class="px-4 py-2 border-b">{{ person.title || 'Não especificado' }}</td>
              <td class="px-4 py-2 border-b">{{ person.checkInDate ? new Date(person.checkInDate).toLocaleString() : 'Não registrado' }}</td>
              <td class="px-4 py-2 border-b">{{ person.checkOutDate ? new Date(person.checkOutDate).toLocaleString() : 'Não registrado' }}</td>
              <td class="px-4 py-2 border-b">
                <!-- Botões de Check-in e Check-out -->
                <button 
                  v-if="!person.checkInDate && !person.checkOutDate" 
                  @click="checkIn(person.id)" 
                  class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Check-in</button>
                <button 
                  v-if="person.checkInDate && !person.checkOutDate" 
                  @click="checkOut(person.id)" 
                  class="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600">Check-out</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        events: [],       // Lista de eventos
        selectedEvent: null, // Evento selecionado
        people: [],       // Lista de pessoas do evento
        eventSummary: null, // Resumo do evento
        companies: {},    // Empresas e contagem de pessoas
        currentPage: 1,   // Página atual
        itemsPerPage: 5,  // Quantidade de itens por página
      };
    },
    computed: {
      // Calcula as empresas a serem exibidas com base na página atual
      paginatedCompanies() {
        const start = (this.currentPage - 1) * this.itemsPerPage;
        const end = start + this.itemsPerPage;
        const companiesArray = Object.entries(this.companies);
        return companiesArray.slice(start, end).map(([company, count]) => ({
          company,
          count,
        }));
      },
      totalPages() {
        return Math.ceil(Object.keys(this.companies).length / this.itemsPerPage);
      },
    },
    methods: {
      async fetchEvents() {
        try {
          const response = await fetch('http://localhost:5203/api/event/communities');
          const data = await response.json();
          this.events = data;  // Aqui você pode ajustar a estrutura da resposta da API conforme necessário
        } catch (error) {
          console.error('Error fetching events:', error);
        }
      },
      async fetchEventDetails() {
        if (this.selectedEvent) {
          try {
            // Faz a requisição para obter as pessoas do evento selecionado
            const response = await fetch(`http://localhost:5203/api/event/people/${this.selectedEvent}`);
            const peopleData = await response.json();
            this.people = peopleData;  // Atualiza a lista de pessoas
  
            // Agora, fazemos a requisição para o resumo do evento
            const summaryResponse = await fetch(`http://localhost:5203/api/event/summary/${this.selectedEvent}`);
            const summaryData = await summaryResponse.json();
            this.eventSummary = summaryData;  // Atualiza o resumo do evento
  
            // Processa as empresas e conta quantas pessoas existem em cada uma
            this.companies = this.getCompaniesCount(peopleData);
            this.currentPage = 1; // Reseta a página ao trocar o evento
          } catch (error) {
            console.error('Error fetching event details:', error);
          }
        }
      },
      getCompaniesCount(people) {
        const companyCounts = {};
        people.forEach(person => {
          const companyName = person.companyName || 'Não especificado';
          companyCounts[companyName] = (companyCounts[companyName] || 0) + 1;
        });
        return companyCounts;
      },
      async checkIn(personId) {
        try {
          await fetch(`http://localhost:5203/api/event/check-in/${personId}`, {
            method: 'POST',
          });
          this.fetchEventDetails(); // Atualiza a lista de pessoas
        } catch (error) {
          console.error('Error checking in person:', error);
        }
      },
      async checkOut(personId) {
        try {
          await fetch(`http://localhost:5203/api/event/check-out/${personId}`, {
            method: 'POST',
          });
          this.fetchEventDetails(); // Atualiza a lista de pessoas
        } catch (error) {
          console.error('Error checking out person:', error);
        }
      },
      nextPage() {
        if (this.currentPage < this.totalPages) {
          this.currentPage++;
        }
      },
      previousPage() {
        if (this.currentPage > 1) {
          this.currentPage--;
        }
      },
    },
    mounted() {
      this.fetchEvents();  // Carrega os eventos assim que o componente for montado
    },
  };
</script>

  
<style scoped>
/* Estilos para o container */
.container {
  max-width: 900px;
  margin: 0 auto;
}

/* Espaçamento entre as tabelas */
table {
  width: 100%;
  border: 1px solid #ddd;
  border-collapse: collapse;
  margin-bottom: 20px; /* Adiciona espaço entre as tabelas */
}

/* Estilo das células e cabeçalhos */
th, td {
  text-align: left;
  padding: 12px;
  border: 1px solid #ddd;
}

/* Cabeçalhos de tabela com fundo colorido */
th {
  background-color: #f4f4f4;
  color: #333;
}

/* Linhas da tabela com cor alternada */
tr:nth-child(even) {
  background-color: #f9f9f9;
}

tr:nth-child(odd) {
  background-color: #ffffff;
}

/* Botão com cor personalizada e hover */
button {
  padding: 8px 16px;
  margin: 5px;
  border-radius: 4px;
  cursor: pointer;
  font-weight: bold;
}

/* Cor do botão de Check-in */
button.bg-blue-500 {
  background-color: #4869d5; /* Azul escuro */
  color: white;
}

button.bg-blue-500:hover {
  background-color: #537dd7; /* Azul mais claro */
}

/* Cor do botão de Check-out */
button.bg-green-500 {
  background-color: #10B981; /* Verde */
  color: white;
}

button.bg-green-500:hover {
  background-color: #16A34A; /* Verde mais escuro */
}

/* Botão desabilitado com opacidade */
button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* Estilo da navegação da paginação */
button:disabled {
  background-color: #d1d5db;
  color: #6B7280;
}

</style>

  

  