import * as React from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet } from 'react-native';
import { FontAwesome5 } from '@expo/vector-icons';

const SearchBar: React.FC = () => {
  return (
    <View className="flex-row items-center justify-between mt-2">
      <View
        style={styles.searchContainer}
        className="flex-row items-center bg-white rounded-lg p-2 flex-1"
      >
        {/* Search icon with spacing */}
        <View className="pl-2 pr-2">
          <FontAwesome5 name="search" size={16} color="#6B7280" />
        </View>

        {/* Vertical divider */}
        <View className="h-full w-px bg-gray-300 mr-2" />

        {/* Input field */}
        <TextInput
          placeholder="Nhập từ khoá tìm kiếm tại đây"
          className="flex-1 text-gray-700 text-base font-quicksand-medium"
          placeholderTextColor="#9CA3AF"
        />

        {/* Filter button */}
        <TouchableOpacity className="flex-row items-center bg-lighter rounded-lg p-3 ml-2">
          <FontAwesome5 name="filter" size={14} color="#374151" className="mr-1" />
          <Text className="text-gray-700 text-sm font-quicksand-medium">Bộ lọc</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  searchContainer: {
    borderWidth: 1,
    borderColor: '#D1D5DB', // gray-300
  },
});

export default SearchBar;
