import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  getAllPlaces,
  getPlaceById,
  getPlacesForUser,
  addPlace,
  updatePlace,
  deletePlace,
} from "@/apis/place.api";
import {
  FilterPlaceParams,
  UpsertPlaceParams,
  UserPlaceParams,
} from "@/models/params/place.param";
import { handleError } from "@/utils/errorUtils";
import {
  PLACE_DETAIL_QUERY_KEY,
  PLACE_QUERY_KEY,
  PLACE_USER_QUERY_KEY,
} from "@/utils/constants";

export const useGetAllPlaces = (params: FilterPlaceParams) => {
  const query = useQuery({
    queryKey: [PLACE_QUERY_KEY, params],
    queryFn: () => getAllPlaces(params),
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetPlaceById = (id: number) => {
  const query = useQuery({
    queryKey: [PLACE_DETAIL_QUERY_KEY, id],
    queryFn: () => getPlaceById(id),
    enabled: !!id,
    retry: false,
    gcTime: 0,
    staleTime: 0,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useGetPlacesForUser = (params: UserPlaceParams) => {
  const query = useQuery({
    queryKey: [PLACE_USER_QUERY_KEY, params],
    queryFn: () => getPlacesForUser(params),
    enabled: !!params.userId,
  });

  if (query.error) {
    toast.error(handleError(query.error));
  }

  return query;
};

export const useAddPlace = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: UpsertPlaceParams) => addPlace(data),
    onSuccess: () => {
      toast.success("Đã thêm địa điểm thành công");
      queryClient.invalidateQueries({ queryKey: [PLACE_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_DETAIL_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_USER_QUERY_KEY] });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useUpdatePlace = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: UpsertPlaceParams) => updatePlace(data),
    onSuccess: () => {
      toast.success("Đã cập nhật địa điểm thành công");
      queryClient.invalidateQueries({ queryKey: [PLACE_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_DETAIL_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_USER_QUERY_KEY] });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};

export const useDeletePlace = () => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (id: number) => deletePlace(id),
    onSuccess: () => {
      toast.success("Đã xóa địa điểm thành công");
      queryClient.invalidateQueries({ queryKey: [PLACE_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_DETAIL_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: [PLACE_USER_QUERY_KEY] });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });

  return mutation;
};
