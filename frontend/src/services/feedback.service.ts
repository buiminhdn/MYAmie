import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getFeedbacks,
  addFeedback,
  updateFeedback,
  deleteFeedback,
  respondFeedback,
} from "@/apis/feedback.api";
import { handleError } from "@/utils/errorUtils";
import {
  AddFeedbackParams,
  FilterFeedbackParams,
  ResponseFeedbackParams,
  UpdateFeedbackParams,
} from "@/models/params/feedback.param";

export const useGetFeedbacks = (params: FilterFeedbackParams) => {
  const query = useQuery({
    queryKey: ["feedbacks", params],
    queryFn: () => getFeedbacks(params),
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useAddFeedback = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: AddFeedbackParams) => addFeedback(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["feedbacks"] });
      toast.success("Đã gửi phản hồi thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useUpdateFeedback = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: UpdateFeedbackParams) => updateFeedback(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["feedbacks"] });
      toast.success("Đã cập nhật phản hồi thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useDeleteFeedback = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: number) => deleteFeedback(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["feedbacks"] });
      toast.success("Đã xóa phản hồi thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useRespondFeedback = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: ResponseFeedbackParams) => respondFeedback(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["feedbacks"] });
      toast.success("Đã phản hồi thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};
