import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  addMarketingEmail,
  deleteMarketingEmail,
  getMarketingEmails,
  requestVerification,
  verifyEmail,
} from "@/apis/email.api";
import {
  AddMarketingEmailParams,
  EmailMarketingParams,
  RequestVerifyParams,
  VerifyEmailParams,
} from "@/models/params/email.param";
import { handleError } from "@/utils/errorUtils";
import { EMAIL_ADMIN_QUERY_KEY } from "@/utils/constants";

export const useRequestVerification = () => {
  const mutation = useMutation({
    mutationFn: (data: RequestVerifyParams) => requestVerification(data),
    onSuccess: () => {
      toast.success("Mã xác thực đã được gửi đến email của bạn");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useVerifyEmail = () => {
  const mutation = useMutation({
    mutationFn: (data: VerifyEmailParams) => verifyEmail(data),
    onSuccess: () => {
      toast.success("Email đã được xác thực thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useGetMarketingEmails = (params: EmailMarketingParams) => {
  return useQuery({
    queryKey: [EMAIL_ADMIN_QUERY_KEY, params],
    queryFn: () => getMarketingEmails(params),
  });
};

export const useAddMarketingEmail = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: AddMarketingEmailParams) => addMarketingEmail(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [EMAIL_ADMIN_QUERY_KEY] });
      toast.success("Đã tạo email thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useDeleteMarketingEmail = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (id: number) => deleteMarketingEmail(id),
    onSuccess: () => {
      toast.success("Đã xóa email thành công");
      queryClient.invalidateQueries({ queryKey: [EMAIL_ADMIN_QUERY_KEY] });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};
