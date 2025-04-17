import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, FlatList, Image } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';

type Message = {
  id: string;
  text: string;
  sender: 'me' | 'other';
  time: string;
};

const sampleMessages: Message[] = [
  { id: '1', text: 'Hi there!', sender: 'other', time: '10:00 AM' },
  { id: '2', text: 'Hey! How are you?', sender: 'me', time: '10:01 AM' },
  { id: '3', text: 'Doing well. You?', sender: 'other', time: '10:02 AM' },
  { id: '4', text: 'Pretty good, thanks!', sender: 'me', time: '10:03 AM' },
];

export default function ChatDetailScreen() {
  const navigation = useNavigation();
  const [messages, setMessages] = useState(sampleMessages);
  const [input, setInput] = useState('');

  const sendMessage = () => {
    if (!input.trim()) return;
    const newMessage: Message = {
      id: Date.now().toString(),
      text: input,
      sender: 'me',
      time: 'Now',
    };
    setMessages((prev) => [...prev, newMessage]);
    setInput('');
  };

  const renderItem = ({ item }: { item: Message }) => (
    <View
      className={`my-1 px-4 py-2 max-w-[80%] rounded-xl ${
        item.sender === 'me'
          ? 'bg-primary self-end rounded-br-none'
          : 'bg-gray-200 self-start rounded-bl-none'
      }`}
    >
      <Text
        className={`${item.sender === 'me' ? 'text-white' : 'text-gray-800'} font-quicksand-medium`}
      >
        {item.text}
      </Text>
    </View>
  );

  return (
    <View className="flex-1 bg-white">
      {/* Header */}
      <View className="mt-5 flex-row items-center px-4 py-3 border-b border-gray-200">
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Ionicons name="chevron-back" size={24} color="#000" />
        </TouchableOpacity>
        <Image
          source={{ uri: 'https://randomuser.me/api/portraits/women/1.jpg' }}
          className="w-10 h-10 rounded-full ml-3"
        />
        <Text className="ml-3 text-base font-quicksand-semibold">Anna</Text>
      </View>

      {/* Messages */}
      <FlatList
        data={messages}
        keyExtractor={(item) => item.id}
        renderItem={renderItem}
        contentContainerStyle={{ paddingHorizontal: 16, paddingVertical: 8 }}
        className="flex-1"
        showsVerticalScrollIndicator={false}
      />

      {/* Message Input */}
      <View className="flex-row items-center border-t border-gray-200 p-3">
        <TextInput
          value={input}
          onChangeText={setInput}
          placeholder="Tin nhắn..."
          className="flex-1 bg-gray-100 rounded-full px-4 py-2 mr-2 font-quicksand-medium"
        />
        <TouchableOpacity onPress={sendMessage} className="bg-primary rounded-full p-2">
          <Ionicons name="send" size={20} color="#fff" />
        </TouchableOpacity>
      </View>
    </View>
  );
}
