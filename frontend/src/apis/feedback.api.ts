import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import {
  AddFeedbackParams,
  FilterFeedbackParams,
  ResponseFeedbackParams,
  UpdateFeedbackParams,
} from "@/models/params/feedback.param";
import { ApiResponse } from "@/models/app.interface";
import { PagedFeedbacksVM } from "@/models/viewmodels/feedback.vm";

// Get feedbacks by filter
export const getFeedbacks = async (params: FilterFeedbackParams) => {
  const response: AxiosResponse<ApiResponse<PagedFeedbacksVM>> =
    await fetchAPI.request({
      url: "/feedback/get-by-id",
      method: "get",
      params,
    });

  return response.data;
};

// Add feedback
export const addFeedback = async (data: AddFeedbackParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/feedback/add",
    method: "post",
    data,
  });

  return response.data;
};

// Update feedback
export const updateFeedback = async (data: UpdateFeedbackParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/feedback/update",
    method: "post",
    data,
  });

  return response.data;
};

// Delete feedback
export const deleteFeedback = async (id: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/feedback/delete",
    method: "post",
    data: { id },
  });

  return response.data;
};

// Respond to feedback
export const respondFeedback = async (data: ResponseFeedbackParams) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/feedback/response",
    method: "post",
    data,
  });

  return response.data;
};
