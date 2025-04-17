import { View, Text, StyleSheet, ScrollView } from 'react-native';
import React from 'react';
import TitleSection from '../../components/TitleSection';
import SearchBar from '../../components/SearchBar';
import { placeCardData } from '../../assets/data/placeData';
import PlaceCard from '../../components/Cards/PlaceCard';

export default function PlacesScreen() {
  return (
    <ScrollView className="flex-1 bg-white">
      <View className="px-5 py-6 mt-10">
        <TitleSection smallTitle="Hơn 400 địa điểm gần đây" bigTitle="Các địa điểm thú vị" />

        <SearchBar />

        <View className="mt-5 mb-3">
          <Text className="text-sm text-gray-800 font-quicksand-semibold">
            Danh sách 320 địa điểm
          </Text>
        </View>

        {placeCardData.map((item) => (
          <PlaceCard
            key={item.id}
            title={item.title}
            description={item.description}
            imageUrl={item.imageUrl}
            location={item.location}
            name={item.name}
            avatar={item.avatar}
            createdAt={item.createdAt}
          />
        ))}
      </View>
    </ScrollView>
  );
}
