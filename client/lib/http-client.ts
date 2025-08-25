import axios from 'axios'

export const httpClient = axios.create({
  baseURL: 'http://localhost:5001',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})
