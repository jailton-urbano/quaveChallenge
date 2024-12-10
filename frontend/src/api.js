import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5203/api',  // Substitua pela URL da sua API
  timeout: 10000,  // Tempo máximo de espera pela resposta
});

export default api;
