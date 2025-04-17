import { ApiResponse } from "@/models/app.interface";
import {
  ChangePasswordParams,
  UpdateAvatarOrCoverParams,
  UpdateBusinessProfileParams,
  UpdateLocationParams,
  UpdateProfileParams,
} from "@/models/params/account.param";
import { ResetPasswordParams } from "@/models/params/email.param";
import {
  AvatarWName,
  BusinessProfileVM,
  UpdateAvatarOrCoverVM,
  UserProfileVM,
} from "@/models/viewmodels/profile.vm";
import fetchAPI from "@/utils/fetchApi";
import createFormData from "@/utils/formatDataUtils";
import { AxiosResponse } from "axios";

export const getProfile = async () => {
  const response: AxiosResponse<ApiResponse<UserProfileVM>> =
    await fetchAPI.request({
      url: "/account/get-profile",
      method: "get",
    });
  return response.data;
};

export const getBusinessProfile = async () => {
  const response: AxiosResponse<ApiResponse<BusinessProfileVM>> =
    await fetchAPI.request({
      url: "/account/get-business-profile",
      method: "get",
    });
  return response.data;
};

export const resetPassword = async (data: ResetPasswordParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/account/reset-password",
    method: "post",
    data: data,
  });
  return response.data;
};

export const changePassword = async (data: ChangePasswordParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/account/change-password",
    method: "post",
    data: data,
  });
  return response.data;
};

export const updateProfile = async (data: UpdateProfileParams) => {
  const formData = createFormData(data);
  const response: AxiosResponse<ApiResponse<UserProfileVM>> =
    await fetchAPI.request({
      url: "/account/update-profile",
      method: "put",
      data: formData,
      headers: { "Content-Type": "multipart/form-data" },
    });
  return response.data;
};

export const updateBusinessProfile = async (
  data: UpdateBusinessProfileParams
) => {
  const formData = createFormData(data);
  const response: AxiosResponse<ApiResponse<BusinessProfileVM>> =
    await fetchAPI.request({
      url: "/account/update-business-profile",
      method: "put",
      data: formData,
      headers: { "Content-Type": "multipart/form-data" },
    });
  return response.data;
};

export const updateAvatarOrCover = async (data: UpdateAvatarOrCoverParams) => {
  const formData = createFormData(data);
  const response: AxiosResponse<ApiResponse<UpdateAvatarOrCoverVM>> =
    await fetchAPI.request({
      url: "/Account/update-image",
      method: "put",
      data: formData,
      headers: { "Content-Type": "multipart/form-data" },
    });
  return response.data;
};

export const updateLocation = async (data: UpdateLocationParams) => {
  const response: AxiosResponse<ApiResponse<UpdateAvatarOrCoverVM>> =
    await fetchAPI.request({
      url: "/Account/update-location",
      method: "put",
      data: data,
    });
  return response.data;
};

export const getAvatarWithName = async (userId: number) => {
  const response: AxiosResponse<ApiResponse<AvatarWName>> =
    await fetchAPI.request({
      url: `/Account/get-avatar-name`,
      method: "get",
      params: { userId },
    });
  return response.data;
};
