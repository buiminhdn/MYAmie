import { PlaceStatus } from "../app.interface";
import { PaginationParams } from "./app.param";

export interface AdminPlaceParams extends PaginationParams {
  searchTerm?: string;
  status?: PlaceStatus;
}

export interface AdminPlaceStatusParams {
  placeId: number;
  status: PlaceStatus;
}
