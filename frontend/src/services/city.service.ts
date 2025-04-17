import { getAllCities } from "@/apis/city.api";
import { handleError } from "@/utils/errorUtils";
import { useQuery } from "@tanstack/react-query";
import toast from "react-hot-toast";

export const useGetAllCities = () => {
  const query = useQuery({
    queryKey: ["cities"],
    queryFn: getAllCities,
    staleTime: Infinity,
    gcTime: Infinity,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};
