export interface ENV {
  apiEndPoint: string;
  apiChatEndPoint: string;
}
const env: ENV = {
  apiEndPoint: import.meta.env.VITE_BASE_URL || "",
  apiChatEndPoint: import.meta.env.VITE_CHAT_URL || "",
};

export default env;
