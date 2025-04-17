// screens/AccountScreen.tsx
import React, { useState } from 'react';
import { View, TextInput, Text, TouchableOpacity } from 'react-native';

export default function AccountScreen() {
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleChangePassword = () => {
    // TODO: Validate and call API to change password
  };

  return (
    <View className="flex-1 bg-white px-6 pt-12">
      <Text className="text-xl font-quicksand-bold mb-6 text-gray-800">Đổi mật khẩu</Text>

      <TextInput
        className="border border-gray-300 rounded-xl px-4 py-3 mb-4 text-gray-800"
        placeholder="Mật khẩu cũ"
        secureTextEntry
        value={oldPassword}
        onChangeText={setOldPassword}
      />

      <TextInput
        className="border border-gray-300 rounded-xl px-4 py-3 mb-4 text-gray-800"
        placeholder="Mật khẩu mới"
        secureTextEntry
        value={newPassword}
        onChangeText={setNewPassword}
      />

      <TextInput
        className="border border-gray-300 rounded-xl px-4 py-3 mb-6 text-gray-800"
        placeholder="Nhập lại mật khẩu mới"
        secureTextEntry
        value={confirmPassword}
        onChangeText={setConfirmPassword}
      />

      <TouchableOpacity
        onPress={handleChangePassword}
        className="bg-primary py-4 rounded-xl items-center"
      >
        <Text className="text-white font-quicksand-bold text-base">Đổi mật khẩu</Text>
      </TouchableOpacity>
    </View>
  );
}
