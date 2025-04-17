import { ROUTE_PATH } from "./route-path";

// Common profile routes
const PROFILE_ROUTES = {
  type: "HỒ SƠ",
  routes: [
    {
      path: ROUTE_PATH.SETTINGS,
      icon: "fa-circle-info",
      label: "Thông tin",
    },
  ],
};

const ACCOUNT_ROUTES = {
  type: "Tài khoản",
  routes: [
    {
      path: ROUTE_PATH.ACCOUNT,
      icon: "fa-lock-keyhole",
      label: "Tài khoản",
    },
  ],
};

// Role-based routes
export const ROLE_ROUTES = {
  USER: [PROFILE_ROUTES, ACCOUNT_ROUTES],
  ADMIN: [
    {
      type: "QUẢN LÝ",
      routes: [
        {
          path: ROUTE_PATH.ADMIN_USERS,
          icon: "fa-user-group",
          label: "Người dùng",
        },
        {
          path: ROUTE_PATH.ADMIN_PLACES,
          icon: "fa-earth-asia",
          label: "Địa điểm",
        },
        {
          path: ROUTE_PATH.ADMIN_EMAIL,
          icon: "fa-envelopes",
          label: "Marketing",
        },
      ],
    },
    ACCOUNT_ROUTES,
  ],
  BUSINESS: [PROFILE_ROUTES, ACCOUNT_ROUTES],
};
