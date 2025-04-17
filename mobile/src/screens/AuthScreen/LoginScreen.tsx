import { View, Text, TextInput, TouchableOpacity, Alert } from 'react-native';
import React, { useState } from 'react';
import { useAuth } from '../../Context/AuthContext';
import { login } from '../../api/userApi';
import { saveAuthData } from '../../utils/auth';

export default function LoginScreen() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login: setLoggedIn } = useAuth();

  const handleLogin = async () => {
    try {
      const res = await login({ email, password });
      Alert.alert(res);
      if (res.isSuccess) {
        const { accessToken, refreshToken, ...userData } = res.data;
        await saveAuthData(accessToken, refreshToken, userData);
        setLoggedIn();
      } else {
        Alert.alert('Lỗi', res.message || 'Đăng nhập thất bại');
      }
    } catch (err) {
      Alert.alert('Lỗi', 'Email hoặc mật khẩu không chính xác');
    }
  };

  return (
    <View className="flex-1 justify-center items-center bg-white px-6">
      <Text className="text-2xl font-quicksand-bold mb-6">Đăng nhập</Text>
      <TextInput
        placeholder="Email"
        value={email}
        onChangeText={setEmail}
        className="border border-gray-300 rounded-xl w-full mb-4 px-4 py-3 text-base"
      />
      <TextInput
        placeholder="Mật khẩu"
        value={password}
        onChangeText={setPassword}
        secureTextEntry
        className="border border-gray-300 rounded-xl w-full mb-6 px-4 py-3 text-base"
      />
      <TouchableOpacity onPress={handleLogin} className="bg-primary rounded-xl px-6 py-3 w-full">
        <Text className="text-white font-quicksand-bold text-center text-base">Đăng nhập</Text>
      </TouchableOpacity>
    </View>
  );
}
