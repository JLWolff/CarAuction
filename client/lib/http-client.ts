import axios from 'axios'

export const httpClient = axios.create({
  baseURL: 'http://carauction_server_1:5001',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})
