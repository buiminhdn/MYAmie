import Avatar from "@/components/Avatar/Avatar";
import { useParams } from "react-router-dom";
import { useSelector } from "react-redux";
import { accountIdSelector } from "@/store/auth/auth.selector";
import Messages from "./Messages";
import { useGetAvatarName } from "@/services/account.service";

function Chat() {
  const { id } = useParams();
  const currentUserId = useSelector(accountIdSelector);
  const userId = Number(id);

  const { data, isLoading, isError } = useGetAvatarName(userId);
  const { avatar, name } = data?.data || {};

  if (currentUserId === userId || userId === 1) {
    return <p className="error mt-10">Không thể chat với chính mình</p>;
  }

  if (isError) return <p className="error mt-10">Lỗi, vui lòng thử lại</p>;

  return isLoading ? (
    <p className="p-4">Đang tải...</p>
  ) : (
    <>
      <div
        data-testid="chat-detail"
        className="relative flex justify-between border-b-2 border-gray-200 p-3"
      >
        <div className="flex gap-2 items-center">
          <Avatar src={avatar} alt="avatar" size="size-10" hasBorder={false} />
          <p className="text-base font-medium">{name}</p>
        </div>
      </div>
      <Messages
        currentUserId={currentUserId}
        otherUserId={userId}
        otherUserAvatar={avatar}
        otherUserName={name}
      />
    </>
  );
}

export default Chat;
