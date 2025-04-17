import { useState, useEffect, useMemo } from "react";
import { useSelector } from "react-redux";
import toast from "react-hot-toast";
import {
  useAddFriend,
  useRemoveFriend,
  useCancelFriend,
  useAcceptFriend,
} from "@/services/friendship.service";
import { FriendshipStatus } from "@/models/app.interface";
import { isLoginSelector } from "@/store/auth/auth.selector";

export const useFriendAction = (
  userId: number,
  initialStatus: FriendshipStatus,
  initialIsRequester: boolean
) => {
  const isLogin = useSelector(isLoginSelector);
  const [friendStatus, setFriendStatus] =
    useState<FriendshipStatus>(initialStatus);
  const [isRequester, setIsRequester] = useState(initialIsRequester);

  // Friendship mutations
  const { mutateAsync: addFriend, isPending: isAddPending } = useAddFriend();
  const { mutateAsync: removeFriend, isPending: isRemovePending } =
    useRemoveFriend();
  const { mutateAsync: cancelFriend, isPending: isCancelPending } =
    useCancelFriend();
  const { mutateAsync: acceptFriend, isPending: isAcceptPending } =
    useAcceptFriend();

  // Sync state with props
  useEffect(() => {
    setFriendStatus(initialStatus);
    setIsRequester(initialIsRequester);
  }, [initialStatus, initialIsRequester]);

  const handleFriendAction = async () => {
    if (!isLogin) {
      toast.error("Vui lòng đăng nhập");
      return;
    }

    try {
      switch (friendStatus) {
        case FriendshipStatus.NONE:
          await addFriend(userId);
          setFriendStatus(FriendshipStatus.PENDING);
          setIsRequester(true);
          break;
        case FriendshipStatus.PENDING:
          if (isRequester) {
            await cancelFriend(userId);
            setFriendStatus(FriendshipStatus.NONE);
          } else {
            await acceptFriend(userId);
            setFriendStatus(FriendshipStatus.ACCEPTED);
          }
          break;
        case FriendshipStatus.ACCEPTED:
          await removeFriend(userId);
          setFriendStatus(FriendshipStatus.NONE);
          break;
        default:
          toast.error("Hành động không hợp lệ");
      }
    } catch (error) {
      toast.error("Có lỗi xảy ra");
    }
  };

  const buttonConfig = useMemo(() => {
    if (friendStatus === FriendshipStatus.BLOCKED) return null;

    const isPending =
      (friendStatus === FriendshipStatus.NONE && isAddPending) ||
      (friendStatus === FriendshipStatus.PENDING &&
        (isRequester ? isCancelPending : isAcceptPending)) ||
      (friendStatus === FriendshipStatus.ACCEPTED && isRemovePending);

    const buttonText = {
      [FriendshipStatus.NONE]: "Kết bạn",
      [FriendshipStatus.PENDING]: isRequester ? "Hủy lời mời" : "Chấp nhận",
      [FriendshipStatus.ACCEPTED]: "Hủy kết bạn",
    }[friendStatus];

    const variant: "ghost" | "outline" | "solid" =
      friendStatus === FriendshipStatus.ACCEPTED ||
      (friendStatus === FriendshipStatus.PENDING && isRequester)
        ? "ghost"
        : "outline";

    return {
      text: buttonText,
      variant,
      isPending,
      handler: handleFriendAction,
    };
  }, [
    friendStatus,
    isRequester,
    isAddPending,
    isCancelPending,
    isAcceptPending,
    isRemovePending,
  ]);

  return { friendStatus, isRequester, buttonConfig };
};
