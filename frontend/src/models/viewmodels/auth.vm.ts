import { Role } from "../app.interface";

export interface AuthAccountVM {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  avatar: string;
  latitude: number;
  longitude: number;
  role: Role;

  // Access & Refresh Token
  accessToken: string;
  refreshToken: string;
}

export interface TokenAuthVM {
  accessToken: string;
  refreshToken: string;
}
