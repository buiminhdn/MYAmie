import { PaginationData } from "../app.interface";
import { CategoryVM } from "./category.vm";
import { CityVM } from "./city.vm";

export interface PlaceVM {
  id: number;
  name: string;
  shortDescription: string;
  cover: string;
  dateCreated: string;
  city: string;
  ownerId: number;
  ownerAvatar: string;
  ownerName: string;
}

export interface PlaceDetailVM {
  id: number;
  name: string;
  shortDescription: string;
  description: string;
  images: string[];
  address: string;
  viewCount: number;

  // Relationship
  city: CityVM;
  ownerId: number;
  ownerAvatar: string;
  ownerName: string;
  dateCreated: string;
  categories: CategoryVM[];
}

export interface PagedPlacesVM {
  places: PlaceVM[];
  pagination: PaginationData;
}
