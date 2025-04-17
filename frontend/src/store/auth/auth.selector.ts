import { AppState } from "../store";

export const tokenSelector = (state: AppState) =>
  state.auth.account.accessToken;
export const refreshTokenSelector = (state: AppState) =>
  state.auth.account.refreshToken;

export const accountSelector = (state: AppState) => state.auth.account;
export const isLoginSelector = (state: AppState) => state.auth.isLogin;
export const accountRoleSelector = (state: AppState) => state.auth.account.role;
export const accountIdSelector = (state: AppState) => state.auth.account.id;
export const accountLatitudeSelector = (state: AppState) =>
  state.auth.account.latitude;
export const accountLongitudeSelector = (state: AppState) =>
  state.auth.account.longitude;
