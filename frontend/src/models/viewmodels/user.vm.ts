import { FriendshipStatus, PaginationData } from "../app.interface";
import { CategoryVM } from "./category.vm";

export interface BusinessDetailVM {
  id: number;
  name: string;
  cover: string;
  avatar: string;
  shortDescription: string;
  categories: CategoryVM[];
  city: string;
  phone: string;
  address: string;
  operatingHours: string;
  description: string;
  images: string[];
}

export interface BusinessVM {
  id: number;
  avatar: string;
  name: string;
  shortDescription: string;
  operatingHours: string;
  city: string;
  cover: string;
  averageRating: number;
  totalFeedback: number;
}

export interface PagedBusinessesVM {
  businesses: BusinessVM[];
  pagination: PaginationData;
}

export interface UserDetailVM {
  id: number;
  avatar: string;
  name: string;
  shortDescription: string;
  categories: CategoryVM[];
  characteristics: string[];
  city: string;
  cover: string;
  description: string;
  images: string[];
}

export interface UserVM {
  id: number;
  avatar: string;
  name: string;
  shortDescription: string;
  characteristics: string[];
  distance: number;
  city: string;
  friendStatus: FriendshipStatus;
  isRequester: boolean;
}

export interface PagedUsersVM {
  users: UserVM[];
  pagination: PaginationData;
}
