import useClickOutside from "@/hooks/useClickOutside";
import EmojiPicker, { EmojiClickData } from "emoji-picker-react";
import { useCallback, useRef, useState } from "react";
import toast from "react-hot-toast";

interface ChatInputProps {
  onSendMessage: (message: string) => void;
}

function ChatInput({ onSendMessage }: ChatInputProps) {
  const [showEmojiPicker, setShowEmojiPicker] = useState(false);
  const [message, setMessage] = useState("");
  const dropdownRef = useClickOutside(() =>
    setShowEmojiPicker(false)
  ) as React.RefObject<HTMLDivElement>;
  const textareaRef = useRef<HTMLTextAreaElement>(null);

  const toggleEmojiPicker = useCallback(() => {
    setShowEmojiPicker((prev) => !prev);
  }, []);

  const handleEmojiClick = useCallback((emojiData: EmojiClickData) => {
    setMessage((prev) => prev + emojiData.emoji);
    textareaRef.current?.focus();
  }, []);

  const handleInputChange = useCallback(
    (e: React.ChangeEvent<HTMLTextAreaElement>) => {
      setMessage(e.target.value);
    },
    []
  );

  const handleSendMessage = useCallback(() => {
    const trimmedMessage = message.trim();
    if (!trimmedMessage) {
      toast.error("Vui lòng nhập nội dung tin nhắn");
      return;
    }
    onSendMessage(trimmedMessage);
    setMessage("");
  }, [message, onSendMessage]);

  const handleKeyDown = useCallback(
    (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
      if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();
        handleSendMessage();
      }
    },
    [handleSendMessage]
  );

  return (
    <div ref={dropdownRef} className="relative flex gap-3 py-3 px-2">
      <div className="relative flex w-full items-center rounded-full border-2 border-gray-300 bg-white px-3">
        <button
          onClick={toggleEmojiPicker}
          aria-label="Chọn emoji"
          className="text-primary fa-xl"
        >
          <i className="fa-regular fa-face-smile"></i>
        </button>
        <textarea
          data-testid="chat-input"
          ref={textareaRef}
          rows={1}
          placeholder="Nhập lời nhắn tại đây..."
          className="w-full resize-none p-2.5 outline-none rounded-full"
          value={message}
          onChange={handleInputChange}
          onKeyDown={handleKeyDown}
        ></textarea>
      </div>

      <button
        data-testid="send-message-button"
        onClick={handleSendMessage}
        disabled={!message.trim()}
        className="flex items-center justify-center gap-2.5 rounded-full bg-primary text-white px-4 hover:bg-opacity-90 disabled:opacity-50 disabled:cursor-not-allowed"
        aria-label="Gửi tin nhắn"
      >
        <i className="fa-solid fa-paper-plane fa-lg"></i>
        <p>GỬI</p>
      </button>

      {showEmojiPicker && (
        <div className="absolute bottom-16 left-0">
          <EmojiPicker onEmojiClick={handleEmojiClick} />
        </div>
      )}
    </div>
  );
}

export default ChatInput;
