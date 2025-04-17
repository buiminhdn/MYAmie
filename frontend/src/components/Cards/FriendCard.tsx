import Avatar from "../Avatar/Avatar";
import { Link } from "react-router-dom";
import { FriendshipStatus } from "@/models/app.interface";
import Button from "../Button/Button";
import { FriendshipVM } from "@/models/viewmodels/friendship.vm";
import { useFriendAction } from "@/hooks/useFriendActions";

type FriendCardProps = {
  friend: FriendshipVM;
};

function FriendCard({ friend }: FriendCardProps) {
  const { buttonConfig, friendStatus } = useFriendAction(
    friend.otherUserId,
    friend.status,
    friend.isRequester
  );

  return (
    <div className="border-2 border-gray-200 bg-white rounded-md p-3 flex flex-col gap-3 relative">
      <div className="flex flex-wrap gap-2 items-center">
        <Avatar
          src={friend.otherUserAvatar}
          alt="Friend avatar"
          size="size-12"
          className="inline"
        />
        <Link
          to={`/user/${friend.otherUserId}`}
          className="font-medium hover:underline"
        >
          {friend.otherUserName}
        </Link>
      </div>
      <div className="flex gap-2 flex-wrap">
        {friendStatus === FriendshipStatus.ACCEPTED && (
          <Button
            onClick={() => window.open(`/chat/${friend.otherUserId}`, "_blank")}
            variant="outline"
            padding="px-4 py-1.5"
            className="text-xs"
          >
            Nhắn tin
          </Button>
        )}
        {buttonConfig && (
          <Button
            onClick={buttonConfig.handler}
            disabled={buttonConfig.isPending}
            variant={buttonConfig.variant}
            padding="px-4 py-1.5"
            className="text-xs"
          >
            {buttonConfig.isPending ? "Đang xử lý" : buttonConfig.text}
          </Button>
        )}
      </div>
    </div>
  );
}

export default FriendCard;
