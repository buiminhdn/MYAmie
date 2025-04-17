import React, { useEffect, useState } from 'react';
import { View, Text, Image, ScrollView, TouchableOpacity, ActivityIndicator } from 'react-native';
import { Entypo } from '@expo/vector-icons';
import { UserDetail } from '../../types/user.type';

const UserDetailScreen = () => {
  const [user, setUser] = useState<UserDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedTab, setSelectedTab] = useState<'description' | 'images'>('description');

  useEffect(() => {
    const fetchUser = async () => {
      try {
        // Simulated network delay
        await new Promise((resolve) => setTimeout(resolve, 1000));

        const sampleUser: UserDetail = {
          id: '1',
          name: 'Trần Mai Hương',
          intro: 'Tìm bạn đi cà phê, tâm sự chuyện cuộc sống và chia sẻ những điều nhỏ bé.',
          description:
            'Mình là người sống nội tâm, thích lắng nghe và đồng hành cùng người khác qua những câu chuyện đời thường. Rất mong được làm quen với những bạn có cùng suy nghĩ!',
          location: 'Đà Nẵng',
          tags: ['Thể thao', 'Làm đẹp', 'Ăn uống'],
          personalityTraits: ['Hướng nội', 'Tình cảm', 'Lắng nghe', 'Hài hước'],
          images: [
            'https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=800&q=60',
            'https://images.unsplash.com/photo-1607746882042-944635dfe10e?auto=format&fit=crop&w=800&q=60',
            'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=800&q=60',
          ],
        };

        setUser(sampleUser);
      } catch (error) {
        console.error('Failed to load user data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []);

  if (loading) {
    return (
      <View className="flex-1 justify-center items-center">
        <ActivityIndicator size="large" color="#666" />
      </View>
    );
  }

  if (!user) {
    return (
      <View className="flex-1 justify-center items-center">
        <Text className="text-gray-500 font-quicksand-semibold">Không tìm thấy người dùng.</Text>
      </View>
    );
  }

  return (
    <ScrollView className="flex-1 bg-white">
      <Image source={{ uri: user.images[0] }} className="w-full h-52" resizeMode="cover" />

      <View className="p-4 mb-20">
        <Text className="text-center text-2xl font-quicksand-bold text-gray-900 mb-2">
          {user.name}
        </Text>
        <Text className="text-sm font-quicksand-semibold text-gray-500 text-center mb-4">
          {user.intro}
        </Text>

        <View className="flex-row justify-center flex-wrap gap-2 mb-4">
          {user.tags.map((tag, index) => (
            <View key={index} className="px-3 py-1 rounded-full border border-gray-300">
              <Text className="text-sm text-gray-600 font-quicksand-medium">{tag}</Text>
            </View>
          ))}
        </View>

        <View className="bg-white p-4 rounded-lg border border-gray-200 mb-4">
          <Text className="text-xs text-gray-400 font-quicksand-bold mb-2">THÔNG TIN CHI TIẾT</Text>
          <View className="space-y-2">
            <View className="flex-row items-center mt-1">
              <Entypo name="location-pin" size={16} color="#888" />
              <Text className="text-sm text-gray-700 ml-1 font-quicksand-medium">
                {user.location}
              </Text>
            </View>
          </View>
          <View className="border-t border-gray-100 my-2" />
          <View className="flex-row mt-2 flex-wrap gap-2">
            {user.personalityTraits.map((trait, index) => (
              <View key={index} className="px-3 py-1 rounded-full border border-gray-300">
                <Text className="text-sm text-gray-600 font-quicksand-medium">{trait}</Text>
              </View>
            ))}
          </View>
        </View>

        <View className="flex-row mb-4">
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${selectedTab === 'description' ? 'border-b-2 border-primary' : 'border-b border-gray-200'}`}
            onPress={() => setSelectedTab('description')}
          >
            <Text
              className={`font-quicksand-semibold ${selectedTab === 'description' ? 'text-primary' : 'text-gray-400'}`}
            >
              Giới thiệu
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            className={`flex-1 items-center pb-2 ${selectedTab === 'images' ? 'border-b-2 border-primary' : 'border-b border-gray-200'}`}
            onPress={() => setSelectedTab('images')}
          >
            <Text
              className={`font-quicksand-semibold ${selectedTab === 'images' ? 'text-primary' : 'text-gray-400'}`}
            >
              Hình ảnh
            </Text>
          </TouchableOpacity>
        </View>

        {selectedTab === 'description' ? (
          <Text className="text-sm text-gray-700 font-quicksand-medium">{user.description}</Text>
        ) : (
          <View className="space-y-4">
            {user.images.slice(1).map((url, idx) => (
              <Image
                key={idx}
                source={{ uri: url }}
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

export default UserDetailScreen;
