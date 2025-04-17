import env from "@/env";
import ConfigureAxios from "./configAxios";
import { store } from "@/store/store";
import { login, logout } from "@/store/auth/auth.slice";

const axiosInstance = new ConfigureAxios({
  configure: {
    baseURL: env.apiEndPoint,
    method: "GET",
    timeout: 10000,
  },
  getAccessToken: () => {
    return store.getState().auth.account.accessToken;
  },
  getRefreshToken: () => {
    return store.getState().auth.account.refreshToken || "";
  },
});

const fetchAPI = axiosInstance.create();

axiosInstance.accessToken({
  setCondition: (config) => {
    return !config.url?.includes("login");
  },
});

axiosInstance.refreshToken({
  setCondition(error) {
    return error.response?.status === 401;
  },
  axiosData(accessToken, refreshToken) {
    return {
      accessToken,
      refreshToken,
    };
  },
  success: (response, config) => {
    store.dispatch(
      login({
        ...store.getState().auth.account,
        accessToken: response.data.accessToken,
        refreshToken: response.data.refreshToken,
      })
    );

    if (config.headers) {
      config.headers["Authorization"] = `Bearer ${response.data.accessToken}`;
    }
  },
  failure: () => {
    store.dispatch(logout());
  },
});

export default fetchAPI;
