import { ApiResponse } from "./../models/app.interface";
import { CategoryVM } from "@/models/viewmodels/category.vm";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";

// Fetch all categories
export const getAllCategories = async () => {
  const response: AxiosResponse<ApiResponse<CategoryVM[]>> =
    await fetchAPI.request({
      url: "/category/get-all",
      method: "get",
    });

  return response.data;
};
