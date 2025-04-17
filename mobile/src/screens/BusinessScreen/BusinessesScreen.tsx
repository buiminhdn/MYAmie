import * as React from 'react';
import { View, Text, ScrollView } from 'react-native';
import SearchBar from '../../components/SearchBar';
import TitleSection from '../../components/TitleSection';
import BusinessCard from '../../components/Cards/BusinessCard';
import { services } from '../../assets/data/businessData';

export default function BusinessesScreen() {
  return (
    <ScrollView className="flex-1 bg-white">
      <View className="px-5 py-6 mt-10">
        <TitleSection smallTitle="Hơn 300 dịch vụ gần đây" bigTitle="Các dịch vụ nổi bật" />

        <SearchBar />

        <View className="mt-5 mb-3">
          <Text className="text-sm text-gray-800 font-quicksand-semibold">
            Danh sách 320 dịch vụ
          </Text>
        </View>

        {services.map((service) => (
          <BusinessCard
            key={service.id}
            title={service.title}
            description={service.description}
            imageUrl={service.imageUrl}
            rating={service.rating}
            location={service.location}
            hours={service.hours}
          />
        ))}
      </View>
    </ScrollView>
  );
}
