import { Role } from "@/models/app.interface";
import { AuthAccountVM } from "@/models/viewmodels/auth.vm";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface AuthenticationState {
  isLogin: boolean;
  account: AuthAccountVM;
}

const initialState: AuthenticationState = {
  isLogin: false,
  account: {
    id: -1,
    email: "",
    firstName: "",
    lastName: "",
    avatar: "",
    latitude: 0,
    longitude: 0,
    role: Role.NONE,
    accessToken: "",
    refreshToken: "",
  },
};

export const authSlice = createSlice({
  name: "@auth",
  initialState,
  reducers: {
    login: (state, action: PayloadAction<AuthAccountVM>) => {
      state.isLogin = true;
      state.account = action.payload;
    },
    logout: (state) => {
      state.isLogin = false;
      state.account = initialState.account;
    },
    setAccessToken: (state, action: PayloadAction<string>) => {
      state.account.accessToken = action.payload;
    },
    setRefreshToken: (state, action: PayloadAction<string>) => {
      state.account.refreshToken = action.payload;
    },
    updateAvatar: (state, action: PayloadAction<string>) => {
      state.account.avatar = action.payload;
    },
  },
});

export const { login, logout, setAccessToken, setRefreshToken, updateAvatar } =
  authSlice.actions;

export default authSlice.reducer;
