import { ApiResponse } from "@/models/app.interface";
import {
  AdminUserParams,
  AdminUserPasswordParams,
  AdminUserStatusParams,
} from "@/models/params/admin-user.param";
import { PagedAdminUsersVM } from "@/models/viewmodels/admin-user.vm";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";

// Lấy danh sách người dùng do admin quản lý
export const getUsersByAdmin = async (params: AdminUserParams) => {
  const response: AxiosResponse<ApiResponse<PagedAdminUsersVM>> =
    await fetchAPI.request({
      url: "/AdminUser/get-all",
      method: "get",
      params,
    });

  return response.data;
};

// Cập nhật trạng thái của người dùng
export const updateUserStatus = async (data: AdminUserStatusParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/AdminUser/update-status",
    method: "put",
    data,
  });

  return response.data;
};

// Cập nhật mật khẩu của người dùng
export const updateUserPassword = async (data: AdminUserPasswordParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/AdminUser/update-password",
    method: "put",
    data,
  });

  return response.data;
};
