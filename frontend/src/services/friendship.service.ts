import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getFriendships,
  addFriend,
  removeFriend,
  cancelFriend,
  acceptFriend,
  getFriendship,
} from "@/apis/friendship.api";
import { handleError } from "@/utils/errorUtils";
import { FilterFriendshipParams } from "@/models/params/friendship.param";

export const useGetFriendships = (params: FilterFriendshipParams) => {
  const query = useQuery({
    queryKey: ["friendships", params],
    queryFn: () => getFriendships(params),
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetFriendship = (otherUserId: number) => {
  const query = useQuery({
    queryKey: ["friendship", otherUserId],
    queryFn: () => getFriendship(otherUserId),
    retry: 1,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useAddFriend = () => {
  const mutation = useMutation({
    mutationFn: (otherUserId: number) => addFriend(otherUserId),
    onSuccess: () => {
      toast.success("Đã gửi lời mời kết bạn");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useCancelFriend = () => {
  const mutation = useMutation({
    mutationFn: (otherUserId: number) => cancelFriend(otherUserId),
    onSuccess: () => {
      toast.success("Đã hủy lời mời kết bạn");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useAcceptFriend = () => {
  const mutation = useMutation({
    mutationFn: (otherUserId: number) => acceptFriend(otherUserId),
    onSuccess: () => {
      toast.success("Đã chấp nhận lời mời kết bạn");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useRemoveFriend = () => {
  const mutation = useMutation({
    mutationFn: (otherUserId: number) => removeFriend(otherUserId),
    onSuccess: () => {
      toast.success("Đã xóa bạn bè");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};
