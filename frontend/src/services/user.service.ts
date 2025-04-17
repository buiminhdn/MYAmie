import { useQuery } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getUsers,
  getUserById,
  getBusinesses,
  getBusinessById,
} from "@/apis/user.api";
import { handleError } from "@/utils/errorUtils";
import {
  FilterUserParams,
  FilterBusinessParams,
} from "@/models/params/user.param";

export const useAllGetUsers = (params: FilterUserParams) => {
  const query = useQuery({
    queryKey: ["users", params],
    queryFn: () => getUsers(params),
    enabled: !!params.latitude && !!params.longitude,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetUserById = (id: number) => {
  const query = useQuery({
    queryKey: ["user", id],
    queryFn: () => getUserById(id),
    enabled: !!id,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetAllBusinesses = (params: FilterBusinessParams) => {
  const query = useQuery({
    queryKey: ["businesses", params],
    queryFn: () => getBusinesses(params),
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetBusinessById = (id: number) => {
  const query = useQuery({
    queryKey: ["business", id],
    queryFn: () => getBusinessById(id),
    enabled: !!id,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};
