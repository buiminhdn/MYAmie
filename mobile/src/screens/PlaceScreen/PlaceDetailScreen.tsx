import React, { useEffect, useState } from 'react';
import { View, Text, Image, ScrollView, TouchableOpacity } from 'react-native';
import { FontAwesome, Entypo } from '@expo/vector-icons';
import { PlaceDetail } from '../../types/place.type';

const PlaceDetailScreen = () => {
  const [selectedTab, setSelectedTab] = useState<'description' | 'images'>('description');
  const [place, setPlace] = useState<PlaceDetail | null>(null);

  useEffect(() => {
    const fetchPlace = async () => {
      // Simulate network delay
      await new Promise((resolve) => setTimeout(resolve, 1000));

      const samplePlace: PlaceDetail = {
        id: 1,
        title: 'GIADUNG',
        description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry...',
        shortIntro:
          'Lorem Ipsum is a dummy or placeholder text commonly used in graphic design, publishing, and web development.',
        tags: ['Thể thao', 'Làm đẹp', 'Ăn uống'],
        user: {
          name: 'Bùi Minh',
          avatar: 'https://randomuser.me/api/portraits/men/75.jpg',
          postedAt: '04/09/2025 02:07:14',
        },
        location: '78 Lê Tấn Trung',
        city: 'Đà Nẵng',
        images: [
          'https://static.vecteezy.com/system/resources/thumbnails/025/282/026/small/stock-of-mix-a-cup-coffee-latte-more-motive-top-view-foodgraphy-generative-ai-photo.jpg',
          'https://upload.wikimedia.org/wikipedia/commons/e/e4/Latte_and_dark_coffee.jpg',
        ],
      };

      setPlace(samplePlace);
    };

    fetchPlace();
  }, []);

  if (!place) return null;

  return (
    <ScrollView className="flex-1 bg-white">
      {/* Cover Image */}
      <Image
        source={{
          uri: 'https://t4.ftcdn.net/jpg/01/16/61/93/360_F_116619399_YA611bKNOW35ffK0OiyuaOcjAgXgKBui.jpg',
        }}
        className="w-full h-52"
        resizeMode="cover"
      />

      <View className="p-4 mb-20">
        {/* Title */}
        <Text className="text-center text-2xl font-quicksand-bold text-gray-900 mb-2">
          {place.title}
        </Text>
        <Text className="text-sm font-quicksand-semibold text-gray-500 text-center mb-4">
          {place.shortIntro}
        </Text>

        {/* Tags */}
        <View className="flex-row justify-center flex-wrap gap-2 mb-4">
          {place.tags.map((tag, index) => (
            <View key={index} className="px-3 py-1 rounded-full border border-gray-300">
              <Text className="text-sm text-gray-600 font-quicksand-medium">{tag}</Text>
            </View>
          ))}
        </View>

        {/* User Info */}
        <View className="bg-white p-4 rounded-lg border border-gray-200 mb-4">
          <Text className="text-xs text-gray-400 mb-2 font-quicksand-bold">NGƯỜI ĐĂNG</Text>
          <View className="flex-row items-center mb-3">
            <Image source={{ uri: place.user.avatar }} className="w-10 h-10 rounded-full mr-2" />
            <View>
              <Text className="font-quicksand-bold text-gray-800">{place.user.name}</Text>
              <Text className="text-xs font-quicksand-medium text-gray-500">
                {place.user.postedAt}
              </Text>
            </View>
          </View>

          <View className="border-t border-gray-100 my-2" />

          <Text className="text-xs text-gray-400 font-quicksand-bold mb-2">THÔNG TIN CHI TIẾT</Text>
          <View className="space-y-2">
            <View className="flex-row items-center mt-1">
              <Entypo name="location-pin" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-1 font-quicksand-medium">
                {place.location}
              </Text>
            </View>
            <View className="flex-row items-center mt-1">
              <Entypo name="location-pin" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-1 font-quicksand-medium">{place.city}</Text>
            </View>
          </View>
        </View>

        {/* Tabs */}
        <View className="flex-row mb-4">
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${selectedTab === 'description' ? 'border-b-2 border-primary' : 'border-b border-gray-200'}`}
            onPress={() => setSelectedTab('description')}
          >
            <Text
              className={`font-quicksand-semibold ${selectedTab === 'description' ? 'text-primary' : 'text-gray-400'}`}
            >
              Mô tả
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${selectedTab === 'images' ? 'border-b-2 border-primary' : 'border-b border-gray-200'}`}
            onPress={() => setSelectedTab('images')}
          >
            <Text
              className={`font-quicksand-semibold ${selectedTab === 'images' ? 'text-primary' : 'text-gray-400'}`}
            >
              Ảnh chi tiết
            </Text>
          </TouchableOpacity>
        </View>

        {/* Content */}
        {selectedTab === 'description' ? (
          <Text className="text-sm text-gray-700 font-quicksand-medium">{place.description}</Text>
        ) : (
          <View className="space-y-4">
            {place.images.map((imageUrl, index) => (
              <Image
                key={index}
                source={{ uri: imageUrl }}
                className="w-full h-52 rounded-lg mb-3"
                resizeMode="cover"
              />
            ))}
          </View>
        )}
      </View>
    </ScrollView>
  );
};

export default PlaceDetailScreen;
