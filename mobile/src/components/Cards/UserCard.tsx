import React from 'react';
import { View, Text, Image, TouchableOpacity } from 'react-native';
import { FontAwesome } from '@expo/vector-icons';
import Category from '../Category';
import Button from '../Button';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { UserStackParamList } from '../../types/navigation.type';
import { useNavigation } from '@react-navigation/native';

const UserCard = () => {
  type NavigationProp = NativeStackNavigationProp<UserStackParamList, 'Users'>;

  const navigation = useNavigation<NavigationProp>();

  const handlePress = () => {
    navigation.navigate('UserDetail', { id: 'your-id-here' });
  };
  return (
    <TouchableOpacity onPress={handlePress}>
      <View className="bg-white p-4 rounded-lg border border-gray-200 mb-4">
        {/* Header */}
        <View className="flex-row">
          <Image
            source={{ uri: 'https://randomuser.me/api/portraits/men/32.jpg' }}
            className="w-12 h-12 mr-3 rounded-full"
          />
          <View>
            <Text className="font-quicksand-bold text-base text-black">Nguyễn Thiên</Text>
            <Text className="text-sm font-quicksand-medium mt-1 text-gray-500">
              In publishing and graphic design, Lorem ipsum is a placeholder text...
            </Text>
          </View>
        </View>

        {/* Tags */}
        <View className="flex-row mt-3">
          <Category label="Thể loại" />
          <Category label="Thể loại" />
        </View>

        {/* Footer */}
        <View className="flex-row justify-between items-end mt-4">
          <View className="flex-row items-center space-x-2">
            <View className="flex-row items-center space-x-1">
              <FontAwesome name="map-marker" size={14} color="#888" />
              <Text className="text-sm text-gray-600 ml-1 font-quicksand-medium">4.7km</Text>
            </View>
            <Text className="text-sm text-gray-600 mx-1">•</Text>
            <Text className="text-sm text-gray-600 font-quicksand-medium">Đà Nẵng</Text>
          </View>

          <Button label="Thêm bạn" onPress={() => {}} />
        </View>
      </View>
    </TouchableOpacity>
  );
};

export default UserCard;
