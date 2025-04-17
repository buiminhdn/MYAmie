import { PaginationParams } from "./app.param";

export interface DeletePlaceParams {
  id: number;
}

export interface FilterPlaceParams extends PaginationParams {
  searchTerm?: string;
  cityId?: number;
  categoryId?: number;
  pageNumber?: number;
  pageSize?: number;
}

export interface UpsertPlaceParams {
  id?: number;
  name: string;
  shortDescription: string;
  cityId: number;
  categoryIds: number[];
  address: string;
  description: string;
  images?: string;

  imageFiles: File[];
}

export interface UserPlaceParams {
  userId: number;
  pageNumber?: number;
  pageSize?: number;
}
