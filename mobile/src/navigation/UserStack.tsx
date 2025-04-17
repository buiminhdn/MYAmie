import { createNativeStackNavigator } from '@react-navigation/native-stack';
import UsersScreen from '../screens/UserScreen/UsersScreen';
import UserDetailScreen from '../screens/UserScreen/UserDetailScreen';
import { UserStackParamList } from '../types/navigation.type';

const Stack = createNativeStackNavigator<UserStackParamList>();

const UserStack = () => (
  <Stack.Navigator>
    <Stack.Screen name="Users" component={UsersScreen} options={{ headerShown: false }} />
    <Stack.Screen
      name="UserDetail"
      component={UserDetailScreen}
      options={{
        title: 'Người dùng',
        headerBackTitle: 'Trở về',
      }}
    />
  </Stack.Navigator>
);

export default UserStack;
