import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import {
  FilterUserParams,
  FilterBusinessParams,
} from "@/models/params/user.param";
import { ApiResponse } from "@/models/app.interface";
import {
  BusinessDetailVM,
  PagedBusinessesVM,
  PagedUsersVM,
  UserDetailVM,
} from "@/models/viewmodels/user.vm";

// Fetch users with filters
export const getUsers = async (params: FilterUserParams) => {
  const response: AxiosResponse<ApiResponse<PagedUsersVM>> =
    await fetchAPI.request({
      url: "/user/get-users",
      method: "get",
      params,
    });

  return response.data;
};

// Fetch user by ID
export const getUserById = async (id: number) => {
  const response: AxiosResponse<ApiResponse<UserDetailVM>> =
    await fetchAPI.request({
      url: "/user/get-user-by-id",
      method: "get",
      params: { id },
    });

  return response.data;
};

// Fetch businesses with filters
export const getBusinesses = async (params: FilterBusinessParams) => {
  const response: AxiosResponse<ApiResponse<PagedBusinessesVM>> =
    await fetchAPI.request({
      url: "/user/get-businesses",
      method: "get",
      params,
    });

  return response.data;
};

// Fetch business by ID
export const getBusinessById = async (id: number) => {
  const response: AxiosResponse<ApiResponse<BusinessDetailVM>> =
    await fetchAPI.request({
      url: "/user/get-business-by-id",
      method: "get",
      params: { id },
    });

  return response.data;
};
