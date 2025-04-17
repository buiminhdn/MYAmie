import { useQuery } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { getAllCategories } from "@/apis/category.api";
import { handleError } from "@/utils/errorUtils";

export const useGetAllCategories = () => {
  const query = useQuery({
    queryKey: ["categories"],
    queryFn: getAllCategories,
    staleTime: Infinity,
    gcTime: Infinity,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};
