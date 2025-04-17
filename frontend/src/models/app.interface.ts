export interface ApiResponse<T = any> {
  isSuccess: boolean;
  message: string;
  data: T | null;
}

export interface PaginationData {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export enum Role {
  NONE = 0,
  ADMIN = 1,
  USER = 2,
  BUSINESS = 3,
}

export enum RoleForFilter {
  NONE = 0,
  USER = 2,
  BUSINESS = 3,
}

export enum FriendshipStatus {
  NONE = 0,
  PENDING = 1,
  ACCEPTED = 2,
  BLOCKED = 3,
}

export enum AccountStatus {
  NONE = 0,
  ACTIVATED = 1,
  SUSPENDED = 2,
}

export enum PlaceStatus {
  NONE = 0,
  ACTIVATED = 1,
  SUSPENDED = 2,
  // DELETED = 3,
}

export enum MessageStatus {
  SENT = 1,
  DELIVERED = 2,
  READ = 3,
  RECALLED = 4,
}
