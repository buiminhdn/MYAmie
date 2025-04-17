import { AccountStatus, RoleForFilter } from "../app.interface";
import { PaginationParams } from "./app.param";

export interface AdminUserParams extends PaginationParams {
  searchTerm?: string;
  status?: AccountStatus;
  role?: RoleForFilter;
}

export interface AdminUserPasswordParams {
  userId: number;
  password: string;
}

export interface AdminUserStatusParams {
  userId: number;
  status: AccountStatus;
}
