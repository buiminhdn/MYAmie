import { useInfiniteQuery } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { getConversations, getMessages } from "@/apis/chat.api";
import { handleError } from "@/utils/errorUtils";
import { useEffect } from "react";

export const useGetConversations = () => {
  const query = useInfiniteQuery({
    queryKey: ["conversations"], // Static key since no params
    queryFn: ({ pageParam = 1 }) => getConversations({ pageNumber: pageParam }),
    getNextPageParam: (lastPage) =>
      lastPage.data?.hasMore ? lastPage.data.pageNumber + 1 : undefined,
    initialPageParam: 1,
    refetchOnWindowFocus: true,
  });

  useEffect(() => {
    if (query.error) {
      toast.error(handleError(query.error));
    }
  }, [query.error]);

  return query;
};

export const useGetMessages = (otherUserId: number) => {
  const query = useInfiniteQuery({
    queryKey: ["messages", otherUserId],
    queryFn: ({ pageParam = 1 }) =>
      getMessages({ otherUserId, pageNumber: pageParam }),
    getNextPageParam: (lastPage) =>
      lastPage.data?.hasMore ? lastPage.data.pageNumber + 1 : undefined,
    initialPageParam: 1,
  });

  useEffect(() => {
    if (query.error) {
      toast.error(handleError(query.error));
    }
  }, [query.error]);

  return query;
};
