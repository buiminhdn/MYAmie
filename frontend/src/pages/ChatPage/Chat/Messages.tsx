import { useCallback, useEffect, useState } from "react";
import toast from "react-hot-toast";
import { useQueryClient } from "@tanstack/react-query";
import { MessageVM } from "@/models/viewmodels/chat.vm";
import useSignalRConnection from "@/hooks/useSignalRConnection";
import { useGetMessages } from "@/services/chat.service";
import {
  extractChatErrorMessage,
  isMessageInCurrentChat,
} from "@/utils/messageUtils";
import ChatInput from "../components/MessageInput";

interface MessageProps {
  currentUserId: number;
  currentUserAvatar?: string;
  currentUserName?: string;
  otherUserId: number;
  otherUserName?: string;
  otherUserAvatar?: string;
}

function Messages({ currentUserId, otherUserId }: MessageProps) {
  const [messages, setMessages] = useState<MessageVM[]>([]);
  const queryClient = useQueryClient();
  const connection = useSignalRConnection(currentUserId, otherUserId);

  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, refetch } =
    useGetMessages(otherUserId!);

  // Handle switching between users
  const handleUserChange = useCallback(async () => {
    queryClient.removeQueries({ queryKey: ["messages", otherUserId] });

    try {
      const { data } = await refetch();
      if (data?.pages) {
        setMessages(data.pages.flatMap((page) => page.data?.messages || []));
      }
    } catch (error) {
      toast.error("Lỗi khi tải tin nhắn, thử lại sau");
    }
  }, [otherUserId, refetch, queryClient]);

  // Refetch and reset state when user changes
  useEffect(() => {
    handleUserChange();
  }, [handleUserChange]);

  // Handle receiving new messages
  const handleReceiveMessage = useCallback(
    (message: MessageVM) => {
      if (isMessageInCurrentChat(message, currentUserId, otherUserId)) {
        setMessages((prevMessages) => {
          if (!prevMessages.some((msg) => msg.id === message.id)) {
            return [...prevMessages, message];
          }
          return prevMessages;
        });
        queryClient.invalidateQueries({ queryKey: ["conversations"] });
      }
    },
    [currentUserId, otherUserId]
  );

  useEffect(() => {
    if (!connection) return;

    connection.on("ReceiveMessage", handleReceiveMessage);

    return () => {
      connection.off("ReceiveMessage", handleReceiveMessage);
    };
  }, [connection, handleReceiveMessage]);

  useEffect(() => {
    if (data?.pages) {
      setMessages((prevMessages) => {
        const olderMessages = data.pages.flatMap(
          (page) => page.data?.messages || []
        );
        return [
          ...olderMessages.filter(
            (msg) =>
              !prevMessages.some((existingMsg) => existingMsg.id === msg.id)
          ),
          ...prevMessages,
        ];
      });
    }
  }, [data]);

  // Send message
  const sendMessage = async (content: string) => {
    if (!connection || !content.trim() || !otherUserId) {
      toast.error("Nội dung tin nhắn không được để trống");
      return;
    }

    try {
      const newMessage: MessageVM = await connection.invoke(
        "SendMessage",
        otherUserId,
        content
      );
      setMessages((prevMessages) => [...prevMessages, newMessage]);
      queryClient.invalidateQueries({ queryKey: ["conversations"] });
    } catch (err) {
      toast.error(extractChatErrorMessage(err));
    }
  };

  const loadMoreMessages = () => {
    if (hasNextPage && !isFetchingNextPage) {
      fetchNextPage();
    }
  };

  return (
    <>
      <div className="h-full overflow-y-auto flex flex-col-reverse">
        <div className="p-2 space-y-2">
          {messages.map((msg) => (
            <div
              key={msg.id}
              className={`group relative p-2 rounded-xl w-fit max-w-[37rem] ${
                msg.senderId === currentUserId
                  ? "ml-auto bg-primary-lighter"
                  : "bg-gray-200"
              }`}
            >
              <p data-testid="message-item">{msg.content}</p>
              <p className="hidden group-hover:block absolute right-0 bg-black bg-opacity-65 rounded-xl text-xs p-2 text-white -top-9 w-fit">
                {msg.sentAt}
              </p>
            </div>
          ))}
        </div>
        {hasNextPage && (
          <p
            className="text-xs mx-auto hover:underline mt-5 text-gray-500 hover:text-gray-700 font-medium hover:cursor-pointer"
            onClick={loadMoreMessages}
          >
            {isFetchingNextPage ? "Đang tải..." : "Tải thêm tin"}
          </p>
        )}
      </div>
      <ChatInput onSendMessage={sendMessage} />
    </>
  );
}

export default Messages;
