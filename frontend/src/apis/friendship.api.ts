import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import { FilterFriendshipParams } from "@/models/params/friendship.param";
import { ApiResponse } from "@/models/app.interface";
import {
  FriendshipVM,
  PagedFriendshipsVM,
} from "@/models/viewmodels/friendship.vm";

// Lấy danh sách bạn bè (có phân trang)
export const getFriendships = async (params: FilterFriendshipParams) => {
  const response: AxiosResponse<ApiResponse<PagedFriendshipsVM>> =
    await fetchAPI.request({
      url: "/friendship/get-all-friendsips",
      method: "get",
      params,
    });

  return response.data;
};

export const getFriendship = async (otherUserId: number) => {
  const response: AxiosResponse<ApiResponse<FriendshipVM>> =
    await fetchAPI.request({
      url: "/friendship/get-friendship",
      method: "get",
      params: { otherUserId },
    });

  return response.data;
};

// Gửi lời mời kết bạn
export const addFriend = async (otherUserId: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/friendship/add-friend",
    method: "post",
    data: { otherUserId },
  });

  return response.data;
};

// Hủy lời mời kết bạn
export const cancelFriend = async (otherUserId: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/friendship/cancel-friend",
    method: "post",
    data: { otherUserId },
  });

  return response.data;
};

// Chấp nhận lời mời kết bạn
export const acceptFriend = async (otherUserId: number) => {
  const response: AxiosResponse<ApiResponse> = await fetchAPI.request({
    url: "/friendship/accept-friend",
    method: "post",
    data: { otherUserId },
  });

  return response.data;
};

// Xóa bạn bè
export const removeFriend = async (otherUserId: number) => {
  const response: AxiosResponse<ApiResponse<string>> = await fetchAPI.request({
    url: "/friendship/remove-friend",
    method: "post",
    data: { otherUserId },
  });

  return response.data;
};
