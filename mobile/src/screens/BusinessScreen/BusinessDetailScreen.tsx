import React, { useState, useEffect } from 'react';
import { View, Text, Image, ScrollView, TouchableOpacity } from 'react-native';
import { FontAwesome, Entypo } from '@expo/vector-icons';
import { BusinessDetail } from '../../types/business.type';

const sampleBusiness: BusinessDetail = {
  name: 'GIADUNG',
  description:
    'Lorem Ipsum is a dummy or placeholder text commonly used in graphic design, publishing, and web development.',
  tags: ['Thể thao', 'Làm đẹp', 'Ăn uống'],
  contact: {
    phone: '0123456789',
    email: 'contact@giadung.com',
    hours: '08:00 - 18:00',
  },
  coverImage:
    'https://t4.ftcdn.net/jpg/01/16/61/93/360_F_116619399_YA611bKNOW35ffK0OiyuaOcjAgXgKBui.jpg',
  detailImages: [
    'https://static.vecteezy.com/system/resources/thumbnails/025/282/026/small/stock-of-mix-a-cup-coffee-latte-more-motive-top-view-foodgraphy-generative-ai-photo.jpg',
    'https://upload.wikimedia.org/wikipedia/commons/e/e4/Latte_and_dark_coffee.jpg',
  ],
};

const BusinessDetailScreen = () => {
  const [selectedTab, setSelectedTab] = useState<'description' | 'images'>('description');
  const [business, setBusiness] = useState<BusinessDetail | null>(null);

  useEffect(() => {
    // Simulate fetch with delay
    const fetchBusiness = async () => {
      await new Promise((resolve) => setTimeout(resolve, 500)); // Fake delay
      setBusiness(sampleBusiness);
    };
    fetchBusiness();
  }, []);

  if (!business) {
    return (
      <View className="flex-1 items-center justify-center bg-white">
        <Text className="text-gray-500 font-quicksand-semibold">Đang tải...</Text>
      </View>
    );
  }

  return (
    <ScrollView className="flex-1 bg-white">
      {/* Cover Image */}
      <Image source={{ uri: business.coverImage }} className="w-full h-52" resizeMode="cover" />

      <View className="p-4 mb-20">
        {/* Title */}
        <Text className="text-center text-2xl font-quicksand-bold text-gray-900 mb-2">
          {business.name}
        </Text>
        <Text className="text-sm font-quicksand-semibold text-gray-500 text-center mb-4">
          {business.description}
        </Text>

        {/* Tags */}
        <View className="flex-row justify-center flex-wrap gap-2 mb-4">
          {business.tags.map((tag, index) => (
            <View key={index} className="px-3 py-1 rounded-full border border-gray-300">
              <Text className="text-sm text-gray-600 font-quicksand-medium">{tag}</Text>
            </View>
          ))}
        </View>

        {/* Contact Info */}
        <View className="bg-white p-4 rounded-lg border border-gray-200 mb-4">
          <Text className="text-xs text-gray-400 mb-2 font-quicksand-bold">THÔNG TIN LIÊN HỆ</Text>

          <View className="space-y-2">
            <View className="flex-row items-center mt-1">
              <FontAwesome name="phone" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-2 font-quicksand-medium">
                {business.contact.phone}
              </Text>
            </View>
            <View className="flex-row items-center mt-1">
              <FontAwesome name="envelope" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-2 font-quicksand-medium">
                {business.contact.email}
              </Text>
            </View>
            <View className="flex-row items-center mt-1">
              <Entypo name="clock" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-2 font-quicksand-medium">
                {business.contact.hours}
              </Text>
            </View>
          </View>
        </View>

        {/* Tabs */}
        <View className="flex-row mb-4">
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${
              selectedTab === 'description'
                ? 'border-b-2 border-primary'
                : 'border-b border-gray-200'
            }`}
            onPress={() => setSelectedTab('description')}
          >
            <Text
              className={`font-quicksand-semibold ${
                selectedTab === 'description' ? 'text-primary' : 'text-gray-400'
              }`}
            >
              Mô tả
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${
              selectedTab === 'images' ? 'border-b-2 border-primary' : 'border-b border-gray-200'
            }`}
            onPress={() => setSelectedTab('images')}
          >
            <Text
              className={`font-quicksand-semibold ${
                selectedTab === 'images' ? 'text-primary' : 'text-gray-400'
              }`}
            >
              Ảnh chi tiết
            </Text>
          </TouchableOpacity>
        </View>

        {/* Conditional Content Rendering */}
        {selectedTab === 'description' ? (
          <Text className="text-sm text-gray-700 font-quicksand-medium">
            Lorem Ipsum is simply dummy text of the printing and typesetting industry...
          </Text>
        ) : (
          <View className="space-y-4">
            {business.detailImages.map((imageUrl, index) => (
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

export default BusinessDetailScreen;
