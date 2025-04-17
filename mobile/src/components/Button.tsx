import React from 'react';
import { Text, TouchableOpacity, TouchableOpacityProps } from 'react-native';

interface ButtonProps extends TouchableOpacityProps {
  label: string;
}

const Button: React.FC<ButtonProps> = ({ label, ...props }) => {
  return (
    <TouchableOpacity className="px-4 py-2 rounded-lg border-2 border-primary" {...props}>
      <Text className="text-primary text-sm font-quicksand-semibold">{label}</Text>
    </TouchableOpacity>
  );
};

export default Button;
