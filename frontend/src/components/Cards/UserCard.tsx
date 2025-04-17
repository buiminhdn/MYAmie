import { Link } from "react-router-dom";
import Avatar from "../Avatar/Avatar";
import IconText from "../IconText/IconText";
import { UserVM } from "@/models/viewmodels/user.vm";
import Button from "../Button/Button";
import { useFriendAction } from "@/hooks/useFriendActions";

function UserCard({ user }: { user: UserVM }) {
  const { buttonConfig } = useFriendAction(
    user.id,
    user.friendStatus,
    user.isRequester
  );

  return (
    <div className="user-card flex flex-col justify-between p-4 border-2 border-gray-200 rounded-lg relative bg-white">
      <div>
        <div className="flex items-center gap-3">
          <Avatar
            src={user.avatar}
            alt={user.name}
            size="size-12"
            hasBorder={false}
          />
          <Link
            to={`/user/${user.id}`}
            className="text-base font-medium hover:underline"
          >
            {user.name}
          </Link>
        </div>
        <p className="text-gray-500 line-clamp-3 lg:line-clamp-2 mt-2">
          {user.shortDescription}
        </p>
      </div>
      <div>
        <div className="flex flex-wrap gap-2 mt-4">
          {user.characteristics.map((char, index) => (
            <span
              key={index}
              className="border-2 border-primary py-1 px-3 rounded-full"
            >
              {char}
            </span>
          ))}
        </div>
        <div className="flex flex-wrap gap-4 lg:gap-10 justify-end mt-3">
          <div className="flex flex-wrap gap-3 sm:gap-8">
            {user.city && <IconText icon="fa-location-dot" text={user.city} />}
            <IconText icon="fa-compass" text={`${user.distance} km`} />
          </div>
          {buttonConfig && (
            <Button
              className="hover:cursor-pointer"
              onClick={buttonConfig.handler}
              disabled={buttonConfig.isPending}
              variant={buttonConfig.variant}
              padding="px-4 py-1.5"
              aria-label={buttonConfig.text}
            >
              {buttonConfig.isPending ? "Đang xử lý..." : buttonConfig.text}
            </Button>
          )}
        </div>
      </div>
    </div>
  );
}

export default UserCard;
