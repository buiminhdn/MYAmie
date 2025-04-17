import { ROUTE_PATH } from "@/routes/route-path";
import { useEffect } from "react";
import { Navigate, Outlet, useLocation, useNavigate } from "react-router-dom";
import Sidebar from "./components/Sidebar/Sidebar";
import { useSelector } from "react-redux";
import {
  accountRoleSelector,
  isLoginSelector,
} from "@/store/auth/auth.selector";
import Navbar from "./components/Navbar/Navbar";

const ROUTE_LABEL_MAP: Record<string, string> = {
  [ROUTE_PATH.SETTINGS]: "Thông tin cá nhân",
  [ROUTE_PATH.ACCOUNT]: "Mật khẩu",
  [ROUTE_PATH.ADMIN_USERS]: "Quản lý người dùng",
  [ROUTE_PATH.ADMIN_PLACES]: "Quản lý địa điểm",
  [ROUTE_PATH.ADMIN_EMAIL]: "Email Marketing",
};

function SidebarLayout() {
  const navigate = useNavigate();
  const location = useLocation();
  const isLoggedIn = useSelector(isLoginSelector);
  const role = useSelector(accountRoleSelector);

  useEffect(() => {
    const handlePopState = () => navigate("/");

    window.addEventListener("popstate", handlePopState);
    return () => window.removeEventListener("popstate", handlePopState);
  }, [navigate]);

  if (!isLoggedIn) return <Navigate to={ROUTE_PATH.LOGIN} replace />;

  const currentLabel = ROUTE_LABEL_MAP[location.pathname] || "";

  return (
    <>
      <Navbar />
      <div className="container px-3 md:px-14 min-h-screen">
        <div className="grid grid-cols-1 xl:grid-cols-5 mt-10 mb-32">
          <Sidebar role={role} />
          <div className="col-span-4 mt-8 xl:mt-0">
            <p className="text-base font-semibold mb-3">{currentLabel}</p>
            <Outlet />
          </div>
        </div>
      </div>
    </>
  );
}

export default SidebarLayout;
