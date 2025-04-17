import Avatar from "@/components/Avatar/Avatar";
import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { ROUTE_PATH } from "@/routes/route-path";
import { accountSelector } from "@/store/auth/auth.selector";
import { Role } from "@/models/app.interface";
import useClickOutside from "@/hooks/useClickOutside";
import { useQueryClient } from "@tanstack/react-query";
import { logout } from "@/store/auth/auth.slice";

function ProfileOptions() {
  const account = useSelector(accountSelector);
  const [isShow, setIsShow] = useState(false);
  const queryClient = useQueryClient();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    queryClient.clear();
    dispatch(logout());
    navigate(0);
  };

  const ref = useClickOutside(() =>
    setIsShow(false)
  ) as React.RefObject<HTMLDivElement>;

  const roleLinks: Record<Role, string> = {
    [Role.ADMIN]: ROUTE_PATH.ADMIN_USERS,
    [Role.BUSINESS]: `/service/${account.id}`,
    [Role.USER]: `/user/${account.id}`,
    [Role.NONE]: "",
  };

  return (
    <div className="relative" ref={ref}>
      <div id="profile-options" className="flex items-center gap-2">
        <Avatar
          onClick={() => setIsShow(!isShow)}
          src={account.avatar}
          alt="avatar"
          size="size-10"
          hasBorder={false}
          className="cursor-pointer"
        />
        <p className="font-medium max-w-24 overflow-hidden text-ellipsis whitespace-nowrap">
          {account.firstName}
        </p>
      </div>

      {isShow && (
        <div className="absolute z-10 right-0 mt-3 bg-white border border-gray-200 shadow-xl rounded-md text-nowrap p-1">
          <MenuItem
            to={roleLinks[account.role]}
            icon="fa-user"
            text="Trang cá nhân"
            id="profile-link"
            setIsShow={setIsShow}
          />
          {account.role === Role.ADMIN ? (
            <MenuItem
              to={ROUTE_PATH.ACCOUNT}
              icon="fa-gear"
              text="Tài khoản"
              setIsShow={setIsShow}
            />
          ) : (
            <MenuItem
              to={ROUTE_PATH.SETTINGS}
              icon="fa-gear"
              text="Thông tin"
              setIsShow={setIsShow}
            />
          )}
          <MenuItem
            id="logout-button"
            isButton
            onClick={handleLogout}
            icon="fa-arrow-right-from-bracket"
            text="Đăng xuất"
          />
        </div>
      )}
    </div>
  );
}

const MenuItem = ({
  to,
  icon,
  text,
  isButton,
  onClick,
  setIsShow,
  id,
}: {
  to?: string;
  icon: string;
  text: string;
  isButton?: boolean;
  onClick?: () => void;
  setIsShow?: (value: boolean) => void;
  id?: string;
}) => {
  const className =
    "flex items-center gap-3 w-full p-2 hover:bg-primary-lighter rounded-sm";

  const handleClick = () => {
    if (setIsShow) setIsShow(false); // Close dropdown when clicking a menu item
  };

  return isButton ? (
    <button
      id={id}
      onClick={onClick}
      className={` hover:cursor-pointer ${className}`}
    >
      <i className={`fa-regular ${icon}`}></i>
      <p>{text}</p>
    </button>
  ) : (
    <Link to={to!} id={id} className={className} onClick={handleClick}>
      <i className={`fa-regular ${icon}`}></i>
      <p>{text}</p>
    </Link>
  );
};

export default ProfileOptions;
