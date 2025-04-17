import React from 'react';
import { View, Text, Image, TouchableOpacity } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { FontAwesome5 } from '@expo/vector-icons';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { AccountStackParamList } from '../../types/navigation.type';

type MenuScreenNavigationProp = NativeStackNavigationProp<AccountStackParamList, 'Menu'>;

export default function MenuScreen() {
  const navigation = useNavigation<MenuScreenNavigationProp>();

  const handleInfoPress = () => {
    navigation.navigate('Info');
  };

  const handleAccountPress = () => {
    navigation.navigate('Account');
  };

  const handleLogout = () => {
    // TODO: Implement logout
  };

  return (
    <View className="flex-1 bg-white pt-16 px-6">
      {/* Avatar + Name */}
      <View className="items-center mb-10">
        <Image
          source={{ uri: 'https://randomuser.me/api/portraits/men/1.jpg' }}
          className="w-24 h-24 rounded-full mb-3"
        />
        <Text className="text-xl font-quicksand-bold text-gray-800">Minh Bùi</Text>
      </View>

      {/* Menu Buttons */}
      <View className="space-y-4">
        <TouchableOpacity
          className="flex-row items-center bg-gray-100 rounded-xl px-5 py-4"
          onPress={handleAccountPress}
        >
          <FontAwesome5 name="user-cog" size={20} color="#374151" />
          <Text className="ml-4 text-base font-quicksand-semibold text-gray-800">Tài khoản</Text>
        </TouchableOpacity>

        <TouchableOpacity
          className="flex-row items-center bg-red-100 rounded-xl px-5 py-4 mt-3"
          onPress={handleLogout}
        >
          <FontAwesome5 name="sign-out-alt" size={20} color="#e74c3c" />
          <Text className="ml-4 text-base font-quicksand-semibold text-red-600">Đăng xuất</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}
