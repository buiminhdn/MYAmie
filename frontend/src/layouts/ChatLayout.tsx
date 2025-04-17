import { ROUTE_PATH } from "@/routes/route-path";
import { isLoginSelector } from "@/store/auth/auth.selector";
import { useSelector } from "react-redux";
import { Navigate, Outlet } from "react-router-dom";

function ChatLayout() {
  const isLoggedIn = useSelector(isLoginSelector);
  if (!isLoggedIn) return <Navigate to={ROUTE_PATH.LOGIN} replace />;

  return (
    <div className="p-5 h-screen bg-primary">
      <main className="bg-white flex h-full flex-col w-full border-2 rounded-xl border-gray-200 overflow-hidden">
        <Outlet />
      </main>
    </div>
  );
}

export default ChatLayout;
