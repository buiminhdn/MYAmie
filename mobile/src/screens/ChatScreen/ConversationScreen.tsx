import React, { useState } from 'react';
import { View, Text, TextInput, FlatList, Image, TouchableOpacity } from 'react-native';
import { Conversation } from '../../types/chat.type';
import { useNavigation } from '@react-navigation/native';
import { ChatStackParamList } from '../../types/navigation.type';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';

const sampleConversations: Conversation[] = [
  {
    id: '1',
    name: 'Anna',
    lastMessage: 'See you tomorrow!',
    avatar: 'https://randomuser.me/api/portraits/women/1.jpg',
    time: '2h ago',
    isRead: false,
  },
  {
    id: '2',
    name: 'Ben',
    lastMessage: 'Got it, thanks!',
    avatar: 'https://randomuser.me/api/portraits/men/2.jpg',
    time: '1d ago',
    isRead: true,
  },
  {
    id: '3',
    name: 'Cindy',
    lastMessage: 'Let’s meet at 5pm.',
    avatar: 'https://randomuser.me/api/portraits/women/3.jpg',
    time: '3d ago',
    isRead: false,
  },
];

export default function ConversationScreen() {
  const [search, setSearch] = useState('');
  type NavigationProp = NativeStackNavigationProp<ChatStackParamList>;

  const navigation = useNavigation<NavigationProp>();

  const filteredConversations = sampleConversations.filter((conv) =>
    conv.name.toLowerCase().includes(search.toLowerCase()),
  );

  const renderItem = ({ item }: { item: Conversation }) => (
    <TouchableOpacity
      className="flex-row items-center py-4 border-b border-gray-200"
      onPress={() => navigation.navigate('ChatDetail', { otherUserId: item.id })}
    >
      <Image source={{ uri: item.avatar }} className="w-14 h-14 rounded-full mr-3" />
      <View className="flex-1 pb-3">
        <View className="flex-row justify-between mb-1">
          <Text
            className={`text-base font-quicksand-semibold ${
              item.isRead ? 'text-gray-500' : 'text-black font-bold'
            }`}
          >
            {item.name}
          </Text>
          <Text className="text-xs text-gray-400">{item.time}</Text>
        </View>
        <Text
          className={`text-sm ${item.isRead ? 'text-gray-500' : 'text-black font-semibold'}`}
          numberOfLines={1}
        >
          {item.lastMessage}
        </Text>
      </View>
    </TouchableOpacity>
  );

  return (
    <View className="flex-1 bg-white px-4 pt-14">
      <Text className="text-lg text-center font-quicksand-bold mb-3">Tin nhắn</Text>

      <TextInput
        placeholder="Tìm kiếm..."
        className="bg-gray-100 rounded-lg p-4 font-quicksand-medium"
        value={search}
        onChangeText={setSearch}
      />

      <FlatList
        data={filteredConversations}
        keyExtractor={(item) => item.id}
        renderItem={renderItem}
        contentContainerStyle={{ paddingBottom: 20 }}
      />
    </View>
  );
}
