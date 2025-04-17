// components/FriendshipButton/FriendshipButton.tsx
import Button from "@/components/Button/Button";
import { useFriendAction } from "@/hooks/useFriendActions";
import { FriendshipStatus } from "@/models/app.interface";
import { ROUTE_PATH } from "@/routes/route-path";
import { useGetFriendship } from "@/services/friendship.service";
import { isLoginSelector } from "@/store/auth/auth.selector";
import { useSelector } from "react-redux";

interface FriendshipButtonProps {
  otherUserId: number;
}

export function FriendshipButton({ otherUserId }: FriendshipButtonProps) {
  const isLogin = useSelector(isLoginSelector);

  // 👇 Nếu chưa đăng nhập, render nút đăng nhập
  if (!isLogin) {
    return (
      <Button
        to={ROUTE_PATH.LOGIN}
        variant="outline"
        aria-label="Login to add friend"
      >
        Kết bạn
      </Button>
    );
  }

  const { data: friendshipData } = useGetFriendship(otherUserId);

  const initialStatus = friendshipData?.data?.status || FriendshipStatus.NONE;
  const initialIsRequester = friendshipData?.data?.isRequester || false;

  const { buttonConfig } = useFriendAction(
    otherUserId,
    initialStatus,
    initialIsRequester
  );

  if (!buttonConfig) return null;

  return (
    <Button
      onClick={buttonConfig.handler}
      disabled={buttonConfig.isPending}
      variant={buttonConfig.variant}
      aria-label={buttonConfig.text}
    >
      {buttonConfig.isPending ? "Đang xử lý..." : buttonConfig.text}
    </Button>
  );
}
