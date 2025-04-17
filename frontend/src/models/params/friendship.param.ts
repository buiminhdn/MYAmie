import { PaginationParams } from "./app.param";

export interface FilterFriendshipParams extends PaginationParams {
  pageNumber?: number;
  pageSize?: number;
}

export interface FriendRequestParams {
  otherUserId: number;
}
