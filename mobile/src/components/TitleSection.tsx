import * as React from 'react';
import { View, Text } from 'react-native';

interface TitleSectionProps {
  smallTitle: string;
  bigTitle: string;
}

const TitleSection: React.FC<TitleSectionProps> = ({ smallTitle, bigTitle }) => {
  return (
    <View className="mb-4 items-center">
      <View className="mb-1 bg-light px-4 py-2 rounded-full">
        <Text className="text-xs font-quicksand-semibold">{smallTitle}</Text>
      </View>
      <Text className="text-2xl font-quicksand-bold text-primary mt-2">{bigTitle}</Text>
    </View>
  );
};

export default TitleSection;
