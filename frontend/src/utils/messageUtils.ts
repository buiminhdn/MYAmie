import { MessageVM } from "@/models/viewmodels/chat.vm";

/**
 * Checks if the message is part of the current chat.
 */
export const isMessageInCurrentChat = (
  message: MessageVM,
  currentUserId: number,
  otherUserId: number
): boolean => {
  return (
    (message.senderId === otherUserId &&
      message.receiverId === currentUserId) ||
    (message.receiverId === otherUserId && message.senderId === currentUserId)
  );
};

export const extractChatErrorMessage = (err: unknown): string => {
  if (err instanceof Error && err.message) {
    const match = /HubException: (.*)$/.exec(err.message);
    return match?.[1] || err.message;
  }
  return "An unexpected error occurred.";
};
