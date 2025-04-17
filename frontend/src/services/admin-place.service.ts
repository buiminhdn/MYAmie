import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { getPlacesByAdmin, updatePlaceStatus } from "@/apis/admin-place.api";
import { handleError } from "@/utils/errorUtils";
import { AdminPlaceParams } from "@/models/params/admin-place.param";

export const useGetPlacesByAdmin = (params: AdminPlaceParams) => {
  return useQuery({
    queryKey: ["admin-places", params],
    queryFn: () => getPlacesByAdmin(params),
  });
};

export const useUpdatePlaceStatus = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updatePlaceStatus,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["admin-places"] });
      toast.success("Trạng thái cập nhật thành công");
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};
