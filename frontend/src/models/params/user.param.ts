import { PaginationParams } from "./app.param";

export interface FilterBusinessParams extends PaginationParams {
  searchTerm?: string;
  cityId?: number;
  categoryId?: number;
}

export interface FilterUserParams extends PaginationParams {
  categoryId?: number;
  distanceInKm?: number;
  latitude?: number;
  longitude?: number;
}
