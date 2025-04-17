import PlaceCard from "@/components/Cards/PlaceCard";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import { UserPlaceParams } from "@/models/params/place.param";
import { useGetPlacesForUser } from "@/services/place.service";
import { handleError } from "@/utils/errorUtils";
import { useEffect, useState } from "react";
import CreatePlaceSection from "../../PlacePages/components/CreatePlaceSection";

interface CheckinPlacesProps {
  userName: string;
  userId: number;
}

function CheckinPlaces({ userName, userId }: CheckinPlacesProps) {
  const [params, setParams] = useState<UserPlaceParams>({
    pageNumber: 1,
    userId: userId,
  });

  useEffect(() => {
    setParams((prev) => ({
      ...prev,
      userId: userId,
      pageNumber: 1, // Reset to page 1 when user changes
    }));
  }, [userId]);

  const { data, isLoading, isError, error } = useGetPlacesForUser(params);
  const pagination = data?.data?.pagination;
  const places = data?.data?.places || [];

  const handlePageChange = (page: number) => {
    setParams((prev) => ({ ...prev, pageNumber: page }));
  };

  let content;
  if (isLoading) {
    content = <Loader className="my-8" />;
  } else if (isError) {
    content = handleError(error);
  } else if (data?.data?.places?.length === 0) {
    content = (
      <p className="text-center text-gray-600">Chưa checkin địa điểm nào</p>
    );
  } else {
    content = (
      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-5">
        {places.map((place) => (
          <PlaceCard key={place.id} place={place} isOwner={true} />
        ))}
      </div>
    );
  }

  return (
    <>
      <div className="flex justify-between items-end">
        <p className="text-base font-medium">
          ⭐ Địa điểm {userName} đã check-in ⭐
        </p>
        <CreatePlaceSection userId={userId} />
      </div>
      <div className="mt-5">
        {content}
        <Pagination
          currentPage={params.pageNumber || 1}
          totalPage={pagination?.totalPages}
          onPageChange={handlePageChange}
        />
      </div>
    </>
  );
}

export default CheckinPlaces;
