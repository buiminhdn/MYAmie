import { FriendshipStatus, PaginationData } from "../app.interface";

export interface FriendshipVM {
  id: number;
  otherUserId: number;
  otherUserName: string;
  otherUserAvatar: string;
  isRequester: boolean;
  status: FriendshipStatus;
}

export interface PagedFriendshipsVM {
  friends: FriendshipVM[];
  pagination: PaginationData;
}
