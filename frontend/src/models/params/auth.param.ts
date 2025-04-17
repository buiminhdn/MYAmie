export interface SignInParams {
  email: string;
  password: string;
}

export interface SignUpBusinessParams {
  email: string;
  password: string;
  shortDescription: string;
  name: string;
  cityId: number;
  categoryIds: number[];
}

export interface SignUpParams {
  lastName: string;
  firstName: string;
  email: string;
  password: string;
  latitude: number;
  longitude: number;
}

export interface RefreshTokenParams {
  accessToken: string;
  refreshToken: string;
}
