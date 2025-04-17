import { createNativeStackNavigator } from '@react-navigation/native-stack';
import MenuScreen from '../screens/ProfileScreen/MenuScreen';
import AccountScreen from '../screens/ProfileScreen/AccountScreen';
import { AccountStackParamList } from '../types/navigation.type';

const Stack = createNativeStackNavigator<AccountStackParamList>();

const AccountStack = () => (
  <Stack.Navigator>
    <Stack.Screen name="Menu" component={MenuScreen} options={{ headerShown: false }} />
    {/* <Stack.Screen
      name="Info"
      component={InfoScreen}
      options={{
        title: 'Thông tin cá nhân',
        headerBackTitle: 'Trở về',
      }}
    /> */}
    <Stack.Screen
      name="Account"
      component={AccountScreen}
      options={{
        title: 'Tài khoản',
        headerBackTitle: 'Trở về',
      }}
    />
  </Stack.Navigator>
);

export default AccountStack;
