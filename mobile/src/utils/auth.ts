import AsyncStorage from '@react-native-async-storage/async-storage';

export const saveAuthData = async (accessToken: string, refreshToken: string, user: object) => {
  await AsyncStorage.setItem('accessToken', accessToken);
  await AsyncStorage.setItem('refreshToken', refreshToken);
  await AsyncStorage.setItem('user', JSON.stringify(user));
};

export const getToken = async () => {
  return await AsyncStorage.getItem('accessToken');
};

export const clearAuthData = async () => {
  await AsyncStorage.removeItem('accessToken');
  await AsyncStorage.removeItem('refreshToken');
  await AsyncStorage.removeItem('user');
};
