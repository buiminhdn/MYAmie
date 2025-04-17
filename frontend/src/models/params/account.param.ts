export interface ChangePasswordParams {
  oldPassword: string;
  newPassword: string;
}

export interface UpdateAvatarOrCoverParams {
  type: ImageTypeParam;
  imageFile: File;
}

export enum ImageTypeParam {
  Avatar = 1,
  Cover = 2,
}

export interface UpdateProfileParams {
  lastName: string;
  firstName: string;
  shortDescription: string;
  description: string;
  dateOfBirth: string;
  cityId: number;
  images: string;
  characteristics: string[];
  categoryIds: number[];

  imageFiles: File[];
}

export interface UpdateBusinessProfileParams {
  name: string;
  shortDescription: string;
  description: string;
  address: string;
  openHour: number;
  closeHour: number;
  phone: string;
  cityId: number;
  categoryIds: number[];
  images: string;

  imageFiles: File[];
}

export interface UpdateLocationParams {
  latitude: number;
  longitude: number;
}
