import Logo from "@/assets/images/Logo";
import cx from "classnames";
import Carousel from "@/components/Carousel/Carousel";
import { ROUTE_PATH } from "@/routes/route-path";
import { Link, Navigate, Outlet, useLocation } from "react-router-dom";
import { useSelector } from "react-redux";
import { isLoginSelector } from "@/store/auth/auth.selector";
import { ads1, ads2, ads3 } from "@/assets/images";

const images = [ads2, ads1, ads3];

function AuthLayout() {
  const location = useLocation();
  const isLoggedIn = useSelector(isLoginSelector);

  if (isLoggedIn) {
    return <Navigate to={ROUTE_PATH.BUSINESSES} replace />;
  }

  const isLoginPage = location.pathname === ROUTE_PATH.LOGIN;
  const isSignupBusinessPage = location.pathname === ROUTE_PATH.SIGNUP_BUSINESS;

  return (
    <div
      className={cx("h-screen", {
        "grid lg:grid-cols-3": !isSignupBusinessPage,
      })}
    >
      {/* Left Section (Form & Navigation) */}
      <div className="col-span-1 flex flex-col justify-between p-4 h-full">
        <Link to={ROUTE_PATH.BUSINESSES} className="mx-auto">
          <Logo />
        </Link>
        <div
          className={cx("w-full mx-auto", {
            "lg:w-3/4 sm:w-1/2": !isSignupBusinessPage,
          })}
        >
          <Outlet />
        </div>
        <div className="flex justify-center gap-1">
          <p>{isLoginPage ? "Chưa có tài khoản?" : "Đã có tài khoản?"}</p>
          <Link
            to={isLoginPage ? ROUTE_PATH.SIGNUP : ROUTE_PATH.LOGIN}
            className="hover:underline font-medium"
          >
            {isLoginPage ? "Đăng ký tại đây" : "Đăng nhập tại đây"}
          </Link>
        </div>
      </div>

      {/* Right Section (Carousel) */}
      {!isSignupBusinessPage && (
        <div className="hidden lg:block col-span-2 py-4 pr-4">
          <Carousel images={images} />
        </div>
      )}
    </div>
  );
}

export default AuthLayout;
