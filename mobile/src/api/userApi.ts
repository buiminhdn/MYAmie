import axiosInstance from './axiosInstance';

interface LoginPayload {
  email: string;
  password: string;
}

export const login = async (payload: LoginPayload) => {
  const response = await axiosInstance.post('/auth/login', payload);
  return response.data;
};
