import React from 'react';
import { Text, View } from 'react-native';

interface CategoryProps {
  label: string;
}

const Category: React.FC<CategoryProps> = ({ label }) => {
  return (
    <View className="mr-2 px-3 py-1 rounded-full border-2 border-primary">
      <Text className="text-xs text-gray-600 font-quicksand-medium">{label}</Text>
    </View>
  );
};

export default Category;
