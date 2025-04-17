import { createNativeStackNavigator } from '@react-navigation/native-stack';
import ConversationScreen from '../screens/ChatScreen/ConversationScreen';
import MessageScreen from '../screens/ChatScreen/ChatDetailScreen';
import { ChatStackParamList } from '../types/navigation.type';

const Stack = createNativeStackNavigator<ChatStackParamList>();

const ChatStack = () => (
  <Stack.Navigator>
    <Stack.Screen
      name="Conversations"
      component={ConversationScreen}
      options={{ headerShown: false }}
    />
    <Stack.Screen name="ChatDetail" component={MessageScreen} options={{ headerShown: false }} />
  </Stack.Navigator>
);

export default ChatStack;
