import { View, Text, StyleSheet, ScrollView } from 'react-native';
import React from 'react';
import TitleSection from '../../components/TitleSection';
import UserCard from '../../components/Cards/UserCard';

export default function FriendsScreen() {
  return (
    <ScrollView className="flex-1 bg-white">
      <View className="px-5 py-6 mt-10">
        <TitleSection smallTitle="Hơn 300 dịch vụ gần đây" bigTitle="Các dịch vụ nổi bật" />

        <View className="mt-5 mb-3">
          <Text className="text-sm text-gray-800 font-quicksand-semibold">
            Danh sách 320 dịch vụ
          </Text>
        </View>

        <UserCard />
      </View>
    </ScrollView>
  );
}
