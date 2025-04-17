import { ApiResponse } from "@/models/app.interface";
import {
  AdminPlaceParams,
  AdminPlaceStatusParams,
} from "@/models/params/admin-place.param";
import { PagedAdminPlacesVM } from "@/models/viewmodels/admin-place.vm";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";

// Lấy danh sách địa điểm do admin quản lý
export const getPlacesByAdmin = async (params: AdminPlaceParams) => {
  const response: AxiosResponse<ApiResponse<PagedAdminPlacesVM>> =
    await fetchAPI.request({
      url: "/AdminPlace/get-all",
      method: "get",
      params,
    });

  return response.data;
};

// Cập nhật trạng thái của địa điểm
export const updatePlaceStatus = async (data: AdminPlaceStatusParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/AdminPlace/update-status",
    method: "put",
    data,
  });

  return response.data;
};
