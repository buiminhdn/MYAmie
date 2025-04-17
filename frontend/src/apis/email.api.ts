import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import { ApiResponse } from "@/models/app.interface";
import {
  AddMarketingEmailParams,
  EmailMarketingParams,
  RequestVerifyParams,
  VerifyEmailParams,
} from "@/models/params/email.param";
import { PagedEmailMarketingVM } from "@/models/viewmodels/email.vm";

// Request email verification code
export const requestVerification = async (data: RequestVerifyParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/Email/request-verification",
    method: "post",
    data,
  });

  return response.data;
};

// Verify email with the code
export const verifyEmail = async (data: VerifyEmailParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/Email/verify-email",
    method: "post",
    data,
  });

  return response.data;
};

export const getMarketingEmails = async (params: EmailMarketingParams) => {
  const response: AxiosResponse<ApiResponse<PagedEmailMarketingVM>> =
    await fetchAPI.request({
      url: "/Email/get-marketing-emails",
      method: "get",
      params,
    });

  return response.data;
};

export const addMarketingEmail = async (data: AddMarketingEmailParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/Email/add-marketing-email",
    method: "post",
    data,
  });

  return response.data;
};

export const deleteMarketingEmail = async (id: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/Email/delete-marketing-email",
    method: "delete",
    data: { id },
  });

  return response.data;
};
