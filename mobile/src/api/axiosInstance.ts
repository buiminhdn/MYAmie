import axios from 'axios';
import { getToken } from '../utils/auth';

const axiosClient = axios.create({
  baseURL: 'https://localhost:7087/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosClient.interceptors.request.use(async (config) => {
  const token = await getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosClient;
