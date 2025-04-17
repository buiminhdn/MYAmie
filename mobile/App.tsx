import { StatusBar } from 'expo-status-bar';
import './global.css';
import { useFonts } from 'expo-font';
import { NavigationContainer } from '@react-navigation/native';
import React from 'react';
import BottomTabs from './src/navigation/BottomTabs';
import RootNavigator from './src/routes/RootNavigator';
import { AuthProvider } from './src/Context/AuthContext';

export default function App() {
  const [fontsLoaded] = useFonts({
    'quicksand-medium': require('./src/assets/fonts/Quicksand-Medium.ttf'),
    'quicksand-semibold': require('./src/assets/fonts/Quicksand-SemiBold.ttf'),
    'quicksand-bold': require('./src/assets/fonts/Quicksand-Bold.ttf'),
  });

  if (!fontsLoaded) return null;

  return (
    <AuthProvider>
      <NavigationContainer>
        <StatusBar style="auto" />
        <RootNavigator />
      </NavigationContainer>
    </AuthProvider>
  );
}
