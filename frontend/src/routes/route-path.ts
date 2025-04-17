export const ROUTE_PATH = {
  // Auth
  LOGIN: "/login",
  SIGNUP: "/signup",
  LOGOUT: "/logout",
  SIGNUP_BUSINESS: "/signup-business",

  // Public
  BUSINESSES: "/",
  BUSINESS_DETAIL: "/service/:id",
  PLACES: "/places",
  PLACE_DETAIL: "/place/:id",
  USERS: "/users",
  USER_DETAIL: "/user/:id",

  // Chat
  CHAT: "/chat",
  CHAT_DETAIL: "/chat/:id",

  // private common
  SETTINGS: "/info",
  ACCOUNT: "/account",
  // admin
  ADMIN_USERS: "/admin-users",
  ADMIN_PLACES: "/admin-places",
  ADMIN_EMAIL: "/admin-email",

  // Forbidden - 403
  FORBIDDEN: "/forbidden",
  NOT_FOUND: "*",
};
