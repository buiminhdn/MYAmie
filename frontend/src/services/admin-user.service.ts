import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getUsersByAdmin,
  updateUserStatus,
  updateUserPassword,
} from "@/apis/admin-user.api";
import { handleError } from "@/utils/errorUtils";
import { AdminUserParams } from "@/models/params/admin-user.param";

export const useGetUsersByAdmin = (params: AdminUserParams) => {
  return useQuery({
    queryKey: ["admin-users", params],
    queryFn: () => getUsersByAdmin(params),
  });
};

export const useUpdateUserStatus = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateUserStatus,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["admin-users"] });
      toast.success("Trạng thái cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useUpdateUserPassword = () => {
  return useMutation({
    mutationFn: updateUserPassword,
    onSuccess: () => {
      toast.success("Mật khẩu cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};
