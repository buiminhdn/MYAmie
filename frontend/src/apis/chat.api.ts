import { ApiResponse } from "./../models/app.interface";
import fetchAPI from "@/utils/fetchApi";
import { AxiosResponse } from "axios";
import {
  PagedConversationsVM,
  PagedMessagesVM,
} from "@/models/viewmodels/chat.vm";

// Fetch conversations
export const getConversations = async ({ pageNumber = 1 }) => {
  const response: AxiosResponse<ApiResponse<PagedConversationsVM>> =
    await fetchAPI.request({
      url: "/chat/get-conversations",
      method: "get",
      params: {
        pageNumber: pageNumber,
      },
    });

  return response.data;
};

// Fetch messages
export const getMessages = async ({
  otherUserId,
  pageNumber = 1,
}: {
  otherUserId: number;
  pageNumber?: number;
}) => {
  const response: AxiosResponse<ApiResponse<PagedMessagesVM>> =
    await fetchAPI.request({
      url: "/chat/get-messages",
      method: "get",
      params: {
        otherUserId,
        pageNumber,
      },
    });

  return response.data;
};
