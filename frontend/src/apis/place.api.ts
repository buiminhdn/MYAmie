import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import { PagedPlacesVM, PlaceDetailVM } from "@/models/viewmodels/place.vm";
import {
  FilterPlaceParams,
  UpsertPlaceParams,
  UserPlaceParams,
} from "@/models/params/place.param";
import { ApiResponse } from "@/models/app.interface";
import createFormData from "@/utils/formatDataUtils";

// Lấy danh sách địa điểm với bộ lọc
export const getAllPlaces = async (params: FilterPlaceParams) => {
  const response: AxiosResponse<ApiResponse<PagedPlacesVM>> =
    await fetchAPI.request({
      url: "/place/get-all",
      method: "get",
      params,
    });

  return response.data;
};

// Lấy thông tin chi tiết của một địa điểm theo ID
export const getPlaceById = async (id: number) => {
  const response: AxiosResponse<ApiResponse<PlaceDetailVM>> =
    await fetchAPI.request({
      url: "/place/get-by-id",
      method: "get",
      params: { id },
    });

  return response.data;
};

// Lấy danh sách địa điểm của một người dùng
export const getPlacesForUser = async (params: UserPlaceParams) => {
  const response: AxiosResponse<ApiResponse<PagedPlacesVM>> =
    await fetchAPI.request({
      url: "/place/get-for-user",
      method: "get",
      params,
    });

  return response.data;
};

// Thêm địa điểm mới
export const addPlace = async (data: UpsertPlaceParams) => {
  const formData = createFormData(data);
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/place/add",
    method: "post",
    data: formData,
    headers: { "Content-Type": "multipart/form-data" },
  });

  return response.data;
};

// Cập nhật địa điểm
export const updatePlace = async (data: UpsertPlaceParams) => {
  const formData = createFormData(data);
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/place/update",
    method: "put",
    data: formData,
    headers: { "Content-Type": "multipart/form-data" },
  });

  return response.data;
};

// Xóa địa điểm
export const deletePlace = async (id: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/place/delete",
    method: "delete",
    data: { id },
  });

  return response.data;
};
