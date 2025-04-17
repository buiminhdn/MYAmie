import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getProfile,
  resetPassword,
  changePassword,
  updateProfile,
  updateBusinessProfile,
  updateAvatarOrCover,
  getBusinessProfile,
  updateLocation,
  getAvatarWithName,
} from "@/apis/account.api";
import { handleError } from "@/utils/errorUtils";
import { useEffect } from "react";

export const useGetProfile = () => {
  const query = useQuery({
    queryKey: ["profile"],
    queryFn: getProfile,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetBusinessProfile = () => {
  const query = useQuery({
    queryKey: ["business-profile"],
    queryFn: getBusinessProfile,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useResetPassword = () => {
  return useMutation({
    mutationFn: resetPassword,
    onSuccess: () => {
      toast.success("Đặt lại mật khẩu thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useChangePassword = () => {
  return useMutation({
    mutationFn: changePassword,
    onSuccess: () => {
      toast.success("Mật khẩu đã được thay đổi thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useUpdateProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateProfile,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["profile"] });
      toast.success("Thông tin cá nhân cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useUpdateBusinessProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateBusinessProfile,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["business-profile"] });
      toast.success("Thông tin cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useUpdateAvatarOrCover = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateAvatarOrCover,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["business-profile"] });
      queryClient.invalidateQueries({ queryKey: ["profile"] });
      toast.success("Hình ảnh đã được cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useUpdateLocation = () => {
  return useMutation({
    mutationFn: updateLocation,
  });
};

export const useGetAvatarName = (userId: number) => {
  const query = useQuery({
    queryKey: ["avatarwname", userId],
    queryFn: () => getAvatarWithName(userId),
    enabled: userId > 0,
  });

  useEffect(() => {
    if (query.error) {
      toast.error(handleError(query.error));
    }
  }, [query.error]);

  return query;
};
