import { createNativeStackNavigator } from '@react-navigation/native-stack';
import BusinessesScreen from '../screens/BusinessScreen/BusinessesScreen';
import BusinessDetailScreen from '../screens/BusinessScreen/BusinessDetailScreen';
import { BusinessStackParamList } from '../types/navigation.type';

const Stack = createNativeStackNavigator<BusinessStackParamList>();

const BusinessStack = () => (
  <Stack.Navigator>
    <Stack.Screen name="Businesses" component={BusinessesScreen} options={{ headerShown: false }} />
    <Stack.Screen
      name="BusinessDetail"
      component={BusinessDetailScreen}
      options={{
        title: 'Dịch vụ',
        headerBackTitle: 'Trở về',
      }}
    />
  </Stack.Navigator>
);

export default BusinessStack;
