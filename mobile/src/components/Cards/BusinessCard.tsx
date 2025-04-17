import * as React from 'react';
import { View, Text, Image, TouchableOpacity } from 'react-native';
import { FontAwesome5, MaterialIcons, Entypo } from '@expo/vector-icons';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { BusinessStackParamList } from '../../types/navigation.type';
import { useNavigation } from '@react-navigation/native';

interface BusinessCardProps {
  title: string;
  description: string;
  imageUrl: string;
  rating: string;
  location: string;
  hours: string;
}

const BusinessCard: React.FC<BusinessCardProps> = ({
  title,
  description,
  imageUrl,
  rating,
  location,
  hours,
}) => {
  type NavigationProp = NativeStackNavigationProp<BusinessStackParamList, 'Businesses'>;

  const navigation = useNavigation<NavigationProp>();

  const handlePress = () => {
    navigation.navigate('BusinessDetail', { id: 'your-id-here' });
  };

  return (
    <TouchableOpacity onPress={handlePress}>
      <View className="flex-row bg-white rounded-lg mb-4 border border-gray-200">
        {/* Business image */}
        <Image source={{ uri: imageUrl }} className="w-32 h-full rounded-l-lg" />

        {/* Content */}
        <View className="flex-1 p-3">
          {/* Title */}
          <Text className="text-lg font-quicksand-bold text-gray-900 mb-1">{title}</Text>

          {/* Description */}
          <Text className="text-sm font-quicksand-medium text-gray-600 mb-2" numberOfLines={2}>
            {description}
          </Text>

          {/* Rating */}
          <View className="flex-row items-center mb-2">
            <FontAwesome5 name="star" size={12} color="#FBBF24" className="mr-1" />
            <Text className="text-sm font-quicksand-medium text-gray-500">{rating} Đánh giá</Text>
          </View>

          {/* Divider */}
          <View className="h-px bg-gray-200 mb-2" />

          {/* Location & Hours */}
          <View className="flex-row justify-between">
            <View className="flex-row items-center">
              <Entypo name="location-pin" size={14} color="#6B7280" className="mr-1" />
              <Text className="text-sm font-quicksand-medium text-gray-500">{location}</Text>
            </View>

            <View className="flex-row items-center">
              <MaterialIcons name="access-time" size={14} color="#6B7280" className="mr-1" />
              <Text className="text-sm font-quicksand-medium text-gray-500">{hours}</Text>
            </View>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );
};

export default BusinessCard;
