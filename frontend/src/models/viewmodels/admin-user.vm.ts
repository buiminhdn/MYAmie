import { PaginationData } from "../app.interface";

export interface AdminUserVM {
  id: number;
  avatar: string;
  name: string;
  email: string;
  city: string;
  role: string;
  status: string;
}

export interface PagedAdminUsersVM {
  users: AdminUserVM[];
  pagination: PaginationData;
}
