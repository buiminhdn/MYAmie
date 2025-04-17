import { createNativeStackNavigator } from '@react-navigation/native-stack';
import PlacesScreen from '../screens/PlaceScreen/PlacesScreen';
import PlaceDetailScreen from '../screens/PlaceScreen/PlaceDetailScreen';
import { PlaceStackParamList } from '../types/navigation.type';

const Stack = createNativeStackNavigator<PlaceStackParamList>();

const PlaceStack = () => (
  <Stack.Navigator>
    <Stack.Screen name="Places" component={PlacesScreen} options={{ headerShown: false }} />
    <Stack.Screen
      name="PlaceDetail"
      component={PlaceDetailScreen}
      options={{
        title: 'Địa điểm',
        headerBackTitle: 'Trở về',
      }}
    />
  </Stack.Navigator>
);

export default PlaceStack;
