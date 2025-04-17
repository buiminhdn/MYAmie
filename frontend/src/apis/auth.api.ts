import { ApiResponse } from "@/models/app.interface";
import {
  SignUpParams,
  SignUpBusinessParams,
  SignInParams,
} from "@/models/params/auth.param";
import { AuthAccountVM } from "@/models/viewmodels/auth.vm";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";

// User Sign-Up
export const signUp = async (data: SignUpParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/auth/sign-up",
    method: "post",
    data,
  });

  return response.data;
};

// Business Sign-Up
export const signUpBusiness = async (data: SignUpBusinessParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/auth/sign-up-business",
    method: "post",
    data,
  });

  return response.data;
};

// User Login
export const signIn = async (data: SignInParams) => {
  const response: AxiosResponse<ApiResponse<AuthAccountVM>> =
    await fetchAPI.request({
      url: "/auth/sign-in",
      method: "post",
      data,
    });

  return response.data;
};

export const signOut = async () => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/Token/revoke-token",
    method: "post",
    data: {},
  });

  return response.data;
};
