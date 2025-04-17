import Loader from "@/components/Loader/Loader";
import useClickOutside from "@/hooks/useClickOutside";
import { useGetConversations } from "@/services/chat.service";
import { useState } from "react";
import ConversationItem from "../components/ConversationItem";

function Conversations() {
  const [isShow, setIsShow] = useState(false);

  // Lấy dữ liệu hội thoại từ API
  const {
    data,
    isLoading,
    fetchNextPage,
    isFetchingNextPage,
    hasNextPage,
    isError,
    error,
  } = useGetConversations();

  // Đóng danh sách khi click ra ngoài
  const ref = useClickOutside(() =>
    setIsShow(false)
  ) as React.RefObject<HTMLDivElement>;

  const conversations =
    data?.pages
      .flatMap((page) => page.data?.conversations || [])
      .filter(Boolean) || [];

  let content;
  if (isLoading) {
    content = <Loader className="pb-3" />;
  } else if (isError) {
    content = <p className="error pb-3">{error.message}</p>;
  } else {
    content = (
      <>
        {conversations.length > 0 ? (
          <div className="h-auto max-h-[70vh] overflow-y-auto">
            <ul className="pr-1">
              {conversations.map((conv) => (
                <ConversationItem key={conv.id} conversation={conv} />
              ))}
            </ul>
            {hasNextPage && (
              <p
                className="text-xs text-center text-gray-500 hover:text-gray-700 font-medium cursor-pointer mt-2"
                onClick={() => fetchNextPage()}
              >
                {isFetchingNextPage ? "Đang tải..." : "Xem thêm"}
              </p>
            )}
          </div>
        ) : (
          <p className="text-gray-500 text-center text-xs">
            Không có cuộc trò chuyện nào
          </p>
        )}
      </>
    );
  }

  return (
    <div ref={ref}>
      {/* Nút mở danh sách hội thoại */}
      <button
        onClick={() => setIsShow(!isShow)}
        className="hover:bg-gray-100 border border-gray-400 rounded-full w-10 h-10 flex items-center justify-center hover:cursor-pointer"
      >
        <i className="text-primary fa-lg fa-solid fa-comments"></i>
      </button>

      {/* Hộp danh sách hội thoại */}
      {isShow && (
        <div className="absolute z-10 right-0 left-0 sm:left-auto sm:right-10 mt-3 w-full sm:max-w-96">
          <div className="bg-white border border-gray-200 shadow-xl rounded-lg py-2 pl-2 mx-4">
            <p className="font-bold text-2xl p-2">Chat</p>
            {content}
          </div>
        </div>
      )}
    </div>
  );
}

export default Conversations;
