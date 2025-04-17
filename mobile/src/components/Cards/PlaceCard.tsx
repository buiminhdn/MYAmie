import * as React from 'react';
import { View, Text, Image, TouchableOpacity } from 'react-native';
import { Entypo } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { PlaceStackParamList } from '../../types/navigation.type';

interface PlaceCardProps {
  id: string;
  title: string;
  description: string;
  imageUrl: string;
  location: string;
  name: string;
  avatar: string;
  createdAt: string;
}

const PlaceCard: React.FC<PlaceCardProps> = ({
  id,
  title,
  description,
  imageUrl,
  location,
  name,
  avatar,
  createdAt,
}) => {
  type NavigationProp = NativeStackNavigationProp<PlaceStackParamList, 'Places'>;

  const navigation = useNavigation<NavigationProp>();

  const handlePress = () => {
    navigation.navigate('PlaceDetail', { id: 'your-id-here' });
  };
  return (
    <TouchableOpacity onPress={handlePress}>
      <View className="flex-row bg-white rounded-lg mb-4 border border-gray-200 overflow-hidden">
        {/* Place image */}
        <Image source={{ uri: imageUrl }} className="w-32 h-full" resizeMode="cover" />

        {/* Content */}
        <View className="flex-1 p-3 justify-between">
          {/* Title */}
          <Text className="text-lg font-quicksand-bold text-gray-900">{title}</Text>

          {/* Description */}
          <Text className="text-sm font-quicksand-medium text-gray-600 mb-3 mt-1" numberOfLines={2}>
            {description}
          </Text>

          {/* Divider */}
          <View className="h-px bg-gray-200 mb-2" />

          {/* Location */}
          <View className="flex-row items-center mb-2">
            <Entypo name="location-pin" size={14} color="#6B7280" className="mr-1" />
            <Text className="text-sm font-quicksand-medium text-gray-500">{location}</Text>
          </View>

          {/* User info */}
          <View className="flex-row items-center">
            <Image source={{ uri: avatar }} className="w-8 h-8 rounded-full mr-2" />
            <Text className="text-sm font-quicksand-semibold text-gray-500">{name}</Text>
            <Text className="text-sm font-quicksand-medium text-gray-400 ml-2">• {createdAt}</Text>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );
};

export default PlaceCard;
