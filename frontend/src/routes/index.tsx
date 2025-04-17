import DefaultLayout from "@/layouts/DefaultLayout";
import { ROUTE_PATH } from "./route-path";
import BusinessPages from "@/pages/Common/BusinessPages/BusinessesPage";
import PlacePages from "@/pages/Common/PlacePages/PlacesPage";
import UserPages from "@/pages/Common/UserPages/UsersPage";
import { createBrowserRouter } from "react-router-dom";
import PlaceDetailPage from "@/pages/Common/PlacePages/PlaceDetailPage";
import BussinessDetailPage from "@/pages/Common/BusinessPages/BusinessDetailPage";
import UserDetailPage from "@/pages/Common/UserPages/UserDetailPage";
import AuthLayout from "@/layouts/AuthLayout";
import LoginPage from "@/pages/AuthPages/LoginPage/LoginPage";
import SignupPage from "@/pages/AuthPages/SignupPage/SignupPage";
import SignupBusinessPage from "@/pages/AuthPages/SignupBusinessPage/SignupBusinessPage";
import SidebarLayout from "@/layouts/SidebarLayout";
import ProtectedRoute from "./ProtectedRoute";
import { Role } from "@/models/app.interface";
import SettingPage from "@/pages/ProfilePages/SettingPage/SettingPage";
import AccountPage from "@/pages/ProfilePages/AccountPage/AccountPage";
import UserManagePage from "@/pages/AdminPages/UserManagePage/UserManagePage";
import PlaceManagePage from "@/pages/AdminPages/PlaceManagePage/PlaceManagePage";
import ChatLayout from "@/layouts/ChatLayout";
import { chatCharacters } from "@/assets/images";
import Chat from "@/pages/ChatPage/Chat/Chat";
import EmailManagePage from "@/pages/AdminPages/EmailManagePage/EmailManagePage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <DefaultLayout />,
    children: [
      {
        index: true,
        element: <BusinessPages />,
      },
      {
        path: ROUTE_PATH.BUSINESS_DETAIL,
        element: <BussinessDetailPage />,
      },
      {
        path: ROUTE_PATH.PLACES,
        element: <PlacePages />,
      },
      {
        path: ROUTE_PATH.PLACE_DETAIL,
        element: <PlaceDetailPage />,
      },
      {
        path: ROUTE_PATH.USERS,
        element: <UserPages />,
      },
      {
        path: ROUTE_PATH.USER_DETAIL,
        element: <UserDetailPage />,
      },
      {
        path: "*",
        element: <BusinessPages />,
      },
    ],
  },
  {
    path: "/",
    element: <AuthLayout />,
    children: [
      {
        path: ROUTE_PATH.LOGIN,
        element: <LoginPage />,
      },
      {
        path: ROUTE_PATH.SIGNUP,
        element: <SignupPage />,
      },
      {
        path: ROUTE_PATH.SIGNUP_BUSINESS,
        element: <SignupBusinessPage />,
      },
    ],
  },
  {
    path: "/",
    element: <SidebarLayout />,
    children: [
      {
        element: <ProtectedRoute allowedRoles={[Role.USER, Role.BUSINESS]} />,
        children: [{ path: ROUTE_PATH.SETTINGS, element: <SettingPage /> }],
      },
      {
        element: (
          <ProtectedRoute
            allowedRoles={[Role.USER, Role.ADMIN, Role.BUSINESS]}
          />
        ),
        children: [{ path: ROUTE_PATH.ACCOUNT, element: <AccountPage /> }],
      },
      {
        element: <ProtectedRoute allowedRoles={[Role.ADMIN]} />,
        children: [
          {
            path: ROUTE_PATH.ADMIN_USERS,
            element: <UserManagePage />,
          },
          {
            path: ROUTE_PATH.ADMIN_PLACES,
            element: <PlaceManagePage />,
          },
          {
            path: ROUTE_PATH.ADMIN_EMAIL,
            element: <EmailManagePage />,
          },
        ],
      },
    ],
  },
  {
    path: ROUTE_PATH.CHAT,
    element: <ChatLayout />,
    children: [
      {
        index: true,
        element: (
          <div className="flex flex-col justify-center items-center h-full">
            <img src={chatCharacters} alt="" className="h-60" />
            <p className="text-wrap">
              Một tin nhắn khởi đầu là hàng trăm câu chuyện đang chờ đón!
            </p>
          </div>
        ),
      },
      {
        path: ROUTE_PATH.CHAT_DETAIL,
        element: <Chat />,
      },
    ],
  },
]);

export default router;
