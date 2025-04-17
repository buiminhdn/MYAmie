import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { FontAwesome5 } from '@expo/vector-icons';
import React from 'react';

import PlaceStack from './PlaceStack';
import ConversationScreen from '../screens/ChatScreen/ConversationScreen';
import MenuScreen from '../screens/ProfileScreen/MenuScreen';
import BusinessStack from './BusinessStack';
import UserStack from './UserStack';
import ChatStack from './ChatStack';
import AccountStack from './AccountStack';

const Tab = createBottomTabNavigator();

const BottomTabs = () => {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ color }) => {
          let iconName = 'home';
          switch (route.name) {
            case 'Trang chủ':
              iconName = 'home';
              break;
            case 'Địa điểm':
              iconName = 'map-marker-alt';
              break;
            case 'Bạn bè':
              iconName = 'user-friends';
              break;
            case 'Chat':
              iconName = 'comment';
              break;
            case 'Menu':
              iconName = 'bars';
              break;
          }
          return <FontAwesome5 name={iconName} size={18} color={color} />;
        },
        tabBarActiveTintColor: '#3F6189',
        tabBarInactiveTintColor: '#9CA3AF',
        tabBarStyle: {
          height: 60,
          borderTopWidth: 0,
          backgroundColor: '#fff',
          shadowColor: '#000',
          shadowOffset: { width: 0, height: -2 },
          shadowOpacity: 0.05,
          shadowRadius: 8,
          elevation: 3,
        },
        headerShown: false,
      })}
    >
      <Tab.Screen name="Trang chủ" component={BusinessStack} />
      <Tab.Screen name="Địa điểm" component={PlaceStack} />
      <Tab.Screen name="Bạn bè" component={UserStack} />
      <Tab.Screen name="Chat" component={ChatStack} />
      <Tab.Screen name="Menu" component={AccountStack} />
    </Tab.Navigator>
  );
};

export default BottomTabs;
