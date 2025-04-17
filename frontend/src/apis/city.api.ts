import { ApiResponse } from "./../models/app.interface";
import { CityVM } from "@/models/viewmodels/city.vm";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";

// Fetch all cities
export const getAllCities = async () => {
  const response: AxiosResponse<ApiResponse<CityVM[]>> = await fetchAPI.request(
    {
      url: "/city/get-all",
      method: "get",
    }
  );

  return response.data;
};
