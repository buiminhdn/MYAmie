export interface BusinessProfileVM {
  name: string;
  shortDescription: string;
  description: string;
  avatar: string;
  cover: string;
  address: string;
  phone: string;
  openHour: number;
  closeHour: number;
  images: string[];
  cityId: number;
  categoryIds: number[];
}

export interface UpdateAvatarOrCoverVM {
  oldPath: string;
  newPath: string;
}

export interface UserProfileVM {
  firstName: string;
  lastName: string;
  shortDescription: string;
  description: string;
  avatar: string;
  cover: string;
  dateOfBirth: string;
  images: string[];
  characteristics: string[];
  cityId: number;
  categoryIds: number[];
}

export interface AvatarWName {
  id: number;
  avatar: string;
  name: string;
}
