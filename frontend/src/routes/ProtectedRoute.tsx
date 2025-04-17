import { Navigate, Outlet } from "react-router-dom";
import { ROUTE_PATH } from "./route-path";
import { useSelector } from "react-redux";
import {
  accountRoleSelector,
  isLoginSelector,
} from "@/store/auth/auth.selector";
import { Role } from "@/models/app.interface";

interface ProtectedRouteProps {
  allowedRoles: Role[];
}

const ProtectedRoute = ({ allowedRoles }: ProtectedRouteProps) => {
  const isLoggedIn = useSelector(isLoginSelector);
  const role = useSelector(accountRoleSelector);

  if (!isLoggedIn) return <Navigate to={ROUTE_PATH.LOGIN} replace />;
  if (!allowedRoles.includes(role))
    return <Navigate to={ROUTE_PATH.BUSINESSES} replace />;

  return <Outlet />;
};

export default ProtectedRoute;
