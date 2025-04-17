import Logo from "@/assets/images/Logo";
import Button from "@/components/Button/Button";
import { ROUTE_PATH } from "@/routes/route-path";
import { Link, NavLink } from "react-router-dom";
import cx from "classnames";
import { useSelector } from "react-redux";
import {
  accountRoleSelector,
  isLoginSelector,
} from "@/store/auth/auth.selector";
import ProfileOptions from "../ProfileOptions/ProfileOptions";
import Coversations from "@/pages/ChatPage/Conversations/Coversations";
import { Role } from "@/models/app.interface";

const navlinks = [
  { id: "navlink-businesses", name: "Dịch vụ", href: ROUTE_PATH.BUSINESSES },
  { id: "navlink-places", name: "Địa điểm", href: ROUTE_PATH.PLACES },
  { id: "navlink-users", name: "Bạn bè", href: ROUTE_PATH.USERS },
];

function Navbar() {
  const isLoggedIn = useSelector(isLoginSelector);
  const curUserRole = useSelector(accountRoleSelector);

  return (
    <div className="container px-4 sm:px-7 py-5 flex flex-wrap gap-7 justify-center sm:justify-between items-center">
      {/* Left: Navigation Links */}
      <nav className="flex space-x-7 font-medium">
        {navlinks.map((link) => (
          <NavLink
            id={link.id}
            key={link.id}
            to={link.href}
            className={({ isActive }) =>
              cx("hover:text-primary transition-colors", {
                "text-primary": isActive,
              })
            }
          >
            {link.name}
          </NavLink>
        ))}
      </nav>

      {/* Center: Logo */}
      <Link
        to={ROUTE_PATH.BUSINESSES}
        className="hidden md:block absolute right-0 left-0 w-fit mx-auto"
      >
        <Logo />
      </Link>

      {isLoggedIn ? (
        <div className="flex gap-3 items-center">
          {curUserRole !== Role.ADMIN && <Coversations />}
          <ProfileOptions />
        </div>
      ) : (
        <div className="flex items-center space-x-6">
          <Link to={ROUTE_PATH.SIGNUP_BUSINESS} className="font-medium">
            Kinh doanh
          </Link>
          <Button id="access-button" shape="rounded" to={ROUTE_PATH.LOGIN}>
            Đăng nhập
          </Button>
        </div>
      )}
    </div>
  );
}

export default Navbar;
