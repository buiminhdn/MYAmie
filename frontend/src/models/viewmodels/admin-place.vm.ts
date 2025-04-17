import { PaginationData } from "../app.interface";

export interface AdminPlaceVM {
  id: number;
  name: string;
  cover: string;
  city: string;
  ownerAvatar: string;
  ownerName: string;
  status: string;
}

export interface PagedAdminPlacesVM {
  places: AdminPlaceVM[];
  pagination: PaginationData;
}
