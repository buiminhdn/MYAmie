export interface ConversationVM {
  id: number;
  name: string;
  avatar: string;
  content: string;
  sentAt: string;
  senderId: number;
}

export interface PagedConversationsVM {
  conversations: ConversationVM[];
  hasMore: boolean;
  pageNumber: number;
}

export interface MessageVM {
  id: number;
  content: string;
  sentAt: string;
  status: string;
  senderId: number;
  receiverId: number;
}

export interface PagedMessagesVM {
  messages: MessageVM[];
  hasMore: boolean;
  pageNumber: number;
}
